using System;

namespace Easy.Tall.UserCenter.Framework.Encrypt
{
    /// <summary>Base64</summary>
    public class Base64Encrypt
    {
        /// <summary>DecodeBase64</summary>
        /// <param name="str">str</param>
        /// <returns>编码串</returns>
        public static byte[] DecodeBase64(string str)
        {
            str = str.Trim().Replace("%", "").Replace(",", "").Replace(" ", "+");
            int num = 4 - (str.Length % 4 == 0 ? 4 : str.Length % 4);
            for (int index = 0; index < num; ++index)
            {
                str += "=";
            }
            return Convert.FromBase64String(str);
        }
    }
}