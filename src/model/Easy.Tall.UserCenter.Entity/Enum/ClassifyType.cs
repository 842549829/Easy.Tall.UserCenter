﻿using System.ComponentModel;

namespace Easy.Tall.UserCenter.Entity.Enum
{
    /// <summary>
    /// 分类类型
    /// </summary>
    public enum ClassifyType
    {
        /// <summary>
        /// 角色分类
        /// </summary>
        [Description("角色分类")]
        Role,

        /// <summary>
        /// 权限分类
        /// </summary>
        [Description("权限分类")]
        Permission
    }
}