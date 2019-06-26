using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Rabbit.Rpc.Convertibles;
using Rabbit.Rpc.Ids;
using Rabbit.Rpc.ProxyGenerator.Utilities;
using Rabbit.Rpc.Runtime.Client;
using Rabbit.Rpc.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#if !NET

using System.Runtime.Loader;
using Microsoft.Extensions.DependencyModel;

#endif

using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using System.IO;
using Rabbit.Rpc.Messages;

namespace Rabbit.Rpc.ProxyGenerator.Implementation
{
    /// <summary>
    /// 服务代理生成器
    /// </summary>
    public class ServiceProxyGenerator : IServiceProxyGenerator
    {
        /// <summary>
        /// 服务Id生成器
        /// </summary>
        private readonly IServiceIdGenerator _serviceIdGenerator;

        /// <summary>
        /// 日志器
        /// </summary>
        private readonly ILogger<ServiceProxyGenerator> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceIdGenerator">服务Id生成器</param>
        /// <param name="logger">日志器</param>
        public ServiceProxyGenerator(IServiceIdGenerator serviceIdGenerator, ILogger<ServiceProxyGenerator> logger)
        {
            _serviceIdGenerator = serviceIdGenerator;
            _logger = logger;
        }

        /// <summary>
        /// 生成服务代理。
        /// </summary>
        /// <param name="interfaceTypes">需要被代理的接口类型。</param>
        /// <returns>服务代理实现。</returns>
        public IEnumerable<Type> GenerateProxies(IEnumerable<Type> interfaceTypes)
        {
#if NET
            var assemblys = AppDomain.CurrentDomain.GetAssemblies();
#else
            var assemblies = DependencyContext.Default.RuntimeLibraries
                .SelectMany(i => i.GetDefaultAssemblyNames(DependencyContext.Default)
                    .Select(z => Assembly.Load(new AssemblyName(z.Name))))
                .Where(a => !a.IsDynamic);
#endif
            var trees = interfaceTypes.Select(GenerateProxyTree).ToList();
            var stream = CompilationUtilities.CompileClientProxy(trees,
                assemblies
                    .Select(a => MetadataReference.CreateFromFile(a.Location)),
                _logger);

            using (stream)
            {
#if NET
                var assembly = Assembly.Load(stream.ToArray());
#else
                var assembly = Assembly.Load(stream.ToArray());
                //var assembly = AssemblyLoadContext.Default.LoadFromStream(stream);
#endif

                return assembly.GetExportedTypes();

            }
        }

        /// <summary>
        /// 生成服务代理代码树。
        /// </summary>
        /// <param name="interfaceType">需要被代理的接口类型。</param>
        /// <returns>代码树。</returns>
        public SyntaxTree GenerateProxyTree(Type interfaceType)
        {
            var className = interfaceType.Name.StartsWith("I") ? interfaceType.Name.Substring(1) : interfaceType.Name;
            className += "ClientProxy" + "_" + Guid.NewGuid().ToString().Replace("-", string.Empty);

            var members = new List<MemberDeclarationSyntax>
            {
                GetConstructorDeclaration(className)
            };

            members.AddRange(GenerateMethodDeclarations(interfaceType.GetMethods()));
            return CompilationUnit()
                .WithUsings(GetUsing())
                .WithMembers(
                    SingletonList<MemberDeclarationSyntax>(
                        NamespaceDeclaration(
                            QualifiedName(
                                QualifiedName(
                                    IdentifierName("Rabbit"),
                                    IdentifierName("Rpc")),
                                IdentifierName("ClientProxies")))
                .WithMembers(
                    SingletonList<MemberDeclarationSyntax>(
                        ClassDeclaration(className)
                            .WithModifiers(TokenList(Token(SyntaxKind.PublicKeyword)))
                            .WithBaseList(
                                BaseList(
                                    SeparatedList<BaseTypeSyntax>(
                                        new SyntaxNodeOrToken[]
                                        {
                                            SimpleBaseType(IdentifierName("ServiceProxyBase")),
                                            Token(SyntaxKind.CommaToken),
                                            SimpleBaseType(GetQualifiedNameSyntax(interfaceType))
                                        })))
                            .WithMembers(List(members))))))
                .NormalizeWhitespace().SyntaxTree;
        }

