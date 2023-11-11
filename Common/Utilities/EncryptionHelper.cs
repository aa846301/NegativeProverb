using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Utilities;

/// <summary>
/// 密碼相關通用方法
/// </summary>
public class EncryptionHelper
{

    /// <summary> 
    /// SHA1密碼轉換 (不建議使用)
    /// </summary>
    /// <param name="password"></param>
    /// <param name="saltKey"></param>
    /// <returns></returns>
    public static string ComputeHmacSha1(string password, string saltKey)
    {
        using (HMACSHA1 hmac = new HMACSHA1(Encoding.UTF8.GetBytes(saltKey)))
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = hmac.ComputeHash(passwordBytes);

            StringBuilder result = new StringBuilder();

            foreach (byte b in hashBytes)
            {
                result.Append(b.ToString("x2"));
            }

            return result.ToString();
        }
    }

    /// <summary>
    /// SHA256密碼轉換
    /// </summary>
    /// <param name="password"></param>
    /// <param name="saltKey"></param>
    /// <returns></returns>
    public static string ComputeHmacSha256(string password, string saltKey)
    {
        using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(saltKey)))
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = hmac.ComputeHash(passwordBytes);

            StringBuilder result = new StringBuilder();

            foreach (byte b in hashBytes)
            {
                result.Append(b.ToString("x2"));
            }

            return result.ToString();
        }
    }
    /// <summary>
    /// 密碼規則檢查
    /// </summary>
    /// <param name="password">密碼字串</param>
    /// <returns></returns>
    public static bool ValidIsPwd(string password)
    {
        //檢查密碼是否符合規則
        return Regex.IsMatch(password, @"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%^&+=]).*$");
    }

    /// <summary>
    /// 信箱規則檢查
    /// </summary>
    /// <param name="mail"></param>
    /// <returns></returns>
    public static bool ValidIsEMail(string mail)
    {
        //檢查信箱是否符合規則
        return Regex.IsMatch(mail, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
    }
    
}
