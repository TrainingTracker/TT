/**************************************************************************************************
*   Created By : Satyabrata                                                                                                                                                           
*   Created On : 2 Sept 2016
*   Modified By :
*   Modified Date:
*   Description: Business class for Release.
**************************************************************************************************/
using System;
using System.Collections.Generic;
using TrainingTracker.BLL.Base;
using TrainingTracker.Common.Entity;


namespace TrainingTracker.BLL
{
    public class ReleaseBl : BussinessBase
    {
        /// <summary>
        /// Function which returns list of releases.
        /// </summary>
        /// <returns>It returns list of releases.</returns>
        public List<Release> GetAllReleases()
        {
            return ReleaseDataAccesor.GetAllReleases();
        }

        /// <summary>
        /// Function which adds new release and put an entry in notification table if the IsPublished field is true .
        /// </summary>
        /// <param name="release">Contain parameter as release object</param>
        /// <param name="userId">UserId</param>
        /// <returns>Returns true if release and notification are added successfully else false 
        /// and if the IsPublished field is false then it depends only on Release entry .</returns>
        public bool AddRelease(Release release, int userId)
        {
            release.AddedBy = new User { UserId = userId };

            try
            {
                int releaseId = ReleaseDataAccesor.AddRelease(release);

                release.IsNew = release.ReleaseId == 0;
                release.ReleaseId = releaseId;
                return new NotificationBl().AddReleaseNotification(release , userId);
            }
            catch (Exception)
            {
                return false;
            }            
        }

        /// <summary>
        /// Function which updates release and put an entry in notification table if added successfully in relese table .
        /// </summary>
        /// <param name="release">Contain parameter as release object</param>
        /// <param name="userId">UserId</param>
        /// <returns></returns>
        public bool UpdateRelease(Release release, int userId)
        {
            return  (ReleaseDataAccesor.UpdateRelease(release) && new NotificationBl().AddReleaseNotification(release, userId));
        }
    }
}
