
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

        public List<Feedback> GetFeedbacksFullDetailsWithFilters(int userId, DateTime startDate, DateTime endDate, Common.Enumeration.FeedbackType feedbackType)
        {
            return FeedbackConverter.ConvertListFromCore(UnitOfWork.FeedbackRepository
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

        public List<CourseTrackerDetails> LoadAllAssignedCourseForTrainee(int userId, DateTime startDate, DateTime endDate)
        {
            return UnitOfWork.CourseRepository.GetAllAssignedCoursesForTrainee(userId)
                                              .Where(x=>x.CourseStarted>=startDate 
                                                        && (x.CourseCompleted.HasValue 
                                                            || x.CourseCompleted<= endDate))
                                              .OrderBy(x=>x.Name)
                                              .ToList();
        }

        public List<Session> GetAllSessionForAttendee(int userId,DateTime startDate, DateTime endDate)
        {
            return SessionConverter.ConvertListFromCore(UnitOfWork.SessionRepository.GetAllSessionForAttendee(userId)
                                                                                    .Where(x => x.SessionDate.HasValue 
                                                                                                && x.SessionDate >= startDate 
                                                                                                && x.SessionDate <= endDate)
                                                                                    .ToList());
        }

        public List<Skill> GetAllSkillsForAttendee(int userId, DateTime startDate, DateTime endDate)
        {
            return UnitOfWork.SkillRepository
                             .GetAllSkillsForTrainee(userId)
                             .OrderBy(x => x.Name)
                             .ToList();
        }
    }
}
