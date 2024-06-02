using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.BusinessModel.Userinfo
{
    public class GetUserAccountInput
    {
        /// <summary>
        /// 使用者帳號 模糊查詢
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 使用者UUID 單筆查詢
        /// </summary>
        public Guid? UesrUUID { get; set; }

        /// <summary>
        /// Mail 模糊查詢
        /// </summary>
        public string EMail { get; set; }

        /// <summary>
        /// 電話 模糊查詢
        /// </summary>
        public string Tel { get; set; }

    }
}
