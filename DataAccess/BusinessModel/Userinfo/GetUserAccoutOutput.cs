using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.BusinessModel.Userinfo;

public class GetUserAccoutOutput
{
    /// <summary>
    /// 使用者UUID
    /// </summary>
    public Guid U_UUID { get; set; }

    /// <summary>
    /// 帳號
    /// </summary>
    public string U_Account { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string U_Name { get; set; }

    /// <summary>
    /// 電子信箱
    /// </summary>
    public string U_EMail { get; set; }

    /// <summary>
    /// 電話
    /// </summary>
    public string U_Tel { get; set; }

}
