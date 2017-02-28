using TrainingTracker.Common.Entity;

namespace TrainingTracker.DAL.Interface
{
    public interface IReleaseRepository : IRepository<EntityFramework.Release>
    {

        PagedResult<EntityFramework.Release> GetPagedFilteredSessions(string wildcard
                                                    , int searchReleaseId
                                                    , int pageNumber
                                                    , int pageSize);
    }
}
