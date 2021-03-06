﻿using System;
using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Entity.Model;
using Easy.Tall.UserCenter.Framework.Data;
using Easy.Tall.UserCenter.Framework.Db;
using Easy.Tall.UserCenter.Framework.Encrypt;
using Easy.Tall.UserCenter.Framework.Exceptions;
using Easy.Tall.UserCenter.IRepository;
using Easy.Tall.UserCenter.IServices;
using Easy.Tall.UserCenter.Services.Factory;
using Microsoft.Extensions.Logging;

namespace Easy.Tall.UserCenter.Services
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public class UserService : UnitOfWorkBase, IUserService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbUnitOfWorkFactory">工作单元</param>
        /// <param name="dbConnectionFactory">数据库链接</param>
        /// <param name="repositoryFactory">仓储工厂</param>
        /// <param name="logger">日志</param>
        public UserService(IDbUnitOfWorkFactory dbUnitOfWorkFactory,
            IDbConnectionFactory dbConnectionFactory,
            IRepositoryFactory repositoryFactory,
            ILogger<UserService> logger)
            : base(dbUnitOfWorkFactory, dbConnectionFactory, repositoryFactory, logger)
        {
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="userAddRequest">用户信息</param>
        /// <returns>添加结果</returns>
        public Result<bool> Add(UserAddRequest userAddRequest)
        {
            return Execute(userAddRequest, (unitOfWork, repositoryFactory, data) =>
            {
                var repository = repositoryFactory.CreateRepository(unitOfWork.Connection);
                var userRepository = repository.CreateUserRepository(unitOfWork);
                userRepository.Add(data.ToUser());
            });
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userUpdatePasswordRequest">用户修改信息</param>
        /// <returns>修改结果</returns>
        public Result<bool> UpdatePassword(UserUpdatePasswordRequest userUpdatePasswordRequest)
        {
            return Execute(userUpdatePasswordRequest, (connection, repositoryFactory, data) =>
            {
                var repository = repositoryFactory.CreateRepository(connection);
                var userRepository = repository.CreateUserRepository(connection);
                if (!userRepository.ValidatePassword(data.Id, MD5Encrypt.Encrypt(data.OldPassword).ToUpper()))
                {
                    throw new BusinessException(400, "修改旧密码错误");
                }
                userRepository.UpdatePassword(data.Id, MD5Encrypt.Encrypt(data.OldPassword).ToUpper());
            });
        }

        /// <summary>
        /// 用户分页查询
        /// </summary>
        /// <param name="userFilter">用户查询条件</param>
        /// <returns>查询数据</returns>
        public Pagination<UserPaginationResponse> GetPagination(UserFilter userFilter)
        {
            return Query(userFilter, (connection, repositoryFactory, filter) =>
            {
                var repository = repositoryFactory.CreateRepository(connection);
                var userRepository = repository.CreateUserRepository(connection);
                return userRepository.GetPagination(filter);
            });
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userLoginRequest">用户登录信息</param>
        /// <returns>登录结果</returns>
        public Result<User> Login(UserLoginRequest userLoginRequest)
        {
            var user = Query(userLoginRequest.Account, (connection, repositoryFactory, filter) =>
            {
                var repository = repositoryFactory.CreateRepository(connection);
                var userRepository = repository.CreateUserRepository(connection);
                return userRepository.QueryUserByAccount(filter);
            });
            if (user == null)
            {
                return Ok<User>(1, "账号不存在");
            }
            if (!string.Equals(user.Password, userLoginRequest.Password, StringComparison.OrdinalIgnoreCase))
            {
                return Ok<User>(2, "密码错误");
            }
            return Ok(0, user);
        }
    }
}