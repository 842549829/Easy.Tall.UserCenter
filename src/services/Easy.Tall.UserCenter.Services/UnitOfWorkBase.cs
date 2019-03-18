using System;
using System.Data;
using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Framework.Db;
using Easy.Tall.UserCenter.Framework.Exceptions;
using Easy.Tall.UserCenter.IRepository;
using Microsoft.Extensions.Logging;

namespace Easy.Tall.UserCenter.Services
{
    /// <summary>
    /// 工作单元基础类
    /// </summary>
    public class UnitOfWorkBase
    {
        /// <summary>
        /// 工作单元
        /// </summary>
        protected readonly IDbUnitOfWorkFactory _dbUnitOfWorkFactory;

        /// <summary>
        /// 数据库链接
        /// </summary>
        protected readonly IDbConnectionFactory _dbConnectionFactory;

        /// <summary>
        /// 仓储工厂
        /// </summary>
        protected readonly IRepositoryFactory _repositoryFactory;

        /// <summary>
        /// 日志
        /// </summary>
        protected readonly ILogger<UserServices> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbUnitOfWorkFactory">工作单元</param>
        /// <param name="dbConnectionFactory">数据库链接</param>
        /// <param name="repositoryFactory">仓储工厂</param>
        /// <param name="logger">日志</param>
        public UnitOfWorkBase(IDbUnitOfWorkFactory dbUnitOfWorkFactory,
            IDbConnectionFactory dbConnectionFactory,
            IRepositoryFactory repositoryFactory,
            ILogger<UserServices> logger)
        {
            _dbUnitOfWorkFactory = dbUnitOfWorkFactory;
            _dbConnectionFactory = dbConnectionFactory;
            _repositoryFactory = repositoryFactory;
            _logger = logger;
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="connectionStringName">数据库链接字符</param>
        /// <param name="action">工作单元任务</param>
        /// <returns>结果</returns>
        public Result<bool> Execute(string connectionStringName, Action<IUnitOfWork, IRepository.IRepository> action)
        {
            using (var unitOfWork = _dbUnitOfWorkFactory.CreateUnitOfWork(connectionStringName))
            {
                try
                {
                    var repository = _repositoryFactory.CreateRepository(unitOfWork.Connection);
                    action(unitOfWork, repository);
                    unitOfWork.Complete();
                }
                catch (BusinessException exception)
                {
                    unitOfWork.Rollback();
                    return new Result<bool> { Code = exception.Code, Msg = exception.Message };
                }
                catch (Exception exception)
                {
                    unitOfWork.Rollback();
                    _logger.LogError("系统异常", exception);
                    return new Result<bool> { Code = 500, Msg = "系统错误" };
                }
            }
            return new Result<bool> { Data = true };
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="connectionStringName">数据库链接字符</param>
        /// <param name="actionConnection">任务</param>
        /// <returns>结果</returns>
        public Result<bool> Execute(string connectionStringName, Action<IDbConnection> actionConnection)
        {
            using (var connection = _dbConnectionFactory.CreateDbConnection(connectionStringName))
            {
                try
                {
                    actionConnection(connection);
                }
                catch (BusinessException exception)
                {
                    return new Result<bool> { Code = exception.Code, Msg = exception.Message };
                }
                catch (Exception exception)
                {
                    _logger.LogError("系统异常", exception);
                    return new Result<bool> { Code = 500, Msg = "系统错误" };
                }
            }
            return new Result<bool> { Data = true };
        }
    }
}