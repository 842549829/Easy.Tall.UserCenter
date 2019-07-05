using System;
using System.Collections.Generic;
using System.Text;
using Easy.Tall.UserCenter.Entity.Model;

namespace Easy.Tall.UserCenter.Entity.Extend
{
    /// <summary>
    /// 修改角色
    /// </summary>
    public class RoleUpdateRequest : BaseIdEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 分组
        /// </summary>
        public string ClassifyId { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Describe { get; set; }
    }
}