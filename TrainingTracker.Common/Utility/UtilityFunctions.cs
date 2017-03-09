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
                 stringBuilder.Append("<div style='display: inline-block;' class='text-muted time'>&nbsp; on " + string.Format("{0:ddd, MMM dd yyyy, h:mm tt}" , cr.AddedOn) + "</div></li>");
            }

            if (response.CodeReviewForTheWeek.Count == 0) stringBuilder.Append("<li class='li-code-review'><div class=''><label class='danger'>No CR added in the week.</label></li>");

            stringBuilder.Append("</ul></div>");
            stringBuilder.Append("</code></div>");

            return stringBuilder.ToString();
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
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                while (count < files.Count)//HttpPostedFileBase file in files)
                {
                    file = files[count++];
                    if (file != null && file.ContentLength > 0)
                    {
                        Guid gId;
                        gId = Guid.NewGuid();

                        string extension = Path.GetExtension(file.FileName);
                        fileNamesList.Add(gId.ToString().Trim() + extension);

                        bool folderExists = Directory.Exists(basePath + LearningAssetsPath.TempFile);

                        if (!folderExists)
                        {
                            Directory.CreateDirectory(basePath + LearningAssetsPath.TempFile);
                        }
                        var path = basePath + LearningAssetsPath.TempFile + fileNamesList.Last();
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

        public static string GenerateHtmlForCourseFeedback(Course course)
        {
            StringBuilder strBuilder =new StringBuilder();
            strBuilder.Append("<div class='weekly-feedback'><code>");

            strBuilder.Append("<div class='feedback-zone'>");
            strBuilder.Append("<div class='col-xs-3'><span class='spn-course-data'>" + string.Format("{0:ddd, MMM dd yyyy, h:mm tt}", course.StartedDateTime) + "</span><label class='lbl-data-info'>Course Started</label></div>");
            strBuilder.Append("<div class='col-xs-3'><span class='spn-course-data'>" + string.Format("{0:ddd, MMM dd yyyy, h:mm tt}", course.CompletedDateTime) + "</span><label class='lbl-data-info'>Course Completed</label></div>");
            strBuilder.Append("<div class='col-xs-3'><span class='spn-course-data'>" + course.CourseSubtopics.SelectMany(x => x.SubtopicContents).Count() + "</span><label class='lbl-data-info'>Links/SubTopics</label></div>");
            strBuilder.Append("<div class='col-xs-3'><span class='spn-course-data'>" + course.CourseSubtopics.SelectMany(x => x.Assignments).Count() + "</span><label class='lbl-data-info'>Assignments</label></div>");
            strBuilder.Append("</div>");
           
            strBuilder.Append("<div id='divSurveyCodeReview' class='feedback-zone'><div class='feedback-question'><label>Assignments Reviews for the course</label></div>");

            StringBuilder feedbackStringBuilder = new StringBuilder();
            foreach (var subtopic in course.CourseSubtopics)
            {
                foreach (var assignment in subtopic.Assignments)
                {
                    feedbackStringBuilder.Append("<label>Assignment: " + assignment.Name + "</label>");
                    feedbackStringBuilder.Append("<ul class='feedback-notes'>");
                    foreach (var feedback in assignment.Feedback)
                    {
                        feedbackStringBuilder.Append("<li class='li-code-review'>");

                        string className = "";

                        if (feedback.FeedbackType.FeedbackTypeId != (int) Common.Enumeration.FeedbackType.Comment)
                        {
                            switch (feedback.Rating)
                            {
                                case 1:
                                    className = "rating-slow";
                                    break;
                                case 2:
                                    className = "rating-Average";
                                    break;
                                case 3:
                                    className = "rating-Fast";
                                    break;
                                case 4:
                                    className = "rating-Exceptional";
                                    break;
                            }
                        }


                        feedbackStringBuilder.Append("<div style='display: inline-block;' class='title ' ><strong><a href='/Profile/UserProfile?userId=" + feedback.AddedBy.UserId + "'>" + feedback.AddedBy.FullName + "</a>&nbsp;</strong></div>");
                        feedbackStringBuilder.Append("<div style='display: inline-block;' class='text-muted time ' > Added <span onclick='my.profileVm.loadFeedbackWithThread(" + feedback.FeedbackId + ")'><a href='#" + feedback.AddedBy.UserId + "'>" + (feedback.Rating > 0 ? "Assignment Feedback" : "Reassignment Comment") + "</a></span></div>");
                        feedbackStringBuilder.Append("<div style='display: inline-block; padding-left: 10px' class=' " + className + "'>");

                        for (var i = 0; i < feedback.Rating; i++)
                        {
                            feedbackStringBuilder.Append("<span class='glyphicon glyphicon-star' style='display: inline-block;'></span>");
                        }

                        feedbackStringBuilder.Append("</div>");
                        feedbackStringBuilder.Append("<div style='display: inline-block;' class='text-muted time'>&nbsp; on " + string.Format("{0:ddd, MMM dd yyyy, h:mm tt}", feedback.AddedOn) + "</div></li>");
                    }
                    feedbackStringBuilder.Append("</ul>");
                }
            }

            strBuilder.Append(feedbackStringBuilder.ToString().Length > 0
                ? feedbackStringBuilder.ToString()
                : "<ul class='feedback-notes'><li class='li-code-review'><div><label class='danger'>No Assignments Reviews for the course.</label></div></li></ul>");

            strBuilder.Append("</div>");
            strBuilder.Append("</code></div>");
            return strBuilder.ToString();
        }

        public static string SubstituteTemplateWithReplacements(StringBuilder body, Dictionary<string, string> substitutions)
        {           
            foreach (var substitution in substitutions)
            {
                body.Replace(substitution.Key, substitution.Value);
            }
            return body.ToString();
        }

        public static StringBuilder FetchEmailTemplateFromPath(string path)
        {
            try
            {
                StringBuilder line = new StringBuilder();
                using (StreamReader rwOpenTemplate = new StreamReader(HttpContext.Current.Server.MapPath(path)))
                {
                    while (!rwOpenTemplate.EndOfStream)
                    {
                        line.Append(rwOpenTemplate.ReadToEnd());
                    }
                }
                return line;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
