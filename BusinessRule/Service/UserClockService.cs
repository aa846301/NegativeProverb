using Common.Model;
using DataAccess.BusinessModel.UserClock;
using DataAccess.ProjectContext;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BusinessRule.Service;

public class UserClockService : BaseService
{
    private readonly ProjectContext _db;
    public UserClockService(ProjectContext projectContext) : base(projectContext)
    {
        _db = projectContext;
    }

    public async Task<BaseModel> UserClockIn(UserClockInInput input)
    {
        var result = new BaseModel()
        {
            Code = ((int)HttpStatusCode.OK).ToString(),
            Success = true
        };

        var user = await _db.User_UserAccount.AsQueryable().AsNoTracking().FirstOrDefaultAsync(x => x.U_UUID == input.U_UUID);
        if (user == null)
        {
            result.Code = ((int)HttpStatusCode.BadRequest).ToString();
            result.Success = false;
            return result;
        }

        //檢查今天是否打過卡



        await _db.SaveChangesAsync();
        return result;
    }
}
