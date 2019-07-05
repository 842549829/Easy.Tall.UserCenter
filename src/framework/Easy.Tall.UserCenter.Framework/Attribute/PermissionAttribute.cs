using System;

namespace Easy.Tall.UserCenter.Framework.Attribute
{
    /// <summary>
    /// 权限
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PermissionAttribute : System.Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="path">路径</param>
        public PermissionAttribute(string path)
        {
            Path = path;
        }

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }
    }
}