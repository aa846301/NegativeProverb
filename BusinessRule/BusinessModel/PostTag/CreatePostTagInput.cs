using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessRule.BusinessModel.Post
{
    public class CreatePostTagInput
    {
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
