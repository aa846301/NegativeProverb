using BusinessRule.Service;
using Common.Model;
using DataAccess.BusinessModel.UserClock;
using Microsoft.AspNetCore.Mvc;

namespace NegativeProverb.Controllers
{
    /// <summary>
    /// 用戶打卡相關操作
    /// </summary>
    public class UserClockController : BaseController
    {
        private readonly UserClockService _userClockService;

        public UserClockController(UserClockService userClockService)
        {
            _userClockService = userClockService;
        }

        /// <summary>
        /// 使用者打卡
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UserClockIn")]
        public async Task<BaseModel> UserClockIn(UserClockInInput input) => await _userClockService.UserClockIn(input);

        /// <summary>
        /// 取得使用者打卡列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetUserPost")]
        public async Task<BaseModel<GetUserPostView>> GetUserPost(GetUserPostInput input) => await _userClockService.GetUserPost(input);
    }
}
