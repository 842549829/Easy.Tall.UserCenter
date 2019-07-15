using System.Collections.Generic;
using CSRedis;
using Easy.Tall.UserCenter.IServices;

namespace Easy.Tall.UserCenter.Services
{
    /// <summary>
    /// 权限缓存
    /// </summary>
    public class PermissionCacheService : IPermissionCacheService
    {
        /// <summary>
        /// redisClient
        /// </summary>
        private readonly IRedisCacheService<CSRedisClient> _redisCacheService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="redisCacheService">redisClient</param>
        public PermissionCacheService(
            IRedisCacheService<CSRedisClient> redisCacheService)
        {
            _redisCacheService = redisCacheService;
        }

        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="userId">key</param>
        /// <param name="paths">paths</param>
        /// <param name="seconds">过期时间(单位秒)</param>
        /// <returns>结果</returns>
        public bool Add(string userId, IEnumerable<string> paths, int seconds)
        {
            var key = GtePermissionPathKey(userId);
            var client = _redisCacheService.GetRedisClient();
            var rows = client.SAdd(key, paths);
            var expire = client.Expire(key, seconds);
            return rows > 0 && expire;
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="path">路径</param>
        /// <returns>结果</returns>
        public bool IsPermission(string userId, string path)
        {
            var key = GtePermissionPathKey(userId);
            return _redisCacheService.GetRedisClient().SIsMember(key, path);
        }

        /// <summary>
        /// 获取权限路径key
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>key</returns>
        private static string GtePermissionPathKey(string userId)
        {
            return $"center:permission:{userId}";
        }
    }
}