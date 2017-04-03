
using Model.dataBase;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.Models
{
 
    public interface IStudySession
    {

        List<IQuestion> QuestionAsked { get; set; }
        IQuestion CurrentQuestion { get; set; }
        ISubQuestion CurrentSubQuestion { get; set; }
        int SessionLength { get; set;}

      string SubCategory { get; set;}
      string Category { get; set;}
        DateTime startTime { get; set; }
    }
}