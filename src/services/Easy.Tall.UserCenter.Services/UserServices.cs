using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Entity.Model;
using Easy.Tall.UserCenter.Framework.Constant;
using Easy.Tall.UserCenter.Framework.Db;
using Easy.Tall.UserCenter.IRepository;
using Easy.Tall.UserCenter.IServices;

namespace Easy.Tall.UserCenter.Services
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public class UserServices : IUserServices
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
        /// 构造函数
        /// </summary>
        /// <param name="dbUnitOfWorkFactory">工作单元</param>
        /// <param name="dbConnectionFactory">数据库链接</param>
        /// <param name="repositoryFactory">仓储工厂</param>
        public UserServices(IDbUnitOfWorkFactory dbUnitOfWorkFactory, IDbConnectionFactory dbConnectionFactory, IRepositoryFactory repositoryFactory)
        {
            _dbUnitOfWorkFactory = dbUnitOfWorkFactory;
            _dbConnectionFactory = dbConnectionFactory;
            _repositoryFactory = repositoryFactory;
        }

        public Result<bool> Add(User user)
        {

            #region 单链接
            //using (var connection = _dbConnectionFactory.CreateDbConnection(AppSettingsSection.TestDb))
            //{
            //    var repository = _repositoryFactory.CreateRepository(connection);
            //    IUserRepository userRepository = repository.CreateUserRepository(connection);
            //    userRepository.Add(user);
            //}
            #endregion

            #region 事物方式
            using (var unitOfWork = _dbUnitOfWorkFactory.CreateUnitOfWork(AppSettingsSection.TestDb))
            {
                var repository = _repositoryFactory.CreateRepository(unitOfWork.Connection);
                IUserRepository userRepository = repository.CreateUserRepository(unitOfWork);
                userRepository.Add(user);
                unitOfWork.Complete();
            }
            #endregion
            return new Result<bool> { Data = true };
        }
    }
}