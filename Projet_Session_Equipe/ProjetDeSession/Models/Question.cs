using System;
using System.Collections.Generic;

#nullable disable

namespace ProjetDeSession.Models
{
    public partial class Question
    {
        public Question()
        {
            ItemOptions = new HashSet<ItemOption>();
            QuestionQuizzes = new HashSet<QuestionQuiz>();
        }

        public int QuestionId { get; set; }
        public string Text { get; set; }
        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<ItemOption> ItemOptions { get; set; }
        public virtual ICollection<QuestionQuiz> QuestionQuizzes { get; set; }
    }
}
