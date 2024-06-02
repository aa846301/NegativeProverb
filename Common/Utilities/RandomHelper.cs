

using System.Security.Cryptography;
using System.Text;


namespace Common.Utilities
{
    /// <summary>
    /// 公用隨機方法
    /// </summary>
    public class RandomHelper
    {
        /// <summary> 
        /// 通用隨機文字(大小寫英文數字符號)
        /// </summary>
        /// <param name="length">字串長度</param>
        /// <returns></returns>
        public static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
            var random = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                random.Append(chars[RandomNumberGenerator.GetInt32(length)]);
            }
           
            return random.ToString();
        }
    }
}
