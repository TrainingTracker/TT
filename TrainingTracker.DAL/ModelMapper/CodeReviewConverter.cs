using TrainingTracker.Common.Entity;
using TrainingTracker.DAL.EntityFramework;
using System.Linq;
using System;

namespace TrainingTracker.DAL.ModelMapper
{
   public class CodeReviewConverter : EntityConverter<CodeReviewMetaData, CodeReview>
    {

       private CodeReviewTagsConverter _codeReviewTagConverter;
       public CodeReviewTagsConverter CodeReviewTagConverter
        {
            get { return _codeReviewTagConverter ?? (_codeReviewTagConverter = new CodeReviewTagsConverter()); }
        }

       public override CodeReview ConvertFromCore(CodeReviewMetaData sourceCodeReview)
        {
            try
            {
                return new CodeReview
                       {
                           Id = sourceCodeReview.CodeReviewMetaDataId,
                           Description = sourceCodeReview.Description,
                           Title = sourceCodeReview.ProjectName,
                           IsDeleted = sourceCodeReview.IsDiscarded ?? false,
                           CreatedOn = sourceCodeReview.CreatedOn,
                           Feedback = sourceCodeReview.Feedback != null ? new FeedbackConverter().ConvertFromCore(sourceCodeReview.Feedback) : null,
                           Tags = CodeReviewTagConverter.ConvertListFromCore(sourceCodeReview.CodeReviewTags.Where(x => !x.IsDeleted).ToList())
                       };
            }
           catch(Exception ex)
            {
                Common.Utility.LogUtility.ErrorRoutine(ex);
                return null;
            }
            
        }

       public override CodeReviewMetaData ConvertToCore(CodeReview sourceCodeReview)
        {
            throw new System.NotImplementedException();
        }
    }
}
