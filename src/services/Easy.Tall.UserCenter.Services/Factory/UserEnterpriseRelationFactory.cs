using Easy.Tall.UserCenter.Entity.Model;

namespace Easy.Tall.UserCenter.Services.Factory
{
    /// <summary>
    /// 用户企业关系
    /// </summary>
    public static class UserEnterpriseRelationFactory
    {
        /// <summary>
        /// 用户企业关系
        /// </summary>
        /// <param name="user">用户</param>
        /// <param name="enterprise">企业</param>
        /// <returns>用户企业关系</returns>
        public static UserEnterpriseRelation ToUserEnterpriseRelation(this User user, Enterprise enterprise)
        {
            return new UserEnterpriseRelation
            {
                EnterpriseId = enterprise.Id,
                UserId = user.Id
            };
        }

        /// <summary>
        /// 用户企业关系
        /// </summary>
        /// <param name="enterprise">企业</param>
        /// <param name="user">用户</param>
        /// <returns>用户企业关系</returns>
        public static UserEnterpriseRelation ToUserEnterpriseRelation(this Enterprise enterprise, User user)
        {
            return new UserEnterpriseRelation
            {
                EnterpriseId = enterprise.Id,
                UserId = user.Id
            };
        }
    }
}