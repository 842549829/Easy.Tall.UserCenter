using System.ComponentModel;

namespace Easy.Tall.UserCenter.Entity.Enum
{
    /// <summary>
    /// 权限类型
    /// </summary>
    public enum PermissionType
    {
        /// <summary>
        /// 菜单
        /// </summary>
        [Description("菜单")]
        Menu,

        /// <summary>
        /// 按钮
        /// </summary>
        [Description("按钮")]
        Button
    }
}