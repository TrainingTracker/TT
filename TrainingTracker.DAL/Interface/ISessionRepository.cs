using System.Collections.Generic;
using TrainingTracker.Common.Entity;

namespace TrainingTracker.DAL.Interface
{
    public interface ISessionRepository : IRepository<EntityFramework.Session>
    {

        PagedResult<EntityFramework.Session> GetPagedFilteredSessions(string wildcard
                                                    , Common.Enumeration.SessionType sessionType 
                                                    , int searchSessionId
                                                    , int teamId
                                                    , int pageNumber
                                                    , int pageSize);

        EntityFramework.Session GetSessionWithAttendees(int sessionId);

        EntityFramework.Session GetSessionWithAttendeesTrackable(int sessionId);

        List<EntityFramework.Session> GetAllSessionForAttendee(int traineeId);
    }
}
