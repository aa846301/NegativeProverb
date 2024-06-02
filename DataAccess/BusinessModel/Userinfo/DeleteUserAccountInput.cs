using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.BusinessModel.Userinfo
{
    public class DeleteUserAccountInput
    {

        public Guid U_UUID { get; set; }

        public string UserID { get; set; }
    }
}
