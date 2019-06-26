using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RpcLib;
using RpcLib.Models;

namespace RpcLibService
{
    public class UserService : BaseContext, IUserService
    {

        public UserService(IContextAccessor context) : base(context)
        {
        }

        public Task<string> GetUserName(int id)
        {
            return Task.FromResult($"id:{id} is name rabbit.");
        }

        public Task<T> GetUserName<T>(int id)
        {
            return Task.FromResult(default(T));
        }

        public Task<string> GetEnterpriseId(int id)
        {
            return Task.FromResult("Test");
        }

        public Task<bool> Exists(int id)
        {
            return Task.FromResult(true);
        }

        public Task<string> GetUserId(string userName)
        {
            return Task.FromResult(_context.UserId);
        }

        public Task<DateTime> GetUserLastSignInTime(int id)
        {
            return Task.FromResult(DateTime.Now);
        }

        public Task<UserModel> GetUser(int id)
        {
            return Task.FromResult(new UserModel
            {
                Name = $"rabbit_{id}",
                Age = id * 2
            });
        }

        public Task<UserInfo<MyUserInfo>> GetUserInfo(UserInfo<string> info)
        {
            return Task.FromResult(new UserInfo<MyUserInfo>());
        }

        public Task<bool> Update(int id, UserModel model)
        {
            return Task.FromResult(true);
        }

        public Task<IDictionary<string, string>> GetDictionary()
        {
            return Task.FromResult<IDictionary<string, string>>(new Dictionary<string, string> { { "key", "value" } });
        }

        public async Task Try()
        {
            Console.WriteLine("start");
            await Task.Delay(5000);
            Console.WriteLine("end");
        }

        public Task TryThrowException()
        {
            throw new RemoteServiceException(123, "用户Id非法！");
        }

        public Task<T> GetUserNameGeneric<T>(T name)
        {
            return Task.FromResult(name);
        }

        public Task<string> GetUserName(string id)
        {
            return Task.FromResult(id);
        }
    }
}
