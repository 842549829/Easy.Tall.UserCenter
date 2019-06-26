namespace Rabbit.Rpc.Transport.Codec.Implementation
{
    /// <inheritdoc />
    /// <summary>
    /// JSON传输消息编解码工厂
    /// </summary>
    public class JsonTransportMessageCodecFactory : ITransportMessageCodecFactory
    {
        #region Field

        /// <summary>
        /// 创建一个JSON编码器
        /// </summary>
        private readonly ITransportMessageEncoder _transportMessageEncoder = new JsonTransportMessageEncoder();

        /// <summary>
        /// 创建一个JSON解码器
        /// </summary>
        private readonly ITransportMessageDecoder _transportMessageDecoder = new JsonTransportMessageDecoder();

        #endregion Field

        #region Implementation of ITransportMessageCodecFactory

        /// <inheritdoc />
        /// <summary>
        /// 获取编码器。
        /// </summary>
        /// <returns>编码器实例。</returns>
        public ITransportMessageEncoder GetEncoder()
        {
            return _transportMessageEncoder;
        }

        /// <inheritdoc />
        /// <summary>
        /// 获取解码器。
        /// </summary>
        /// <returns>解码器实例。</returns>
        public ITransportMessageDecoder GetDecoder()
        {
            return _transportMessageDecoder;
        }

        #endregion Implementation of ITransportMessageCodecFactory
    }
}