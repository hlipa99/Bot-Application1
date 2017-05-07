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
            try
            {
                var log = new OtherLog();
                log.time = DateTime.UtcNow;
                log.data = v;
                new DataBaseController().addOtherLog(log);
            }
            catch (Exception ex)
            {

            }
        }

        public static void addErrorLog(string context,string error)
        {
            try
            {
                var log = new ErrorLog();
                log.time = DateTime.UtcNow;
                log.context = context;
                log.error = error;
                System.Diagnostics.Trace.TraceError(log.error);

                new DataBaseController().addErrorLog(log);
            } catch(Exception ex)
            {

            }
}

        public static void addAnswerOutput(string answerText, string userAnswer, AnswerFeedback feedback,string userID,string QuestionID)
        {
            try
            {
                var log = new answersLog();
                log.time = DateTime.UtcNow;
                log.userAnswer = userAnswer;
                log.question = answerText;
                log.missingEntities = "";
                log.entities = feedback.score.ToString();
                log.userID = userID;
                log.questionID = QuestionID;
                foreach (var e in feedback.missingEntitis)
                {
                    log.missingEntities += ";" + e.entityType + "#" + e.entityValue + ";";
                }
                new DataBaseController().addAnswerLog(log);
                System.Diagnostics.Trace.TraceInformation("question:" + log.question + ",userAnswer:" + log.userAnswer + "score:" + log.entities);

            }
            catch (Exception ex)
            {

            }
        }
    }
}
