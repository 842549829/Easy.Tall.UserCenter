namespace Easy.Tall.UserCenter.Framework.Db
{
    /// <summary>
    /// Db工厂
    /// </summary>
    public interface IDbUnitOfWorkFactory
    {
        /// <summary>
        /// 工作单元
        /// </summary>
        /// <param name="name">链接名称</param>
        /// <returns>工作单元</returns>
        IUnitOfWork CreateUnitOfWork(string name);
    }
}