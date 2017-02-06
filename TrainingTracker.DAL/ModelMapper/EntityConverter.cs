using System.Collections.Generic;

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
            var targetList = new List<TTargetEntity>();
            foreach (var entity in sourceEntity)
            {
                targetList.Add(ConvertFromCore(entity));
            }
            return targetList;
        }

        public List<TSourceEntity> ConvertListToCore(List<TTargetEntity> sourceEntity)
        {
            var targetList = new List<TSourceEntity>();
            foreach (var entity in sourceEntity)
            {
                targetList.Add(ConvertToCore(entity));
            }
            return targetList;
        }
    }
}