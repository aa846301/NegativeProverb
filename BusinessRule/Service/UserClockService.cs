using Common.Model;
using DataAccess.BusinessModel.PostTag;
using DataAccess.BusinessModel.UserClock;
using DataAccess.ProjectContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Net;

namespace BusinessRule.Service;

public class UserClockService : BaseService
{
    private readonly ProjectContext _db;
    public UserClockService(ProjectContext projectContext) : base(projectContext)
    {
        _db = projectContext;
    }

    /// <summary>
    /// 用戶打卡
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
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
        var userClockList = await _db.User_UserPost.AsQueryable().AsNoTracking().Where(x => x.U_UUID == input.U_UUID).ToListAsync();
        if (userClockList.Exists(x => x.CreateTime.Value.Day == DateTime.Now.Day))
        {
            result.Code = ((int)HttpStatusCode.BadRequest).ToString();
            result.Success = false;
            result.Exception = "今天已打卡";
            return result;
        }

        //查詢哪些語錄 還沒被使用者取得
        var userPostList = userClockList.Select(x => x.P_UUID).ToList();
        var postList = await _db.Post_Post.AsQueryable().AsNoTracking().Where(x => !userPostList.Contains(x.P_UUID)).ToListAsync();
        if (postList == null)
        {
            result.Code = ((int)HttpStatusCode.BadRequest).ToString();
            result.Success = false;
            result.Exception = "已經沒有語錄可以給你打卡囉!";
            return result;
        }


        Random random = new Random();
        var postUUID = postList.OrderBy(x => random.Next()).FirstOrDefault().P_UUID;
        if (postUUID == null || postUUID == Guid.Empty)
        {
            result.Code = ((int)HttpStatusCode.BadRequest).ToString();
            result.Success = false;
            result.Exception = "已經沒有語錄可以給你打卡囉!";
            return result;
        }
        //建立打卡紀錄
        var newUserClock = new User_UserPost()
        {
            UP_UUID = Guid.NewGuid(),
            U_UUID = input.U_UUID,
            P_UUID = postUUID,
            Creator = input.UserID,
            CreateTime = DateTime.Now
        };
        _db.User_UserPost.Add(newUserClock);
        var flag = await _db.SaveChangesAsync();
        if (flag <= 0)
        {
            result.Code = ((int)HttpStatusCode.BadRequest).ToString();
            result.Success = false;
            result.Exception = "打卡失敗";
            return result;
        }

        return result;
    }

    /// <summary>
    /// 查詢用戶打卡紀錄
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<BaseModel<GetUserPostView>> GetUserPost(GetUserPostInput input)
    {
        var result = new BaseModel<GetUserPostView>()
        {
            Data = new GetUserPostView()
            {
                UserPostList = new List<GetUserPostOutput>()
            },
            Code = ((int)HttpStatusCode.OK).ToString(),
            Success = true
        };

        var userPostList = await _db.User_UserPost.AsQueryable().AsNoTracking().Include(x => x.P_UU).ThenInclude(x => x.Post_PostTag).ThenInclude(x => x.PT_UU).Where(x => x.U_UUID == input.U_UUID).OrderByDescending(x => x.CreateTime).ToListAsync();

        var userPostOutputList = userPostList.Select(x => new GetUserPostOutput()
        {
            U_UUID = x.U_UUID,
            P_UUID = x.P_UUID,
            P_Post = x.P_UU.P_Post,
            PostTagList = x.P_UU.Post_PostTag.Select(y => new GetPostTagOutput()
            {
                PT_UUID = y.PT_UUID,
                PostTag = y.PT_UU.PT_Tag
            }).ToList(),
            ClockTime = x.CreateTime
        }).ToList();
        result.Data.UserPostList = userPostOutputList;


        return result;
    }
}
