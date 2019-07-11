namespace Easy.Tall.UserCenter.Entity.Extend.Options
{
    /// <summary>
    /// SSO配置
    /// </summary>
    public class SsoOptions
    {
        /// <summary>
        /// 发行者
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 接收者
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// 签名密钥
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// token有效时长(单位分钟)
        /// </summary>
        public int Expire { get; set; }

        /// <summary>
        /// 验证token有效时长延长时间(单位分钟)
        /// </summary>
        public int ClockSkew { get; set; }
    }
}