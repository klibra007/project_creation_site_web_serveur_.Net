using System;
using System.Collections.Generic;

#nullable disable

namespace ProjetDeSession.Models
{
    public partial class QuestionQuiz
    {
        public int QuestionId { get; set; }
        public int QuizId { get; set; }

        public virtual Question Question { get; set; }
        public virtual Quiz Quiz { get; set; }
    }
}
