using BusinessRule.BusinessModel.NewFolder;
using BusinessRule.BusinessModel.Post;
using Common.Model;
using DataAccess.ProjectContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Net;
using System.Runtime.CompilerServices;

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


    /// <summary>
    /// 查詢負能量語錄
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<BaseModel<GetPostView>> GetPost(GetPostInput input)
    {
        var result = new BaseModel<GetPostView>()
        {
            Data = new GetPostView()
            {
                PostList = new List<GetPostOutput>()
            },
            Code = ((int)HttpStatusCode.OK).ToString(),
            Success = true
        };

        var postList = await _db.Post_Post.AsQueryable().AsNoTracking().Include(x => x.Post_PostTag).ThenInclude(x => x.PT_UU).ToListAsync();
        if (!string.IsNullOrEmpty(input.PostKeyWord))
        {
            postList = postList.Where(x => x.P_Post.Contains(input.PostKeyWord)).ToList();
        }
        if (input.PostTagUUID != null && input.PostTagUUID.Any())
        {
            postList = postList.Where(x => x.Post_PostTag.Any(y => input.PostTagUUID.Contains(y.PT_UUID))).ToList();
        }

        var outputList = postList.Select(x => new GetPostOutput
        {
            PostUUID = x.P_UUID,
            Post = x.P_Post,
            PostTagList = x.Post_PostTag.Select(y => new PostTagOutput
            {
                PostTagUUID = y.PT_UUID,
                PostTag = y.PT_UU.PT_Tag
            }).ToList(),
        }).ToList();

        result.Data.PostList = outputList;
        return result;
    }

}
