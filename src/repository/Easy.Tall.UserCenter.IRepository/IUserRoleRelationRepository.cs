using System;
using System.Collections.Generic;
using Easy.Tall.UserCenter.Entity.Model;

namespace Easy.Tall.UserCenter.IRepository
{
    /// <summary>
    /// 用户角色关系
    /// </summary>
    public interface IUserRoleRelationRepository : IDisposable
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="userRoleRelation">用户角色</param>
        void AddRange(IEnumerable<UserRoleRelation> userRoleRelation);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="userRoleRelation">用户角色</param>
        void RemoveRange(IEnumerable<UserRoleRelation> userRoleRelation);

        /// <summary>
        /// 根据角色Id查询用户角色
        /// </summary>
        /// <param name="roleId">角色Id</param>
        /// <returns>结果</returns>
        IEnumerable<UserRoleRelation> QueryByRoleId(string roleId);

        /// <summary>
        /// 根据用户Id查询用户角色
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>结果</returns>
        IEnumerable<UserRoleRelation> QueryByUserId(string userId);
    }
}