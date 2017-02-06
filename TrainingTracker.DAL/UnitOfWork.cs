﻿using System.Runtime.Remoting.Contexts;
using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.Interface;
using TrainingTracker.DAL.Repositories;

namespace TrainingTracker.DAL
{
    public class UnitOfWork
    {
        private readonly TrainingTrackerEntities _context;

        public UnitOfWork()
        {
            _context = new TrainingTrackerEntities();
        }

        private ICourseRepository _courseRepository;
        public ICourseRepository CourseRepository
        {
            get { return _courseRepository ?? (_courseRepository = new CourseRepository(_context)); }
        }

        private IRepository<ForumUserHelpCategory> _forumUserHelpCategoryRepository;
        public IRepository<ForumUserHelpCategory> ForumUserHelpCategoryRepository
        {
            get { return _forumUserHelpCategoryRepository ?? (_forumUserHelpCategoryRepository = new Repository<ForumUserHelpCategory>(_context)); }
        }

        private IRepository<ForumUserHelpThread> _forumUserHelpThreadRepository;
        public IRepository<ForumUserHelpThread> ForumUserHelpThreadRepository
        {
            get { return _forumUserHelpThreadRepository ?? (_forumUserHelpThreadRepository = new Repository<ForumUserHelpThread>(_context)); }
        }

        private IForumUserHelpPostRepository _forumUserHelpPostRepository;
        public IForumUserHelpPostRepository ForumUserHelpPostRepository
        {
            get { return _forumUserHelpPostRepository ?? (_forumUserHelpPostRepository = new ForumUserHelpPostRepository(_context)); }
        }

        public int Commit()
        {
           return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}