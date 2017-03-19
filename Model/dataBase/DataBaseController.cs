﻿


//using Bot_Application1.Exceptions;
//using Bot_Application1.log;
using Model;
using Model.dataBase;
using Model.Models;
using NLP.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
//using static Bot_Application1.Controllers.ConversationController;

namespace Model.dataBase
{

    [Serializable]
    public class DataBaseController
    {
        static object syncLock = new object();
        static Entities8 DB;
        static DataBaseController controller;

        public static void setStubInstance(DataBaseController ctrl)
        {
            controller = ctrl;
        }

        public static DataBaseController getInstance()
        {

            lock (syncLock)
            {
                if (controller == null)
                {
                   
                    controller = new DataBaseController();
                    DB = new Entities8();
                    return controller;
                }
                else
                {
                    return controller;
                }
            }
             
        }

        internal void addAnswerLog(answersLog log)
        {
            DB.answersLog.Add(log);
            DB.SaveChanges();
        }

        public virtual bool isUserExist(string userId)
        {
           
            bool exist = false;
            try
            {

                exist = DB.User.Any(x => x.UserID == userId);
            }
            catch (Exception e)
            {
             //   Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new DBException();
            }

            return exist;


        }

        public virtual void saveEntitiesFromQuestions(List<entity> Entities)
        {
            var questions = DB.SubQuestion;
            foreach(var e in Entities)
            {
                if(e.entityValue.Length < 50)
                DB.entity.Add(e);
            }
            try
            {
                DB.SaveChangesAsync();
            }catch(Exception ex)
            {

            }
        }
         


        public virtual void addNewUser(string channelId, string id, string name)
        {
            try
            {

                var NewIUser = new User();
                NewIUser.UserID = id;
                NewIUser.UserName = name;
                NewIUser.UserCreated = DateTime.UtcNow;

                DB.User.Add(NewIUser);
                DB.SaveChanges();

            }
            catch (Exception e)
            {
             //   Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new DBException();
            }



        }


        public virtual void addNewEntity(string value, string type)
        {

            try
            {

               

                if (DB.entity.Count(x => x.entitySynonimus.Contains(value)) == 0)
                {
                    var entity = new entity();
                    entity.entityValue = value;
                    entity.entitySynonimus = ";" + value + ";";
                    entity.entityType = type;
                    DB.entity.Add(entity);
                    DB.SaveChanges();
                }
              

            }
            catch (Exception e)
            {
             //   Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new DBException();
            }



        }



        public virtual void addNewUser(IUser user)
        {

            try
            {


                DB.User.Add((User)user);
                DB.SaveChanges();

            }
            catch (Exception e)
            {
            //    Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new DBException();
            }



        }



        public virtual IUser getUser(string userId)
        {
            IUser NewIUser = new User();
            List<User> visitors = new List<User>();

            try
            {

                NewIUser = new User();


                visitors = (from t in DB.User
                            where t.UserID == userId
                            select t).ToList();
            }
            catch (Exception e)
            {
              //  Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new DBException();
            }


            return visitors[0];

        }



        public virtual void deleteUser(string userId)
        {

            IUser NewIUser = new User();
            List<IUser> visitors = new List<IUser>();

            try
            {

                NewIUser = new User();

                var itemToRemove = DB.User.SingleOrDefault(x => x.UserID == userId);

                if (itemToRemove != null)
                {
                    DB.User.Remove(itemToRemove);
                    DB.SaveChanges();
                }

            }
            catch (Exception e)
            {
           //     Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new DBException();
            }



        }


        public virtual IQuestion[] getQuestion(string category)
        {

            IQuestion question = new Question();
            Question[] questions = null;

            try
            {

                questions = (from t in DB.Question
                            where t.Category == category
                             //   &&   t.Flags == "sorcePic"   //TODO tempfix
                             select t).ToArray();

            }
            catch (Exception e)
            {
           //     Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new DBException();
            }



            return questions;
        }

        public virtual IQuestion[] getQuestion(string catgoty, string subCategory)
        {

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
            //    Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new DBException();
            }



            return questions;

        }


        public virtual string[] getAllCategory()
        {

            Question question = new Question();
            string[] catagories = null;

            try
            {

                catagories = (from t in DB.Question
                            select t.Category).Distinct().ToArray();

            }
            catch (Exception e)
            {
            //    Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new DBException();
            }

            return catagories;

        }

        public virtual IEnumerable<Ientity> getEntitys()
        {
            var entityList = new List<Ientity>();
            foreach(var e in DB.entity)
            {
                entityList.Add((Ientity)e);
            }
            return entityList.ToArray();
        }

        public virtual string[] getAllSubCategory(string catgoty)
        {
 
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
               // Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new DBException();
            }

            return catagory;

        }


        public virtual string[] getMedia(string key, string type, string flags)
        {

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
             //   Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new DBException();
            }

            return urls;

        }

        public virtual ISubQuestion[] getAllSubQuestions()
        {
     //       var x = System.Configuration.ConfigurationManager.ConnectionStrings;

            return DB.SubQuestion.ToArray();
        }

        public virtual string[] getBotPhrase(Pkey pKey, string[] flags, string[] flagsNot)
        {
     
            media media = new media();
            string[] phrases = null;
            var key = Enum.GetName(typeof(Pkey), pKey).ToLower();
            try
            {
                //improvse runtime 
                if(flags.Length + flagsNot.Length > 0)
                {
                    phrases = (from t in DB.botphrase
                               where t.Pkey.ToLower() == key && flags.All(x => t.Flags.Contains(x)) && flagsNot.All(x => !t.Flags.Contains(x))
                               select t.Text).ToArray();
                }else
                {
                    phrases = (from t in DB.botphrase
                               where t.Pkey.ToLower() == key && !t.Flags.Contains("text")
                               select t.Text).ToArray();
                }
 
            }
            catch (Exception e)
            {
             //   Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw e;
            }

            return phrases;

        }

        internal void addOtherLog(OtherLog log)
        {
         //   var DB = new Entities();
            DB.OtherLog.Add(log);
            DB.SaveChanges();
        }
    }
}