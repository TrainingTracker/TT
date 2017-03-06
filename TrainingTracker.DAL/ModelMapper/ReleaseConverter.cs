using TrainingTracker.Common.Entity;

namespace TrainingTracker.DAL.ModelMapper
{
    public class ReleaseConverter :EntityConverter<EntityFramework.Release,Release>
    {

        private UserConverter _userConverter;
        public UserConverter UserConverter
        {
            get { return _userConverter ?? (_userConverter = new UserConverter()); }
        }

        public override Release ConvertFromCore(EntityFramework.Release sourceEntity)
        {
            return new Release
            {
                ReleaseId = sourceEntity.ReleaseId,
                IsPublished = sourceEntity.IsPublished,
                Description = sourceEntity.Description,
                Major = sourceEntity.Major,
                Minor = sourceEntity.Minor,
                Patch = sourceEntity.Patch,
                ReleaseDate = sourceEntity.ReleaseDate,
                ReleaseTitle = sourceEntity.Title,
                AddedBy = UserConverter.ConvertFromCore(sourceEntity.User)
            };
        }

        public override EntityFramework.Release ConvertToCore(Release targetEntity)
        {
            return new EntityFramework.Release
            {
                ReleaseId = targetEntity.ReleaseId,
                IsPublished = targetEntity.IsPublished,
                Description = targetEntity.Description,
                Major = targetEntity.Major,
                Minor = targetEntity.Minor,
                Patch = targetEntity.Patch,
                ReleaseDate = targetEntity.ReleaseDate,
                Title = targetEntity.ReleaseTitle,
                AddedBy = targetEntity.AddedBy.UserId
            };
        }
    }
}
