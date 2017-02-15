using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.RepoInterface;

namespace TrainingTracker.DAL.Interface
{
   public interface IFeedbackRepository:IRepository<Feedback>
    {
        Feedback LoadFeedbackAndThreads(int feedbackId);
    }
}
