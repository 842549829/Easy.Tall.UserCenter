using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using CSRedis;
using Easy.Tall.UserCenter.Entity.Model;
using Easy.Tall.UserCenter.IServices;
using Newtonsoft.Json;

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
        /// 缓存
        /// </summary>
        private readonly IDistributedCache _distributedCache;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="redisCacheService">redisClient</param>
        /// <param name="distributedCache">缓存</param>
        public PermissionCacheService(
            IRedisCacheService<CSRedisClient> redisCacheService,
            IDistributedCache distributedCache)
        {
            _redisCacheService = redisCacheService;
            _distributedCache = distributedCache;
        }

        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="userId">key</param>
        /// <param name="paths">paths</param>
        /// <param name="seconds">过期时间(单位秒)</param>
        /// <returns>结果</returns>
        public bool AddPermissionPath(string userId, IEnumerable<string> paths, int seconds)
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
        /// 添加用户Token
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="token">token</param>
        /// <param name="seconds">过期时间(单位秒)</param>
        /// <returns>结果</returns>
        public bool AddUserToken(string userId, string token, int seconds)
        {
            _distributedCache.Set(GteUserTokenKey(userId), Encoding.UTF8.GetBytes(token), new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(seconds)
            });
            return true;
        }

        /// <summary>
        /// 获取用户Id
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>用户</returns>
        public async Task<string> GetUserTokenAsync(string userId)
        {
            return await _distributedCache.GetStringAsync(GteUserTokenKey(userId));
        }

        /// <summary>
        /// 刷新用户Token
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>表示一个异步</returns>
        public async Task RefreshUserTokenAsync(string userId)
        {
            await _distributedCache.RefreshAsync(GteUserTokenKey(userId));
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

        /// <summary>
        /// 获取Token key
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>key</returns>
        private static string GteUserTokenKey(string userId)
        {
            return $"center:token:{userId}";
        }
    }
}