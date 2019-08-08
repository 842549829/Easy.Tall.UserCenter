namespace Easy.Tall.UserCenter.Framework.Data
{
    /// <summary>
    /// 结果
    /// </summary>
    public abstract class Result
    {
        /// <summary>
        /// 代码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }
    }

    /// <summary>
    /// 结果
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    public class Result<T> : Result
    {
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }
    }

    /// <summary>
    /// api结果
    /// </summary>
    public abstract class ApiResult
    {
        /// <summary>
        /// 代码
        /// </summary>
        public int Code { get; set; }
    }

    /// <summary>
    /// 消息结果
    /// </summary>
    public class ApiMsgResult : ApiResult
    {
        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; }
    }

    /// <summary>
    /// 数据结果
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    public class ApiResult<T> : ApiResult
    {
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }
    }
}