using TrainingTracker.DAL.EntityFramework;
using User = TrainingTracker.Common.Entity.User;

namespace TrainingTracker.DAL.ModelMapper
{
    public class UserConverter : EntityConverter<EntityFramework.User, User>
    {
        public override User ConvertFromCore(EntityFramework.User sourceUser)
        {
            return new User
            {
                UserId = sourceUser.UserId,
                FirstName = sourceUser.FirstName,
                LastName = sourceUser.LastName,
                FullName = sourceUser.FirstName + " " + sourceUser.LastName,
                ProfilePictureName = sourceUser.ProfilePictureName,
                DateAddedToSystem = sourceUser.DateAddedToSystem,
                IsActive =  sourceUser.IsActive.GetValueOrDefault()
            };
        }

        public override EntityFramework.User ConvertToCore(User sourceUser)
        {
            return new EntityFramework.User
            {
                UserId = sourceUser.UserId,
                FirstName = sourceUser.FirstName,
                LastName = sourceUser.LastName,
                ProfilePictureName = sourceUser.ProfilePictureName,
                IsActive = sourceUser.IsActive
            };
        }
    }
}