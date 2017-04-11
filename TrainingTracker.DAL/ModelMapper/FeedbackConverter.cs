#region Assemblies
using System;
using System.Collections.Generic;
using System.Linq;
using TrainingTracker.Common.Entity;
using TrainingTracker.Common.Utility;
#endregion

namespace TrainingTracker.DAL.ModelMapper
{
    public class FeedbackConverter : EntityConverter<EntityFramework.Feedback, Common.Entity.Feedback>
    {
        private UserConverter _userConverter;
        public UserConverter UserConverter
        {
            get { return _userConverter ?? (_userConverter = new UserConverter()); }
        }

        public override Feedback ConvertFromCore(EntityFramework.Feedback sourceEntity)
        {
            return new Feedback
            {
                FeedbackId = sourceEntity.FeedbackId,
                AddedOn = sourceEntity.AddedOn.GetValueOrDefault(),
                FeedbackText = sourceEntity.FeedbackText,
                FeedbackType = new FeedbackType
                {
                    FeedbackTypeId = sourceEntity.FeedbackType.GetValueOrDefault()
                },
                AddedBy = UserConverter.ConvertFromCore(sourceEntity.User),
                Rating = sourceEntity.Rating.GetValueOrDefault(),
                ThreadCount = sourceEntity.FeedbackThreads.Count
            };
        }

        public Feedback ConvertFromCoreWithMinimalDetails(EntityFramework.Feedback sourceEntity)
        {
            return new Feedback
            {
                FeedbackId = sourceEntity.FeedbackId,
                AddedOn = sourceEntity.AddedOn.GetValueOrDefault(),
                FeedbackType = new FeedbackType
                {
                    FeedbackTypeId = sourceEntity.FeedbackType.GetValueOrDefault()
                },
                StartDate = sourceEntity.StartDate.GetValueOrDefault(),
                EndDate = sourceEntity.EndDate.GetValueOrDefault(),
                AddedBy = UserConverter.ConvertFromCore(sourceEntity.User),
                Rating = sourceEntity.Rating.GetValueOrDefault(),
                ThreadCount = sourceEntity.FeedbackThreads.Count
            };
        }

        public List<Feedback> ConvertListFromCoreWithMinimalDetails(IEnumerable<EntityFramework.Feedback> sourceList)
        {
            return sourceList.Select(ConvertFromCoreWithMinimalDetails).ToList();
        }

        public new List<Feedback> ConvertListFromCore(IEnumerable<EntityFramework.Feedback> sourceList)
        {
            try
            {
                return sourceList.Select(ConvertFromCore).ToList();
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
            }
            return new List<Feedback>();
        }

        public override EntityFramework.Feedback ConvertToCore(Feedback targetEntity)
        {
            throw new NotImplementedException();
        }
    }
}
