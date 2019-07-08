using System.Collections.Generic;
using Easy.Tall.UserCenter.Entity.Enum;
using Easy.Tall.UserCenter.Entity.Model;
using Easy.Tall.UserCenter.Framework.Db;

namespace Easy.Tall.UserCenter.IRepository
{
    /// <summary>
    /// 功能
    /// </summary>
    public interface IFunctionRepository : IRepository<string, Function>
    {
        /// <summary>
        /// 查询功能权限
        /// </summary>
        /// <param name="parentId">父级Id</param>
        /// <returns>功能</returns>
        IEnumerable<Function> GetFunctions(string parentId);

        /// <summary>
        /// 查询功能权限
        /// </summary>
        /// <param name="functionClassify">分类</param>
        /// <returns>功能</returns>
        IEnumerable<Function> GetFunctions(FunctionClassify functionClassify);

        /// <summary>
        /// 删除节点包含所有的子节点
        /// </summary>
        /// <param name="id">id</param>
        void RemoveChildren(string id);

        /// <summary>
        /// 是否包含子节点
        /// </summary>
        /// <param name="parentId">上级父Id</param>
        /// <returns>结果</returns>
        bool ContainsChildren(string parentId);
    }
}