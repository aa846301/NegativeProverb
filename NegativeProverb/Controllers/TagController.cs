using BusinessRule.Service;
using Common.Model;
using DataAccess.BusinessModel.PostTag;
using Microsoft.AspNetCore.Mvc;

namespace NegativeProverb.Controllers
{
    public class TagController : BaseController
    {
        private readonly TagService _tagService;

        public TagController(TagService tagService)
        {
            _tagService = tagService;
        }


        /// <summary>
        /// 新增語錄標籤
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreatePostTag")]
        public async Task<BaseModel> CreatePostTag(CreatePostTagInput input) => await _tagService.CreatePostTag(input);

        /// <summary>
        /// 更新語錄標籤
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdatePostTag")]
        public async Task<BaseModel> UpdatePostTag(UpdatePostTag input) => await _tagService.UpdatePostTag(input);


        /// <summary>
        /// 取得語錄標籤列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("GetPostTag")]
        public async Task<BaseModel<GetPostTagView>> GetPostTag() => await _tagService.GetPostTag();

        /// <summary>
        /// 刪除語錄標籤
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("DeletePostTag")]
        public async Task<BaseModel> DeletePostTag(DeletePostTagInput input) => await _tagService.DeletePostTag(input);
    }
}
