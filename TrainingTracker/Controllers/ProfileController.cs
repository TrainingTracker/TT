﻿using System;
using System.IO;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Microsoft.SqlServer.Server;
using TrainingTracker.Authorize;
using TrainingTracker.BLL;
using TrainingTracker.Common.Constants;
using TrainingTracker.Common.Entity;
using TrainingTracker.Common.Utility;

namespace TrainingTracker.Controllers
{
    [CustomAuthorizeAttribute]
    public class ProfileController : BaseController
    {

        // GET: UserProfile?userId=
        [CustomAuthorize(Roles = UserRoles.Administrator+","+UserRoles.Manager+","+UserRoles.Trainer+","+UserRoles.Trainee)]
        public ActionResult UserProfile(int userId)
        {
            return View("Profile");
        }
        /// <summary>
        /// Manage users profile.
        /// </summary>
        /// <returns></returns>
        public ActionResult ManageProfile()
        {
            return PartialView("_PartialProfile");
        }
        /// <summary>
        /// Shows all the profiles.
        /// </summary>
        /// <returns></returns>        
        public ActionResult AllProfiles()
        {
            return View("AllProfiles");
        }

        /// <summary>
        /// Customize profile setting
        /// </summary>
        /// <returns>Customize View</returns>        
        public ActionResult CustomizeProfile()
        {
            return View("CustomizeProfile");
        }


