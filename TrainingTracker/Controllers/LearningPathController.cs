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
    [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer)]
    public class LearningPathController : Controller
    {

        public ActionResult CourseEditor()
        {
            return View();
        }


        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer + "," + UserRoles.Trainee)]
        public ActionResult Courses()
        {
            return View();
        }

        public ActionResult Course(int courseId)
        {
            return View();
        }


        [HttpPost]
        public JsonResult AddCourse(Course courseToAdd)
        {
            //ToDo: Find a better way to fetch loged in user id
            User currentUser = new UserBl().GetUserByUserName(User.Identity.Name);
            courseToAdd.AddedBy = currentUser.UserId;

            return Json(new LearningPathBL().AddCourse(courseToAdd), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult UpdateCourse(Course courseToUpdate)
        {
            // how to know who edited the course
            return Json(new LearningPathBL().UpdateCourse(courseToUpdate));
        }

        public JsonResult GetCourseWithSubtopics(int courseId)
        {
            return courseId > 0 ? Json(new LearningPathBL().GetCourseWithSubtopics(courseId), JsonRequestBehavior.AllowGet) : null;
        }

        public JsonResult GetCourseWithAllData(int courseId)
        {
            return courseId > 0 ? Json(new LearningPathBL().GetCourseWithAllData(courseId), JsonRequestBehavior.AllowGet) : null;
        }

        public JsonResult GetAllCourses()
        {
            return Json(new LearningPathBL().GetAllCourses(), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult AddCourseSubtopic(CourseSubtopic subtopicToAdd)
        {
            User currentUser = new UserBl().GetUserByUserName(User.Identity.Name);
            subtopicToAdd.AddedBy = currentUser.UserId;

            return Json(new LearningPathBL().AddCourseSubtopic(subtopicToAdd), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult UpdateCourseSubtopic(CourseSubtopic subtopicToUpdate)
        {
            return Json(new LearningPathBL().UpdateCourseSubtopic(subtopicToUpdate));
        }

        public JsonResult DeleteCourse(int id)
        {

            return Json(new LearningPathBL().DeleteCourse(id), JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteCourseSubtopic(int id)
        {
            return Json(new LearningPathBL().DeleteCourseSubtopic(id), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetSubtopicContents(int subtopicId)
        {
            return Json(new LearningPathBL().GetSubtopicContents(subtopicId), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult AddSubtopicContent(SubtopicContent dataToAdd)
        {
            User currentUser = new UserBl().GetUserByUserName(User.Identity.Name);
            dataToAdd.AddedBy = currentUser.UserId;
            int id;
            new LearningPathBL().AddSubtopicContent(dataToAdd, out id);
            return Json(id);
        }


        [HttpPost]
        public JsonResult UpdateSubtopicContent(SubtopicContent dataToUpdate)
        {
            return Json(new LearningPathBL().UpdateSubtopicContent(dataToUpdate));
        }


        public JsonResult DeleteSubtopicContent(int id)
        {
            return Json(new LearningPathBL().DeleteSubtopicContent(id), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult AddAssignment(Assignment dataToAdd)
        {
            User currentUser = new UserBl().GetUserByUserName(User.Identity.Name);
            dataToAdd.AddedBy = currentUser.UserId;
            int id;
            new LearningPathBL().AddAssignment(dataToAdd, out id);
            return Json(id);
        }

        [HttpPost]
        public JsonResult UpdateAssignment(Assignment dataToUpdate)
        {
            return Json(new LearningPathBL().UpdateAssignment(dataToUpdate));
        }

        public JsonResult DeleteAssignment(int id)
        {
            return Json(new LearningPathBL().DeleteAssignment(id), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAssignments(int subtopicContentId)
        {
            return Json(new LearningPathBL().GetAssignments(subtopicContentId), JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveSubtopicOrder(List<CourseSubtopic> data)
        {

            return Json(new LearningPathBL().SaveSubtopicOrder(data), JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveSubtopicContentOrder(List<SubtopicContent> data)
        {

            return Json(new LearningPathBL().SaveSubtopicContentOrder(data), JsonRequestBehavior.AllowGet);
        }

        public JsonResult PublishCourse(int id)
        {

            return Json(new LearningPathBL().PublishCourse(id), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Request Handler for fetching and filtering courses
        /// </summary>
        /// <param name="searchKeyword">search keyword for free text</param>
        /// <returns>json result for courses set</returns>
        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer + "," + UserRoles.Trainee)]
        public JsonResult FilterCourses(string searchKeyword)
        {
            return Json(new LearningPathBL().FilterCourses(searchKeyword), JsonRequestBehavior.AllowGet);
        }

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


        [HttpPost]
        public JsonResult UploadFile()
        {
            return Json(UtilityFunctions.UploadFile(Request.Files));
        }


        public FileResult DownloadAssignment(string fileName)
        {
            var FileVirtualPath = LearningAssetsPath.AppRootToAssignment + fileName;
            return File(FileVirtualPath, "application/force-download", Path.GetFileName(FileVirtualPath));
        }
    }
}