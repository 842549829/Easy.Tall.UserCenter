using System.Security.Cryptography;
using System.Text;

namespace Rabbit.Rpc.Runtime.Client.Address.Resolvers.Implementation.Selectors.Implementation
{
    /// <summary>
    /// HashAlgorithm
    /// </summary>
    public class HashAlgorithm
    {
        /// <summary>
        /// Hash
        /// </summary>
        /// <param name="digest">digest</param>
        /// <param name="nTime">nTime</param>
        /// <returns>long</returns>
        public static long Hash(byte[] digest, int nTime)
        {
            var rv = ((long)(digest[3 + nTime * 4] & 0xFF) << 24)
                     | ((long)(digest[2 + nTime * 4] & 0xFF) << 16)
                     | ((long)(digest[1 + nTime * 4] & 0xFF) << 8)
                     | ((long)digest[0 + nTime * 4] & 0xFF);
            return rv & 0xffffffffL;
        }

        /// <summary>
        /// Get the md5 of the given key.
        /// </summary>
        /// <param name="k">key</param>
        /// <returns>keyBytes</returns>
        public static byte[] ComputeMd5(string k)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                var keyBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(k));
                md5.Clear();
                return keyBytes;
            }
        }
    }
}