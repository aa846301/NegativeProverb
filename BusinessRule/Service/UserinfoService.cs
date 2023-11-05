using BusinessRule.BusinessModel.Userinfo;
using Common.Model;
using DataAccess.ProjectContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessRule.Service
{
    /// <summary>
    /// 使用者操作
    /// </summary>
    public class UserinfoService : BaseService
    {

        public UserinfoService(ProjectContext projectContext) : base(projectContext)
        {

        }

        /// <summary>
        /// 新增使用者帳號
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<BaseModel> CreateUserAccount(CreateUserAccountInput input)
        {
            var result = new BaseModel();
            if (input == null)
            {
                result.Success = false;
                result.Code = HttpStatusCode.BadRequest.ToString("D3");
                result.Exception = "不正確的輸入";
                return result;
            }

            var newUserAccount = new User_UserAccount
            {
                U_UUID = Guid.NewGuid(),
                U_Account = input.U_Account,
                U_Pwd = input.U_Pwd,
                U_Name = input.U_Name,
                U_EMail = input.U_EMail,
                U_Tel = input.U_Tel,
                Creator = input.UserID,
                CreateTime = DateTime.Now
            };
            _db.User_UserAccount.Add(newUserAccount);
            var flag = await _db.SaveChangesAsync();
            if (flag > 0)
            {
                result.Success = true;
                result.Code = HttpStatusCode.OK.ToString("D3");
                result.Exception = "新增成功";
            }
            else
            {
                result.Success = false;
                result.Code = HttpStatusCode.BadRequest.ToString("D3");
                result.Exception = "新增失敗";
            }


            return result;
        }
    }
}
