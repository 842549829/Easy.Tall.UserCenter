using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Entity.Model;

namespace Easy.Tall.UserCenter.IServices
{
    /// <summary>
    /// 用户服务接口
    /// </summary>
    public interface IUserServices
    {
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user">用户信息</param>
        /// <returns>添加结果</returns>
        Result<bool> Add(User user);
    }
}