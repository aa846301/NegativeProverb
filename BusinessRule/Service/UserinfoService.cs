using Common.Model;
using DataAccess.BusinessModel.Userinfo;
using DataAccess.ProjectContext;
using Microsoft.EntityFrameworkCore;
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
                result.Code = ((int)HttpStatusCode.BadRequest).ToString();
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
                result.Code = ((int)HttpStatusCode.OK).ToString();
            }
            else
            {
                result.Success = false;
                result.Code = ((int)HttpStatusCode.BadRequest).ToString();
                result.Exception = "新增失敗";
            }


            return result;
        }


        /// <summary>
        /// 取得使用者帳號列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<BaseModel<GetUserAccountView>> GetUserAccount(GetUserAccountInput input)
        {
            var result = new BaseModel<GetUserAccountView>()
            {
                Success = true,
                Data = new GetUserAccountView()
                {
                    UserAccountList = new List<GetUserAccoutOutput>()
                }
            };
            if (input == null)
            {
                result.Success = false;
                result.Code = ((int)HttpStatusCode.BadRequest).ToString();
                result.Exception = "不正確的輸入";
                return result;
            }

            var accountQuery = _db.User_UserAccount.AsQueryable().AsNoTracking();
            if (!string.IsNullOrEmpty(input.Account))
            {
                accountQuery = accountQuery.Where(x => x.U_Account.Contains(input.Account));
            }
            if (!string.IsNullOrEmpty(input.EMail))
            {
                accountQuery = accountQuery.Where(x => x.U_EMail.Contains(input.EMail));
            }
            if (!string.IsNullOrEmpty(input.Tel))
            {
                accountQuery = accountQuery.Where(x => x.U_Tel.Contains(input.Tel));
            }
            if (input.UesrUUID != null)
            {
                accountQuery = accountQuery.Where(x => x.U_UUID == input.UesrUUID);
            }
            var accountList = await accountQuery.ToListAsync();

            result.Data.UserAccountList = accountList.OrderBy(x => x.U_Sort).Select(x => new GetUserAccoutOutput
            {
                U_UUID = x.U_UUID,
                U_Account = x.U_Account,
                U_EMail = x.U_EMail,
                U_Name = x.U_Name,
                U_Tel = x.U_Tel
            }).ToList();

            return result;
        }

        /// <summary>
        /// 修改使用者帳號資訊
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<BaseModel> UpdateUserAccount(UpdateUserAccountInput input)
        {
            var result = new BaseModel();
            if (input == null)
            {
                result.Success = false;
                result.Code = ((int)HttpStatusCode.BadRequest).ToString();
                result.Exception = "不正確的輸入";
                return result;
            }

            var userAccount = await _db.User_UserAccount.FirstOrDefaultAsync(x => x.U_UUID == input.U_UUID);
            if (userAccount == null)
            {
                result.Success = false;
                result.Code = ((int)HttpStatusCode.BadRequest).ToString();
                result.Exception = "找不到使用者帳號";
                return result;
            }

            userAccount.U_Pwd = !string.IsNullOrEmpty(input.U_Pwd) ? input.U_Pwd : userAccount.U_Pwd;
            userAccount.U_Name = !string.IsNullOrEmpty(input.U_Name) ? input.U_Name : userAccount.U_Name;
            userAccount.U_EMail = !string.IsNullOrEmpty(input.U_EMail) ? input.U_EMail : userAccount.U_EMail;
            userAccount.U_Tel = !string.IsNullOrEmpty(input.U_Tel) ? input.U_Tel : userAccount.U_Tel;
            userAccount.Updator = input.UserID;
            userAccount.UpdateTime = DateTime.Now;

            var flag = await _db.SaveChangesAsync();
            if (flag > 0)
            {
                result.Success = true;
                result.Code = ((int)HttpStatusCode.OK).ToString();
            }
            else
            {
                result.Success = false;
                result.Code = ((int)HttpStatusCode.BadRequest).ToString();
                result.Exception = "更新失敗";
            }
            return result;
        }

        /// <summary>
        /// 刪除使用者帳號
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<BaseModel> DeleteUserAccount(DeleteUserAccountInput input)
        {
            var result = new BaseModel()
            {
                Success = true,
                Code = ((int)HttpStatusCode.OK).ToString(),
            };
            if (input == null)
            {
                result.Success = false;
                result.Code = ((int)HttpStatusCode.BadRequest).ToString();
                result.Exception = "不正確的輸入";
                return result;
            }
            var userAccount = await _db.User_UserAccount.FirstOrDefaultAsync(x => x.U_UUID == input.U_UUID);
            if (userAccount == null)
            {
                result.Success = false;
                result.Code = ((int)HttpStatusCode.BadRequest).ToString();
                result.Exception = "找不到使用者帳號";
                return result;
            }

            _db.User_UserAccount.Remove(userAccount);

            var flag = await _db.SaveChangesAsync();
            if (flag > 0)
            {
                result.Success = true;
                result.Code = ((int)HttpStatusCode.OK).ToString();
            }
            else
            {
                result.Success = false;
                result.Code = ((int)HttpStatusCode.BadRequest).ToString();
                result.Exception = "刪除失敗";
            }

            return result;
        }



    }
}
