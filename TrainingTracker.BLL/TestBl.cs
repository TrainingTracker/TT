using TrainingTracker.BLL.Base;

namespace TrainingTracker.BLL
{
    public class TestBl : BussinessBase
    {
        private void TestMethod()
        {
            var courses = UnitOfWork.CourseRepository.GetCourses(1, 10);
            var activeCourses = UnitOfWork.CourseRepositoryWithoutRepo.Find(x => x.IsActive);
        }
    }
}