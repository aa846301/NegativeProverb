using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.BusinessModel.Userinfo
{
    /// <summary>
    /// 驗證使用者帳號 輸入
    /// </summary>
    public class VerifyUserAccountInput
    {
        /// <summary>
        /// 使用者帳戶
        /// </summary>
        public string UserAccount {  get; set; }


        /// <summary>
        /// 使用者驗證碼
        /// </summary>
        public string U_VerifyCode { get; set; }
    }
}
