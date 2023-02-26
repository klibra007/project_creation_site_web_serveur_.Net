using System;
using System.Collections.Generic;

#nullable disable

namespace ProjetDeSession.Models
{
    public partial class Category
    {
        public Category()
        {
            Questions = new HashSet<Question>();
        }

        public int CategoryId { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Question> Questions { get; set; }
    }
}
