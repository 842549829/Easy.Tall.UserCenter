using System.Security.Cryptography;

namespace Easy.Tall.UserCenter.Framework.Encrypt
{
    /// <summary>
    /// MD5加密
    /// </summary>
    public class MD5Encrypt
    {
        /// <summary>
        /// 获取字符串的MD5哈希序列
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>MD5哈希序列</returns>
        public static string Encrypt(string str)
        {
            return Cryptography.Hash(MD5.Create(), str);
        }

        /// <summary>
        /// 获取字符串集合的md5哈希序列
        /// </summary>
        /// <param name="str">字符串集合</param>
        /// <returns>md5哈希序列</returns>
        public static string Encrypt(params string[] str)
        {
            return Cryptography.Hash(MD5.Create(), str);
        }
    }
}