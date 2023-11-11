using DataAccess.BusinessModel.PostTag;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.BusinessModel.UserClock
{
    public class GetUserPostOutput
    {
        /// <summary>
        /// 使用者UUID
        /// </summary>
        public Guid U_UUID { get; set; }

        /// <summary>
        /// 語錄UUID
        /// </summary>
        public Guid P_UUID { get; set; }

        /// <summary>
        /// 語錄內容
        /// </summary>
        public string P_Post { get; set; }

        /// <summary>
        /// 打卡時間
        /// </summary>
        public DateTime? ClockTime { get; set; }

        /// <summary>
        /// 語錄標籤列表
        /// </summary>
        public List<GetPostTagOutput> PostTagList { get; set; }

    }
}
