using System;
using System.IO;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using TrainingTracker.Authorize;
using TrainingTracker.BLL;
using TrainingTracker.Common.Constants;
using TrainingTracker.Common.Entity;
using TrainingTracker.Common.Utility;

namespace TrainingTracker.Controllers
{
    [CustomAuthorizeAttribute]
    public class ProfileController : Controller
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
            status = new UserBl().AddUser(userData , out userId);
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
            return Json(new { status= new UserBl().UpdateUser(userData)});
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
            User currentUser = new UserBl().GetUserByUserName(User.Identity.Name);

            return Json(new UserBl().GetManageProfileVm(currentUser) , JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer)]
        public ActionResult GetAllUsersByTeam()
        {
            return Json(new UserBl().GetAllUsersByTeam(new UserBl().GetUserByUserName(User.Identity.Name)) , JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// ActionMethod for GetActiveUsers
        /// </summary>
        /// <returns> Returns list of active user as json object.</returns>
        [HttpGet]
        public ActionResult GetActiveUsers()
        {
            return Json(new UserBl().GetActiveUsers(new UserBl().GetUserByUserName(User.Identity.Name)) , JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Action to return user profile View model
        /// </summary>
        /// <param name="userId">user id of logged user</param>
        /// <returns>Json Result for loggin User</returns>
        public ActionResult GetUserProfileVm(int userId)
        {
            return Json(new UserBl().GetUserProfileVm(userId), JsonRequestBehavior.AllowGet);
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
            return Json(new FeedbackBl().GetFeedbackWithThreads(feedbackId , new UserBl().GetUserByUserName(User.Identity.Name)) , JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Action MEthod to fetch Feedback's threads
        /// </summary>
        /// <param name="feedbackId"></param>
        /// <returns></returns>
        public JsonResult GetFeedbackThreads(int feedbackId)
        {
            return Json(new FeedbackBl().GetFeedbackThreads(feedbackId , new UserBl().GetUserByUserName(User.Identity.Name)) , JsonRequestBehavior.AllowGet);
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
                                      UserId   = new UserBl().GetUserByUserName(User.Identity.Name).UserId
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
                                                                        new UserBl().GetUserByUserName(User.Identity.Name).TeamId??0) , JsonRequestBehavior.AllowGet);
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
    }
}