        #region Private Method

        /// <summary>
        /// 获取限定名语法
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>限定名语法</returns>
        private static NameSyntax GetQualifiedNameSyntax(Type type)
        {
            var fullName = type.Namespace + "." + type.Name;
            return GetQualifiedNameSyntax(fullName);
        }

        /// <summary>
        /// 获取限定名语法
        /// </summary>
        /// <param name="fullName">全命名</param>
        /// <returns>限定名语法</returns>
        private static NameSyntax GetQualifiedNameSyntax(string fullName)
        {
            return GetQualifiedNameSyntax(fullName.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries));
        }

        /// <summary>
        /// 获取限定名语法
        /// </summary>
        /// <param name="names">名称</param>
        /// <returns>限定名语法</returns>
        private static NameSyntax GetQualifiedNameSyntax(IReadOnlyCollection<string> names)
        {
            var ids = names.Select(IdentifierName).ToArray();

            var index = 0;
            QualifiedNameSyntax left = null;
            if (names.Count == 1)
            {
                return IdentifierName(names.ElementAt(0));
            }
            while (index + 1 < names.Count)
            {
                left = left == null ? QualifiedName(ids[index], ids[index + 1]) : QualifiedName(left, ids[index + 1]);
                index++;
            }
            return left;
        }

        /// <summary>
        /// 获取命名空间语法树
        /// </summary>
        /// <returns>命名空间语法树</returns>
        private static SyntaxList<UsingDirectiveSyntax> GetUsing()
        {
            return List(
                new[]
                {
                    UsingDirective(IdentifierName("System")),
                    UsingDirective(GetQualifiedNameSyntax("System.Threading.Tasks")),
                    UsingDirective(GetQualifiedNameSyntax("System.Collections.Generic")),
                    UsingDirective(GetQualifiedNameSyntax(typeof(ITypeConvertibleService).Namespace)),
                    UsingDirective(GetQualifiedNameSyntax(typeof(IRemoteInvokeService).Namespace)),
                    UsingDirective(GetQualifiedNameSyntax(typeof(IRpcContextAccessor).Namespace)),
                    UsingDirective(GetQualifiedNameSyntax(typeof(ISerializer<>).Namespace)),
                    UsingDirective(GetQualifiedNameSyntax(typeof(ServiceProxyBase).Namespace))
                });
        }

        /// <summary>
        /// 获取构造函数构造函数
        /// </summary>
        /// <param name="className">类名</param>
        /// <returns>构造函数构造函数</returns>
        private static ConstructorDeclarationSyntax GetConstructorDeclaration(string className)
        {
            return ConstructorDeclaration(Identifier(className))
                .WithModifiers(
                    TokenList(
                        Token(SyntaxKind.PublicKeyword)))
                .WithParameterList(
                    ParameterList(
                        SeparatedList<ParameterSyntax>(
                            new SyntaxNodeOrToken[]{
                                Parameter(
                                    Identifier("remoteInvokeService"))
                                .WithType(
                                    IdentifierName("IRemoteInvokeService")),
                                Token(SyntaxKind.CommaToken),
                                Parameter(
                                    Identifier("typeConvertibleService"))
                                .WithType(
                                    IdentifierName("ITypeConvertibleService")),
                                Token(SyntaxKind.CommaToken),
                                Parameter(
                                    Identifier("contextAccessor"))
                                .WithType(
                                    IdentifierName("IRpcContextAccessor"))
                                .WithDefault(
                                    EqualsValueClause(
                                        LiteralExpression(
                                            SyntaxKind.NullLiteralExpression)))})))
                .WithInitializer(
                    ConstructorInitializer(
                        SyntaxKind.BaseConstructorInitializer,
                        ArgumentList(
                            SeparatedList<ArgumentSyntax>(
                                new SyntaxNodeOrToken[]{
                                    Argument(
                                        IdentifierName("remoteInvokeService")),
                                    Token(SyntaxKind.CommaToken),
                                    Argument(
                                        IdentifierName("typeConvertibleService")),
                                    Token(SyntaxKind.CommaToken),
                                    Argument(
                                        IdentifierName("contextAccessor"))}))))
                .WithBody(Block());
        }

