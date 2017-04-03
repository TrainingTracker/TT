
using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<Feedback> LoadWeeklyFeedbackLearningTimelines(int userId, DateTime startDate, DateTime endDate)
        {
            return
                UnitOfWork.FeedbackRepository.LoadWeeklyFeedbackLearningTimelines(userId, startDate, endDate).ToList();
        }

        public List<Feedback> LoadWeeklyFeedbackTipDetails(int userId, DateTime startDate, DateTime endDate)
        {
            return
               UnitOfWork.FeedbackRepository.LoadWeeklyFeedbackTipDetails(userId, startDate, endDate).ToList();
        }
    }
}
