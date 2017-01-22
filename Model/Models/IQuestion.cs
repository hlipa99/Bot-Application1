using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public interface IQuestion
    {
        bool Equals(object other);
        int GetHashCode();

        int ID { get; set; }
        string Category { get; set; }
        string SubCategory { get; set; }
        string QuestionText { get; set; }
        string AnswerText { get; set; }
        int AnswerScore { get; set; }
    }


}
