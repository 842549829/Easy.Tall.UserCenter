using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RpcLib.Models;

namespace RpcLib
{
    [RpcService]
    public interface IUserService
    {
        Task<string> GetUserName(int id);

        Task<T> GetUserName<T>(int id);

        Task<string> GetUserName(string id);

        Task<T> GetUserNameGeneric<T>(T name);

        Task<string> GetEnterpriseId(int id);

        Task<bool> Exists(int id);

        Task<string> GetUserId(string userName);

        Task<DateTime> GetUserLastSignInTime(int id);

        Task<UserModel> GetUser(int id);

        Task<UserInfo<MyUserInfo>> GetUserInfo(UserInfo<string> info);

        Task<bool> Update(int id, UserModel model);

        Task<IDictionary<string, string>> GetDictionary();

        Task Try();

        Task TryThrowException();
    }
}