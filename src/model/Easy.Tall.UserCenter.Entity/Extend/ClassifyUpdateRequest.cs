using Easy.Tall.UserCenter.Entity.Model;

namespace Easy.Tall.UserCenter.Entity.Extend
{
    /// <summary>
    /// 修改分类
    /// </summary>
    public class ClassifyUpdateRequest : BaseIdEntity
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}