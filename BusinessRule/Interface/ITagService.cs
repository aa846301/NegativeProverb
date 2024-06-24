using Common.Model;
using DataAccess.BusinessModel.PostTag;

namespace BusinessRule.Interface
{
    public interface ITagService
    {
        Task<BaseModel> CreatePostTag(CreatePostTagInput input);
        Task<BaseModel> DeletePostTag(DeletePostTagInput input);
        Task<BaseModel<GetPostTagView>> GetPostTag();
        Task<BaseModel> UpdatePostTag(UpdatePostTag input);
    }
}