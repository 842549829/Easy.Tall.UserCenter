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
    /// 分类
    /// </summary>
    public class ClassifyService : UnitOfWorkBase, IClassifyService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbUnitOfWorkFactory">工作单元</param>
        /// <param name="dbConnectionFactory">数据库链接</param>
        /// <param name="repositoryFactory">仓储工厂</param>
        /// <param name="logger">日志</param>
        public ClassifyService(IDbUnitOfWorkFactory dbUnitOfWorkFactory,
            IDbConnectionFactory dbConnectionFactory,
            IRepositoryFactory repositoryFactory,
            ILogger<ClassifyService> logger)
            : base(dbUnitOfWorkFactory, dbConnectionFactory, repositoryFactory, logger)
        {
        }

        /// <summary>
        /// 添加分类
        /// </summary>
        /// <param name="classifyAddRequest">分类信息</param>
        /// <returns>结果</returns>
        public Result<bool> Add(ClassifyAddRequest classifyAddRequest)
        {
            return Execute(classifyAddRequest, (connection, repositoryFactory, data) =>
            {
                var repository = repositoryFactory.CreateRepository(connection);
                var classifyRepository = repository.CreateClassifyRepository(connection);
                classifyRepository.Add(data.ToClassify());
            });
        }

        /// <summary>
        /// 根据主键Id修改分类
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>结果</returns>
        public Result<bool> Remove(string id)
        {
            return Execute(id, (unitOfWork, repositoryFactory, data) =>
            {
                var repository = repositoryFactory.CreateRepository(unitOfWork.Connection);
                var classifyRepository = repository.CreateClassifyRepository(unitOfWork);
                var classify = classifyRepository.Query(id);
                if (classify == null)
                {
                    throw new BusinessException("删除的分类不存在");
                }
                switch (classify.Type)
                {
                    case ClassifyType.Permission:
                        var permissionRepository = repository.CreatePermissionRepository(unitOfWork);
                        if (permissionRepository.ContainsClassifyType(id))
                        {
                            throw new BusinessException(1, "该分类正在权限中使用");
                        }
                        break;
                    default:
                        var roleRepository = repository.CreateRoleRepository(unitOfWork);
                        if (roleRepository.ContainsClassifyType(id))
                        {
                            throw new BusinessException(2, "该分类正在角色中使用");
                        }
                        break;
                }
                classifyRepository.Remove(new Classify { Id = data });
            });
        }

        /// <summary>
        /// 修改分类
        /// </summary>
        /// <param name="classifyUpdateRequest">分类信息</param>
        /// <returns>结果</returns>
        public Result<bool> Update(ClassifyUpdateRequest classifyUpdateRequest)
        {
            return Execute(classifyUpdateRequest, (connection, repositoryFactory, data) =>
            {
                var repository = repositoryFactory.CreateRepository(connection);
                var userRepository = repository.CreateClassifyRepository(connection);
                userRepository.Update(data.ToClassify());
            });
        }

        /// <summary>
        /// 分类分页
        /// </summary>
        /// <param name="classifyFilter">查询条件</param>
        /// <returns>数据</returns>
        public Pagination<ClassifyPaginationResponse> GetPagination(ClassifyFilter classifyFilter)
        {
            return Query(classifyFilter, (connection, repositoryFactory, filter) =>
            {
                var repository = repositoryFactory.CreateRepository(connection);
                var classifyRepository = repository.CreateClassifyRepository(connection);
                return classifyRepository.GetPagination(filter);
            });
        }
    }
}