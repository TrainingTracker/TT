/**************************************************************************************************
*   Created By : Satyabrata                                                                                                                                                           
*   Created On : 5 Sept 2016
*   Modified By :
*   Modified Date:
*   Description: Interface class for Release.
**************************************************************************************************/
using System.Collections.Generic;
using TrainingTracker.Common.Entity;
namespace TrainingTracker.DAL.Interface
{
    public interface IReleaseDal
    {
        /// <summary>
        /// Gets all Releases.
        /// </summary>
        /// <returns>List of all release.</returns>
        List<Release> GetAllReleases();

        /// <summary>
        /// Adds new Release.
        /// </summary>
        /// <param name="release">Release object</param>
        /// <returns>returns release Id</returns>
        int AddRelease(Release release);

        /// <summary>
        /// Update release
        /// </summary>
        /// <param name="release">Release object</param>
        /// <returns>returns boolean value</returns>
        bool UpdateRelease(Release release);
    }
}
