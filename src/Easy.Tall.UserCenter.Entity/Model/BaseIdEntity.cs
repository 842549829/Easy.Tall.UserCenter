using Easy.Tall.UserCenter.Entity.Interface;

namespace Easy.Tall.UserCenter.Entity.Model
{
    /// <summary>
    /// Id基础类
    /// </summary>
    public abstract class BaseIdEntity: IBaseIdEntity
    {
        /// <inheritdoc />
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }
    }
}