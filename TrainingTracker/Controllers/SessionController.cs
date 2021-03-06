﻿
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using System.IO.Directory.CreateDirectory;
using TrainingTracker.Authorize;
using TrainingTracker.BLL;
using TrainingTracker.Common.Constants;
using TrainingTracker.Common.Entity;
using TrainingTracker.Common.Utility;

namespace TrainingTracker.Controllers
{
    /// <summary>
    /// Controller class for session
    /// </summary>
    [CustomAuthorize]
    public class SessionController : BaseController
    {
        /// <summary>
        /// Action method for Index
        /// </summary>
        /// <returns>View Results</returns>
        public ActionResult Index()
        {
            return View("Sessions");
        }

        /// <summary>
        /// Get Sessions for user on filter
        /// </summary>
        /// <returns>Json result</returns>
        public ActionResult GetSessionsOnFilter(int pageNumber, int seminarType, string searchKeyword = "", int sessionId = 0)
        {
            return Json(new SessionBl().GetSessionOnFilter(pageNumber, seminarType, sessionId, searchKeyword, CurrentUser), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get Sessions for user on filter
        /// </summary>
        /// <returns>Json result</returns>
        public ActionResult GetSessionVm(int pageNumber, int seminarType, string searchKeyword = "", int sessionId = 0)
        {
            return Json(new SessionBl().GetSessionVm(pageNumber, seminarType, sessionId, searchKeyword, CurrentUser), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///Add or Edit Session
        /// </summary>
        /// <returns>Json result</returns>
        public ActionResult AddNewSession(Session sessionDetails)
        {
            return Json(new SessionBl().AddNewSession(sessionDetails, CurrentUser), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        ///Add or Edit Session
        /// </summary>
        /// <returns>Json result</returns>
        public ActionResult UpdateSessionDetails(Session sessionDetails)
        {
            return Json(new SessionBl().UpdateSessionsDetails(sessionDetails, CurrentUser), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Action method for upload session video file.
        /// </summary>
        /// <param name="fileName">Contain parameter fileName as HttpPostedFileBase object</param>
        /// <returns>Return filename as JSON object.</returns>
        /// [HttpPost]
        /// 
        public ActionResult UploadVideo(HttpPostedFileBase fileName)
        {
                HttpPostedFileBase file = Request.Files["file"];

                string[] allowedFormat = new string[] { ".mp4", ".ogg",".webm"};

                if (!allowedFormat.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new Exception("Format Not Allowed");
                }


                if (file != null && file.ContentLength > 0)
                {
                    Guid gId = Guid.NewGuid();
                    string strFileName = gId.ToString().Trim() + FileExtensions.Mp4;

                    if (!Directory.Exists(Server.MapPath(SessionAssets.VideoPath)))
                    {
                        Directory.CreateDirectory(Server.MapPath(SessionAssets.VideoPath));
                    }

                    file.SaveAs(Path.Combine(Server.MapPath(SessionAssets.VideoPath), strFileName));


                    return Json(strFileName);
                }
            throw new Exception("File Missing");
        }


        /// <summary>
        /// Action method for upload session slide.
        /// </summary>
        /// <param name="fileName">Contain parameter fileName as HttpPostedFileBase object</param>
        /// <returns>Return filename as JSON object.</returns>
        [HttpPost]
        public ActionResult UploadSlide(HttpPostedFileBase fileName)
        {
            HttpPostedFileBase file = Request.Files["file"];

           
                string[] allowedFormat = new string[]{".ppt",".pptx"};

                if (!allowedFormat.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase)))
                {
                    throw new Exception("Format Not Allowed");
                }

                if (file != null && file.ContentLength > 0)
                {
                    Guid gId = Guid.NewGuid();
                    string strSlideName = gId.ToString().Trim() + ".ppt";

                    if (!Directory.Exists(Server.MapPath(SessionAssets.SlidePath)))
                    {
                        Directory.CreateDirectory(Server.MapPath(SessionAssets.SlidePath));
                    }

                    file.SaveAs(Path.Combine(Server.MapPath(SessionAssets.SlidePath), strSlideName));

                    return Json(strSlideName);
                }
                throw new Exception("File Missing");
        }
    }
}