using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TrainingTracker.Common.Entity;
using TrainingTracker.DAL.DataAccess;
using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.Interface;
//using Session = TrainingTracker.DAL.EntityFramework.Session;

namespace TrainingTracker.DAL.Repositories
{
    class SessionRepository : Repository<EntityFramework.Session>, ISessionRepository
    {
        private readonly TrainingTrackerEntities _context;
        public SessionRepository(TrainingTrackerEntities context)
            : base(context)
        {
            _context = context;
        }

        public PagedResult<EntityFramework.Session> GetPagedFilteredSessions(string wildcard, Common.Enumeration.SessionType statusId, int searchSessionId
                                                                            , int teamId, int pageNumber, int pageSize)
        {
            return BaseFilterSearchQuery(wildcard, statusId, searchSessionId, teamId).AsNoTracking()
                                                                                     .Page(pageNumber, pageSize);
        }

        public EntityFramework.Session GetSessionWithAttendees(int sessionId)
        {
            return _context.Sessions.Include(x => x.UserSessionMappings)
                                    .Include(x => x.User)
                                    .FirstOrDefault(x => x.SessionId == sessionId);         
        }

        private IQueryable<EntityFramework.Session> BaseFilterSearchQuery(string wildcard, Common.Enumeration.SessionType statusId, int searchSessionId, int teamId)
        {
            bool wildcardFilter = !string.IsNullOrEmpty(wildcard);
            bool statusFilter = (int)statusId != 0;
            bool sessionFilter = searchSessionId != 0;
            bool teamIdFilter = teamId != 0;

            IQueryable<EntityFramework.Session> filterQuery = _context.Sessions;

            if (wildcardFilter) filterQuery = filterQuery.Where(x => x.Title.Contains(wildcard) || x.Description.Contains(wildcard));

            if (statusFilter)
            {
                 DateTime today = DateTime.Now.Date;

                 if (statusId == Common.Enumeration.SessionType.ToBePresented) filterQuery = filterQuery.Where(x => x.SessionDate >= today);
                 if (statusId == Common.Enumeration.SessionType.AlreadyPresented) filterQuery = filterQuery.Where(x => x.SessionDate < today);                
            };

            if (sessionFilter) filterQuery = filterQuery.Where(x => x.SessionId == searchSessionId);
            if (teamIdFilter) filterQuery = filterQuery.Where(x => x.User.TeamId == teamId);

            return filterQuery.OrderByDescending(x => x.SessionDate);
        }
    }
}
