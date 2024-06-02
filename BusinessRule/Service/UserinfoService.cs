using Common.Model;
using Common.Utilities;
using DataAccess.BusinessModel.Userinfo;
using DataAccess.ProjectContext;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Net;



namespace BusinessRule.Service
{
    /// <summary>
    /// 使用者操作
    /// </summary>
    public class UserinfoService : BaseService
    {
        private readonly IConfiguration _configuration;
        private readonly MailHelper _mailHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserinfoService(ProjectContext projectContext, IConfiguration configuration, MailHelper mailHelper, IHttpContextAccessor httpContextAccessor) : base(projectContext)
        {
            _configuration = configuration;
            _mailHelper = mailHelper;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 新增使用者帳號
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<BaseModel> CreateUserAccount(CreateUserAccountInput input)
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

            //檢查帳號或信箱是否已存在
            var userAccount = await _db.User_UserAccount.Where(x => x.U_Account == input.U_Account || x.U_EMail.ToUpper() == input.U_EMail.ToUpper()).FirstOrDefaultAsync();
            if (userAccount != null)
            {
                result.Success = false;
                result.Code = ((int)HttpStatusCode.BadRequest).ToString();
                result.Exception = "已有相同的帳號或信箱";
                return result;
            }

            //密碼
            if (!EncryptionHelper.ValidIsPwd(input.U_Pwd))
            {
                result.Success = false;
                result.Code = ((int)HttpStatusCode.BadRequest).ToString();
                result.Exception = "密碼必須包含大小寫英文字母、數字、特殊符號，且長度必須大於8";
                return result;
            }
            //檢查信箱格式
            if (!EncryptionHelper.ValidIsEMail(input.U_EMail))
            {
                result.Success = false;
                result.Code = ((int)HttpStatusCode.BadRequest).ToString();
                result.Exception = "信箱格式不正確";
                return result;
            }

            var newUserAccount = new User_UserAccount
            {
                U_UUID = Guid.NewGuid(),
                U_Account = input.U_Account,
                U_Pwd = EncryptionHelper.ComputeHmacSha256(input.U_Pwd, _configuration.GetValue<string>("PwdSetting:SaltKey")),
                U_Name = input.U_Name,
                U_EMail = input.U_EMail,
                U_Tel = input.U_Tel,
                U_Verify = false,
                U_VerifyCode = string.Empty,
                Creator = input.UserID,
                CreateTime = DateTime.Now
            };
            _db.User_UserAccount.Add(newUserAccount);

            var flag = await _db.SaveChangesAsync();
            if (flag <= 0)
            {

                result.Success = false;
                result.Code = ((int)HttpStatusCode.BadRequest).ToString();
                result.Exception = "新增失敗";
                return result;
            }
            await SendVerifyMail(input.U_Account, input.U_EMail);

            return result;
        }

        /// <summary>
        /// 寄會員驗證信
        /// </summary>
        /// <param name="userAccount">會員帳號</param>
        /// <param name="userMail">會員信箱</param>
        /// <returns></returns>
        public async Task<BaseModel> SendVerifyMail(string userAccount, string userMail)
        {
            var result = new BaseModel()
            {
                Success = true,
                Code = ((int)HttpStatusCode.OK).ToString(),
            };
            if (string.IsNullOrEmpty(userAccount))
            {
                result.Success = false;
                result.Code = ((int)HttpStatusCode.BadRequest).ToString();
                result.Exception = "不正確的輸入";
                return result;
            }
            var U_UserAccount = await _db.User_UserAccount.FirstOrDefaultAsync(x => x.U_Account == userAccount);
            if (U_UserAccount.U_Verify == true)
            {
                result.Success = false;
                result.Code = ((int)HttpStatusCode.BadRequest).ToString();
                result.Exception = "此帳號已通過驗證";
                return result;
            }
            //產生隨機驗證碼
            string U_VerifyCode = RandomHelper.GenerateRandomString(50);
            string domainName = _httpContextAccessor.HttpContext.Request.Host.Value;
            string MailBody =
                $@"<p> 親愛的 {userAccount}會員</p>" +
                $@"<p> 請點擊以下連結進行會員驗證：</ p > " +
                $@"<a href=""https://{domainName}"">點擊這裡進行驗證 </a>" +
                $@"<p>如果以上連結無法正常點擊，請複製以下連結到瀏覽器中進行：</p>" +
                $@"<p>https://{domainName}/api/userinfo/verifyuserinfo?UserAccount={userAccount}&U_VerifyCode={U_VerifyCode}</p>" +
                $@"<p>感謝您的註冊！</p>";
            //https://localhost:44393/api/userinfo/verifyuserinfo?UserAccount=dino753159&U_VerifyCode=ljGHHXEjxlstJaFuKoXQMCHhjlKbtVYbhlIuAQiXZPMNhTSUpN
            U_UserAccount.U_VerifyCode = U_VerifyCode;
            var flag = await _db.SaveChangesAsync();
            if (flag <= 0)
            {
                result.Success = false;
                result.Code = ((int)HttpStatusCode.BadRequest).ToString();
                result.Exception = "新增失敗";
                return result;
            }

            //寄出驗證信
            await _mailHelper.SendMail(userMail, "負能量語錄驗證信", MailBody);

            return result;

        }

