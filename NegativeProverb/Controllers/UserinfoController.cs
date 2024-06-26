﻿using BusinessRule.Service;
using Common.Model;
using DataAccess.BusinessModel.Userinfo;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

namespace NegativeProverb.Controllers
{
    /// <summary>
    /// 使用者操作
    /// </summary>
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
        /// 寄會員驗證信
        /// </summary>
        /// <param name="userAccount">會員帳號</param>
        /// <param name="userMail">會員信箱</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SendVerifyMail")]
        public async Task<BaseModel> SendVerifyMail(string userAccount, string userMail) => await _userinfoService.SendVerifyMail(userAccount, userMail);

        /// <summary>
        /// 驗證會員帳號
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("VerifyUserinfo")]
        public async Task<BaseModel> VerifyUserinfo([FromQuery] VerifyUserAccountInput input) => await _userinfoService.VerifyUserinfo(input);

        /// <summary>
        /// 取得使用者帳號列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GetUserAccount")]
        public async Task<BaseModel<GetUserAccountView>> GetUserAccount(GetUserAccountInput input) => await _userinfoService.GetUserAccount(input);

        /// <summary>
        /// 修改使用者帳號資訊
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateUserAccount")]
        public async Task<BaseModel> UpdateUserAccount(UpdateUserAccountInput input) => await _userinfoService.UpdateUserAccount(input);


        /// <summary>
        /// 刪除使用者帳號
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeleteUserAccount")]
        public async Task<BaseModel> DeleteUserAccount(DeleteUserAccountInput input) => await _userinfoService.DeleteUserAccount(input);


    }
}
