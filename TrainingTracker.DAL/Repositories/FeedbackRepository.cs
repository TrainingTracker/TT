#region Assemblies
using System;
using System.Collections.Generic;
using System.Linq;
using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.Interface;
using FeedbackType = TrainingTracker.Common.Enumeration.FeedbackType;

#endregion

namespace TrainingTracker.DAL.Repositories
{
    public class FeedbackRepository : Repository<Feedback>, IFeedbackRepository
    {
        private readonly TrainingTrackerEntities _context;

        public FeedbackRepository(TrainingTrackerEntities context)
            : base(context)
        {
            _context = context;
        }

        public Feedback LoadFeedbackAndThreads(int feedbackId)
        {
            return SingleOrDefault(x => x.FeedbackId == feedbackId);
        }

        public IEnumerable<Feedback> LoadFeedbacksAndThreadsFromFilters(int userId,
                                                                     DateTime startDate,
                                                                     DateTime endDate,
                                                                     FeedbackType feedbackType)
        {
            if (feedbackType != FeedbackType.Weekly)
            {
                return Find(x => x.FeedbackType == (int)feedbackType
                            && x.AddedFor == userId
                            && x.AddedOn >= startDate
                            && x.AddedOn <= endDate);
            }
            // for weekly feedback it should work onweekly strat date and end date range

            return Find(x => x.FeedbackType == (int)feedbackType
                             && x.AddedFor == userId
                             && (x.StartDate >= startDate || x.EndDate <= endDate));
        }
    }
}
