using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.BusinessModel.UserClock
{
    public class GetUserPostView
    {
        public List<GetUserPostOutput> UserPostList { get; set; } = new List<GetUserPostOutput>();
    }
}
