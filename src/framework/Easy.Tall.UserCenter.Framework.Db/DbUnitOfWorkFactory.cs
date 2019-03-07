namespace Easy.Tall.UserCenter.Framework.Db
{
    /// <summary>
    /// 默认DB链接实现
    /// </summary>
    public class DbUnitOfWorkFactory : IDbUnitOfWorkFactory
    {
        /// <summary>
        /// 数据链接工厂
        /// </summary>
        private readonly IDbConnectionFactory _dbConnectionFactory;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dbConnectionFactory">数据链接工厂</param>
        public DbUnitOfWorkFactory(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        /// <summary>
        /// 工作单元
        /// </summary>
        /// <param name="name">链接名称</param>
        /// <returns>工作单元</returns>
        public IUnitOfWork CreateUnitOfWork(string name)
        {
            return new UnitOfWork(_dbConnectionFactory.CreateDbConnection(name));
        }
    }
}