        public ActionResult AddEditProfile()
        {
            return View();
        }
        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="userData"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer)]
        public ActionResult CreateUser(User userData)
        {
            int userId;
            bool status = false;
            status = new UserBl().AddUser(userData ,CurrentUser.UserId , out userId);
            var data = new
            {
                userId = userId ,
                status = status
            };

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Updates the existing user.
        /// </summary>
        /// <param name="userData">Input user object</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateUser(User userData)
        {            
            return Json(new { status= new UserBl().UpdateUser(userData,CurrentUser.UserId)});
        }

        [HttpPost]
        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer)]
        public ActionResult AddFeedback(Feedback feedbackPost)
        {
            return Json(new FeedbackBl().AddFeedback(feedbackPost) ? "true" : "false");
        }

        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer)]
        public ActionResult GetManageProfileVm()
        {
            return Json(new UserBl().GetManageProfileVm(CurrentUser) , JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer)]
        public ActionResult GetAllUsersByTeam()
        {
            return Json(new UserBl().GetAllUsersByTeam(CurrentUser) , JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// ActionMethod for GetActiveUsers
        /// </summary>
        /// <returns> Returns list of active user as json object.</returns>
        [HttpGet]
        public ActionResult GetActiveUsers()
        {
            
            return Json(new UserBl().GetActiveUsers(CurrentUser) , JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Action to return user profile View model
        /// </summary>
        /// <param name="userId">user id of logged user</param>
        /// <returns>Json Result for loggin User</returns>
        public ActionResult GetUserProfileVm(int userId)
        {
            return Json(UserBl.GetUserProfileVm(userId, CurrentUser), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Action method to handle xhr request for fetching filters based on filter  condition
        /// </summary>
        /// <param name="pageSize">no of records to return</param>
        /// <param name="feedbackType">type of feedback</param>
        /// <param name="userId">user if for feedback to be fetched</param>
        /// <param name="startDate">start date range</param>
        /// <param name="endDate">end date range</param>
        /// <returns></returns>
        [CustomAuthorize]
        public JsonResult GetUserFeedbackOnFilter(int pageSize, int feedbackType, int userId, DateTime? startDate = null, DateTime? endDate = null)
        {
            return Json(new UserBl().GetUserFeedbackOnFilter(userId , pageSize , feedbackType , startDate , endDate) , JsonRequestBehavior.AllowGet);
        }

        //Added for Upload Image 
        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase fileName)
        {
            HttpPostedFileBase file = Request.Files["file"];
            string strFileName = string.Empty;
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    Guid gId;
                    gId = Guid.NewGuid();
                    strFileName = gId.ToString().Trim() + ".jpg";

                    bool folderExists = Directory.Exists(Server.MapPath("~/Uploads/ProfilePicture/"));

                    if (!folderExists)
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Uploads/ProfilePicture/"));
                    }
                    var path = Path.Combine(Server.MapPath("~/Uploads/ProfilePicture/"), strFileName);
                    file.SaveAs(path);
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return null;
            }
            return Json(strFileName);
        }

        /// <summary>
        /// Method to handle xhr request for plot service.
        /// </summary>
        /// <param name="traineeId">trainee's id</param>
        /// <param name="startDate">start date</param>
        /// <param name="endDate">end date</param>
        /// <param name="arrayFeedbackType">array of feedback type</param>
        /// <param name="trainerId">trainer id</param>
        /// <returns>returns json results, contains feedback based on applied filters</returns>
        public JsonResult GetUserFeedbackOnFilterForPlot(int traineeId, DateTime? startDate, DateTime? endDate,
                                                         string arrayFeedbackType, int trainerId)
        {
            return Json(new UserBl().GetUserFeedbackOnFilterForPlot(traineeId , startDate , endDate , arrayFeedbackType , trainerId) , JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Action Method to fetch Feedback and its thread data.
        /// </summary>
        /// <param name="feedbackId">feedback Id</param>
        /// <returns></returns>
        public JsonResult GetFeedbackWithThreads(int feedbackId )
        {
            return Json(new FeedbackBl().GetFeedbackWithThreads(feedbackId , CurrentUser) , JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Action MEthod to fetch Feedback's threads
        /// </summary>
        /// <param name="feedbackId"></param>
        /// <returns></returns>
        public JsonResult GetFeedbackThreads(int feedbackId)
        {
            return Json(new FeedbackBl().GetFeedbackThreads(feedbackId , CurrentUser) , JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Action method to handle new thread add Request
        /// </summary>
        /// <param name="thread">instance of thread from UI</param>
        /// <returns>Success event of threads</returns>
        public JsonResult AddNewThread(Threads thread)
        {
            thread.AddedBy =new User
                                    {
                                      UserId   = CurrentUser.UserId
                                    };

            return Json(new FeedbackBl().AddNewThread(thread) , JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Fetch weekly Survey Questions for the team
        ///  </summary>
        /// <param name="traineeId">trainee id</param>
        /// <param name="startDate">start date</param>
        /// <param name="endDate">end date</param>
        /// <returns></returns>
        [HttpGet]
        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer)]
        public JsonResult FetchWeeklySurveyQuestionForTeam(int traineeId,DateTime startDate,DateTime endDate)
        {
            return Json(new SurveyBl().FetchWeeklySurveyQuestionForTeam(traineeId,
                                                                        startDate,
                                                                        endDate,
                                                                        CurrentUser.TeamId??0) , JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Fetch feedback Response preview
        /// </summary>
        /// <param name="surveyResponse">instance of survey respons</param>
        /// <returns>Json Result</returns>
        public JsonResult FetchWeeklyFeedbackPreview( SurveyResponse surveyResponse )
        {
            return Json(new SurveyBl().FetchWeeklyFeedbackPreview(surveyResponse) , JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Saves Response for the weekly feedback
        /// </summary>
        /// <param name="surveyResponse">instance of survey respons</param>
        /// <returns>Json Result</returns>
        public JsonResult SaveWeeklySurveyResponseForTrainee(SurveyResponse surveyResponse)
        {
            return Json(new SurveyBl().SaveWeeklySurveyResponseForTrainee(surveyResponse) , JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPosts(string wildcard, int traineeId, int statusId, int searchPostId, int pageNumber)
        {
            return Json(DiscussionForumBl.GetFilteredPagedPosts(wildcard, statusId, searchPostId, traineeId, pageNumber, 5), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPostById(int postId)
        {
            return Json(DiscussionForumBl.GetPostWithThreads(postId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddPost(ForumPost post)
        {
            return Json(DiscussionForumBl.AddPost(post, CurrentUser).ToString());
        }

        [HttpPost]
        public ActionResult UpdatePostStatus(int postId, int statusId, string message, int addedFor)
        {
            return Json(DiscussionForumBl.UpdatePostStatus(postId, statusId, message,addedFor, CurrentUser).ToString());
        }

        [HttpPost]
        public ActionResult AddPostThread(ForumThread postThread)
        {
            return Json(DiscussionForumBl.AddPostThread(postThread, CurrentUser).ToString());
        }

        public JsonResult GetAllSkills()
        {
            return Json(new UserBl().GetAllSkills(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Fetch all the members working under a lead from GPS API
        /// </summary>
        /// <returns> Returns list of users working under a lead as json object.</returns>
        [HttpGet]
        public async System.Threading.Tasks.Task<ActionResult> GetMembersUnderLead()
        {
            return Json(await new UserBl().GetMembersUnderLead(CurrentUser.EmployeeId), JsonRequestBehavior.AllowGet);
        }
                
        /// <summary>
        /// Updates the EmployeeId of TT members that matches with GPS API data
        /// </summary>
        /// <returns>Json Result</returns>
        [HttpPost]
        public async System.Threading.Tasks.Task<ActionResult> SyncGPSUsers() 
        {
            return Json(await new UserBl().SyncGPSUsers(CurrentUser), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Get All Designation
        /// </summary>
        /// <returns>returns list of all designation</returns>
        public JsonResult GetAllDesignation()
        {
            return Json(new UserBl().GetAllDesignation(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]       
        public JsonResult GetUserByUserId(int userId)
        {
            return Json(new UserBl().GetUserByUserId(userId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomAuthorize(Roles = UserRoles.Manager + "," + UserRoles.Trainer)]
        public JsonResult SubmitCodeReviewMetaData(CodeReview codeReview)
        {
            codeReview.AddedBy = CurrentUser;
            return Json(new FeedbackBl().SubmitCodeReviewMetaData(codeReview), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomAuthorize(Roles = UserRoles.Manager + "," + UserRoles.Trainer)]
        public JsonResult SubmitCodeReviewPoint(CodeReviewPoint codeReviewPoint)
        {
            return Json(new FeedbackBl().SubmitCodeReviewPoint(codeReviewPoint), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult FetchCodeReviewPreview(int codeReviewId, bool isFeedback)
        {
            return Json(new FeedbackBl().FetchCodeReviewPreview(codeReviewId, isFeedback), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomAuthorize(Roles = UserRoles.Manager + "," + UserRoles.Trainer)]
        public JsonResult SubmitCodeReviewFeedback(CodeReview codeReview)
        {
            return Json(new FeedbackBl().SubmitCodeReviewFeedback(codeReview), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomAuthorize(Roles = UserRoles.Manager + "," + UserRoles.Trainer)]
        public JsonResult DiscardCodeReviewFeedback(int codeReviewId)
        {
            return Json(new FeedbackBl().DiscardCodeReviewFeedback(codeReviewId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomAuthorize(Roles = UserRoles.Manager + "," + UserRoles.Trainer)]
        public JsonResult DiscardTagFromCodeReviewFeedback(int codeReviewId,int codeReviewTagId)
        {
            return Json(new FeedbackBl().DiscardTagFromCodeReviewFeedback(codeReviewId,codeReviewTagId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddCategory(Skill category)
        {
            return Json(new UserBl().AddSkill(category));
        }

        [HttpPost]
        [CustomAuthorize(Roles = UserRoles.Manager + "," + UserRoles.Trainer)]
        public ActionResult FetchPrevCodeReviewData(int traineeId,int[] ratingFilter, int count=5)
        {
            ratingFilter = ratingFilter ?? new int[] {};

            return Json(new FeedbackBl().GetPrevCodeReviewDataForTrainee(traineeId,ratingFilter, count), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CustomAuthorize(Roles = UserRoles.Manager + "," + UserRoles.Trainer)]
        public JsonResult CalculateCodeReviewRating(CodeReview codeReview)
        {
            return Json(new FeedbackBl().CalculateCodeReviewRating(codeReview), JsonRequestBehavior.AllowGet);
        }
    }
}