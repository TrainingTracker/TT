using System.Web.Mvc;
using TrainingTracker.Authorize;
using TrainingTracker.BLL;
using TrainingTracker.Common.Constants;
using TrainingTracker.Common.Entity;

namespace TrainingTracker.Controllers
{
    /// <summary>
    /// Controller class for Questions, inherit Controller of MVC  framework
    /// </summary>
    [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer)]
    public class QuestionsController : Controller
    {
        /// <summary>
        /// Action Method for controllers  Default
        /// </summary>
        /// <returns>View Result</returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Request handlers for Get Question VM xhr request
        /// </summary>
        /// <returns>Json Result</returns>
        public ActionResult GetQuestionsVm()
        {
            return Json(new QuestionBl().GetQuestionVm(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Request Handler for fetching Questions based on Skill and Expereince
        /// </summary>
        /// <param name="skillId">skill id</param>
        /// <param name="startExperience">start experience</param>
        /// <param name="endExperience">end Experience</param>
        /// <returns>Json Result</returns>
        public ActionResult GetQuestionsBySkillAndExperience(int skillId, int startExperience, int endExperience)
        {
            return Json(new QuestionBl().GetQuestionsBySkillAndExperience(skillId, startExperience, endExperience), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Request handler for Get Questions By Skill and uset Id
        /// </summary>
        /// <param name="skillId">skill Id</param>
        /// <param name="userId">user id</param>
        /// <returns>Json Result</returns>
        public ActionResult GetQuestionsBySkillAndUserId(int skillId, int userId)
        {
            return Json(new QuestionBl().GetQuestionsBySkillAndUserId(skillId, userId), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Request handler for fetching Questions by User Id
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>Json Result</returns>
        public ActionResult GetQuestionsByUserId(int userId)
        {
            return Json(new QuestionBl().GetQuestionsByUserId(userId), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Request handler for Add Questions
        /// </summary>
        /// <param name="question">Instnce of Question entity</param>
        /// <returns>Json Result</returns>
        [HttpPost]
        public ActionResult AddQuestion(Question question)
        {
            return Json(new QuestionBl().AddQuestion(question).ToString());
        }

        /// <summary>
        /// Request Handler for Adding Category
        /// </summary>
        /// <param name="category">Instance of skill</param>
        /// <returns>Json Result</returns>
        [HttpPost]
        public ActionResult AddCategory(Skill category)
        {
            return Json(new QuestionBl().AddSkill(category).ToString());
        }
    }
}