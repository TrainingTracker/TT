using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.RepoInterface;

namespace TrainingTracker.DAL.Interface
{
    public interface IEmailContentRepository:IRepository<EmailContent>
    {

    }
}
