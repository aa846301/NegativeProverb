using BusinessRule.Service;
using NSwag.Annotations;

namespace NegativeProverb.Controllers
{
    /// <summary>
    /// 使用者操作
    /// </summary>
    [OpenApiTags("使用者操作(Userinfo)")]
    public class UserinfoController : BaseController
    {
        private readonly UserinfoService _userinfoService;

        public UserinfoController(UserinfoService userinfoService)
        {
            _userinfoService = userinfoService;
        }


    }
}
