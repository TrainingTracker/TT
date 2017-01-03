using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using TrainingTracker.Authorize;
using TrainingTracker.BLL;
using TrainingTracker.Common.Constants;
using TrainingTracker.Common.Entity;
using TrainingTracker.Common.Utility;

namespace TrainingTracker.Controllers
{
    [CustomAuthorizeAttribute]
    public class LearningPathController : Controller
    {
        [CustomAuthorize(Roles = UserRoles.Manager + "," + UserRoles.Trainer )]
        public ActionResult CourseEditor()
        {
            return View();
        }


        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer)]
        public ActionResult Courses()
        {
            return View();
        }

        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer + "," + UserRoles.Trainee)]
        public ActionResult Course(int courseId)
        {
            return View();
        }

        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer)]
        [HttpPost]
        public JsonResult AddCourse(Course courseToAdd)
        {
            //ToDo: Find a better way to fetch loged in user id
            User currentUser = new UserBl().GetUserByUserName(User.Identity.Name);
            courseToAdd.AddedBy = currentUser.UserId;

            return Json(new LearningPathBL().AddCourse(courseToAdd), JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer)]
        [HttpPost]
        public JsonResult UpdateCourse(Course courseToUpdate)
        {
            // how to know who edited the course
            return Json(new LearningPathBL().UpdateCourse(courseToUpdate));
        }

        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer + "," + UserRoles.Trainee)]
        public JsonResult GetCourseWithSubtopics(int courseId)
        {
            return courseId > 0 ? Json(new LearningPathBL().GetCourseWithSubtopics(courseId), JsonRequestBehavior.AllowGet) : null;
        }

        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer + "," + UserRoles.Trainee)]
        public JsonResult GetCourseWithAllData(int courseId)
        {
            if(courseId > 0)
            {
                User currentUser = new UserBl().GetUserByUserName(User.Identity.Name);
                if (currentUser != null && currentUser.IsTrainee && currentUser.UserId > 0)
                {
                    return Json(new LearningPathBL().GetCourseWithAllData(courseId, currentUser.UserId), JsonRequestBehavior.AllowGet);
                }
                return Json(new LearningPathBL().GetCourseWithAllData(courseId), JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer)]
        public JsonResult GetAllCourses()
        {
            return Json(new LearningPathBL().GetAllCourses(), JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer)]
        [HttpPost]
        public JsonResult AddCourseSubtopic(CourseSubtopic subtopicToAdd)
        {
            User currentUser = new UserBl().GetUserByUserName(User.Identity.Name);
            subtopicToAdd.AddedBy = currentUser.UserId;

            return Json(new LearningPathBL().AddCourseSubtopic(subtopicToAdd), JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer)]
        [HttpPost]
        public JsonResult UpdateCourseSubtopic(CourseSubtopic subtopicToUpdate)
        {
            return Json(new LearningPathBL().UpdateCourseSubtopic(subtopicToUpdate));
        }

        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer)]
        public JsonResult DeleteCourse(int id)
        {

            return Json(new LearningPathBL().DeleteCourse(id), JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer)]
        public JsonResult DeleteCourseSubtopic(int id)
        {
            return Json(new LearningPathBL().DeleteCourseSubtopic(id), JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer)]
        public JsonResult GetSubtopicContents(int subtopicId)
        {
            return Json(new LearningPathBL().GetSubtopicContents(subtopicId), JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer)]
        [HttpPost]
        public JsonResult AddSubtopicContent(SubtopicContent dataToAdd)
        {
            User currentUser = new UserBl().GetUserByUserName(User.Identity.Name);
            dataToAdd.AddedBy = currentUser.UserId;
            int id;
            new LearningPathBL().AddSubtopicContent(dataToAdd, out id);
            return Json(id);
        }

        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer)]
        [HttpPost]
        public JsonResult UpdateSubtopicContent(SubtopicContent dataToUpdate)
        {
            return Json(new LearningPathBL().UpdateSubtopicContent(dataToUpdate));
        }

        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer)]
        public JsonResult DeleteSubtopicContent(int id)
        {
            return Json(new LearningPathBL().DeleteSubtopicContent(id), JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer)]
        [HttpPost]
        public JsonResult AddAssignment(Assignment dataToAdd)
        {
            User currentUser = new UserBl().GetUserByUserName(User.Identity.Name);
            dataToAdd.AddedBy = currentUser.UserId;
            int id;
            new LearningPathBL().AddAssignment(dataToAdd, out id);
            return Json(id);
        }

        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer)]
        [HttpPost]
        public JsonResult UpdateAssignment(Assignment dataToUpdate)
        {
            return Json(new LearningPathBL().UpdateAssignment(dataToUpdate));
        }

        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer)]
        public JsonResult DeleteAssignment(int id)
        {
            return Json(new LearningPathBL().DeleteAssignment(id), JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer)]
        public JsonResult GetAssignments(int subtopicContentId)
        {
            return Json(new LearningPathBL().GetAssignments(subtopicContentId), JsonRequestBehavior.AllowGet);
        }

        //public JsonResult UpdateAssignmentProgress(Assignment data)
        //{
        //    User currentUser = new UserBl().GetUserByUserName(User.Identity.Name);
        //    return Json(new LearningPathBL().UpdateAssignmentProgress(data, currentUser))            
        //}

        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer)]
        public JsonResult SaveSubtopicOrder(List<CourseSubtopic> data)
        {

            return Json(new LearningPathBL().SaveSubtopicOrder(data), JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer)]
        public JsonResult SaveSubtopicContentOrder(List<SubtopicContent> data)
        {

            return Json(new LearningPathBL().SaveSubtopicContentOrder(data), JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer)]
        public JsonResult PublishCourse(int id)
        {

            return Json(new LearningPathBL().PublishCourse(id), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Request Handler for fetching and filtering courses
        /// </summary>
        /// <param name="searchKeyword">search keyword for free text</param>
        /// <returns>json result for courses set</returns>
        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer)]
        public JsonResult FilterCourses(string searchKeyword)
        {
            return Json(new LearningPathBL().FilterCourses(searchKeyword), JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer + "," + UserRoles.Trainee)]
        [HttpPost]
        public JsonResult UploadImage()
        {
            HttpPostedFileBase file = Request.Files[0];// work only is single file is uploaded. HttpFileCollectionBase can be used for multiple files
            string strFileName = string.Empty;
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    Guid gId;
                    gId = Guid.NewGuid();

                    string extension = Path.GetExtension(file.FileName);
                    strFileName = gId.ToString().Trim()  + extension;

                    bool folderExists = Directory.Exists(Server.MapPath(LearningAssetsPath.CourseIcon));

                    if (!folderExists)
                    {
                        Directory.CreateDirectory(Server.MapPath(LearningAssetsPath.CourseIcon));
                    }
                    var path = Path.Combine(Server.MapPath(LearningAssetsPath.CourseIcon), strFileName);
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

        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer + "," + UserRoles.Trainee)]
        [HttpPost]
        public JsonResult UploadFile()
        {
            return Json(UtilityFunctions.UploadFile(Request.Files));
        }

        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer + "," + UserRoles.Trainee)]
        public FileResult DownloadAssignment(string fileName)
        {
            var FileVirtualPath = LearningAssetsPath.AppRootToAssignment + fileName;
            return File(FileVirtualPath, "application/force-download", Path.GetFileName(FileVirtualPath));
        }

        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer + "," + UserRoles.Trainee)]
        public JsonResult SaveSubtopicContentProgress(int subtopicContentId)
        {
            User currentUser = new UserBl().GetUserByUserName(User.Identity.Name);
            if (currentUser != null && currentUser.IsTrainee && currentUser.UserId > 0 && subtopicContentId > 0)
            {
                return Json( new LearningPathBL().SaveSubtopicContentProgress(subtopicContentId, currentUser.UserId), JsonRequestBehavior.AllowGet);
            }
            return Json(false);
        }
    }
}