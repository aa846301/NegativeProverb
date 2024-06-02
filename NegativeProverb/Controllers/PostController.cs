using BusinessRule.Service;
using Common.Model;
using DataAccess.BusinessModel.Post;
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


    /// <summary>
    /// 更新負能量語錄
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("UpdatePost")]
    public async Task<BaseModel> UpdatePost(UpdatePostInput input) => await _postService.UpdatePost(input);


    /// <summary>
    /// 刪除負能量語錄
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("DeletePost")]
    public async Task<BaseModel> DeletePost(DeletePostInput input) => await _postService.DeletePost(input);
}
