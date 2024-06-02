using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.BusinessModel.Post
{
    /// <summary>
    /// 查詢語錄 列表 輸入
    /// </summary>
    public class GetPostInput
    {

        /// <summary>
        /// 負能量語錄UUID
        /// </summary>
        public Guid? PostUUID { get; set; }

        /// <summary>
        /// 貼文查詢 模糊查詢
        /// </summary>
        public string PostKeyWord { get; set; }

        /// <summary>
        /// 標籤列表 多選
        /// </summary>
        public List<Guid> PostTagUUID { get; set; }

        /// <summary>
        /// 標籤 模糊查詢
        /// </summary>
        public string PostTagKeyWord { get; set; }
    }
}
