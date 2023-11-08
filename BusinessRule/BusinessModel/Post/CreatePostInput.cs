using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessRule.BusinessModel.Post;

public class CreatePostInput
{
    /// <summary>
    /// 負能量語錄
    /// </summary>
    public string P_Post { get; set; }

    /// <summary>
    /// 語錄標籤列表
    /// </summary>
    public List<Guid> PostTagList { get; set; } 

    /// <summary>
    /// 操作者
    /// </summary>
    public string UserID { get; set; }
}

