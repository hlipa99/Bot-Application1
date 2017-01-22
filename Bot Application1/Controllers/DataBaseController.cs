
using Bot_Application1.dataBase;
using Bot_Application1.Exceptions;
using Bot_Application1.log;
using Model;
using Model.dataBase;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using static Bot_Application1.Controllers.ConversationController;

namespace Bot_Application1.Controllers
{

    [Serializable]
    public class DataBaseController
    {

        public bool isUserExist(string userId)
        {
            Entities DB = new Entities();
            bool exist = false;
            try
            {

                exist = DB.Users.Any(x => x.UserID == userId);
            }
            catch (Exception e)
            {
                Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new DBException();
            }

            return exist;


        }



        public void addNewUser(string channelId, string id, string name, string text)
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

            }
            catch (Exception e)
            {
                  Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new DBException();
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
                throw new DBException();
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
                throw new DBException();
            }


            return visitors[0];

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
                throw new DBException();
            }



        }


        internal Question[] getQuestion(string category)
        {
            Entities DB = new Entities();
            Question question = new Question();
            Question[] questions = null;

            try
            {

                questions = (from t in DB.Question
                            where t.Category == category
                            select t).ToArray();

            }
            catch (Exception e)
            {
                Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new DBException();
            }



            return questions;
        }

        public Question[] getQuestion(string catgoty, string subCategory)
        {
            Entities DB = new Entities();
            Question question = new Question();
            Question[] questions = null;

            try
            {

                questions = (from t in DB.Question
                            where t.Category == catgoty && t.SubCategory == subCategory
                            select t).ToArray();

            }
            catch (Exception e)
            {
                Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new DBException();
            }



            return questions;

        }


        public string[] getAllCategory()
        {
            Entities DB = new Entities();
            Question question = new Question();
            string[] catagories = null;

            try
            {

                catagories = (from t in DB.Question
                            select t.Category).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new DBException();
            }

            return catagories;

        }


        public string[] getAllSubCategory(string catgoty)
        {
            Entities DB = new Entities();
            Question question = new Question();
            string[] catagory = null;

            try
            {

                catagory = (from t in DB.Question
                            where t.Category == catgoty
                            select t.SubCategory).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new DBException();
            }

            return catagory;

        }


        public string[] getMedia(string key, string type, string flags)
        {
            Entities DB = new Entities();
            media media = new media();
            string[] urls = new string[] { };

            try
            {

                urls = (from t in DB.media
                        where t.type == type && t.mediaKey == key
                        select t.value).Distinct().ToArray();

            }
            catch (Exception e)
            {
                Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new DBException();
            }

            return urls;

        }

        public string[] getBotPhrase(Pkey pKey, string[] flags, string[] flagsNot)
        {
            Entities DB = new Entities();
            media media = new media();
            string[] phrases = null;
            var key = Enum.GetName(typeof(Pkey), pKey);
            try
            {
                //improvse runtime 
                if(flags.Length + flagsNot.Length > 0)
                {
                    phrases = (from t in DB.botphrase
                               where t.Pkey == key && flags.All(x => t.Flags.Contains(x)) && flagsNot.All(x => !t.Flags.Contains(x))
                               select t.Text).ToArray();
                }else
                {
                    phrases = (from t in DB.botphrase
                               where t.Pkey == key
                               select t.Text).ToArray();
                }
 
            }
            catch (Exception e)
            {
                Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());

            }

            return phrases;

        }

    }
}