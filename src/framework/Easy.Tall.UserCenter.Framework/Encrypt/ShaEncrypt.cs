using System.Security.Cryptography;

namespace Easy.Tall.UserCenter.Framework.Encrypt
{
    /// <summary>
    /// SHA加密
    /// </summary>
    public class ShaEncrypt
    {
        /// <summary>
        /// 获取字符串的sha1哈希序列
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>哈希序列</returns>
        public static string SHA1Hash(string str)
        {
            return Cryptography.Hash(SHA1.Create(), str);
        }

        /// <summary>
        /// 获取字符串集合的sha1哈希序列
        /// </summary>
        /// <param name="strings">字符串集合</param>
        /// <returns>sha1哈希序列</returns>
        public static string SHA1Hash(params string[] strings)
        {
            return Cryptography.Hash(SHA1.Create(), strings);
        }

        /// <summary>
        /// 获取字符串的sha256哈希序列
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>sha256哈希序列</returns>
        public static string SHA256Hash(string str)
        {
            return Cryptography.Hash(SHA256.Create(), str);
        }

        /// <summary>
        /// 获取字符串集合的sha256哈希序列
        /// </summary>
        /// <param name="strings">字符串集合</param>
        /// <returns>sha256哈希序列</returns>
        public static string SHA256Hash(params string[] strings)
        {
            return Cryptography.Hash(SHA256.Create(), strings);
        }

        /// <summary>
        /// 获取字符串的sha384哈希序列
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>sha384哈希序列</returns>
        public static string SHA384Hash(string str)
        {
            return Cryptography.Hash(SHA384.Create(), str);
        }

        /// <summary>
        /// 获取字符串集合的sha384哈希序列
        /// </summary>
        /// <param name="strings">字符串集合</param>
        /// <returns>sha384哈希序列</returns>
        public static string SHA384Hash(params string[] strings)
        {
            return Cryptography.Hash(SHA384.Create(), strings);
        }

        /// <summary>
        /// 获取字符串的sha512哈希序列
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>sha512哈希序列</returns>
        public static string SHA512Hash(string str)
        {
            return Cryptography.Hash(SHA512.Create(), str);
        }

        /// <summary>
        /// 获取字符串集合的sha384哈希序列
        /// </summary>
        /// <param name="strings">字符串集合</param>
        /// <returns>sha512哈希序列</returns>
        public static string SHA512Hash(params string[] strings)
        {
            return Cryptography.Hash(SHA512.Create(), strings);
        }
    }
}