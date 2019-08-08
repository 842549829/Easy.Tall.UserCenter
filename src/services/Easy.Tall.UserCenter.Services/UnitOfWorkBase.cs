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
        private readonly IDbUnitOfWorkFactory _dbUnitOfWorkFactory;

        /// <summary>
        /// 数据库链接
        /// </summary>
        private readonly IDbConnectionFactory _dbConnectionFactory;

        /// <summary>
        /// 仓储工厂
        /// </summary>
        private readonly IRepositoryFactory _repositoryFactory;

        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILogger _logger;

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
            ILogger logger)
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
        public Result<bool> Execute<In>(string connectionStringName, In data, Action<IUnitOfWork, IRepositoryFactory, In> action)
        {
            using (var unitOfWork = _dbUnitOfWorkFactory.CreateUnitOfWork(connectionStringName))
            {
                try
                {
                    action(unitOfWork, _repositoryFactory, data);
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
        public Result<bool> Execute<In>(In data, Action<IUnitOfWork, IRepositoryFactory, In> action)
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
        public Result<bool> Execute<In>(string connectionStringName, In data, Action<IDbConnection, IRepositoryFactory, In> action)
        {
            using (var connection = _dbConnectionFactory.CreateDbConnection(connectionStringName))
            {
                try
                {
                    action(connection, _repositoryFactory, data);
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
        public Result<bool> Execute<In>(In data, Action<IDbConnection, IRepositoryFactory, In> action)
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
        public Out Query<Out, In>(string connectionStringName, In filter, Func<IDbConnection, IRepositoryFactory, In, Out> func)
        {
            try
            {
                Out result;
                using (var connection = _dbConnectionFactory.CreateDbConnection(connectionStringName))
                {
                    result = func(connection, _repositoryFactory, filter);
                }
                return result;
            }
            catch (Exception exception)
            {
                _logger.LogError("系统异常", exception);
                return default;
            }
        }

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <typeparam name="Out">查询数据类型</typeparam>
        /// <typeparam name="In">查询条件类型</typeparam>
        /// <param name="filter">过滤器</param>
        /// <param name="func">回调函数</param>
        /// <returns>查询结果</returns>
        public Out Query<Out, In>(In filter, Func<IDbConnection, IRepositoryFactory, In, Out> func)
        {
            return Query(AppSettingsSection.UserCenterDb, filter, func);
        }

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <typeparam name="Out">查询数据类型</typeparam>
        /// <param name="connectionStringName">链接字符串</param>
        /// <param name="func">回调函数</param>
        /// <returns>查询结果</returns>
        public Out Query<Out>(string connectionStringName, Func<IDbConnection, IRepositoryFactory, Out> func)
        {
            try
            {
                Out result;
                using (var connection = _dbConnectionFactory.CreateDbConnection(connectionStringName))
                {
                    result = func(connection, _repositoryFactory);
                }
                return result;
            }
            catch (Exception exception)
            {
                _logger.LogError("系统异常", exception);
                return default;
            }
        }

        /// <summary>
        /// 执行查询
        /// </summary>
        /// <typeparam name="Out">查询数据类型</typeparam>
        /// <param name="func">回调函数</param>
        /// <returns>查询结果</returns>
        public Out Query<Out>(Func<IDbConnection, IRepositoryFactory, Out> func)
        {
            return Query(AppSettingsSection.UserCenterDb, func);
        }

        /// <summary>Ok</summary>
        /// <param name="code">code</param>
        /// <param name="value">value</param>
        /// <returns>OkObjectResult</returns>
        protected virtual Result<T> Ok<T>(int code, T value)
        {
            return CreateResult(code, value);
        }

        /// <summary>Ok</summary>
        /// <param name="code">code</param>
        /// <param name="msg">msg</param>
        /// <returns>OkObjectResult</returns>
        protected virtual Result<T> Ok<T>(int code, string msg)
        {
            return CreateResult(code, default(T), msg);
        }

        /// <summary>Ok</summary>
        /// <param name="code">code</param>
        /// <param name="value">value</param>
        /// <param name="msg">msg</param>
        /// <returns>OkObjectResult</returns>
        protected virtual Result<T> Ok<T>(int code, T value, string msg)
        {
            return CreateResult(code, value, msg);
        }

        /// <summary>CreateResult</summary>
        /// <typeparam name="T">T</typeparam>
        /// <param name="code">code</param>
        /// <param name="value">value</param>
        /// <param name="msg">msg</param>
        /// <returns>Result</returns>
        protected virtual Result<T> CreateResult<T>(int code, T value, string msg = null)
        {
            return new Result<T>
            {
                Code = code,
                Data = value,
                Msg = msg
            };
        }
    }
}