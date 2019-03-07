namespace Easy.Tall.UserCenter.Framework.Db
{
    /// <summary>
    /// 仓储接口(CRUD)
    /// </summary>
    /// <typeparam name="TKey">主键</typeparam>
    /// <typeparam name="TValue">实体</typeparam>
    public interface IRepository<in TKey, TValue> : IAddRepository<TValue>, IRemoveRepository<TValue>, IUpdateRepository<TValue>, IQueryRepository<TKey, TValue>
    {
    }
}