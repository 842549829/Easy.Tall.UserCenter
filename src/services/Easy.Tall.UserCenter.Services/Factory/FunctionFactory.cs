using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Entity.Model;

namespace Easy.Tall.UserCenter.Services.Factory
{
    /// <summary>
    /// 功能
    /// </summary>
    public static class FunctionFactory
    {
        /// <summary>
        /// 创建功能
        /// </summary>
        /// <param name="functionAddRequest">添加功能信息</param>
        /// <returns>功能</returns>
        public static Function ToFunction(this FunctionAddRequest functionAddRequest)
        {
            return new Function
            {
                Name = functionAddRequest.Name,
                Type = functionAddRequest.Type,
                Classify = functionAddRequest.Classify,
                Describe = functionAddRequest.Describe,
                Flag = functionAddRequest.Flag,
                Icon = functionAddRequest.Icon,
                ParentId = functionAddRequest.ParentId,
                Path = functionAddRequest.Path,
                Sort = functionAddRequest.Sort
            };
        }

        /// <summary>
        /// 创建功能
        /// </summary>
        /// <param name="functionUpdateRequest">修改功能信息</param>
        /// <returns>功能</returns>
        public static Function ToFunction(this FunctionUpdateRequest functionUpdateRequest)
        {
            return new Function
            {
                Id = functionUpdateRequest.Id,
                Name = functionUpdateRequest.Name,
                Describe = functionUpdateRequest.Describe,
                Flag = functionUpdateRequest.Flag,
                Icon = functionUpdateRequest.Icon,
                Path = functionUpdateRequest.Path,
                Sort = functionUpdateRequest.Sort
            };
        }
    }
}