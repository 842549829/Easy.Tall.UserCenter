﻿using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Entity.Model;
using Easy.Tall.UserCenter.Framework.Data;

namespace Easy.Tall.UserCenter.IServices
{
    /// <summary>
    /// 用户服务接口
    /// </summary>
    public interface IUserService
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

        /// <summary>
        /// 用户分页查询
        /// </summary>
        /// <param name="userFilter">用户查询条件</param>
        /// <returns>查询数据</returns>
        Pagination<UserPaginationResponse> GetPagination(UserFilter userFilter);

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userLoginRequest">用户登录信息</param>
        /// <returns>登录结果</returns>
        Result<User> Login(UserLoginRequest userLoginRequest);
    }
}