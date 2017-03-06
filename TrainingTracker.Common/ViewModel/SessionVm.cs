using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingTracker.Common.Entity;

namespace TrainingTracker.Common.ViewModel
{
    public class SessionVm
    {
        public PagedResult<Session> SessionList { get; set; }
        public Session DefaultSession { get; set;}
        public List<User> AllAttendees { get; set; }
    }
}
