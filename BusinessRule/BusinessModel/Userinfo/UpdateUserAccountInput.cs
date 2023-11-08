using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessRule.BusinessModel.Userinfo
{
    public class UpdateUserAccountInput
    {
        /// <summary>
        /// 使用者UUID
        /// </summary>
        public Guid U_UUID { get; set; }

        /// <summary>
        /// 使用者密
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

        /// <summary>
        /// 操作者
        /// </summary>
        public string UserID { get; set; }
    }
}
