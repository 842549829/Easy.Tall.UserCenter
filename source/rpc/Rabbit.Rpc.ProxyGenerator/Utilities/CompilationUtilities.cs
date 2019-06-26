using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Logging;
using Rabbit.Rpc.Runtime.Client;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

#if !NET
using Microsoft.Extensions.DependencyModel;
#endif

namespace Rabbit.Rpc.ProxyGenerator.Utilities
{
    /// <summary>
    /// CompilationUtilities
    /// </summary>
    public class CompilationUtilities
    {
        /// <summary>
        /// 编译客户端代理类
        /// </summary>
        /// <param name="trees">源文档的解析表示</param>
        /// <param name="references">元数据图像引用</param>
        /// <param name="logger">日志</param>
        /// <returns>对象流</returns>
        public static MemoryStream CompileClientProxy(IEnumerable<SyntaxTree> trees, IEnumerable<MetadataReference> references, ILogger logger = null)
        {
#if NET
            var assemblys = new[]
            {
                "System.Runtime",
                "mscorlib",
                "System.Threading.Tasks"
            };
            references = assemblys.Select(i => MetadataReference.CreateFromFile(Assembly.Load(new AssemblyName(i)).Location)).Concat(references);
#endif
            references = new[]
            {
                MetadataReference.CreateFromFile(typeof(Task).GetTypeInfo().Assembly.Location),
                MetadataReference.CreateFromFile(typeof(ServiceDescriptor).GetTypeInfo().Assembly.Location),
                MetadataReference.CreateFromFile(typeof(IRemoteInvokeService).GetTypeInfo().Assembly.Location),
                MetadataReference.CreateFromFile(typeof(IServiceProxyGenerator).GetTypeInfo().Assembly.Location)
            }.Concat(references);
            return Compile(AssemblyInfo.Create("Rabbit.Rpc.ClientProxies"), trees, references, logger);
        }

        /// <summary>
        /// 编译
        /// </summary>
        /// <param name="assemblyInfo">程序集版本信息</param>
        /// <param name="trees">源文档的解析表示</param>
        /// <param name="references">元数据图像引用</param>
        /// <param name="logger">日志</param>
        /// <returns>对象流</returns>
        public static MemoryStream Compile(AssemblyInfo assemblyInfo, IEnumerable<SyntaxTree> trees, IEnumerable<MetadataReference> references, ILogger logger = null)
        {
            return Compile(assemblyInfo.Title, assemblyInfo, trees, references, logger);
        }

