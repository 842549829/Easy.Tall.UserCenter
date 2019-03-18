using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Framework.Constant;
using Easy.Tall.UserCenter.Framework.Db;
using Easy.Tall.UserCenter.Framework.Encrypt;
using Easy.Tall.UserCenter.Framework.Exceptions;
using Easy.Tall.UserCenter.IRepository;
using Easy.Tall.UserCenter.IServices;
using Easy.Tall.UserCenter.Services.Factory;
using Microsoft.Extensions.Logging;

namespace Easy.Tall.UserCenter.Services
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public class UserServices : UnitOfWorkBase, IUserServices
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbUnitOfWorkFactory">工作单元</param>
        /// <param name="dbConnectionFactory">数据库链接</param>
        /// <param name="repositoryFactory">仓储工厂</param>
        /// <param name="logger">日志</param>
        public UserServices(IDbUnitOfWorkFactory dbUnitOfWorkFactory, IDbConnectionFactory dbConnectionFactory, IRepositoryFactory repositoryFactory, ILogger<UserServices> logger)
            : base(dbUnitOfWorkFactory, dbConnectionFactory, repositoryFactory, logger)
        {
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="userAddRequest">用户信息</param>
        /// <returns>添加结果</returns>
        public Result<bool> Add(UserAddRequest userAddRequest)
        {
            return Execute(AppSettingsSection.TestDb, (unitOfWork, repository) =>
            {
                IUserRepository userRepository = repository.CreateUserRepository(unitOfWork);
                userRepository.Add(userAddRequest.ToUser());
            });
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userUpdatePasswordRequest">用户修改信息</param>
        /// <returns>修改结果</returns>
        public Result<bool> UpdatePassword(UserUpdatePasswordRequest userUpdatePasswordRequest)
        {
            return Execute(AppSettingsSection.TestDb, connection =>
            {
                var repository = _repositoryFactory.CreateRepository(connection);
                IUserRepository userRepository = repository.CreateUserRepository(connection);
                if (!userRepository.ValidatePassword(userUpdatePasswordRequest.Id, MD5Encrypt.Encrypt(userUpdatePasswordRequest.OldPassword).ToUpper()))
                {
                    throw new BusinessException(400, "修改旧密码错误");
                }
                userRepository.UpdatePassword(userUpdatePasswordRequest.Id, MD5Encrypt.Encrypt(userUpdatePasswordRequest.OldPassword).ToUpper());
            });
        }
    }
}