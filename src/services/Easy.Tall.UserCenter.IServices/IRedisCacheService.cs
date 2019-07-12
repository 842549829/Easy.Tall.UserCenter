namespace Easy.Tall.UserCenter.IServices
{
    /// <summary>
    /// redis
    /// </summary>
    /// <typeparam name="T">客户端连接</typeparam>
    public interface IRedisCacheService<out T>
    {
        /// <summary>
        /// 获取缓存连接
        /// </summary>
        /// <returns></returns>
        T GetRedisClient();

        /// <summary>
        /// 获取权限路径key
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>key</returns>
        string GtePermissionPathKey(string userId);
    }
}