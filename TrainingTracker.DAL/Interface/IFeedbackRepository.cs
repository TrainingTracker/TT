#region Assemblies
using System;
using System.Collections.Generic;
using TrainingTracker.DAL.EntityFramework;
#endregion

namespace TrainingTracker.DAL.Interface
{
   public interface IFeedbackRepository:IRepository<Feedback>
    {
        Feedback LoadFeedbackAndThreads(int feedbackId);

        IEnumerable<Feedback> LoadFeedbacksAndThreadsFromFilters(int userId, 
                                                          DateTime startDate,
                                                          DateTime endDate,
                                                          Common.Enumeration.FeedbackType feedbackType);
    }
}
