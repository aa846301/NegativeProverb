using Common.Model;
using DataAccess.BusinessModel.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessRule.Interface
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPostService 
    {
        Task<BaseModel> CreatePost(CreatePostInput input);

        Task<BaseModel<GetPostView>> GetPost(GetPostInput input);

        Task<BaseModel> UpdatePost(UpdatePostInput input);

        Task<BaseModel> DeletePost(DeletePostInput input);
    }
}
