
﻿using System;
﻿using System.Collections.Concurrent;
﻿using System.Collections.Generic;
using System.Linq;
﻿using System.Threading.Tasks;
﻿using TrainingTracker.Common.Entity;
﻿using TrainingTracker.Common.Utility;
﻿using TrainingTracker.Common.ViewModel;
﻿using TrainingTracker.DAL.DataAccess;
﻿using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.Interface;
using System.Data.Entity;
﻿using TrainingTracker.DAL.ModelMapper;
﻿using Feedback = TrainingTracker.DAL.EntityFramework.Feedback;
﻿using User = TrainingTracker.DAL.EntityFramework.User;


namespace TrainingTracker.DAL.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly TrainingTrackerEntities _context;

        public UserRepository(TrainingTrackerEntities context)
            : base(context)
        {
            _context = context;
        }

        public List<EntityFramework.User> GetAllTrainees(int teamId, bool includeInActiveTrainee)
        {
            return
                _context.Users.Where(x => x.IsTrainee == true &&
                                          (teamId == 0 || x.TeamId == teamId) &&
                                          (includeInActiveTrainee || x.IsActive == true))
                              .ToList();
        }

        public DashboardVm GetAllTraineeDataForDashbaord(int teamId)
        {

            var data = _context.Users.Include(x=>x.Courses)
                                     .Where(x => x.IsTrainee == true && (teamId == 0 || x.TeamId == teamId) && x.IsActive == true)
                                     .GroupJoin(_context.Feedbacks.Join(_context.Users, t => t.AddedBy, v => v.UserId, (t, v) => new { t, v }), u => u.UserId, f => f.t.AddedFor, (u, f) => new { u, f })
                                     .GroupJoin(_context.CourseUserMappings.Join(_context.Courses,cum=>cum.CourseId,c=>c.Id,(cum,c)=> new {cum,c}),s=>s.u.UserId,cum=>cum.cum.UserId,(s,cum)=>new{s,cum})
                                     .ToList();

            if (data.Any(r => r.s.f != null && r.s.f.Any(t => t.v== null)))
            {
                LogUtility.ErrorRoutine(new Exception("lastWeeklyFeedback is null:"));
            }

           UserConverter userConverter = new UserConverter();
           FeedbackConverter feedbackConverter = new FeedbackConverter();

           DateTime mondayTwoWeeksAgo = Common.Utility.UtilityFunctions.GetLastDateByDay(DayOfWeek.Monday, DateTime.Now.AddDays(-14));
           DateTime lastFriday = Common.Utility.UtilityFunctions.GetLastDateByDay(DayOfWeek.Friday, DateTime.Now);
           DateTime lastMonday = lastFriday.AddDays(-5);

           var concurrentTrainee = new ConcurrentQueue<UserData>();
          //  data.AsParallel().ForAll(traineeLocal=>
          //Parallel.ForEach(data, traineeLocal =>
           foreach(var trainee in data)
           {
             //  var trainee = traineeLocal;
               bool lastWeekFeedbackAdded =  trainee.s.u.DateAddedToSystem >= lastFriday
                                            || (trainee.s.f.Any(x => x.t.FeedbackType == (int)Common.Enumeration.FeedbackType.Weekly
                                            && (x.t.StartDate >= lastMonday || (x.t.EndDate <= lastFriday && x.t.EndDate >= lastMonday))));

               Feedback lastWeeklyFeedback = lastWeekFeedbackAdded ? (trainee.s.f.OrderByDescending(feedback => feedback.t.FeedbackId)
                                                                                       .Select(x=>x.t)
                                                                                       .FirstOrDefault(x => x.FeedbackType == (int)Common.Enumeration.FeedbackType.Weekly))
                                                                    : null;

               List<string> weeksforFeedbackNotPresent = new List<string>();

               if (!lastWeekFeedbackAdded)
               {
                   weeksforFeedbackNotPresent = Common.Utility.UtilityFunctions.GetAllWeeksBetweenDates(trainee.s.u.DateAddedToSystem, lastFriday);

                   foreach (var feedback in trainee.s.f.Where(x => x.t.FeedbackType == (int)Common.Enumeration.FeedbackType.Weekly))
                   {
                       var startOfWeeks = Common.Utility.UtilityFunctions.GetLastDateByDay(DayOfWeek.Monday, feedback.t.StartDate.GetValueOrDefault());

                       // feedback spans to multiple week.
                       while (startOfWeeks <= feedback.t.EndDate)
                       {
                           weeksforFeedbackNotPresent.Remove(startOfWeeks.ToString("dd/MM/yyyy") + "-" + startOfWeeks.AddDays(4).ToString("dd/MM/yyyy"));
                           startOfWeeks = startOfWeeks.AddDays(7);
                       }
                   }
               }
              
              
               concurrentTrainee.Enqueue(new UserData
               {
                   User =  userConverter.ConvertFromCore(trainee.s.u),

                   IsCodeReviewAdded = trainee.s.u.DateAddedToSystem >= lastFriday
                                       || trainee.s.f.Any(x => x.t.FeedbackType == (int)Common.Enumeration.FeedbackType.CodeReview && x.t.AddedOn >= mondayTwoWeeksAgo),

                   LastWeekFeedbackAdded = lastWeekFeedbackAdded,

                   WeeklyFeedback = lastWeeklyFeedback == null ? new List<Common.Entity.Feedback>() : 
                                                                 new List<Common.Entity.Feedback> { feedbackConverter.ConvertFromCore(lastWeeklyFeedback)},

                   RemainingFeedbacks = feedbackConverter.ConvertListFromCore(trainee.s.f.Select(x=>x.t).Where(x => x.FeedbackType == (int)Common.Enumeration.FeedbackType.CodeReview
                                                                                                        || x.FeedbackType == (int)Common.Enumeration.FeedbackType.Weekly
                                                                                                        || x.FeedbackType == (int)Common.Enumeration.FeedbackType.Assignment
                                                                                                        || x.FeedbackType == (int)Common.Enumeration.FeedbackType.Skill
                                                                                                        || x.FeedbackType == (int)Common.Enumeration.FeedbackType.RandomReview)
                                                                                        .OrderByDescending(x=>x.FeedbackId)
                                                                                        .Take(5)
                                                                                        .ToList()),
                   WeekForFeedbackNotPresent = weeksforFeedbackNotPresent,
                   AllAssignedCourses = trainee.cum.Where(x => x.cum.CompletedOn == null && x.c.IsActive && x.c.CourseSubtopics
                                                                                                               .Where(y=>y.IsActive)
                                                                                                               .SelectMany(y => y.AssignmentSubtopicMaps)
                                                                                                 .Select(y => y.Assignment)
                                                                                                 .Where(a=>a.IsActive)
                                                                                                 .SelectMany(y => y.AssignmentUserMaps)
                                                                                                 .Any(y => !y.IsApproved && y.IsCompleted && y.TraineeId == trainee.s.u.UserId))
                                                                    .Select(x => new CourseTrackerDetails
                                                                                       {
                                                                                           Name = x.c.Name,
                                                                                           PendingAssignmentCount = x.c.CourseSubtopics.SelectMany(y => y.AssignmentSubtopicMaps)
                                                                                                                                      .Select(y => y.Assignment)
                                                                                                                                      .SelectMany(y => y.AssignmentUserMaps)
                                                                                                                                      .Count(y => !y.IsApproved && y.IsCompleted && y.TraineeId == trainee.s.u.UserId)
                                                                                       })
                                                    .OrderBy(x => x.PendingAssignmentCount)
                                                    .ToList(),
                   AnyActiveCourse = trainee.cum.Any(x=>x.cum.CompletedOn == null && x.c.IsActive)

               });
           }
          //  );

           return new DashboardVm
           {
               Trainees = concurrentTrainee.OrderBy(x=>x.User.FirstName).ToList()
           };          
       }
    }
}
