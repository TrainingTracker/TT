using System;
using System.Collections.Generic;
using System.Linq;
using TrainingTracker.BLL.Base;
using TrainingTracker.Common.Constants;
using TrainingTracker.Common.Entity;
using TrainingTracker.Common.ViewModel;

namespace TrainingTracker.BLL
{
    /// <summary>
    /// Bussiness Layer class for Dashboard, Inherits Bussiness Base Class
    /// </summary>
    public class DashboardBl:BussinessBase
    {
        /// <summary>
        /// Fetch Dashboard data by User
        /// </summary>
        /// <param name="user">Instance of logged in Team</param>
        /// <returns></returns>
        public DashboardVm GetDashboardData(User user)
        {
            int? teamId = user.TeamId.HasValue ? user.TeamId.Value : (user.IsAdministrator ? 0 : new int?());
            var dashboardVm = new DashboardVm
            {
                Trainees = teamId.HasValue ? UserDataAccesor.GetDashboardData(teamId.Value) : new List<UserData>() ,
            };


            DateTime lastFriday = Common.Utility.UtilityFunctions.GetLastDateByDay(DayOfWeek.Friday,DateTime.Now);

            foreach (var trainee in dashboardVm.Trainees)
            {
                bool isFeedbackAdded = false;

                // No Feedback pending should be there if user is added after last friday
                if ((trainee.User.DateAddedToSystem >= lastFriday))
                {
                    isFeedbackAdded = true;
                    trainee.IsCodeReviewAdded = true;
                }
                else
                {
                    trainee.WeekForFeedbackNotPresent = Common.Utility.UtilityFunctions.GetAllWeeksBetweenDates(trainee.User.DateAddedToSystem , lastFriday);
                   
                    foreach (var feedback in trainee.WeeklyFeedback)
                    {
                        var startOfWeeks = Common.Utility.UtilityFunctions.GetLastDateByDay(DayOfWeek.Monday , feedback.StartDate); 

                        // Check for last week feedback added or not
                        if (feedback.StartDate >= lastFriday.AddDays(-5) || (feedback.EndDate <= lastFriday && feedback.EndDate >= lastFriday.AddDays(-5))) isFeedbackAdded = true;

                        feedback.WeekForFeedbackPresent = feedback.StartDate.ToString("dd/MM/yyyy") + "-" + feedback.EndDate.ToString("dd/MM/yyyy");
                      
                        // feedback spans to multiple week.
                        while (startOfWeeks <= feedback.EndDate)
                        {
                            trainee.WeekForFeedbackNotPresent.Remove(startOfWeeks.ToString("dd/MM/yyyy") + "-" + startOfWeeks.AddDays(4).ToString("dd/MM/yyyy"));
                            startOfWeeks = startOfWeeks.AddDays(7);
                        }      
                    }
                }

                trainee.LastWeekFeedbackAdded = isFeedbackAdded;
                trainee.WeeklyFeedback = trainee.WeeklyFeedback.Take(1).ToList();
                trainee.IsCodeReviewAdded = trainee.RemainingFeedbacks.Where(x => x.FeedbackType.FeedbackTypeId == (int) Common.Enumeration.FeedbackType.CodeReview && x.AddedOn >= lastFriday.AddDays(-12))
                                              .OrderByDescending(x => x.AddedOn)
                                              .Take(1)
                                              .Any();
                trainee.RemainingFeedbacks = trainee.RemainingFeedbacks.OrderByDescending(x => x.AddedOn).Take(5).ToList();
            }

            return dashboardVm;
        }
    }
}