using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.BusinessModel.Post
{
    public class DeletePostInput
    {
        /// <summary>
        /// 負能量語錄UUID
        /// </summary>
        public Guid PostUUID { get; set; }

        /// <summary>
        /// 操作者
        /// </summary>
        public string UserID { get; set; }
    }
}
