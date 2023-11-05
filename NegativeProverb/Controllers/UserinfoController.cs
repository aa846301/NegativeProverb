using BusinessRule.BusinessModel.Userinfo;
using BusinessRule.Service;
using Common.Model;
using Microsoft.AspNetCore.Mvc;
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

        /// <summary>
        /// 新增使用者帳號
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateUserAccount")]
        public async Task<BaseModel> CreateUserAccount(CreateUserAccountInput input) => await _userinfoService.CreateUserAccount(input);


        /// <summary>
        /// 取得使用者帳號列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetUserAccount")]
        public async Task<BaseModel<GetUserAccountView>> GetUserAccount(GetUserAccountInput input) => await _userinfoService.GetUserAccount(input);

    }
}
