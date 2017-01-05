using Bot_Application1.log;
using Bot_Application1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                Models.mindcetEntities DB = new Models.mindcetEntities();
                exist = DB.UserLog.Any(x => x.UserID == userId);
           }
            catch(Exception e)
            {
                Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());

            }

            return exist;

           
        }

        

        public void addNewUser(string channelId,string id,string name,string text)
        {
            try
            {
                Models.mindcetEntities DB = new Models.mindcetEntities();
                Models.UserLog NewUserLog = new Models.UserLog();


                NewUserLog.Channel = channelId;
                NewUserLog.UserID = id;
                NewUserLog.UserName = name;
                NewUserLog.created = DateTime.UtcNow;
                NewUserLog.Message = text.Truncate(500);

                DB.UserLog.Add(NewUserLog);
                DB.SaveChanges();

            }catch(Exception e)
            {
                Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name,e.ToString());
            }



        }

        public void addNewUser(UserLog user)
        {
            try
            {
                Models.mindcetEntities DB = new Models.mindcetEntities();

                DB.UserLog.Add(user);
                DB.SaveChanges();

            }
            catch (Exception e)
            {
                Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
            }



        }



        public Models.UserLog getUser(string userId)
        {
            Models.UserLog NewUserLog = new Models.UserLog();
            List<Models.UserLog> visitors = new List<Models.UserLog>();

            try
            {
                Models.mindcetEntities DB = new Models.mindcetEntities();
                NewUserLog = new Models.UserLog();
                

                visitors = (from t in DB.UserLog
                            where t.UserID == userId
                            select t).ToList();
       }
            catch (Exception e)
            {
                Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());

            }

           
            return visitors[0] ;

        }



        public void deleteUser(string userId)
        {
            Models.UserLog NewUserLog = new Models.UserLog();
            List<Models.UserLog> visitors = new List<Models.UserLog>();

            try
            {
                Models.mindcetEntities DB = new Models.mindcetEntities();
                NewUserLog = new Models.UserLog();

                var itemToRemove = DB.UserLog.SingleOrDefault(x => x.UserID == userId);

                if (itemToRemove != null)
                {
                    DB.UserLog.Remove(itemToRemove);
                    DB.SaveChanges();
                }

            }
            catch (Exception e)
            {
                Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());

            }

                       

        }



        public List<Models.Table> getQuestion(string catgoty, string subCategory)
        {
            Models.Table question = new Models.Table();
            List<Models.Table> visitors = new List<Models.Table>();

            try
            {
                Models.mindcetEntities DB = new Models.mindcetEntities();
                 visitors = (from t in DB.Table
                            where t.C_Category == catgoty && t.SubCategory == subCategory
                            select t).ToList();
                                                
            }
            catch (Exception e)
            {
                Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());

            }



            return visitors;

        }


        public List<string> getAllCategory(string catgoty)
        {
            Models.Table question = new Models.Table();
            List<String> visitors = new List<string>();

            try
            {
                Models.mindcetEntities DB = new Models.mindcetEntities();
                visitors = (from t in DB.Table
                                    select t.C_Category).ToList();

            }
            catch (Exception e)
            {
                Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());

            }
            
            return visitors;

        }


        public List<string> getAllSubCategory(string catgoty)
        {
            Models.Table question = new Models.Table();
            List<String> visitors = new List<string>();

            try
            {
                Models.mindcetEntities DB = new Models.mindcetEntities();
                visitors = (from t in DB.Table
                            where t.C_Category == catgoty 
                            select t.SubCategory).ToList();

            }
            catch (Exception e)
            {
                Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());

            }

            return visitors;

        }


    }
}