using DataAccess.ProjectContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessRule.Service
{

    public class BaseService
    {
        private readonly ProjectContext _db;
        public BaseService(ProjectContext projectContext)
        {
            _db = projectContext;
        }
    }
}
