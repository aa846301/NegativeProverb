using BusinessRule.BusinessModel.Post;
using Common.Model;
using DataAccess.ProjectContext;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BusinessRule.Service;

/// <summary>
/// 負能量語錄管理
/// </summary>
public class PostService : BaseService
{
    public PostService(ProjectContext projectContext) : base(projectContext)
    {
    }

    /// <summary>
    /// 新增負能量語錄
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<BaseModel> CreatePost(CreatePostInput input)
    {
        var result = new BaseModel()
        {
            Code = ((int)HttpStatusCode.OK).ToString(),
            Success = true
        };
        var post = await _db.Post_Post.AsQueryable().AsNoTracking().FirstOrDefaultAsync(x => x.P_Post == input.P_Post);
        if (post != null)
        {
            result.Code = ((int)HttpStatusCode.BadRequest).ToString();
            result.Success = false;
            result.Exception = "已有相同的語錄";
            return result;
        }

        var newPost = new Post_Post()
        {
            P_UUID = Guid.NewGuid(),
            P_Post = input.P_Post,
            Creator = input.UserID,
            CreateTime = DateTime.Now
        };
        _db.Post_Post.Add(newPost);
        var postTagList = input.PostTagList.Select(x => new Post_PostTag()
        {
            PPT_UUID = Guid.NewGuid(),
            PT_UUID = x,
            P_UUID = newPost.P_UUID,
            Creator = input.UserID,
            CreateTime = DateTime.Now
        });
        _db.Post_PostTag.AddRange(postTagList);
        var flag = await _db.SaveChangesAsync();
        if (flag <= 0)
        {
            result.Code = ((int)HttpStatusCode.BadRequest).ToString();
            result.Success = false;
            result.Exception = "新增語錄失敗";
            return result;
        }

        return result;
    }

    #region 語錄標籤管理

    /// <summary>
    /// 新增語錄標籤
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<BaseModel> CreatePostTag(CreatePostTagInput input)
    {
        var result = new BaseModel()
        {
            Code = ((int)HttpStatusCode.OK).ToString(),
            Success = true
        };
        if (await _db.Post_Tag.AsQueryable().AsNoTracking().AnyAsync(x => x.PT_Tag == input.PostTag))
        {
            result.Code = ((int)HttpStatusCode.BadRequest).ToString();
            result.Success = false;
            result.Exception = "已有相同的標籤名稱";
            return result;
        }
        var newPostTag = new Post_Tag()
        {
            PT_UUID = Guid.NewGuid(),
            PT_Tag = input.PostTag,
            Creator = input.UserID,
            CreateTime = DateTime.Now
        };
        _db.Post_Tag.Add(newPostTag);
        var flag = await _db.SaveChangesAsync();
        if (flag <= 0)
        {
            result.Code = ((int)HttpStatusCode.BadRequest).ToString();
            result.Success = false;
            result.Exception = "新增失敗";
            return result;
        }

        return result;
    }

    /// <summary>
    /// 更新語錄標籤
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<BaseModel> UpdatePostTag(UpdatePostTag input)
    {
        var result = new BaseModel()
        {
            Code = ((int)HttpStatusCode.OK).ToString(),
            Success = true
        };
        var postTag = await _db.Post_Tag.AsQueryable().FirstOrDefaultAsync(x => x.PT_UUID == input.PT_UUID);
        if (postTag == null)
        {
            result.Code = ((int)HttpStatusCode.BadRequest).ToString();
            result.Success = false;
            result.Exception = "找不到相同的標籤UUID";
            return result;
        }
        postTag.PT_Tag = !string.IsNullOrEmpty(input.PostTag) ? input.PostTag : postTag.PT_Tag;
        postTag.Updator = input.UserID;
        postTag.UpdateTime = DateTime.Now;

        var flag = await _db.SaveChangesAsync();
        if (flag <= 0)
        {
            result.Code = ((int)HttpStatusCode.BadRequest).ToString();
            result.Success = false;
            result.Exception = "更新失敗";
            return result;
        }

        return result;
    }

    /// <summary>
    /// 取得語錄標籤列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<BaseModel<GetPostTagView>> GetPostTag()
    {
        var result = new BaseModel<GetPostTagView>()
        {
            Data = new GetPostTagView()
            {
                PostTagList = new List<GetPostTagOutput>()
            },
            Code = ((int)HttpStatusCode.OK).ToString(),
            Success = true,
        };
        var postTagList = await _db.Post_Tag.AsQueryable().AsNoTracking().OrderBy(x => x.PT_Sort).Select(x => new GetPostTagOutput()
        {
            PT_UUID = x.PT_UUID,
            PostTag = x.PT_Tag
        }).ToListAsync();

        if (postTagList == null)
        {
            result.Code = ((int)HttpStatusCode.BadRequest).ToString();
            result.Success = false;
            result.Exception = "不存在任何標籤";
            return result;
        }
        result.Data.PostTagList = postTagList;
        return result;
    }

    /// <summary>
    /// 刪除語錄標籤
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<BaseModel> DeletePostTag(DeletePostTagInput input)
    {
        var result = new BaseModel()
        {
            Code = ((int)HttpStatusCode.OK).ToString(),
            Success = true
        };
        var postTag = await _db.Post_PostTag.AsQueryable().FirstOrDefaultAsync(x => x.PT_UUID == input.PT_UUID);
        if (postTag == null)
        {
            result.Code = ((int)HttpStatusCode.BadRequest).ToString();
            result.Success = false;
            result.Exception = "找不到相同的標籤UUID";
            return result;
        }
        _db.Post_PostTag.Remove(postTag);
        var flag = await _db.SaveChangesAsync();
        if (flag <= 0)
        {
            result.Code = ((int)HttpStatusCode.BadRequest).ToString();
            result.Success = false;
            result.Exception = "刪除失敗";
            return result;
        }

        return result;
    }


    #endregion
}
