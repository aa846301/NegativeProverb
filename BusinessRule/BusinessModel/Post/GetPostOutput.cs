using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessRule.BusinessModel.Post
{
    public class GetPostOutput
    {
        /// <summary>
        /// 語錄UUID
        /// </summary>
        public Guid PostUUID { get; set; }

        /// <summary>
        /// 貼文內容
        /// </summary>
        public string Post { get; set; }

        /// <summary>
        /// 標籤列表
        /// </summary>
        public List<PostTagOutput> PostTagList { get; set; }
    }

    public class PostTagOutput
    {
        /// <summary>
        /// 語錄標籤UUID
        /// </summary>
        public Guid PostTagUUID { get; set; }

        /// <summary>
        /// 標籤中文名稱
        /// </summary>
        public string PostTag { get; set; }

    }
}
