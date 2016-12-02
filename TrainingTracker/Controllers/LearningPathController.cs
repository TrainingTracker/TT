using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
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
        
         [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer)]
         public ActionResult CourseEditor()
         {
             return View();
         }

         [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer)]
         public ActionResult CourseEditorNew()
         {
             return View();
         }

         [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer + "," + UserRoles.Trainee)]
         public ActionResult Courses()
         {
             return View();
         }


         [HttpPost]
         [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer)]
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


         public JsonResult GetAllCoursesWithSubtopics()
         {
             return Json(new LearningPathBL().GetAllCoursesWithSubtopics(), JsonRequestBehavior.AllowGet);
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
             return Json(new LearningPathBL().DeleteAssignment(id));
         }

         public JsonResult GetAssignments(int subtopicContentId)
         {
             return Json(new LearningPathBL().GetAssignments(subtopicContentId), JsonRequestBehavior.AllowGet);
         }

         /// <summary>
         /// Request Handler for fetching and filtering courses
         /// </summary>
         /// <param name="searchKeyword">search keyword for free text</param>
         /// <returns>json result for courses set</returns>
         public JsonResult FilterCourses(string searchKeyword)
         {
             return Json(new LearningPathBL().FilterCourses(searchKeyword) , JsonRequestBehavior.AllowGet);
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
                     strFileName = gId.ToString().Trim() + ".jpg";

                     bool folderExists = Directory.Exists(Server.MapPath("~/Uploads/CourseIcon/"));

                     if (!folderExists)
                     {
                         Directory.CreateDirectory(Server.MapPath("~/Uploads/CourseIcon/"));
                     }
                     var path = Path.Combine(Server.MapPath("~/Uploads/CourseIcon/"), strFileName);
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
    }
}