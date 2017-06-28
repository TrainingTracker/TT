using System;
using System.Collections.Generic;
using System.Linq;

namespace TrainingTracker.DAL.ModelMapper
{
    public class CodeReviewTagsConverter : EntityConverter<DAL.EntityFramework.CodeReviewTag, Common.Entity.CodeReviewTag>
    {
        private CodeReviewPointConverter _codeReviewPointConverter;
        public CodeReviewPointConverter CodeReviewPointConverter
        {
            get { return _codeReviewPointConverter ?? (_codeReviewPointConverter = new CodeReviewPointConverter()); }
        }


        public override Common.Entity.CodeReviewTag ConvertFromCore(DAL.EntityFramework.CodeReviewTag source)
        {
            try
            {
                return new Common.Entity.CodeReviewTag
                {
                    CodeReviewTagId = source.CodeReviewTagId,
                    Skill = new Common.Entity.Skill
                    {
                        SkillId = source.SkillId == null ? 0 : source.SkillId.Value,
                        Name = source.SkillId == null ? "General" : source.Skill.Name
                    },
                    ReviewPoints = source.CodeReviewPoints != null ? CodeReviewPointConverter.ConvertListFromCore(source.CodeReviewPoints.Where(x=>!x.IsDeleted).ToList()) : new List<Common.Entity.CodeReviewPoint>()
                };
            }
            catch(Exception ex)
            {
                Common.Utility.LogUtility.ErrorRoutine(ex);
                throw ex;
            }
            
        }

        public override DAL.EntityFramework.CodeReviewTag ConvertToCore(Common.Entity.CodeReviewTag source)
        {
            throw new System.NotImplementedException();
        }
    }
}
