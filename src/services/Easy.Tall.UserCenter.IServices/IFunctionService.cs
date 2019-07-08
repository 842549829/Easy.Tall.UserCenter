using System.Collections.Generic;
using Easy.Tall.UserCenter.Entity.Enum;
using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Entity.Model;
using Easy.Tall.UserCenter.Framework.Data;

namespace Easy.Tall.UserCenter.IServices
{
    /// <summary>
    /// 功能
    /// </summary>
    public interface IFunctionService
    {
        /// <summary>
        /// 添加功能
        /// </summary>
        /// <param name="functionAddRequest">功能信息</param>
        /// <returns>结果</returns>
        Result<bool> Add(FunctionAddRequest functionAddRequest);

        /// <summary>
        /// 删除功能
        /// </summary>
        /// <param name="functionRemoveRequest">删除信息</param>
        /// <returns>结果</returns>
        Result<bool> Remove(FunctionRemoveRequest functionRemoveRequest);

        /// <summary>
        /// 修改功能
        /// </summary>
        /// <param name="functionUpdateRequest">功能信息</param>
        /// <returns>结果</returns>
        Result<bool> Update(FunctionUpdateRequest functionUpdateRequest);

        /// <summary>
        /// 查询功能权限
        /// </summary>
        /// <param name="functionClassify">所属分类</param>
        /// <returns>功能</returns>
        IEnumerable<Function> GetFunctions(FunctionClassify functionClassify);
    }
}