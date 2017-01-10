
using Bot_Application1.log;
using Model.dataBase;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Bot_Application1.dataBase
{

    [Serializable]
    public class DataBaseControler
    {

        public bool isUserExist(string userId)
        {
            Entities DB = new Entities();
            bool exist = false;
            try
            {
                
                exist = DB.Users.Any(x => x.UserID == userId);
           }
            catch(Exception e)
            {
                Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());

            }

            return exist;

           
        }

        

        public void addNewUser(string channelId,string id,string name,string text)
        {
            Entities DB = new Entities();
            try
            {
               
                var NewUsers = new Users();


                NewUsers.Channel = channelId;
                NewUsers.UserID = id;
                NewUsers.UserName = name;
                NewUsers.created = DateTime.UtcNow;
                NewUsers.Message = text.Truncate(500);

                DB.Users.Add(NewUsers);
                DB.SaveChanges();

            }catch(Exception e)
            {
                Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name,e.ToString());
            }



        }


        public void addNewUser(Users user)
        {
            Entities DB = new Entities();
            try
            {
               

                DB.Users.Add(user);
                DB.SaveChanges();

            }
            catch (Exception e)
            {
                Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
            }



        }



        public Users getUser(string userId)
        {
            Entities DB = new Entities();
            Users NewUsers = new Users();
            List<Users> visitors = new List<Users>();

            try
            {
               
                NewUsers = new Users();
                

                visitors = (from t in DB.Users
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
            Entities DB = new Entities();
            Users NewUsers = new Users();
            List<Users> visitors = new List<Users>();

            try
            {
               
                NewUsers = new Users();

                var itemToRemove = DB.Users.SingleOrDefault(x => x.UserID == userId);

                if (itemToRemove != null)
                {
                    DB.Users.Remove(itemToRemove);
                    DB.SaveChanges();
                }

            }
            catch (Exception e)
            {
                Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());

            }

                       

        }


        internal List<Question> getQuestion(string category)
        {
            Entities DB = new Entities();
            Question question = new Question();
            List<Question> visitors = new List<Question>();

            try
            {
               
                visitors = (from t in DB.Question
                            where t.Category == category
                            select t).ToList();

            }
            catch (Exception e)
            {
                Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());

            }



            return visitors;
        }

        public List<Question> getQuestion(string catgoty, string subCategory)
        {
            Entities DB = new Entities();
            Question question = new Question();
            List<Question> visitors = new List<Question>();

            try
            {
               
                 visitors = (from t in DB.Question
                            where t.Category == catgoty && t.SubCategory == subCategory
                            select t).ToList();
                                                
            }
            catch (Exception e)
            {
                Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());

            }



            return visitors;

        }


        public List<string> getAllCategory()
        {
            Entities DB = new Entities();
            Question question = new Question();
            List<String> visitors = new List<string>();

            try
            {
               
                visitors = (from t in DB.Question
                                    select t.Category).Distinct().ToList();

            }
            catch (Exception e)
            {
                Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());

            }
            
            return visitors;

        }


        public List<string> getAllSubCategory(string catgoty)
        {
            Entities DB = new Entities();
            Question question = new Question();
            List<String> visitors = new List<string>();

            try
            {
               
                visitors = (from t in DB.Question
                            where t.Category == catgoty 
                            select t.SubCategory).Distinct().ToList();

            }
            catch (Exception e)
            {
                Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());

            }

            return visitors;

        }


        public List<string> getMedia( string key,string type,string flags)
        {
            Entities DB = new Entities();
            media media = new media();
            List<String> urls = new List<string>();

            try
            {

                urls = (from t in DB.media
                            where t.type == type && t.mediaKey == key
                        select t.value).Distinct().ToList();

            }
            catch (Exception e)
            {
                Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());

            }

            return urls;

        }


    }
}