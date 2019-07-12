﻿using CSRedis;
using Easy.Tall.UserCenter.IServices;

namespace Easy.Tall.UserCenter.Services
{
    /// <summary>
    /// redis
    /// </summary>
    public class RedisCacheService : IRedisCacheService<CSRedisClient>
    {
        /// <summary>
        /// redisClient
        /// </summary>
        private readonly CSRedisClient _redisClient;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="redisClient">redisClient</param>
        public RedisCacheService(CSRedisClient redisClient)
        {
            _redisClient = redisClient;
        }

        /// <summary>
        /// 获取缓存连接
        /// </summary>
        /// <returns></returns>
        public CSRedisClient GetRedisClient()
        {
            return _redisClient;
        }

        /// <summary>
        /// 获取权限路径key
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>key</returns>
        public string GtePermissionPathKey(string userId)
        {
            return $"center:permission:{userId}";
        }
    }
}