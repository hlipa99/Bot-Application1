using Bot_Application1.dataBase;
using Model.dataBase;
using NLPtest.Models;
using System;
using System.Collections.Generic;

namespace Bot_Application1.IDialog
{
 
    [Serializable]
    internal class EducationController
    {
         DataBaseControler db = new DataBaseControler();
        internal string[] getStudyCategory()
        {
            var res = db.getAllCategory();
            return res.ToArray();
        }

        internal IEnumerable<string> getStudySubCategory(string category)
        {
            var res = db.getAllSubCategory(category);
            return res.ToArray();
        }


        internal Question getQuestion(string category, string subCategory, StudySession studySession)
        {
            List<Question> res;
            if(subCategory == null)
            {
                res = db.getQuestion(category);
            }
            else
            {
                 res = db.getQuestion(category, subCategory);
            }
            res.RemoveAll(x => studySession.QuestionAsked.Contains(x));
            var r = new Random();
            if (res.Count > 0)
            {
                return res[r.Next(res.Count)];
            }
            else
            {
                throw new CategoryOutOfQuestionException();
            }
        }
    }
}