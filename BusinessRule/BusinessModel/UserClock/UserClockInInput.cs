using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessRule.BusinessModel.UserClock
{
    public class UserClockInInput
    {
        /// <summary>
        /// 使用者UUID
        /// </summary>
        public Guid U_UUID { get; set; }


        /// <summary>
        /// 操作者
        /// </summary>
        public string UserID { get; set; }
    }
}
