using System;
using System.Collections.Generic;
using System.Linq;
using TrainingTracker.BLL.Base;
using TrainingTracker.Common.Entity;
using TrainingTracker.Common.ViewModel;
using TrainingTracker.DAL.EntityFramework;
using User = TrainingTracker.Common.Entity.User;

namespace TrainingTracker.BLL
{
    public class DiscussionForumBl : BussinessBase
    {
        public ForumPost GetPostWithThreads(int postId)
        {
            return ForumDiscussionPostConverter.ConvertFromCore(UnitOfWork.ForumDiscussionPostRepository.GetPostWithThreads(postId));
        }

        public PagedResult<ForumPost> GetFilteredPagedPosts(string wildcard, int statusId, int searchPostId, int addedBy,
            int pageNumber, int pageSize)
        {
            var result = UnitOfWork.ForumDiscussionPostRepository.GetPagedFilteredPosts(wildcard, 1, statusId, searchPostId, addedBy,
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

        public bool AddPost(ForumPost post, User currentUser)
        {
            var postToAdd = ForumDiscussionPostConverter.ConvertToCore(post);
            postToAdd.CreatedOn = DateTime.Now;
            postToAdd.AddedBy = currentUser.UserId;
            postToAdd.CategoryId = 1;// Need to change HardCoded Value
            UnitOfWork.ForumDiscussionPostRepository.Add(postToAdd);
            UnitOfWork.Commit();

            if (postToAdd.Id > 0)
            {
                post.AddedBy = currentUser.UserId;
                post.PostId = postToAdd.Id;
                post.AddedByUser = currentUser;
                new NotificationBl().AddNewDiscussionPostNotification(post);
            }

            return postToAdd.Id > 0;
        }

        public bool AddPostThread(ForumThread postThread, User currentUser)
        {
            var threadToAdd = ForumDiscussionThreadConverter.ConvertToCore(postThread);
            threadToAdd.CreatedOn = DateTime.Now;
            threadToAdd.AddedBy = currentUser.UserId;
            UnitOfWork.ForumDiscussionThreadRepository.Add(threadToAdd);
            UnitOfWork.Commit();
            if (threadToAdd.Id > 0)
            {
                postThread.AddedBy = currentUser.UserId;
                postThread.PostId = threadToAdd.PostId;
                postThread.AddedByUser = currentUser;
                new NotificationBl().AddNewDiscussionThreadNotification(postThread);
            }
            
            return threadToAdd.Id > 0;
        }

        public bool UpdatePostStatus(int postId, int statusId, string message, int addedFor, User currentUser)
        {
            var post = UnitOfWork.ForumDiscussionPostRepository.Get(postId);
            post.StatusId = statusId;
            post.ForumDiscussionThreads.Add(new ForumDiscussionThread
            {
                CreatedOn = DateTime.Now,
                Description = message,
                AddedBy = currentUser.UserId
            });


            if (UnitOfWork.Commit() > 0)
            {
                new NotificationBl().AddNewDiscussionThreadNotification(new ForumThread()
                {
                    PostId = postId,
                    Description = message,
                    AddedBy = currentUser.UserId,
                    AddedFor = addedFor,
                    AddedByUser = currentUser
                });
            }

            return UnitOfWork.Commit() > 0;
        }
    }
}