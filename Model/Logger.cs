using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;
using Model.dataBase;

namespace Model
{
    public static class Logger
    {
        public static void addLog(string v)
        {
            var log = new OtherLog();
            log.time = DateTime.UtcNow;
            log.data = v;
            DataBaseController.getInstance().addOtherLog(log);
        }

        public static void addAnswerOutput(string answerText, string userAnswer, AnswerFeedback feedback)
        {
            var log = new answersLog();
            log.time = DateTime.UtcNow;
            log.userAnswer = userAnswer;
            log.question = answerText;
            log.missingEntities = "";
            log.entities = feedback.score.ToString();
            foreach (var e in feedback.missingEntitis)
            {
                log.missingEntities += ";" + e.entityType + "#" + e.entityValue + ";";
            }
            DataBaseController.getInstance().addAnswerLog(log);
        }
    }
}
