using Common.Attributes;
using DataAccess.ProjectContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessRule.Service
{

    public class BaseService
    {
        public readonly ProjectContext _db;
        public BaseService(ProjectContext projectContext)
        {
            _db = projectContext;
        }
    }
}
