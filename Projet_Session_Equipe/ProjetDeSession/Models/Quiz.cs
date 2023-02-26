using System;
using System.Collections.Generic;

#nullable disable

namespace ProjetDeSession.Models
{
    public partial class Quiz
    {
        public Quiz()
        {
            Answers = new HashSet<Answer>();
            QuestionQuizzes = new HashSet<QuestionQuiz>();
        }

        public int QuizId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<QuestionQuiz> QuestionQuizzes { get; set; }
    }
}
