using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.BusinessModel.UserClock
{
    public class GetUserPostInput
    {
        /// <summary>
        /// 用戶UUID
        /// </summary>
        public Guid U_UUID { get; set; }

        /// <summary>
        /// 操作者
        /// </summary>
        public string UserId { get; set; }
    }
}
