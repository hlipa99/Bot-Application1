
using Bot_Application1.dataBase;
using Model.dataBase;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NLPtest.Models
{
 
    public interface IStudySession
    {

      HashSet<IQuestion> IQuestionAsked { get; set; }
      IQuestion ICurrentQuestion { get; set; }
      ISubQuestion ICurrentSubQuestion { get; set; }
      int SessionLength { get; set;}

      string SubCategory { get; set;}
      string Category { get; set;}
       
    }
}