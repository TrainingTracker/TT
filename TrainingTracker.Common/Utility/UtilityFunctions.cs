using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using TrainingTracker.Common.Constants;
using TrainingTracker.Common.Entity;
using System.Web;
using System.Linq;

namespace TrainingTracker.Common.Utility
{
    /// <summary>
    /// Common Utility Functions to be used through the application
    /// </summary>
    public static class UtilityFunctions
    {
       /// <summary>
       /// Static Function that returns the Last Date on the given day
       /// * eg. this Function returns the Last Friday Date wrt Today's date, If today is Friday, last friday will be today
       /// </summary>
       /// <param name="dayOfWeek">enumeration of type day of week</param>
        /// <param name="referenceDate">reference Date from where the last date wrt day to be calculated</param>
       /// <returns>returns the last day</returns> 
        public static DateTime GetLastDateByDay(DayOfWeek dayOfWeek,DateTime referenceDate )
       {
           var differenceInDay =  dayOfWeek - referenceDate.DayOfWeek ;           
           return differenceInDay > 0 ? referenceDate.AddDays(-7 + differenceInDay) : referenceDate.AddDays(differenceInDay);
       }

        /// <summary>
        /// Returns All the weeks list between two given dates, * week here starts on Monday
        /// </summary>
        /// <param name="startDate">start date</param>
        /// <param name="endDate">end date</param>
        /// <returns>List of week strings</returns>
        public static List<string> GetAllWeeksBetweenDates(DateTime? startDate, DateTime endDate)
        {
            List<string> allWeeksList =new List<string>();

            if (!startDate.HasValue) return allWeeksList;

            while (endDate > startDate)
            {
                allWeeksList.Add(endDate.AddDays(-4).Date.ToString("dd/MM/yyyy") + "-" + endDate.Date.ToString("dd/MM/yyyy"));
                endDate = endDate.AddDays(-7);
            }
            return allWeeksList;
        }

        /// <summary>
        /// Generates Html for feedback On response
        /// </summary>
        /// <param name="response">Insance of survey response</param>
        /// <returns>Html string</returns>
        public static string GenerateHtmlForFeedbackOnSurveyResponse(SurveyResponse response)
        {
            StringBuilder stringBuilder= new StringBuilder();

            stringBuilder.Append("<div class='weekly-feedback'><code>");

            foreach (var responseAnswers in response.Response)
            {
                stringBuilder.Append("<div class='feedback-zone'>");
                stringBuilder.Append("<div class='feedback-question'><label>").Append(responseAnswers.QuestionText.Replace("[[[trainee]]]" , response.AddedFor.FirstName)).Append("</label></div>");

                if (!string.IsNullOrEmpty(Convert.ToString(responseAnswers.AnswerText))) 
                stringBuilder.Append("<div class='feedback-answer'><label> ").Append(responseAnswers.AnswerText).Append("</label></div>");

                if (!string.IsNullOrEmpty(Convert.ToString(responseAnswers.AdditionalNotes)) && responseAnswers.AdditionalNotes.Trim().Length > 0) 
                stringBuilder.Append("<div class='feedback-notes'><label><q>").Append(responseAnswers.AdditionalNotes.Trim().Replace("<script>" , "&lt;script&gt;").Replace("</script>" , "&lt;/script&gt;")).Append("</q></label></div>");

                if (string.IsNullOrEmpty(Convert.ToString(responseAnswers.AdditionalNotes)) &&
                    string.IsNullOrEmpty(responseAnswers.AnswerText))
                {
                    stringBuilder.Append("<div class='feedback-notes'><label class='danger'>").Append("Question Skipped").Append("</label></div>");
                }
                stringBuilder.Append("</div>");
            }

            stringBuilder.Append("<div id='divSurveyCodeReview' class='feedback-zone'><div class='feedback-question'><label>Code reviews for the week.</label></div><ul class='feedback-notes'>");

            foreach (var cr in response.CodeReviewForTheWeek)
            {
                stringBuilder.Append("<li class='li-code-review'>");

                string className = "";
                switch (cr.Rating)
                {
                    case 1:
                        className = "rating-slow";
                        break;
                    case 2:
                         className = "rating-Average";
                         break;
                    case 3  :
                         className = "rating-Fast";
                         break;
                    case 4:
                        className = "rating-Exceptional";
                        break;
                }

                stringBuilder.Append("<div style='display: inline-block;' class='title ' ><strong><a href='/Profile/UserProfile?userId="+cr.AddedBy.UserId+"'>"+cr.AddedBy.FullName+"</a>&nbsp;</strong></div>");
                stringBuilder.Append("<div style='display: inline-block;' class='text-muted time ' > Added <span onclick='my.profileVm.loadFeedbackWithThread("+cr.FeedbackId+")'><a href='#" + cr.AddedBy.UserId + "'>Code Review Feedback</a></span></div>");               
                stringBuilder.Append("<div style='display: inline-block; padding-left: 10px' class=' " + className + "'>");

                for (var i = 0; i < cr.Rating; i++)
                {
                    stringBuilder.Append("<span class='glyphicon glyphicon-star' style='display: inline-block;'></span>");
                }
                 stringBuilder.Append("</div>");
                 stringBuilder.Append("<div style='display: inline-block;' class='text-muted time'>&nbsp; on " + string.Format("{0:ddd, MMM dd yyyy, h:mm}" , cr.AddedOn) + "</div></li>");
            }

            if (response.CodeReviewForTheWeek.Count == 0) stringBuilder.Append("<li class='li-code-review'><div class=''><label class='danger'>No CR added in the week.</label></li>");

            stringBuilder.Append("</ul></div>");
            stringBuilder.Append("</code></div>");

            return stringBuilder.ToString();
        }

