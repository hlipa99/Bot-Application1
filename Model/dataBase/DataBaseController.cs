


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
using System.Threading.Tasks;
using System.Runtime.Caching;
using System.Web;
using Bot_Application1;
//using static Bot_Application1.Controllers.ConversationController;

namespace Model.dataBase
{

    [Serializable]
    public class DataBaseController
    {
        static object syncLock = new object();
        Entities8 DB;
    //    static DataBaseController controller;

        public DataBaseController()
        {
            DB = new Entities8();
            DB.Configuration.AutoDetectChangesEnabled = false;
        //    DB.Configuration.LazyLoadingEnabled = false;
        }

        public void updateSampleQuestion(answersLog a, int score)
        {
            var log = DB.answersLog.Where(l => l.id == a.id).Single();
            log.entities = score.ToString();
            DB.SaveChanges();
        }


        public void setStubInstance(Entities8 text)
        {
            DB = text;
        }


        internal async void addErrorLog(ErrorLog log)
        {
            try
            {
                DB.ErrorLog.Add(log);
                await DB.SaveChangesAsync();

            }
            catch (Exception ex)
            {

            }
        }

        internal async void addAnswerLog(answersLog log)
        {

            try
            {
                DB.answersLog.Add(log);
                await DB.SaveChangesAsync();

            }
            catch (Exception ex)
            {

            }
         
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

        public void addUpdateQuestion(Question question)
        {
       



            if (question.QuestionID == null)
            {

                question.QuestionID = (DB.Question.Count()+1).ToString();
                var c = DB.Question.Count();

                var i = 1;
                foreach (var sq in question.SubQuestion)
                {
                    sq.questionID = question.QuestionID;
                    sq.Question = question;
                    sq.subQuestionID = i.ToString();
                    i++;
                }
                DB.Question.Add(question);
            }
            else
            {
                var qa = DB.Question.Where(q => q.QuestionID == question.QuestionID).Single();
                qa.Category = question.Category;
                qa.Flags = question.Flags;
                qa.questionMedia = question.questionMedia;
                qa.QuestionText = question.QuestionText;
                qa.SubCategory = question.SubCategory;
                qa.SubQuestion = question.SubQuestion;
            }

                DB.SaveChanges();

        }

        public async virtual void saveEntitiesFromQuestions(List<entity> Entities)
        {
            var questions = DB.SubQuestion;
            foreach(var e in Entities)
            {
                if(e!= null && e.entityValue.Length < 50)
                {
                    DB.entity.Add(e);

                }
                else
                {

                }
                try

                {
                     DB.SaveChanges();
                }
                catch (Exception ex)
                {
                  //  var exStr = ex.InnerException.ToString();
                 //   var idx = exStr.IndexOf("The duplicate key value is (") + "The duplicate key value is (".Length);
                  //  var str2 = exStr.Remove(0, idx);
                  //  idx =  str2.IndexOf(")");
                 //   var val = str2.Remove(idx);
                 //   DB.entity.


                    DB.entity.Remove(e);
                }
            }
            
        }



        public async virtual Task addUserScore(userScore userScore)
        {
            try
            {
                DB.userScore.Add(userScore);
                await DB.SaveChangesAsync();
            }
            catch (Exception e)
            {
                //   Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new DBException();
            }
        }


        public async virtual void addUpdateUser(User user)
        {
            try
            {
                var userUpdateDB = DB.User.Find(user.UserID);
              if (userUpdateDB != null)
                {

                    userUpdateDB.PreviusParses = user.PreviusParses;
                    userUpdateDB.TimeConnected = user.TimeConnected;
                    userUpdateDB.UserAddress = user.UserAddress;
                    userUpdateDB.UserGender = user.UserGender;
                    userUpdateDB.UserClass = user.UserClass;
                    userUpdateDB.UserID = user.UserID;
                    userUpdateDB.UserLastSession = user.UserLastSession;
                    userUpdateDB.UserName = user.UserName;
                    userUpdateDB.UserOverallTime = user.UserOverallTime;
                    userUpdateDB.UserTimesConnected = user.UserTimesConnected;
                    DB.Entry(userUpdateDB).State = EntityState.Modified;
                }
                else
                {
                    DB.User.Add(user);
                }
                 DB.SaveChanges();
             
            }
            catch (Exception e)
            {
                //   Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
              //  throw new DBException();
            }

        }



        public async virtual void addNewEntity(string value, string type)
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
                    await DB.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
             //   Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new DBException();
            }



        }

        public void addUpdateEntity(entity newEnt)
        {
  
            try
            {
            
                var old = DB.entity.Where(e => e.entityValue == newEnt.entityValue);
                if (old.Any())
                {
                    var oldEnt = old.Single();
                    foreach(var s in newEnt.entitySynonimus.Split(';'))
                    {
                        if (s != "") oldEnt.entitySynonimus += s + ";";
                    }
                }
                else
                {
                    DB.entity.Add(newEnt);
                }

                DB.SaveChanges();
                
            }
            catch (Exception e)
            {
                //   Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new DBException();
            }

        }

        public void addUpdateEntity(multyEntity newEnt)
        {

            try
            {

                var old = DB.multyEntity.Where(e => e.entityValue == newEnt.entityValue);
                if (old.Any())
                {
                    old.Single().parts = newEnt.parts;
                    old.Single().singleValue = newEnt.parts;
                    old.Single().entityType = newEnt.parts;
                }
                else
                {
                    DB.multyEntity.Add(newEnt);
                }

                DB.SaveChanges();

            }
            catch (Exception e)
            {
                //   Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new DBException();
            }

        }

