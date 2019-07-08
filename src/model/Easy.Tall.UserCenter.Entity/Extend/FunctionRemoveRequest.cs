using System;
using Easy.Tall.UserCenter.Entity.Model;

namespace Easy.Tall.UserCenter.Entity.Extend
{
    /// <summary>
    /// 功能删除
    /// </summary>
    public class FunctionRemoveRequest : BaseIdEntity
    {
        /// <summary>
        /// 是否全部删除
        /// </summary>
        public bool All { get; set; }
    }
}