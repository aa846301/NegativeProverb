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
    /// 新增語錄標籤
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("CreatePostTag")]
    public async Task<BaseModel> CreatePostTag(CreatePostTagInput input) => await _postService.CreatePostTag(input);

    /// <summary>
    /// 更新語錄標籤
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("UpdatePostTag")]
    public async Task<BaseModel> UpdatePostTag(UpdatePostTag input) => await _postService.UpdatePostTag(input);


    /// <summary>
    /// 取得語錄標籤列表
    /// </summary>
    /// <returns></returns>
    [HttpPost]
    [Route("GetPostTag")]
    public async Task<BaseModel<GetPostTagView>> GetPostTag() => await _postService.GetPostTag();

    /// <summary>
    /// 刪除語錄標籤
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("DeletePostTag")]
    public async Task<BaseModel> DeletePostTag(DeletePostTagInput input) => await _postService.DeletePostTag(input);
}
