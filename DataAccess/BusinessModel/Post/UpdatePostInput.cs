using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.BusinessModel.Post
{
    public class UpdatePostInput
    {
        /// <summary>
        /// 語錄UUID
        /// </summary>
        public Guid PostUUID { get; set; }


        /// <summary>
        /// 語錄內容
        /// </summary>
        public string Post { get; set; }

        /// <summary>
        /// 語錄標籤列表
        /// </summary>
        public List<Guid> PostTagList { get; set; }

        /// <summary>
        /// 操作者
        /// </summary>
        public string UserID { get; set; }
    }
}
