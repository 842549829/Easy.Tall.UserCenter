using System.Collections.Generic;
using System.Data;
using System.Linq;
using Easy.Tall.UserCenter.Entity.Enum;
using Easy.Tall.UserCenter.Entity.Model;
using Easy.Tall.UserCenter.Framework.Db;
using Easy.Tall.UserCenter.IRepository;
using Dapper;

namespace Easy.Tall.UserCenter.Repository.MySql
{
    /// <summary>
    /// 功能
    /// </summary>
    public class MySqlFunctionRepository : BaseRepository, IFunctionRepository
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="unit">工作单元</param>
        public MySqlFunctionRepository(IUnitOfWork unit) : base(unit)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbConnection">数据库连接字符串</param>
        public MySqlFunctionRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>返回结果</returns>
        public void Add(Function entity)
        {
            var sql = "INSERT INTO `Function` (Id,CreateTime,ModifyTime,Name,ParentId,Icon,`Type`,Sort,Path,`Describe`,Flag,Classify) VALUES(@Id,@CreateTime,@ModifyTime,@Name,@ParentId,@Icon,@Type,@Sort,@Path,@Describe,@Flag,@Classify);";
            Connection.Execute(sql, entity);
        }

        /// <summary>
        /// Remove
        /// </summary>
        /// <param name="entity">T</param>
        /// <returns>返回结果</returns>
        public void Remove(Function entity)
        {
            var sql = "DELETE FROM `Function` WHERE Id=@Id;";
            Connection.Execute(sql, entity);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>返回结果</returns>
        public void Update(Function entity)
        {
            var sql = "UPDATE `Function` SET Icon=@Icon,Sort=@Sort,Path=@Path,`Describe`=@Describe WHERE Id=@Id";
            Connection.Execute(sql, entity);
        }

        /// <summary>
        /// 查询功能权限
        /// </summary>
        /// <param name="parentId">父级Id</param>
        /// <returns>功能</returns>
        public IEnumerable<Function> GetFunctions(string parentId)
        {
            var sql = @"WITH RECURSIVE _children AS
                        (
                         SELECT fun.* FROM `Function` fun WHERE fun.ParentId=@ParentId
                            UNION ALL
                         SELECT fun.* FROM _children,`Function` fun WHERE fun.ParentId=_children.Id
                        )
                        SELECT * FROM _children;";
            return Connection.Query<Function>(sql, new { ParentId = parentId });
        }

        /// <summary>
        /// 查询功能权限
        /// </summary>
        /// <param name="functionClassify">分类</param>
        /// <returns>功能</returns>
        public IEnumerable<Function> GetFunctions(FunctionClassify functionClassify)
        {
            var sql = @"WITH RECURSIVE _children AS
                        (
                         SELECT fun.* FROM `Function` fun WHERE (fun.ParentId IS NULL OR fun.ParentId = '0') AND (fun.Classify =@Classify)
                            UNION ALL
                         SELECT fun.* FROM _children,`Function` fun WHERE fun.ParentId=_children.Id
                        )
                        SELECT * FROM _children;";
            return Connection.Query<Function>(sql, new { Classify = functionClassify });
        }

        /// <summary>
        /// 删除节点包含所有的子节点
        /// </summary>
        /// <param name="id">id</param>
        public void RemoveChildren(string id)
        {
            var sql = @"WITH RECURSIVE _children AS
                        (
                         SELECT fun.* FROM `Function` fun WHERE fun.ParentId=@ParentId
                            UNION ALL
                         SELECT fun.* FROM _children,`Function` fun WHERE fun.ParentId=_children.Id
                        )
                       DELETE FROM `Function` WHERE Id IN (SELECT Id FROM _children);";

            Connection.Execute(sql, new { ParentId = id });
        }

        /// <summary>
        /// 是否包含子节点
        /// </summary>
        /// <param name="parentId">上级父Id</param>
        /// <returns>结果</returns>
        public bool ContainsChildren(string parentId)
        {
            var sql = "SELECT COUNT(1) FROM `Function` WHERE ParentId=@ParentId;";
            var count = Connection.Query<int>(sql, new { ParentId = parentId }).SingleOrDefault();
            return count > 0;
        }

        /// <summary>
        /// Query
        /// </summary>
        /// <param name="key">id</param>
        /// <returns>返回查询单条数据</returns>
        public Function Query(string key)
        {
            var sql = "SELECT COUNT(1) FROM `Function` WHERE Id=@Id;";
            return Connection.Query<Function>(sql, new { Id = key }).SingleOrDefault();
        }
    }
}