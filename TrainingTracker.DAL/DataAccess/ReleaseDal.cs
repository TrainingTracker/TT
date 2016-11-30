/**************************************************************************************************
*   Created By : Satyabrata                                                                                                                                                           
*   Created On : 5 Sept 2016
*   Modified By :
*   Modified Date:
*   Description: Data Access class for Release.
**************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using TrainingTracker.Common.Entity;
using TrainingTracker.Common.Utility;
using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.Interface;

namespace TrainingTracker.DAL.DataAccess
{
    public class ReleaseDal : IReleaseDal
    {
        /// <summary>
        /// Function GetsAllReleases which returns list of releases.
        /// </summary>
        /// <returns>Returns list of releases.</returns>
        public List<Common.Entity.Release> GetAllReleases()
        {
            try
            {
                using (TrainingTrackerEntities context = new TrainingTrackerEntities())
                {
                    return context.Releases
                      .Select(x => new Common.Entity.Release
                                          {
                                              ReleaseId = x.ReleaseId,
                                              Major = x.Major,
                                              Minor = x.Minor,
                                              Patch = x.Patch,
                                              ReleaseTitle = x.Title,
                                              Description = x.Description,
                                              ReleaseDate = x.ReleaseDate,
                                              IsPublished = x.IsPublished,
                                              SortOrder = (x.Major == 0 && x.Minor == 0 && x.Patch == 0) ? 0 : 1,
                                              AddedBy = new  Common.Entity.User
                                                             {
                                                                 UserId = x.AddedBy,
                                                                 FullName = x.User.FirstName + " " + x.User.LastName
                                                             }
                                          })
                     .OrderBy(x => x.SortOrder)
                     .ThenByDescending(x => x.Major)
                     .ThenByDescending(x => x.Minor)
                     .ThenByDescending(x => x.Patch)
                     .ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return null;
            }
        }

        /// <summary>
        /// Adds new release and returns an boolean status,
        /// from which we can know release was added successfully or not.
        /// </summary>
        /// <param name="release">Release class object</param>
        /// <returns>Return true if a release is added successfully else false.</returns>
        public int AddRelease(Common.Entity.Release release)
        {
            try
            {
                using (TrainingTrackerEntities context = new TrainingTrackerEntities())
                {
                    var releaseDetail = new EntityFramework.Release
                    {
                        Major = release.Major,
                        Minor = release.Minor,
                        Patch = release.Patch,
                        Title = release.ReleaseTitle,
                        Description = release.Description,
                        IsPublished = release.IsPublished,
                        ReleaseDate = release.ReleaseDate,
                        AddedBy = release.AddedBy.UserId
                    };

                    context.Releases.Add(releaseDetail);
                    context.SaveChanges();
                    return releaseDetail.ReleaseId;
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return 0;
            }
        }

        /// <summary>
        /// Update release and returns an boolean status,
        /// from which we can know release was updated successfully or not.
        /// </summary>
        /// <param name="release">Release class object</param>
        /// <returns>Return true if a release is updated successfully else false.</returns>
        public bool UpdateRelease(Common.Entity.Release release)
        {
            try
            {
                using (TrainingTrackerEntities context = new TrainingTrackerEntities())
                {
                    var releaseContext = context.Releases.FirstOrDefault(x => x.ReleaseId == release.ReleaseId);

                    if (releaseContext == null) return false;

                    releaseContext.Title = release.ReleaseTitle;
                    releaseContext.Description = release.Description;
                    releaseContext.Major = release.Major;
                    releaseContext.Minor = release.Minor;
                    releaseContext.Patch = release.Patch;
                    releaseContext.ReleaseDate = release.ReleaseDate;
                    releaseContext.IsPublished = release.IsPublished;

                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return false;
            }
        }
    }
}
