using System;

namespace Easy.Tall.UserCenter.WebApi.Attribute
{
    /// <summary>
    /// 权限
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class PermissionAttribute: System.Attribute
    {
        public string Id { get; set; }
    }
}