﻿using System;
using System.Collections.Generic;

#nullable disable

namespace ProjetDeSession.Models
{
    public partial class Answer
    {
        public int AnswerId { get; set; }
        public int? OptionId { get; set; }
        public int? QuizId { get; set; }

        public virtual ItemOption Option { get; set; }
        public virtual Quiz Quiz { get; set; }
    }
}
