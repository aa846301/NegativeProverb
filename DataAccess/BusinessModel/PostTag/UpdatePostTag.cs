using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.BusinessModel.PostTag
{
    public class UpdatePostTag
    {
        /// <summary>
        /// 語錄標籤UUID
        /// </summary>
        public Guid PT_UUID { get; set; }

        /// <summary>
        /// 語錄標籤
        /// </summary>
        public string PostTag { get; set; }

        /// <summary>
        /// 操作者
        /// </summary>
        public string UserID { get; set; }
    }
}
