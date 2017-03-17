using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingTracker.Common.Entity;
using TrainingTracker.Common.Utility;

namespace TrainingTracker.DAL.ModelMapper
{
    class FeedbackConverter : EntityConverter<EntityFramework.Feedback,Common.Entity.Feedback>
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
                AddedBy =  UserConverter.ConvertFromCore(sourceEntity.User) ,
                Rating = sourceEntity.Rating.GetValueOrDefault()
            };
        }

        public new List<Feedback> ConvertListFromCore(List<EntityFramework.Feedback>  sourceList)
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
