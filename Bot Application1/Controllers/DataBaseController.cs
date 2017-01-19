
using Bot_Application1.dataBase;
using Bot_Application1.Exceptions;
using Model;
using Model.dataBase;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Bot_Application1.Controllers
{

    [Serializable]
    public class DataBaseController
    {

        public virtual bool isUserExist(string userId)
        {
            Entities DB = new Entities();
            bool exist = false;
            try
            {

                exist = DB.Users.Any(x => x.UserID == userId);
            }
            catch (Exception e)
            {
         //       Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new DBException(e.ToString());
            }

            return exist;


        }



        public virtual void addNewUser(string channelId, string id, string name, string text)
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
        //          Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new DBException(e.ToString());
            }



        }


        public virtual void addNewUser(Users user)
        {
            Entities DB = new Entities();
            try
            {


                DB.Users.Add(user);
                DB.SaveChanges();

            }
            catch (Exception e)
            {
          //      Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new DBException(e.ToString());
            }



        }



        public virtual IUser getUser(string userId)
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
           //     Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new DBException(e.ToString());
            }


            return visitors[0];

        }



        public virtual void deleteUser(string userId)
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
         //       Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new DBException(e.ToString());
            }



        }


        public virtual IQuestion[] getQuestion(string category)
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
           //     Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new DBException(e.ToString());
            }



            return questions;
        }

        public virtual IQuestion[] getQuestion(string catgoty, string subCategory)
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
             //   Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new DBException(e.ToString());
            }



            return questions;

        }


        public virtual string[] getAllCategory()
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
          //      Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new DBException(e.ToString());
            }

            return catagories;

        }


        public virtual string[] getAllSubCategory(string catgoty)
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
           //     Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new DBException(e.ToString());
            }

            return catagory;

        }


        public virtual string[] getMedia(string key, string type, string flags)
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
           //     Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new DBException(e.ToString());
            }

            return urls;

        }

        public virtual string[] getBotPhrase(Pkey pKey, string[] flags, string[] flagsNot)
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
             //   Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new DBException(e.ToString());
            }

            return phrases;

        }

    }
}