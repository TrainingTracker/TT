#region Assemblies
using System;
using System.Collections.Generic;
using TrainingTracker.DAL.EntityFramework;
#endregion

namespace TrainingTracker.DAL.Interface
{
    public interface IFeedbackRepository : IRepository<Feedback>
    {
        Feedback LoadFeedbackAndThreads(int feedbackId);

        IEnumerable<Feedback> LoadFeedbacksAndThreadsFromFilters(int userId,
                                                          DateTime startDate,
                                                          DateTime endDate,
                                                          Common.Enumeration.FeedbackType feedbackType);

        IEnumerable<Common.Entity.Feedback> LoadWeeklyFeedbackLearningTimelines(int userId,
                                                                   DateTime startDate,
                                                                   DateTime endDate);

        IEnumerable<Common.Entity.Feedback> LoadWeeklyFeedbackTipDetails(int userId,
                                                                  DateTime startDate,
                                                                  DateTime endDate);
    }
}
