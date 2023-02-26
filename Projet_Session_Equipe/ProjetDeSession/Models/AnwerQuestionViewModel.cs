using System.Collections.Generic;

namespace ProjetDeSession.Models
{
    public class AnwerQuestionViewModel
    {
        public QuestionQuiz QuestionQuiz { get; set; }
        public List<ItemOption> QuestionOptions { get; set;}
        public int selectedOptionId { get; set; }

    }
}
