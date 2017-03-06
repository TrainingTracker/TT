using System.Collections.Generic;
using System.Linq;

namespace TrainingTracker.DAL.ModelMapper
{
    public abstract class EntityConverter<TSourceEntity, TTargetEntity>
        where TSourceEntity : class
        where TTargetEntity : class
    {
        public abstract TTargetEntity ConvertFromCore(TSourceEntity sourceEntity);
        public abstract TSourceEntity ConvertToCore(TTargetEntity targetEntity);

        public List<TTargetEntity> ConvertListFromCore(List<TSourceEntity> sourceEntity)
        {
            return sourceEntity.Select(ConvertFromCore).ToList();
        }

        public List<TSourceEntity> ConvertListToCore(List<TTargetEntity> sourceEntity)
        {
            return sourceEntity.Select(ConvertToCore).ToList();
        }
    }
}