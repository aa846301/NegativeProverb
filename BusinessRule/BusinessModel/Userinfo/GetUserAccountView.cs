using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessRule.BusinessModel.Userinfo
{
    public class GetUserAccountView
    {
        public List<GetUserAccoutOutput> UserAccountList { get; set; } = new List<GetUserAccoutOutput>();
    }
}
