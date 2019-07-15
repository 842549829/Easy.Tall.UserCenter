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
    }
}