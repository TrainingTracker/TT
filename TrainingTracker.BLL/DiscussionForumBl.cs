using System;
using System.Collections.Generic;
using System.Linq;
using TrainingTracker.BLL.Base;
using TrainingTracker.Common.Entity;
using TrainingTracker.Common.ViewModel;
using TrainingTracker.DAL.EntityFramework;

namespace TrainingTracker.BLL
{
    public class DiscussionForumBl : BussinessBase
    {
        public ForumPost GetPostWithThreads(int postId)
        {
            return ForumDiscussionPostConverter.ConvertFromCore(UnitOfWork.ForumDiscussionPostRepository.GetPostWithThreads(postId));
        }

        public HelpForumVm GetHelpForumVm(string wildcard, int categoryId, int statusId, int searchPostId, int pageNumber)
        {
            var forumVm = new HelpForumVm
            {
                Posts = GetFilteredPagedPosts(wildcard, categoryId, statusId, searchPostId, pageNumber, 5)
            };
            if (forumVm.Posts.Results != null && forumVm.Posts.Results.Count > 0)
            {
                forumVm.DefaultPost = GetPostWithThreads(forumVm.Posts.Results[0].PostId);
            }
            return forumVm;
        }

        public PagedResult<ForumPost> GetFilteredPagedPosts(string wildcard, int categoryId, int statusId, int searchPostId,
            int pageNumber, int pageSize)
        {
            var result = UnitOfWork.ForumDiscussionPostRepository.GetPagedFilteredPosts(wildcard, categoryId, statusId, searchPostId,
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
                        : ForumDiscussionPostConverter.ConvertListFromCore(result.Results.ToList())
            };
        }

        public bool AddPost(ForumPost post)
        {
            var postToAdd = ForumDiscussionPostConverter.ConvertToCore(post);
            postToAdd.CreatedOn = DateTime.Now;
            UnitOfWork.ForumDiscussionPostRepository.Add(postToAdd);
            UnitOfWork.Commit();
            return postToAdd.Id > 0;
        }

        public bool AddPostThread(ForumThread postThread)
        {
            var threadToAdd = ForumDiscussionThreadConverter.ConvertToCore(postThread);
            threadToAdd.CreatedOn = DateTime.Now;
            UnitOfWork.ForumDiscussionThreadRepository.Add(threadToAdd);
            UnitOfWork.Commit();
            return threadToAdd.Id > 0;
        }

        public bool UpdatePostStatus(int postId, int statusId, string message, int userId)
        {
            var post = UnitOfWork.ForumDiscussionPostRepository.Get(postId);
            post.StatusId = statusId;
            post.ForumDiscussionThreads.Add(new ForumDiscussionThread
            {
                CreatedOn = DateTime.Now,
                Description = message,
                AddedBy = userId
            });
            return UnitOfWork.Commit() > 0;
        }
    }
}