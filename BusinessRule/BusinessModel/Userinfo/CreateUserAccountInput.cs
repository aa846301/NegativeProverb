using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessRule.BusinessModel.Userinfo
{
    public class CreateUserAccountInput
    {

        /// <summary>
        /// 帳號
        /// </summary>
        public string U_Account { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        public string U_Pwd { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string U_Name { get; set; }

        /// <summary>
        /// 電子信箱
        /// </summary>
        public string U_EMail { get; set; }

        /// <summary>
        /// 電話
        /// </summary>
        public string U_Tel { get; set; }


        public string UserID { get; set; }
    }
}
