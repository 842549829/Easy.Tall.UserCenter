using Easy.Tall.UserCenter.Entity.Enum;
using Easy.Tall.UserCenter.Entity.Extend;
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
    /// 企业服务
    /// </summary>
    public class EnterpriseService : UnitOfWorkBase, IEnterpriseService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbUnitOfWorkFactory">工作单元</param>
        /// <param name="dbConnectionFactory">数据库链接</param>
        /// <param name="repositoryFactory">仓储工厂</param>
        /// <param name="logger">日志</param>
        public EnterpriseService(IDbUnitOfWorkFactory dbUnitOfWorkFactory, 
            IDbConnectionFactory dbConnectionFactory, 
            IRepositoryFactory repositoryFactory, 
            ILogger<EnterpriseService> logger) : base(dbUnitOfWorkFactory, dbConnectionFactory, repositoryFactory, logger)
        {
        }

        /// <summary>
        /// 添加企业
        /// </summary>
        /// <param name="request">企业信息</param>
        /// <returns>添加结果</returns>
        public Result<bool> Add(EnterpriseAddRequest request)
        {
            return Execute(request, (unitOfWork, repositoryFactory, data) =>
            {
                var repository = repositoryFactory.CreateRepository(unitOfWork.Connection);
                // 检查企业是否已存在
                var enterpriseRepository = repository.CreateEnterpriseRepository(unitOfWork);
                if (enterpriseRepository.ExistEnterpriseByAccount(data.Account))
                {
                    throw new BusinessException(1, "企业账号已经存在");
                }
                if (enterpriseRepository.ExistEnterpriseByName(data.Name))
                {
                    throw new BusinessException(1, "企业名称已经存在");
                }
                // 用户如果已经存在则判断绑定用户
                var userRepository = repository.CreateUserRepository(unitOfWork);
                var user = userRepository.QueryUserByAccount(data.Account);
                if (user != null)
                {
                    // 查看用户身份如果是普通用户则更改为管理员
                    if (user.Identity == IdentityType.Normal)
                    {
                        user.Identity = IdentityType.Admin;
                        userRepository.UpdateIdentityType(user);
                    }
                }
                else
                {
                    // 添加用户
                    user = data.ToUser();
                    userRepository.Add(user);
                }
              
                // 添加企业
                var enterprise = data.ToEnterprise();
                enterpriseRepository.Add(enterprise);
                // 添加用户企业关系
                var userEnterpriseRelationRepository = repository.CreateUserEnterpriseRelationRepository(unitOfWork);
                var userEnterpriseRelation = enterprise.ToUserEnterpriseRelation(user);
                userEnterpriseRelationRepository.Add(userEnterpriseRelation);
            });
        }

        /// <summary>
        /// 企业信息分页查询
        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns>结果</returns>
        public Pagination<EnterprisePaginationResponse> GetPagination(EnterpriseFilter filter)
        {
            throw new System.NotImplementedException();
        }
    }
}