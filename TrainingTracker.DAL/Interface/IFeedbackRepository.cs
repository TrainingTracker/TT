using TrainingTracker.DAL.EntityFramework;

namespace TrainingTracker.DAL.Interface
{
   public interface IFeedbackRepository:IRepository<Feedback>
    {
        Feedback LoadFeedbackAndThreads(int feedbackId);
    }
}
