﻿/**************************************************************************************************
*   Created By : Satyabrata                                                                                                                                                           
*   Created On : 1 Sept 2016
*   Modified By :
*   Modified Date:
*   Description: Release controller file containing Action methods for Release.
**************************************************************************************************/
using TrainingTracker.BLL;
using System.Web.Mvc;
using TrainingTracker.Common.Entity;
using TrainingTracker.Authorize;

namespace TrainingTracker.Controllers
{
    [CustomAuthorize]
    public class ReleaseController : BaseController
    {
        /// <summary>
        /// Action method for Index which rendering all releases in a table grid.
        /// </summary>
        /// <returns>Return view of all releases.</returns>
        public ActionResult Index()
        {
            return View("Release");
        }

        /// <summary>
        /// Action method for GetAllReleases.
        /// </summary>
        /// <returns>Return list of releases as JSON object.</returns>
        public ActionResult GetReleaseOnFilter(string keyword,int releaseId,int pageNumber)
        {
            return Json(new ReleaseBl().GetReleaseOnFilter(keyword, releaseId, pageNumber), JsonRequestBehavior.AllowGet);
        }
      
        /// <summary>
        /// Action method for AddRelease, which adds new release.
        /// </summary>
        /// <param name="release">Release object</param>
        /// <returns> Return a boolean value as a JSON object.</returns>
        [HttpPost]
        public ActionResult AddRelease(Release release)
        {
            return Json(new ReleaseBl().AddRelease(release , CurrentUser.UserId));
        }

    }
}