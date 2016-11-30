using System.Collections.Generic;
using TrainingTracker.Common.Entity;

namespace TrainingTracker.DAL.Interface
{
    public interface IQuestionDal
    {
        List<Question> GetQuestionsBySkillAndExperience(int skillId, int startExperience, int endExperience);
        List<Question> GetQuestionsBySkillAndUserId(int skillId, int userId);
        List<Question> GetQuestionsByUserId(int userId);
        bool AddQuestion(Question question);
        bool AddQuestionMapping(List<QuestionLevelMapping> mappings, int questionId);
    }
}