        /// <summary>
        /// 驗證會員帳號
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<BaseModel> VerifyUserinfo(VerifyUserAccountInput input)
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
            //檢查帳號存在、並未被驗證
            var userAccount = await _db.User_UserAccount.Where(x => x.U_Account == input.UserAccount).FirstOrDefaultAsync();
            if (userAccount == null)
            {
                result.Success = false;
                result.Code = ((int)HttpStatusCode.BadRequest).ToString();
                result.Exception = "帳號不存在";
                return result;
            }
            if (userAccount.U_Verify == true)
            {
                result.Success = false;
                result.Code = ((int)HttpStatusCode.BadRequest).ToString();
                result.Exception = "帳號已被驗證過";
                return result;
            }
            //檢查驗證碼
            if (userAccount.U_VerifyCode != input.U_VerifyCode)
            {
                result.Success = false;
                result.Code = ((int)HttpStatusCode.BadRequest).ToString();
                result.Exception = "會員驗證碼不正確";
                return result;
            }

            userAccount.U_VerifyCode = string.Empty;
            userAccount.U_Verify = true;
            var flag = await _db.SaveChangesAsync();
            if (flag <= 0)
            {
                result.Success = false;
                result.Code = ((int)HttpStatusCode.BadRequest).ToString();
                result.Exception = "會員驗證失敗";
                return result;
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

            if (!string.IsNullOrEmpty(input.U_EMail))
            {
                var oldUserAccount = await _db.User_UserAccount.Where(x => x.U_EMail == input.U_EMail).FirstOrDefaultAsync();
                if (oldUserAccount != null && oldUserAccount.U_UUID != input.U_UUID)
                {
                    result.Success = false;
                    result.Code = ((int)HttpStatusCode.BadRequest).ToString();
                    result.Exception = "已有相同的信箱";
                    return result;
                }
                //檢查信箱格式
                if (!EncryptionHelper.ValidIsEMail(input.U_EMail))
                {
                    result.Code = ((int)HttpStatusCode.BadRequest).ToString();
                    result.Exception = "信箱格式不正確";
                    return result;
                }
                else
                {
                    userAccount.U_EMail = input.U_EMail;
                }

            }

            if (!string.IsNullOrEmpty(input.U_Pwd) && EncryptionHelper.ValidIsPwd(input.U_Pwd))
            {
                userAccount.U_Pwd = EncryptionHelper.ComputeHmacSha256(input.U_Pwd, _configuration.GetValue<string>("PwdSetting:SaltKey"));
            }

            userAccount.U_Name = !string.IsNullOrEmpty(input.U_Name) ? input.U_Name : userAccount.U_Name;
            userAccount.U_Tel = !string.IsNullOrEmpty(input.U_Tel) ? input.U_Tel : userAccount.U_Tel;
            userAccount.Updator = input.UserID;
            userAccount.UpdateTime = DateTime.Now;

            var flag = await _db.SaveChangesAsync();
            if (flag <= 0)
            {
                result.Success = false;
                result.Code = ((int)HttpStatusCode.BadRequest).ToString();
                result.Exception = "更新失敗";
                return result;
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
            var userPostList = await _db.User_UserPost.Where(x => x.U_UUID == input.U_UUID).ToListAsync();
            if (userPostList != null && userPostList.Any())
            {
                _db.User_UserPost.RemoveRange(userPostList);
            }

            _db.User_UserAccount.Remove(userAccount);

            var flag = await _db.SaveChangesAsync();
            if (flag <= 0)
            {

                result.Success = false;
                result.Code = ((int)HttpStatusCode.BadRequest).ToString();
                result.Exception = "刪除失敗";
            }


            return result;
        }



    }
}
