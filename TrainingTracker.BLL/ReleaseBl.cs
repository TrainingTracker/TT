/**************************************************************************************************
*   Created By : Satyabrata                                                                                                                                                           
*   Created On : 2 Sept 2016
*   Modified By :
*   Modified Date:
*   Description: Business class for Release.
**************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using TrainingTracker.BLL.Base;
using TrainingTracker.Common.Entity;


namespace TrainingTracker.BLL
{
    public class ReleaseBl : BussinessBase
    {

        /// <summary>
        /// Function which adds new release and put an entry in notification table if the IsPublished field is true .
        /// </summary>
        /// <param name="release">Contain parameter as release object</param>
        /// <param name="userId">UserId</param>
        /// <returns>Returns true if release and notification are added successfully else false 
        /// and if the IsPublished field is false then it depends only on Release entry .</returns>
        public PagedResult<Release> AddRelease(Release release, int userId)
        {
            release.AddedBy = new User { UserId = userId };

            Release lastRelease = ReleaseConverter.ConvertFromCore(UnitOfWork.ReleaseRepository.GetRecentRelease());


            if (lastRelease == null) throw new Exception("There should be existing release");

            DAL.EntityFramework.Release coreRelease = new DAL.EntityFramework.Release
            {
                AddedBy = release.AddedBy.UserId,
                Description = release.Description,
                Title = release.ReleaseTitle,
                IsPublished = true,
                ReleaseDate = DateTime.Now

            };

            switch (release.ReleaseType)
            {
                case (int)Common.Enumeration.ReleaseType.Major:
                    coreRelease.Major = ++lastRelease.Major;
                    coreRelease.Minor = 0;
                    coreRelease.Patch = 0;
                    break;

                case (int)Common.Enumeration.ReleaseType.Minor:
                    coreRelease.Major = lastRelease.Major;
                    coreRelease.Minor = ++lastRelease.Minor;
                    coreRelease.Patch = 0;
                    break;

                case (int)Common.Enumeration.ReleaseType.Patch:
                    coreRelease.Major = lastRelease.Major;
                    coreRelease.Minor = lastRelease.Minor;
                    coreRelease.Patch = ++lastRelease.Patch;
                    break;
            }

            UnitOfWork.ReleaseRepository.Add(coreRelease);
            UnitOfWork.Commit();

            release.IsNew = true;
            release.IsPublished = true;
            release.ReleaseId = coreRelease.ReleaseId;
            new NotificationBl().AddReleaseNotification(release, userId);

            return GetReleaseOnFilter("", 0, 1);

        }


        public PagedResult<Release> GetReleaseOnFilter(string wildcard, int searchReleaseId, int pageNumber)
        {
            var release = UnitOfWork.ReleaseRepository.GetPagedFilteredSessions(wildcard, searchReleaseId, pageNumber, 6);

            if (release == null) return new PagedResult<Release>();

            return new PagedResult<Release>
            {
                CurrentPage = release.CurrentPage,
                PageCount = release.PageCount,
                PageSize = release.PageSize,
                RowCount = release.RowCount,
                Results = release.Results == null
                           ? new List<Release>()
                           : ReleaseConverter.ConvertListFromCore(release.Results.ToList())
            };
        }
    }
}
