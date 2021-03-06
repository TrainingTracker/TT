﻿#region Assemblies
using System;
using System.Collections.Generic;
using System.Linq;
using TrainingTracker.Common.Entity;
using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.Interface;
using TrainingTracker.DAL.ModelMapper;
using Feedback = TrainingTracker.DAL.EntityFramework.Feedback;
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
                                 && x.AddedOn <= endDate)
                       .OrderBy(x=>x.AddedOn);
            }
            // for weekly feedback it should work onweekly start date and end date range
            return Find(x => x.FeedbackType == (int)feedbackType
                             && x.AddedFor == userId
                             && (x.StartDate >= startDate && x.EndDate <= endDate))
                   .OrderBy(x=>x.AddedOn)
                   .ThenBy(x=>x.StartDate)
                   .ThenBy(x => x.EndDate);
        }


        public IEnumerable<Common.Entity.Feedback> LoadWeeklyFeedbackLearningTimelines(int userId,
                                                                                       DateTime startDate,
                                                                                       DateTime endDate)
        {
            return LoadFeedbacksAndThreadsFromFilters(userId,startDate,endDate,FeedbackType.Weekly)
                  .Join(_context.WeeklyFeedbackSurveyMappings,f=>f.FeedbackId,m=>m.FeedbackId,(f,m)=>new {f,m})
                  .Join(_context.SurveyCompletedMetaDatas,s=>s.m.SurveyCompletedMetaDataId,md=>md.SurveyCompletedMetaDataId, (s,md)=> new {s,md})
                  .AsEnumerable()
                  .Select(x=>new Common.Entity.Feedback
                  {
                      AddedOn = x.s.f.AddedOn.GetValueOrDefault(),
                      StartDate = x.s.f.StartDate.GetValueOrDefault(),
                      EndDate = x.s.f.EndDate.GetValueOrDefault(),
                      AddedBy =  new UserConverter().ConvertFromCore(x.s.f.User),
                      Rating = Common.Utility.UtilityFunctions.GetFeedbackRatingFromFeedbackTypeString(x.md.SurveyResponses.FirstOrDefault(r=>r.SurveyQuestionId == 1)
                                                                                                                           .SurveyAnswer
                                                                                                                           .OptionText)
                  })
                  .OrderBy(x => x.AddedOn)
                  .ThenBy(x => x.StartDate)
                  .ThenBy(x => x.EndDate);
        }

        public IEnumerable<Common.Entity.Feedback> LoadWeeklyFeedbackTipDetails(int userId,
                                                                                      DateTime startDate,
                                                                                      DateTime endDate)
        {
            return LoadFeedbacksAndThreadsFromFilters(userId, startDate, endDate, FeedbackType.Weekly)
                  .Join(_context.WeeklyFeedbackSurveyMappings, f => f.FeedbackId, m => m.FeedbackId, (f, m) => new { f, m })
                  .Join(_context.SurveyCompletedMetaDatas, s => s.m.SurveyCompletedMetaDataId, md => md.SurveyCompletedMetaDataId, (s, md) => new { s, md })
                  .Where(x => x.md.SurveyResponses.Any(r => r.SurveyQuestionId == 4 && r.SurveyAnswerId == 5))
                  .AsEnumerable()
                  .Select(x => new Common.Entity.Feedback
                  {
                      AddedOn = x.s.f.AddedOn.GetValueOrDefault(),
                      StartDate = x.s.f.StartDate.GetValueOrDefault(),
                      EndDate = x.s.f.EndDate.GetValueOrDefault(),
                      AddedBy = new UserConverter().ConvertFromCore(x.s.f.User),
                      FeedbackText = x.md.SurveyResponses.FirstOrDefault(r => r.SurveyQuestionId == 4 && r.SurveyAnswerId == 5).AdditionalNote
                  })
                  .OrderBy(x => x.AddedOn)
                  .ThenBy(x => x.StartDate)
                  .ThenBy(x => x.EndDate);
        }

    }
}