        /// <summary>
        /// 创建方法语法树
        /// </summary>
        /// <param name="methods">方法信息</param>
        /// <returns>方法语法树</returns>
        private IEnumerable<MemberDeclarationSyntax> GenerateMethodDeclarations(IEnumerable<MethodInfo> methods)
        {
            var array = methods.ToArray();
            return array.Select(GenerateMethodDeclaration).ToArray();
        }

        /// <summary>
        /// 获取类型语法树
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>类型语法树</returns>
        private static TypeSyntax GetTypeSyntax(Type type)
        {
            //没有返回值。
            if (type == null)
            {
                return null;
            }

            //非泛型。
            if (!type.GetTypeInfo().IsGenericType)
            {
                return GetQualifiedNameSyntax(type.FullName);
            }

            var list = new List<SyntaxNodeOrToken>();

            foreach (var genericTypeArgument in type.GenericTypeArguments)
            {
                var typeInfo = genericTypeArgument.GetTypeInfo();
                list.Add(typeInfo.IsGenericType
                    ? GetTypeSyntax(genericTypeArgument)
                    : GetQualifiedNameSyntax(genericTypeArgument.FullName ?? genericTypeArgument.Name));
                list.Add(Token(SyntaxKind.CommaToken));
            }

            var array = list.Take(list.Count - 1).ToArray();
            var typeArgumentListSyntax = TypeArgumentList(SeparatedList<TypeSyntax>(array));
            return GenericName(type.FullName?.Substring(0, type.FullName.IndexOf('`')) ?? type.Name.Substring(0, type.Name.IndexOf('`')))
                 .WithTypeArgumentList(typeArgumentListSyntax);
        }

        /// <summary>
        /// 获取语法节点或令牌
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>语法节点或令牌</returns>
        private static SyntaxNodeOrToken GetTypeSyntaxToken(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            //非泛型。
            if (!typeInfo.IsGenericType && !typeInfo.IsGenericParameter)
            {
                return GetQualifiedNameSyntax(type);
            }

            var list = new List<SyntaxNodeOrToken>();

            foreach (var genericTypeArgument in type.GenericTypeArguments)
            {
                var argInfo = genericTypeArgument.GetTypeInfo();
                list.Add(argInfo.IsGenericType
                    ? GetTypeSyntaxToken(genericTypeArgument)
                    : argInfo.IsGenericParameter ? IdentifierName(genericTypeArgument.Name) : GetTypeSyntaxToken(genericTypeArgument));
                list.Add(Token(SyntaxKind.CommaToken));
            }

            if (list.Any())
            {
                var array = list.Take(list.Count - 1).ToArray();
                var typeArgumentListSyntax = TypeArgumentList(SeparatedList<TypeSyntax>(array));
                return GetGenericName(type, typeArgumentListSyntax);
            }
            return typeInfo.IsGenericParameter ? (TypeSyntax)IdentifierName(type.Name) : GetQualifiedNameSyntax(type);
        }

        /// <summary>
        /// 获取属名
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="typeArgumentListSyntax">参数列表</param>
        /// <returns>属名</returns>
        private static TypeSyntax GetGenericName(Type type, TypeArgumentListSyntax typeArgumentListSyntax)
        {
            string fullName = GetFullName(type);
            var name = type.Name.Substring(0, type.Name.IndexOf('`'));

            if (fullName == "System.Nullable")
            {
                return GenericName(name).WithTypeArgumentList(typeArgumentListSyntax);
            }

            var qualifiedNameSyntax = GetQualifiedNameSyntax(fullName.Substring(0, fullName.LastIndexOf('.')));
            if (qualifiedNameSyntax != null)
            {
                return QualifiedName(qualifiedNameSyntax,
                    GenericName(name).WithTypeArgumentList(typeArgumentListSyntax));
            }
            return GenericName(name).WithTypeArgumentList(typeArgumentListSyntax);
        }

