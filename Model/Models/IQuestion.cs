
using Model.dataBase;
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

        string QuestionID { get; set; }
        string Category { get; set; }
        string SubCategory { get; set; }
        string QuestionText { get; set; }
        int AnswerScore { get; set; }

        ICollection<SubQuestion> SubQuestion { get; set; }
        int Enumerator { get; set; }
    }


}
