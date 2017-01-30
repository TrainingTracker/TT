using System;
using System.Collections.Generic;
using System.Linq;
using TrainingTracker.BLL.Base;
using TrainingTracker.Common.Entity;

namespace TrainingTracker.BLL
{
    public class UserHelpForumBl : BussinessBase
    {
        public ForumPost GetPostWithThreads(int postId)
        {
            return ForumUserHelpPostConverter.ConvertFromCore(UnitOfWork.ForumUserHelpPostRepository.GetPostWithThreads(postId));
        }

        public PagedResult<ForumPost> GetFilteredPagedPosts(string wildcard, int categoryId, int statusId,
            int pageNumber, int pageSize)
        {
            var result = UnitOfWork.ForumUserHelpPostRepository.GetPagedFilteredPosts(wildcard, categoryId, statusId,
                pageNumber, pageSize);

            if (result == null) return new PagedResult<ForumPost>();

            return new PagedResult<ForumPost>
            {
                CurrentPage = result.CurrentPage,
                PageCount = result.PageCount,
                PageSize = result.PageSize,
                RowCount = result.RowCount,
                Results = result.Results == null
                        ? new List<ForumPost>()
                        : ForumUserHelpPostConverter.ConvertListFromCore(result.Results.ToList())
            };
        }

        public bool AddPost(ForumPost post)
        {
            var postToAdd = ForumUserHelpPostConverter.ConvertToCore(post);
            postToAdd.CreatedOn = DateTime.Now;
            UnitOfWork.ForumUserHelpPostRepository.Add(postToAdd);
            UnitOfWork.Commit();
            return postToAdd.Id > 0;
        }

        public bool AddPostThread(ForumThread postThread)
        {
            var threadToAdd = ForumUserHelpThreadConverter.ConvertToCore(postThread);
            threadToAdd.CreatedOn = DateTime.Now;
            UnitOfWork.ForumUserHelpThreadRepository.Add(threadToAdd);
            UnitOfWork.Commit();
            return threadToAdd.Id > 0;
        }

    }
}