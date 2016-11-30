using System.Collections.Generic;
using TrainingTracker.Common.Entity;

namespace TrainingTracker.Common.ViewModel
{
    /// <summary>
    /// View model class for Manage profile
    /// </summary>
   public class ManageProfileVm
    {
       /// <summary>
       /// Gets and sets all user for VM
       /// </summary>
       public List<User> AllUser { get; set; }

       /// <summary>
       /// Gets and Sets All Team
       /// </summary>
       public List<Team> AllTeams { get; set; }
    }
}
