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
        /// <param name="userAddRequest">用户信息</param>
        /// <returns>添加结果</returns>
        Result<bool> Add(UserAddRequest userAddRequest);

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userUpdatePasswordRequest">用户修改信息</param>
        /// <returns>修改结果</returns>
        Result<bool> UpdatePassword(UserUpdatePasswordRequest userUpdatePasswordRequest);
    }
}