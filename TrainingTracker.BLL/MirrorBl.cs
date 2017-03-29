
using System;
using System.Collections.Generic;
using TrainingTracker.BLL.Base;
using TrainingTracker.Common.Entity;

namespace TrainingTracker.BLL
{
    public class MirrorBl : BusinessBase
    {
        public List<Feedback> GetFeedbacksWithFilters(int userId, DateTime startDate, DateTime endDate, Common.Enumeration.FeedbackType feedbackType)
        {
            return FeedbackConverter.ConvertListFromCoreWithMinimalDetails(UnitOfWork.FeedbackRepository
                                                                                      .LoadFeedbacksAndThreadsFromFilters(userId,
                                                                                                                          startDate, 
                                                                                                                          endDate,
                                                                                                                          feedbackType));
        }
    }
}
