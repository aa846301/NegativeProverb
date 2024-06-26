﻿using Common.Model;
using DataAccess.BusinessModel.PostTag;
using DataAccess.ProjectContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BusinessRule.Service
{
    public class TagService : BaseService
    {
        public TagService(ProjectContext projectContext) : base(projectContext)
        {
        }


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

    }
}
