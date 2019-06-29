using System;
using System.Data;
using Easy.Tall.UserCenter.Framework.Constant;
using Easy.Tall.UserCenter.Framework.Data;
using Easy.Tall.UserCenter.Framework.Db;
using Easy.Tall.UserCenter.Framework.Exceptions;
using Easy.Tall.UserCenter.IRepository;
using Microsoft.Extensions.Logging;

namespace Easy.Tall.UserCenter.Services
{
    /// <summary>
    /// 工作单元基础类
    /// </summary>
    public abstract class UnitOfWorkBase
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
        protected UnitOfWorkBase(IDbUnitOfWorkFactory dbUnitOfWorkFactory,
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
        /// <param name="data">数据</param>
        /// <param name="action">工作单元任务</param>
        /// <returns>结果</returns>
        public Result<bool> Execute<In>(string connectionStringName, In data, Action<IUnitOfWork, In> action)
        {
            using (var unitOfWork = _dbUnitOfWorkFactory.CreateUnitOfWork(connectionStringName))
            {
                try
                {
                    action(unitOfWork, data);
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
        /// <param name="data">数据</param>
        /// <param name="action">工作单元任务</param>
        /// <returns>结果</returns>
        public Result<bool> Execute<In>(In data, Action<IUnitOfWork, In> action)
        {
            return Execute(AppSettingsSection.UserCenterDb, data, action);
        }

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="connectionStringName">数据库链接字符</param>
        /// <param name="data">数据</param>
        /// <param name="action">任务</param>
        /// <returns>结果</returns>
        public Result<bool> Execute<In>(string connectionStringName, In data, Action<IDbConnection, In> action)
        {
            using (var connection = _dbConnectionFactory.CreateDbConnection(connectionStringName))
            {
                try
                {
                    action(connection, data);
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

        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="action">任务</param>
        /// <returns>结果</returns>
        public Result<bool> Execute<In>(In data, Action<IDbConnection, In> action)
        {
            return Execute(AppSettingsSection.UserCenterDb, data, action);
        }

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <typeparam name="Out">查询数据类型</typeparam>
        /// <typeparam name="In">查询条件类型</typeparam>
        /// <param name="connectionStringName">链接字符串</param>
        /// <param name="filter">过滤器</param>
        /// <param name="func">回调函数</param>
        /// <returns>查询结果</returns>
        public Out Query<Out, In>(string connectionStringName, In filter, Func<IDbConnection, In, Out> func)
        {
            Out result;
            using (var connection = _dbConnectionFactory.CreateDbConnection(connectionStringName))
            {
                result = func(connection, filter);
            }
            return result;
        }

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <typeparam name="Out">查询数据类型</typeparam>
        /// <typeparam name="In">查询条件类型</typeparam>
        /// <param name="filter">过滤器</param>
        /// <param name="func">回调函数</param>
        /// <returns>查询结果</returns>
        public Out Query<Out, In>(In filter, Func<IDbConnection, In, Out> func)
        {
            return Query(AppSettingsSection.UserCenterDb, filter, func);
        }
    }
}