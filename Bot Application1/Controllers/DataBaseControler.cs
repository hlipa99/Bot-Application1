
using Bot_Application1.log;
using Model.dataBase;
using System;
using System.Collections.Generic;
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
            bool exist = false;
            try
            {
                var DB = new Entities();
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
            try
            {
                var DB = new Entities();
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
            try
            {
                var DB = new Entities();

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
            Users NewUsers = new Users();
            List<Users> visitors = new List<Users>();

            try
            {
                var DB = new Entities();
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
            Users NewUsers = new Users();
            List<Users> visitors = new List<Users>();

            try
            {
                var DB = new Entities();
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
            Question question = new Question();
            List<Question> visitors = new List<Question>();

            try
            {
                var DB = new Entities();
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
            Question question = new Question();
            List<Question> visitors = new List<Question>();

            try
            {
                var DB = new Entities();
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
            Question question = new Question();
            List<String> visitors = new List<string>();

            try
            {
                var DB = new Entities();
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
            Question question = new Question();
            List<String> visitors = new List<string>();

            try
            {
                var DB = new Entities();
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


    }
}