        /// <summary>
        /// 编译
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="assemblyInfo">程序集版本信息</param>
        /// <param name="trees">源文档的解析表示</param>
        /// <param name="references">元数据图像引用</param>
        /// <param name="logger">日志</param>
        /// <returns>对象流</returns>
        public static MemoryStream Compile(string assemblyName, AssemblyInfo assemblyInfo, IEnumerable<SyntaxTree> trees, IEnumerable<MetadataReference> references, ILogger logger = null)
        {
            //var text = File.ReadAllText(@"E:\Projects\ERP\framework\test\Rpc\Holder.Framework.Test.RpcClient\Class1.cs");
            trees = trees.Select(t => CSharpSyntaxTree.ParseText(t.GetText().ToString()));
            var compilation = CSharpCompilation.Create(assemblyName, trees, references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            var stream = new MemoryStream();
            var result = compilation.Emit(stream);
            if (!result.Success)
            {
                if (logger != null)
                {
                    foreach (var message in result.Diagnostics.Select(i => i.ToString()))
                    {
                        logger.LogError(message);
                    }
                }
                return null;
            }
            stream.Seek(0, SeekOrigin.Begin);
            return stream;
        }

        /// <summary>
        /// 获取源文档的解析表示
        /// </summary>
        /// <param name="info">版本信息</param>
        /// <returns>源文档的解析表示</returns>
        private static SyntaxTree GetAssemblyInfo(AssemblyInfo info)
        {
            return CompilationUnit()
                .WithUsings(
                    List(
                        new[]
                        {
                            UsingDirective(
                                QualifiedName(
                                    IdentifierName("System"),
                                    IdentifierName("Reflection"))),
                            UsingDirective(
                                QualifiedName(
                                    QualifiedName(
                                        IdentifierName("System"),
                                        IdentifierName("Runtime")),
                                    IdentifierName("InteropServices"))),
                            UsingDirective(
                                QualifiedName(
                                    QualifiedName(
                                        IdentifierName("System"),
                                        IdentifierName("Runtime")),
                                    IdentifierName("Versioning")))
                        }))
                .WithAttributeLists(
                    List(
                        new[]
                        {
                            AttributeList(
            SingletonSeparatedList(
                Attribute(
                    IdentifierName("TargetFramework"))
                .WithArgumentList(
                    AttributeArgumentList(
                        SeparatedList<AttributeArgumentSyntax>(
                            new SyntaxNodeOrToken[]{
                                AttributeArgument(
                                    LiteralExpression(
                                        SyntaxKind.StringLiteralExpression,
                                        Literal(".NETFramework,Version=v4.5"))),
                                Token(SyntaxKind.CommaToken),
                                AttributeArgument(
                                    LiteralExpression(
                                        SyntaxKind.StringLiteralExpression,
                                        Literal(".NET Framework 4.5")))
                                .WithNameEquals(
                                    NameEquals(
                                        IdentifierName("FrameworkDisplayName")))})))))
        .WithTarget(
            AttributeTargetSpecifier(
                Token(SyntaxKind.AssemblyKeyword))),
                            AttributeList(
                                SingletonSeparatedList(
                                    Attribute(
                                        IdentifierName("AssemblyTitle"))
                                        .WithArgumentList(
                                            AttributeArgumentList(
                                                SingletonSeparatedList(
                                                    AttributeArgument(
                                                        LiteralExpression(
                                                            SyntaxKind.StringLiteralExpression,
                                                            Literal(info.Title))))))))
                                .WithTarget(
                                    AttributeTargetSpecifier(
                                        Token(SyntaxKind.AssemblyKeyword))),
                            AttributeList(
                                SingletonSeparatedList(
                                    Attribute(
                                        IdentifierName("AssemblyProduct"))
                                        .WithArgumentList(
                                            AttributeArgumentList(
                                                SingletonSeparatedList(
                                                    AttributeArgument(
                                                        LiteralExpression(
                                                            SyntaxKind.StringLiteralExpression,
                                                            Literal(info.Product))))))))
                                .WithTarget(
                                    AttributeTargetSpecifier(
                                        Token(SyntaxKind.AssemblyKeyword))),
                            AttributeList(
                                SingletonSeparatedList(
                                    Attribute(
                                        IdentifierName("AssemblyCopyright"))
                                        .WithArgumentList(
                                            AttributeArgumentList(
                                                SingletonSeparatedList(
                                                    AttributeArgument(
                                                        LiteralExpression(
                                                            SyntaxKind.StringLiteralExpression,
                                                            Literal(info.Copyright))))))))
                                .WithTarget(
                                    AttributeTargetSpecifier(
                                        Token(SyntaxKind.AssemblyKeyword))),
                            AttributeList(
                                SingletonSeparatedList(
                                    Attribute(
                                        IdentifierName("ComVisible"))
                                        .WithArgumentList(
                                            AttributeArgumentList(
                                                SingletonSeparatedList(
                                                    AttributeArgument(
                                                        LiteralExpression(info.ComVisible
                                                            ? SyntaxKind.TrueLiteralExpression
                                                            : SyntaxKind.FalseLiteralExpression)))))))
                                .WithTarget(
                                    AttributeTargetSpecifier(
                                        Token(SyntaxKind.AssemblyKeyword))),
                            AttributeList(
                                SingletonSeparatedList(
                                    Attribute(
                                        IdentifierName("Guid"))
                                        .WithArgumentList(
                                            AttributeArgumentList(
                                                SingletonSeparatedList(
                                                    AttributeArgument(
                                                        LiteralExpression(
                                                            SyntaxKind.StringLiteralExpression,
                                                            Literal(info.Guid))))))))
                                .WithTarget(
                                    AttributeTargetSpecifier(
                                        Token(SyntaxKind.AssemblyKeyword))),
                            AttributeList(
                                SingletonSeparatedList(
                                    Attribute(
                                        IdentifierName("AssemblyVersion"))
                                        .WithArgumentList(
                                            AttributeArgumentList(
                                                SingletonSeparatedList(
                                                    AttributeArgument(
                                                        LiteralExpression(
                                                            SyntaxKind.StringLiteralExpression,
                                                            Literal(info.Version))))))))
                                .WithTarget(
                                    AttributeTargetSpecifier(
                                        Token(SyntaxKind.AssemblyKeyword))),
                            AttributeList(
                                SingletonSeparatedList(
                                    Attribute(
                                        IdentifierName("AssemblyFileVersion"))
                                        .WithArgumentList(
                                            AttributeArgumentList(
                                                SingletonSeparatedList(
                                                    AttributeArgument(
                                                        LiteralExpression(
                                                            SyntaxKind.StringLiteralExpression,
                                                            Literal(info.FileVersion))))))))
                                .WithTarget(
                                    AttributeTargetSpecifier(
                                        Token(SyntaxKind.AssemblyKeyword)))
                        }))
                .NormalizeWhitespace()
                .SyntaxTree;
        }
    }
}