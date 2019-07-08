using System.Collections.Generic;
using Easy.Tall.UserCenter.Entity.Enum;
using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Entity.Model;
using Easy.Tall.UserCenter.Framework.Data;
using Easy.Tall.UserCenter.Framework.Db;
using Easy.Tall.UserCenter.Framework.Exceptions;
using Easy.Tall.UserCenter.IRepository;
using Easy.Tall.UserCenter.IServices;
using Easy.Tall.UserCenter.Services.Factory;
using Microsoft.Extensions.Logging;

namespace Easy.Tall.UserCenter.Services
{
    /// <summary>
    /// 功能
    /// </summary>
    public class FunctionService : UnitOfWorkBase, IFunctionService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbUnitOfWorkFactory">工作单元</param>
        /// <param name="dbConnectionFactory">数据库链接</param>
        /// <param name="repositoryFactory">仓储工厂</param>
        /// <param name="logger">日志</param>
        public FunctionService(IDbUnitOfWorkFactory dbUnitOfWorkFactory, IDbConnectionFactory dbConnectionFactory, IRepositoryFactory repositoryFactory, ILogger logger) : base(dbUnitOfWorkFactory, dbConnectionFactory, repositoryFactory, logger)
        {
        }

        /// <summary>
        /// 添加功能
        /// </summary>
        /// <param name="functionAddRequest">功能信息</param>
        /// <returns>结果</returns>
        public Result<bool> Add(FunctionAddRequest functionAddRequest)
        {
            return Execute(functionAddRequest, (unitOfWork, repositoryFactory, data) =>
            {
                var repository = repositoryFactory.CreateRepository(unitOfWork.Connection);
                var function = repository.CreateFunctionRepository(unitOfWork);
                function.Add(data.ToFunction());
            });
        }

        /// <summary>
        /// 删除功能
        /// </summary>
        /// <param name="functionRemoveRequest">删除信息</param>
        /// <returns>结果</returns>
        public Result<bool> Remove(FunctionRemoveRequest functionRemoveRequest)
        {
            return Execute(functionRemoveRequest, (unitOfWork, repositoryFactory, data) =>
            {
                var repository = repositoryFactory.CreateRepository(unitOfWork.Connection);
                var function = repository.CreateFunctionRepository(unitOfWork);
                if (data.All)
                {
                    function.Remove(new Function { Id = data.Id });
                    function.RemoveChildren(data.Id);
                }
                else
                {
                    if (function.ContainsChildren(data.Id))
                    {
                        throw new BusinessException("包含子节点不允许删除");
                    }
                    function.Remove(new Function { Id = data.Id });
                }
            });
        }

        /// <summary>
        /// 修改功能
        /// </summary>
        /// <param name="functionUpdateRequest">功能信息</param>
        /// <returns>结果</returns>
        public Result<bool> Update(FunctionUpdateRequest functionUpdateRequest)
        {
            return Execute(functionUpdateRequest, (unitOfWork, repositoryFactory, data) =>
            {
                var repository = repositoryFactory.CreateRepository(unitOfWork.Connection);
                var function = repository.CreateFunctionRepository(unitOfWork);
                function.Add(data.ToFunction());
            });
        }

        /// <summary>
        /// 查询功能权限
        /// </summary>
        /// <param name="functionClassify">所属分类</param>
        /// <returns>功能</returns>
        public IEnumerable<Function> GetFunctions(FunctionClassify functionClassify)
        {
            return Query(functionClassify, (connection, repositoryFactory, filter) =>
            {
                var repository = repositoryFactory.CreateRepository(connection);
                var function = repository.CreateFunctionRepository(connection);
                return function.GetFunctions(filter);
            });
        }
    }
}