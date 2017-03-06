using System.Collections.Generic;
using System.Linq;
using TrainingTracker.Common.Entity;

namespace TrainingTracker.DAL.ModelMapper
{
    public class SessionConverter : EntityConverter<EntityFramework.Session, Session>
    {
        private UserConverter _userConverter;
        public UserConverter UserConverter
        {
            get { return _userConverter ?? (_userConverter = new UserConverter()); }
        }

        public override Session ConvertFromCore(EntityFramework.Session sourceEntity)
        {
            return new Session
            {
                Id = sourceEntity.SessionId,
                AddedOn = sourceEntity.AddedOn.GetValueOrDefault(),
                Date = sourceEntity.SessionDate.GetValueOrDefault(),
                Description = sourceEntity.Description,
                Presenter = sourceEntity.User == null? null : UserConverter.ConvertFromCore(sourceEntity.User),
                SlideName = sourceEntity.SlideName,
                VideoFileName = sourceEntity.VideoFileName,
                Title = sourceEntity.Title
            };
        }

        public override EntityFramework.Session ConvertToCore(Session targetEntity)
        {
            return new EntityFramework.Session
            {
                AddedOn = targetEntity.AddedOn,
                Description = targetEntity.Description,
                Presenter = targetEntity.Presenter.UserId,
                SlideName = targetEntity.SlideName,
                Title = targetEntity.Title,
                SessionId = targetEntity.Id,
                VideoFileName = targetEntity.VideoFileName,
                SessionDate = targetEntity.Date
            };
        }

        public  Session ConvertFromCoreWithAttendees(EntityFramework.Session sourceEntity)
        {

            Session objSession = ConvertFromCore(sourceEntity);
            objSession.SessionAttendees = UserConverter.ConvertListFromCore(sourceEntity.UserSessionMappings
                                                                                        .Select(x => x.User1)
                                                                                        .ToList());

            return objSession;

        }

    }
}
