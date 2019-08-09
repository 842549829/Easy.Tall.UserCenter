using System.Collections.Generic;
using System.Threading.Tasks;
using Easy.Tall.UserCenter.Entity.Model;

namespace Easy.Tall.UserCenter.IServices
{
    /// <summary>
    /// 权限缓存
    /// </summary>
    public interface IPermissionCacheService
    {
        /// <summary>
        /// 添加权限路径
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="paths">paths</param>
        /// <param name="seconds">过期时间(单位秒)</param>
        /// <returns>结果</returns>
        bool AddPermissionPath(string userId, IEnumerable<string> paths, int seconds);

        /// <summary>
        /// 用户Id
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="path">路径</param>
        /// <returns>结果</returns>
        bool IsPermission(string userId, string path);

        /// <summary>
        /// 添加用户Token
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="token">token</param>
        /// <param name="seconds">过期时间(单位秒)</param>
        /// <returns>结果</returns>
        bool AddUserToken(string userId, string token, int seconds);

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>用户</returns>
        Task<string> GetUserTokenAsync(string userId);

        /// <summary>
        /// 刷新用户Token
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>表示一个异步</returns>
        Task RefreshUserTokenAsync(string userId);
    }
}