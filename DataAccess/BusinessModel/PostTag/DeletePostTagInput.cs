using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.BusinessModel.PostTag
{
    /// <summary>
    /// 刪除語錄標籤 輸入
    /// </summary>
    public class DeletePostTagInput
    {
        /// <summary>
        /// 語錄標籤UUID
        /// </summary>
        public Guid PT_UUID { get; set; }

        /// <summary>
        /// 操作者
        /// </summary>
        public string UserID { get; set; }

    }
}
