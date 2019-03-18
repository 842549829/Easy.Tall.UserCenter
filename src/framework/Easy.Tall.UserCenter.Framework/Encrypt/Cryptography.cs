using System;
using System.Security.Cryptography;
using System.Text;

namespace Easy.Tall.UserCenter.Framework.Encrypt
{
    /// <summary>
    /// 哈希加密签名
    /// </summary>
    public class Cryptography
    {
        /// <summary>
        /// 获取字节数组的哈希
        /// </summary>
        /// <param name="algorithm">algorithm</param>
        /// <param name="bytes">字节数组</param>
        /// <returns>哈希序列</returns>
        public static string Hash(HashAlgorithm algorithm, byte[] bytes)
        {
            using (algorithm)
            {
                var dataHashed = algorithm.ComputeHash(bytes);
                var hash = BitConverter.ToString(dataHashed).Replace("-", "");
                return hash;
            }
        }

        /// <summary>
        /// 获取字符串的哈希
        /// </summary>
        /// <param name="algorithm">哈希算法名称</param>
        /// <param name="str">字符串</param>
        /// <returns>哈希序列</returns>
        public static string Hash(HashAlgorithm algorithm, string str)
        {
            var enc = new ASCIIEncoding();
            var bytes = enc.GetBytes(str);
            return Hash(algorithm, bytes);
        }

        /// <summary>
        /// 获取字符串的哈希
        /// </summary>
        /// <param name="algorithm">哈希算法名称</param>
        /// <param name="str">字符串集合</param>
        /// <returns>哈希序列</returns>
        public static string Hash(HashAlgorithm algorithm, params string[] str)
        {
            var text = string.Join("\n", str);
            return Hash(algorithm, text);
        }
    }
}