        public static List<FeedbackType> GetSystemFeedbackTypes()
        {
            return new List<FeedbackType>
            {
                new FeedbackType
                {
                    FeedbackTypeId = 1,
                    Description = "Comment"
                },
                new FeedbackType
                {
                    FeedbackTypeId = 2,
                    Description = "Skill"
                },
                new FeedbackType
                {
                    FeedbackTypeId = 3,
                    Description = "Assignment"
                },
                new FeedbackType
                {
                    FeedbackTypeId = 4,
                    Description = "Code Review"
                },
                new FeedbackType
                {
                    FeedbackTypeId = 5,
                    Description = "Weekly Feedback"
                }
            };
        }

        public static bool CopyFile(string fileName, string sourcePath, string targetPath)
        {
            if (!String.IsNullOrEmpty(fileName))
            {
                string strPath = AppDomain.CurrentDomain.BaseDirectory;

                targetPath = strPath + targetPath;
                sourcePath = strPath + sourcePath;

                string targetFile = targetPath + fileName;
                string sourceFile = sourcePath + fileName;

                if (!File.Exists(sourceFile))
                {
                    return File.Exists(targetFile);
                }


                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }

                File.Copy(sourceFile, targetFile, true);
                return File.Exists(targetFile);

            }
            return false;
        }

        public static bool DeleteFile(string fileName, string filePath)
        {
            string strPath = AppDomain.CurrentDomain.BaseDirectory;
            filePath = strPath + filePath;
            string sourceFile = filePath + fileName;
            try
            {
                File.Delete(sourceFile);
            }
            catch (Exception e)
            {
                LogUtility.ErrorRoutine(e);
                return false;
            }
            return true;
        }

        public static List<string> UploadFile(HttpFileCollectionBase files)
        {
            //HttpPostedFileBase file = files[1];// work only is single file is uploaded. HttpFileCollectionBase can be used for multiple files
            List<string> fileNamesList = new List<string>();
            try
            {
                //for(HttpPostedFileBase file)
                int count = 0;
                HttpPostedFileBase file;
                while (count < files.Count)//HttpPostedFileBase file in files)
                {
                    file = files[count++];
                    if (file != null && file.ContentLength > 0)
                    {
                        Guid gId;
                        gId = Guid.NewGuid();

                        string extension = Path.GetExtension(file.FileName);
                        fileNamesList.Add(gId.ToString().Trim() + extension);

                        bool folderExists = Directory.Exists(HttpContext.Current.Server.MapPath(LearningAssetsPath.TempFile));

                        if (!folderExists)
                        {
                            Directory.CreateDirectory(HttpContext.Current.Server.MapPath(LearningAssetsPath.TempFile));
                        }
                        var path = Path.Combine(HttpContext.Current.Server.MapPath(LearningAssetsPath.TempFile), fileNamesList.Last());
                        file.SaveAs(path);

                    }
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return null;
            }
            return fileNamesList;
        }
    }
}