        public void addUpdateEntity(IentityBase newEnt)
        {
            if (newEnt is multyEntity) addUpdateEntity((multyEntity) newEnt);
            if (newEnt is entity) addUpdateEntity((entity)newEnt);
        }




        public media getRandomMedia(string type, string[] possibleFlags)
        {
            media media = new media();


            try
            {

                var mediaRes = DB.media.Where(x=>x.mediaKey == type);
                List<media> match = new List<media>();
                foreach (var f in possibleFlags)
                {
                    var tryMatch = mediaRes.Where(m => m.flags.Contains(f));
                    if (tryMatch.Any())
                    {
                        match.AddRange(tryMatch);
                    }
                }

                if (!match.Any())
                {
                    match.AddRange(mediaRes);
                }

                var rnd = RandomNum.getNumber(match.Count);

                return match[rnd];


            }
            catch (Exception e)
            {
                //   Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new DBException();
            }

            


            return media;
        }

        public async virtual void addNewUser(IUser user)
        {

            try
            {


                DB.User.Add((User)user);
                await DB.SaveChangesAsync();

            }
            catch (Exception e)
            {
            //    Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new DBException();
            }



        }



        public virtual IUser getUser(string userId)
        {
   
            List<User> visitors = new List<User>();

            try
            {


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



        public async virtual void deleteUser(string userId)
        {

            IUser NewIUser = null;
            List<IUser> visitors = new List<IUser>();

            try
            {

                NewIUser = new User();

                var itemToRemove = DB.User.SingleOrDefault(x => x.UserID == userId);

                if (itemToRemove != null)
                {
                    DB.User.Remove(itemToRemove);
                    await DB.SaveChangesAsync();
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

        public Entities8 updateDBmanual()
        {
            return DB;
        }

        public virtual string[] getAllCategory()
        {
            ObjectCache cache = MemoryCache.Default;
            var cachedItem = cache.Get("getAllCategory()");
            if (cachedItem == null)
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

                var exp = new CacheItemPolicy();
                exp.SlidingExpiration = (new TimeSpan(1, 0, 0, 0));
                cache.Set("getAllCategory", catagories, exp);
                return catagories;
            }
            else
            {
                return (string[])cachedItem;
            }
          

        }

        public virtual IEnumerable<Ientity> getEntitys()
        {
            ObjectCache cache = MemoryCache.Default;
            var cachedItem = cache.Get("getEntitys");
            if (cachedItem == null)
            {
                var item = DB.entity;
                var exp = new CacheItemPolicy();
                exp.SlidingExpiration = (new TimeSpan(1, 0, 0, 0));
                cache.Set("getEntitys", item, exp);
                return item;
            }
            else
            {
                return (IEnumerable <Ientity> )cachedItem;
            }

                
        }

        public virtual IEnumerable<IMultyEntity> getMultyEntitys()
        {
            ObjectCache cache = MemoryCache.Default;
            var cachedItem = cache.Get("getMultyEntitys");
            if (cachedItem == null)
            {
                var item = DB.multyEntity;
                var exp = new CacheItemPolicy();
                exp.SlidingExpiration = (new TimeSpan(1, 0, 0, 0));
                cache.Set("getMultyEntitys", item, exp);
                return item;
            }
            else
            {
                return (IEnumerable<IMultyEntity>)cachedItem;
            }


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
     
          //  media media = new media();
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
                }

                if(phrases == null) { 
                    ObjectCache cache = MemoryCache.Default;
                    var cachedItem = cache.Get("getBotPhrase");
                    if (cachedItem == null)
                    {
                        //phrases = (from t in DB.botphrase
                        //           where t.Pkey.ToLower() == key && (t.Flags == null || !t.Flags.Contains("text"));
                        //                 select t.Text).ToArray();
                        var exp = new CacheItemPolicy();
                        exp.SlidingExpiration = (new TimeSpan(1, 0, 0, 0));
                        var dbParses = DB.botphrase.GroupBy(x => x.Pkey.ToLower()).ToDictionary(group => group.Key, group => group.Select(p=>p.Text).ToArray());
                        cache.Set("getBotPhrase", dbParses, exp);
                        phrases = dbParses[key];
                    }
                    else
                    {
                        phrases = ((Dictionary<string, string[]>) cachedItem)[key];
                        if(phrases == null || phrases.Length == 0)
                        {
                            throw new Exception("empty phrase");
                        }
                    }
                }
 
            }
            catch (Exception e)
            {
                //   Logger.log(this.GetType().Name, MethodBase.GetCurrentMethod().Name, e.ToString());
                throw new NotImplementedException("key:" + key + " not in dictionary");
            }

            return phrases;

        }

        internal async  void addOtherLog(OtherLog log)
        {
            //   var DB = new Entities();
            try
            {
            DB.OtherLog.Add(log);
            await DB.SaveChangesAsync();

            }
            catch(Exception ex)
            {

            }
        }

        public List<userScore> getUserAnswersStat(string userID)
        {
            var scorse = DB.userScore.Where(x => x.userID == userID);
            return scorse.ToList(); ;
        }

        public List<answersLog> getSampleQuestions()
        {
            return DB.answersLog.Where(a=>a.userID == "testCase").ToList();
        }


    }
}