using System.Collections.Generic;

namespace Easy.Tall.UserCenter.IServices
{
    /// <summary>
    /// 权限缓存
    /// </summary>
    public interface IPermissionCacheService
    {
        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="userId">key</param>
        /// <param name="paths">paths</param>
        /// <param name="seconds">过期时间(单位秒)</param>
        /// <returns>结果</returns>
        bool Add(string userId, IEnumerable<string> paths, int seconds);

        /// <summary>
        /// 用户Id
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="path">路径</param>
        /// <returns>结果</returns>
        bool IsPermission(string userId, string path);
    }
}