        /// <summary>
        /// 获取全命名
        /// </summary>
        /// <param name="type">类型</param>
        /// <returns>全命名</returns>
        private static string GetFullName(Type type)
        {
            if (type.IsGenericType)
            {
                return (type.DeclaringType == null ? type.Namespace : GetFullName(type.DeclaringType)) + "." + type.Name.Substring(0, type.Name.IndexOf('`'));
            }
            return (type.DeclaringType == null ? type.Namespace : GetFullName(type.DeclaringType)) + "." + type.Name;
        }

        /// <summary>
        /// 创建方法描述
        /// </summary>
        /// <param name="method">方法信息</param>
        /// <returns>方法</returns>
        private MemberDeclarationSyntax GenerateMethodDeclaration(MethodInfo method)
        {
            var serviceId = _serviceIdGenerator.GenerateServiceId(method);
            var returnDeclaration = GetTypeSyntax(method.ReturnType);
            var parameterList = GetParameterList(method);
            var parameterDeclarationList = GetParameterDeclarationList(method);
            var genericTypeArguments = method.GetGenericArguments();
            var genericArgumentsList = GetGenericArguments(method, genericTypeArguments);
            var declaration = GetMethodDeclaration(method, returnDeclaration, genericTypeArguments);

            SyntaxTokenList tokenList;
            ExpressionSyntax expressionSyntax;
            StatementSyntax statementSyntax;

            if (method.ReturnType == typeof(Task) && method.ReturnType != typeof(Task<>))
            {
                tokenList = TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.AsyncKeyword));
                expressionSyntax = IdentifierName("InvokeAsync");
            }
            else if (method.ReturnType.IsGenericType && method.ReturnType.BaseType == typeof(Task))
            {
                tokenList = TokenList(Token(SyntaxKind.PublicKeyword), Token(SyntaxKind.AsyncKeyword));
                expressionSyntax = GenericName(Identifier("InvokeAsync")).WithTypeArgumentList(
                        TypeArgumentList(SeparatedList<TypeSyntax>(genericArgumentsList)));
            }
            else
            {
                tokenList = TokenList(Token(SyntaxKind.PublicKeyword));
                if (genericArgumentsList.Any())
                {
                    expressionSyntax = GenericName(Identifier("Invoke")).WithTypeArgumentList(
                        TypeArgumentList(SeparatedList<TypeSyntax>(genericArgumentsList)));
                }
                else
                {
                    expressionSyntax = IdentifierName("Invoke");
                }
            }

            declaration = declaration.WithModifiers(tokenList)
                .WithParameterList(ParameterList(SeparatedList<ParameterSyntax>(parameterDeclarationList)));

            expressionSyntax = BodySyntax(method, serviceId, parameterList, expressionSyntax);

            if (tokenList.Count > 1)
            {
                expressionSyntax = AwaitExpression(expressionSyntax);
            }

            if (method.ReturnType != typeof(void) && method.ReturnType != typeof(Task))
            {
                statementSyntax = ReturnStatement(expressionSyntax);
            }
            else
            {
                statementSyntax = ExpressionStatement(expressionSyntax);
            }

            declaration = declaration.WithBody(
                        Block(
                            SingletonList(statementSyntax)));

            return declaration;
        }

        /// <summary>
        /// 获取方法声明
        /// </summary>
        /// <param name="method">方法</param>
        /// <param name="returnDeclaration">返回声明</param>
        /// <param name="genericTypeArguments">参数类型</param>
        /// <returns>方法声明</returns>
        private static MethodDeclarationSyntax GetMethodDeclaration(MethodInfo method, TypeSyntax returnDeclaration, Type[] genericTypeArguments)
        {
            var declaration = MethodDeclaration(
                       method.ReturnType == typeof(void) ? SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)) : returnDeclaration,
                       Identifier(method.Name));

            if (genericTypeArguments.Any())
            {
                declaration = declaration.WithTypeParameterList(
                        TypeParameterList(
                            SeparatedList<TypeParameterSyntax>(
                                genericTypeArguments.ToList().ConvertAll<TypeParameterSyntax>(t => TypeParameter(t.Name)))));

                IEnumerable<TypeParameterConstraintClauseSyntax> constraintClauses = GetConstraintClauses(genericTypeArguments);
                if (constraintClauses.Any())
                {
                    declaration = declaration.AddConstraintClauses(constraintClauses.ToArray());
                }
            }

            return declaration;
        }

        /// <summary>
        /// 获取泛型参数约束
        /// </summary>
        /// <param name="genericTypeArguments">参数类型</param>
        /// <returns>泛型参数约束</returns>
        private static IEnumerable<TypeParameterConstraintClauseSyntax> GetConstraintClauses(Type[] genericTypeArguments)
        {
            List<TypeParameterConstraintClauseSyntax> constraintClauses =
                new List<TypeParameterConstraintClauseSyntax>(genericTypeArguments.Length);

            foreach (var genericTypeArgument in genericTypeArguments)
            {
                var parameterConstraints = genericTypeArgument.GetGenericParameterConstraints();

                List<TypeParameterConstraintSyntax> constraints = new System.Collections.Generic.List<TypeParameterConstraintSyntax>();

                if ((genericTypeArgument.GenericParameterAttributes & GenericParameterAttributes.ReferenceTypeConstraint) != GenericParameterAttributes.None)
                {
                    constraints.Add(ClassOrStructConstraint(SyntaxKind.ClassConstraint));
                }

                if (parameterConstraints.Any())
                {
                    constraints.AddRange(parameterConstraints.Select(arg => TypeConstraint(GetTypeSyntax(arg))));
                }

                if (constraints.Any())
                {
                    var constraintClause = TypeParameterConstraintClause(genericTypeArgument.Name);
                    constraintClause = constraintClause.AddConstraints(constraints.ToArray());
                    constraintClauses.Add(constraintClause);
                }
            }

            return constraintClauses;
        }

        /// <summary>
        /// 获取泛型参数
        /// </summary>
        /// <param name="method">方法</param>
        /// <param name="genericArguments">参数类型</param>
        /// <returns>泛型参数</returns>
        private static List<SyntaxNodeOrToken> GetGenericArguments(MethodInfo method, Type[] genericArguments)
        {
            var methodGenericArgumentList = new List<SyntaxNodeOrToken>();
            if (genericArguments.Any())
            {
                foreach (var methodGenericArgument in genericArguments)
                {
                    methodGenericArgumentList.Add(GetTypeSyntaxToken(methodGenericArgument));
                    methodGenericArgumentList.Add(Token(SyntaxKind.CommaToken));
                }
            }
            if (method.ReturnType == typeof(void))
            {
                if (methodGenericArgumentList.Any())
                {
                    methodGenericArgumentList.RemoveAt(methodGenericArgumentList.Count - 1);
                }
            }
            else
            {
                if (method.ReturnType == typeof(Task))
                {
                    //methodGenericArgumentList.Add(GetTypeSyntaxToken(method.ReturnType));
                }
                else if (method.ReturnType.BaseType == typeof(Task))
                {
                    methodGenericArgumentList.Add(GetTypeSyntaxToken(method.ReturnType.GenericTypeArguments.First()));
                }
                else
                {
                    methodGenericArgumentList.Add(GetTypeSyntaxToken(method.ReturnType));
                }
            }
            return methodGenericArgumentList;
        }

        /// <summary>
        /// 获取声明参数列表语法树
        /// </summary>
        /// <param name="method">方法</param>
        /// <returns>声明参数列表语法树</returns>
        private static List<SyntaxNodeOrToken> GetParameterDeclarationList(MethodInfo method)
        {
            List<SyntaxNodeOrToken> parameterDeclarationList = new List<SyntaxNodeOrToken>();
            foreach (var parameter in method.GetParameters())
            {
                if (parameter.ParameterType.IsGenericType)
                {
                    if (parameter.ParameterType.BaseType == typeof(Nullable))
                    {
                        parameterDeclarationList.Add(Parameter(
                                            Identifier(parameter.Name))
                                            .WithType(
                                                NullableType(GetTypeSyntax(parameter.ParameterType.GetGenericTypeDefinition()))));
                    }
                    else
                    {
                        parameterDeclarationList.Add(Parameter(
                                            Identifier(parameter.Name))
                                            .WithType(GetTypeSyntax(parameter.ParameterType)));
                    }
                }
                else
                {
                    parameterDeclarationList.Add(Parameter(
                                        Identifier(parameter.Name))
                                        .WithType(parameter.ParameterType.IsGenericParameter ? (TypeSyntax)IdentifierName(parameter.ParameterType.Name) : (TypeSyntax)GetQualifiedNameSyntax(parameter.ParameterType)));
                }
                parameterDeclarationList.Add(Token(SyntaxKind.CommaToken));

            }
            if (parameterDeclarationList.Any())
            {
                parameterDeclarationList.RemoveAt(parameterDeclarationList.Count - 1);
            }

            return parameterDeclarationList;
        }

       /// <summary>
       /// 获取参数列表
       /// </summary>
       /// <param name="method">方法</param>
       /// <returns>参数列表</returns>
        private static List<SyntaxNodeOrToken> GetParameterList(MethodInfo method)
        {
            List<SyntaxNodeOrToken> parameterList = new System.Collections.Generic.List<SyntaxNodeOrToken>();
            foreach (var parameter in method.GetParameters())
            {
                parameterList.Add(InitializerExpression(
                    SyntaxKind.ComplexElementInitializerExpression,
                    SeparatedList<ExpressionSyntax>(
                        new SyntaxNodeOrToken[]{
                            LiteralExpression(
                                SyntaxKind.StringLiteralExpression,
                                Literal(parameter.Name)),
                            Token(SyntaxKind.CommaToken),
                            IdentifierName(parameter.Name)})));
                parameterList.Add(Token(SyntaxKind.CommaToken));
            }
            if (parameterList.Any())
            {
                parameterList.RemoveAt(parameterList.Count - 1);
            }
            return parameterList;
        }

        /// <summary>
        /// 获取表达式
        /// </summary>
        /// <param name="method">方法</param>
        /// <param name="serviceId">服务Id</param>
        /// <param name="parameterList">参数列表</param>
        /// <param name="expressionSyntax">表达式</param>
        /// <returns>表达式</returns>
        private static ExpressionSyntax BodySyntax(MethodInfo method, string serviceId, List<SyntaxNodeOrToken> parameterList, ExpressionSyntax expressionSyntax)
        {
            expressionSyntax = InvocationExpression(expressionSyntax)
                    .WithArgumentList(
                        ArgumentList(
                            SeparatedList<ArgumentSyntax>(
                                new SyntaxNodeOrToken[]
                                {
                                        Argument(
                                            ObjectCreationExpression(
                                                GenericName(
                                                    Identifier("Dictionary"))
                                                    .WithTypeArgumentList(
                                                        TypeArgumentList(
                                                            SeparatedList<TypeSyntax>(
                                                                new SyntaxNodeOrToken[]
                                                                {
                                                                    PredefinedType(
                                                                        Token(SyntaxKind.StringKeyword)),
                                                                    Token(SyntaxKind.CommaToken),
                                                                    PredefinedType(
                                                                        Token(SyntaxKind.ObjectKeyword))
                                                                }))))
                                                .WithInitializer(
                                                    InitializerExpression(
                                                        SyntaxKind.CollectionInitializerExpression,
                                                        SeparatedList<ExpressionSyntax>(
                                                            parameterList)))),
                                        Token(SyntaxKind.CommaToken),
                                        Argument(
                                            LiteralExpression(
                                                SyntaxKind.StringLiteralExpression,
                                                Literal(serviceId)))
                                })));
            return expressionSyntax;
        }

        #endregion Private Method
    }
}
