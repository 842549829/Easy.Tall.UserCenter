namespace Easy.Tall.UserCenter.Entity.Extend
{
    /// <summary>
    /// 结果
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class Result<T>
    {
        /// <summary>
        /// 错误代码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 异常消息
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }
    }
}