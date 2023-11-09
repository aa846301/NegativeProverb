using BusinessRule.BusinessModel.NewFolder;
using BusinessRule.BusinessModel.Post;
using BusinessRule.Service;
using Common.Model;
using Microsoft.AspNetCore.Mvc;

namespace NegativeProverb.Controllers;

/// <summary>
/// 負能量語錄管理
/// </summary>
public class PostController : BaseController
{
    private readonly PostService _postService;

    public PostController(PostService postService)
    {
        _postService = postService;
    }

    /// <summary>
    /// 新增負能量語錄
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("CreatePost")]
    public async Task<BaseModel> CreatePost(CreatePostInput input) => await _postService.CreatePost(input);

    /// <summary>
    /// 查詢負能量語錄
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("GetPost")]
    public async Task<BaseModel<GetPostView>> GetPost(GetPostInput input) => await _postService.GetPost(input);
}
