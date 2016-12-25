using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Application1.dataBase
{
    public class DataBaseControler
    {

        public bool isUserExist(string userId)
        {
            bool exist = false;
            try
            {
                Models.BotDataEntities1 DB = new Models.BotDataEntities1();
                exist = DB.UserLogs.Any(x => x.UserID == userId);
           }
            catch(Exception e)
            {
                //add loger
                
            }

            return exist;

           
        }

        public  void addNewUser(string channelId,string id,string name,string text)
        {
            try
            {
                Models.BotDataEntities1 DB = new Models.BotDataEntities1();
                Models.UserLog NewUserLog = new Models.UserLog();


                NewUserLog.Channel = channelId;
                NewUserLog.UserID = id;
                NewUserLog.UserName = name;
                NewUserLog.created = DateTime.UtcNow;
                NewUserLog.Message = text.Truncate(500);

                DB.UserLogs.Add(NewUserLog);
                DB.SaveChanges();

            }catch(Exception e)
            {
                //add loger
            }



        }

        public Models.UserLog getUser(string userId)
        {
            Models.UserLog NewUserLog = new Models.UserLog();
            List<Models.UserLog> visitors = new List<Models.UserLog>();

            try
            {
                Models.BotDataEntities1 DB = new Models.BotDataEntities1();
                NewUserLog = new Models.UserLog();

                visitors = (from t in DB.UserLogs
                            where t.UserID == userId
                            select t).ToList();

                

                

            }
            catch (Exception e)
            {
                //add loger

            }

           

            return visitors[0] ;

        }


        public List<Models.Question> getQuestion(string catgoty, string subCategory)
        {
            Models.Question question = new Models.Question();
            List<Models.Question> visitors = new List<Models.Question>();

            try
            {
                Models.BotDataEntities1 DB = new Models.BotDataEntities1();
                 visitors = (from t in DB.Questions
                            where t.Category == catgoty && t.SubCategory == subCategory
                            select t).ToList();





            }
            catch (Exception e)
            {
                //add loger

            }



            return visitors;

        }


    }
}