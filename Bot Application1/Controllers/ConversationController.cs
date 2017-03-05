


using NLPtest.Models;
using NLPtest.view;
using NLPtest.WorldObj;
using System;
using System.Collections.Generic;

using Model.dataBase;
using Model;
using Bot_Application1.Controllers;
using NLPtest;
using Model.Models;
using Bot_Application1.Exceptions;
using Bot_Application1.Models;
using NLPtest.NLP;
using NLPtest.Controllers;

namespace Bot_Application1.Controllers
{


    public class ConversationController
    {


        MessageComposer composer;
        // Dictionary<string, string[]> PraseDictionary;
        DataBaseController db;
        EducationController ec;

        private ContentList last;
        NLPControler nlpControler;
        IUser user;
        IStudySession studySession;
  

        public static string BOT_NAME = "מיסטר אייצ" + "'";

        public static string BOT_SUBJECT = "היסטוריה";

        public virtual DataBaseController Db
        {
            get
            {
                return DataBaseController.getInstance();
            }
        }

        public virtual IUser User
        {
            get
            {
                return user;
            }

            set
            {
                user = value;
            }
        }

        public virtual IStudySession StudySession
        {
            get
            {
                return studySession;
            }

            set
            {
                studySession = value;
            }
        }

        public ConversationController(){}

        public ConversationController(IUser user, IStudySession studySession)
        {
            this.User = user;
            this.StudySession = studySession;
            this.db = db;
            ec = new EducationController(user, studySession, this);
        }


        public  T FindMatchFromOptions<T>(string str, IEnumerable<T> options)
        {
            var res = "";
            if (str != "")
            {
                foreach (var o in options as IEnumerable<string>)
                {

                    if (str.Contains(o)) res = o;
                    if (o.Contains(str)) res = o;
                }


                //bypass to keep the type T
                foreach (var o in options)
                {
                    if (o.Equals(res)) return o;
                }
            }
            return default(T);
        }

        internal string[] MainMenuOptions()
        {
            return new string[]
            {
                getPhrase(Pkey.MenuLearn)[0],getPhrase(Pkey.MenuNotLearn)[0]
            };
        }


        public  bool isStopSession(string answer)
        {

            //TODONLP
            var stop =  new string[]
         {
                "מספיק",
                "נמאס",
                "אין לי כח",
                "לא בא לי",
                "נמשיך מחר",
                "תשתוק"
         };

            foreach(var s in stop)
            {
                if (answer.Contains(s))
                {
                    return true;
                }
            }


            return false;
        }


        public string[] getGenderOptions()
        {
            return new string[]
           {
                "בת","בן"

           };
        }

        public string[] getClassOptions()
        {
            return new string[]
           {
                "י" + "'",
                "יא"+ "'",
                "יב"+ "'"
           };
        }


        public string[] endOfSession()
        {
            if (StudySession.QuestionAsked.Count <= 1)
            {
                return getPhrase(Pkey.earlyDiparture);
             } else
            {
                var average = 0;
                foreach (var q in StudySession.QuestionAsked)
                {
                    average += q.AnswerScore / StudySession.QuestionAsked.Count;
                }

                if (average > 60)
                {
                    return getPhrase(Pkey.goodSessionEnd, textVar: average + "");
                 }else{
                               return getPhrase(Pkey.badSessionEnd,textVar: average + "");

                }



            }
        }


        public int getNum(string conv)
        {
            int res = -1;
            if(int.TryParse(conv, out res))
            {
                if (res < 0) return -1;
                else if (res <= 10) return res * 10;
                else if (res <= 100) return res;
                else return -1;
            }
            return -1;

        }

        private  string getGufSecond()
        {
            if(User.UserGender == "feminine")
            {
                return "את";
            }else
            {
                return "אתה";
            }
        }


        public string getName(string text)
        {
            if (text[0] >= '\u05D0' && text[0] <= '\u05EA')
            {
                if (text.Split(' ').Length == 1)
                    return text;
            }
            return null;
          //  return nlpControler.getName(text);
        }

        public string getGenderValue(string text)
        {
            if (text == "בן")
            {
                return "masculine";
            }
            else if(text == "בת")
            {
                return "feminine";
            }

            return null;
           // return nlpControler.GetGender(text);
        }

        public string getGenderName(string text)
        {
            if (text == "many")
            {
                if(User.UserGender == "masculine")
                {
                    return "בנים";
                }
                else
                {
                    return "בנות";
                }
            }
            else
            {
                if (User.UserGender == "masculine")
                {
                    return "בן";
                }
                else
                {
                    return "בת";
                }
            }
            // return nlpControler.GetGender(text);
        }

        public string getGenderOpositeName(string text)
        {
            if (text == "many")
            {
                if (User.UserGender == "masculine")
                {
                    return "בנות";
                  
                }
                else
                {
                    return "בנים";
                }
            }
            else
            {
                if (User.UserGender == "masculine")
                {
                    return "בת";
                }
                else
                {
                    return "בן";
                }
            }
            // return nlpControler.GetGender(text);
        }

        public  string getClass(string text)
        {
            text = text.Replace("'", "");
            if (text == "יב" || text == "יא" || text == "י")
                return text;
            else return null;
          //  return nlpControler.getClass(text);
        }

        //public string getGeneralFeeling(string text)   //TODO: real feeling
        //{
        // //   return nlpControler.GetGeneralFeeling(text);
        //}




        //private Dictionary<string, string[]> loadDictionary()  //TODO
        //{
        //    var d = new Dictionary<string, string[]>();
        //    d.Add("", new string[] {"" });

        //    return d;
        //}
        public string[] createReplayToUser(string text, UserContext context)
        {
            NLPControler nlp = new NLPControler();
            //  var answer = nlp.Analize(text);
            var answerIntent = nlp.getUserIntent(text, context.dialog);
            if (context.dialog == "LerningDialog")
            {
                return ec.createReplayToUser(text, answerIntent);
            }
            else if (context.dialog == "startConv")
            {

                switch (answerIntent)
                {
                    case UserIntent.answer:
                        if (context.dialog == "lerningSession")
                        {
                            throw new StopSessionException();
                        }
                        break;
                        if (context.dialog == "farewell")
                        {
                            throw new StopSessionException();
                        }

                    case UserIntent.dontKnow:

                        break;

                    case UserIntent.question:

                        break;

                    case UserIntent.unknown:

                        break;

                    case UserIntent.stopSession:

                        throw new StopSessionException();

                    default:

                        break;
                }
            }
            else if (context.dialog == "farewell")
            {

                switch (answerIntent)
                {
                    case UserIntent.answer:
                        if (context.dialog == "lerningSession")
                        {
                            throw new StopSessionException();
                        }
                        break;
                        if (context.dialog == "farewell")
                        {
                            throw new StopSessionException();
                        }

                    case UserIntent.dontKnow:

                        break;

                    case UserIntent.question:

                        break;

                    case UserIntent.unknown:

                        break;

                    case UserIntent.stopSession:

                        throw new StopSessionException();

                    default:

                        break;
                }
            }
            else
            {
                throw new ContextException();
            }
            return null;
        }



        public virtual string[] getPhrase(Pkey key,string[] flags = null, string[] flagesNot = null, string textVar = null)
        {
          
                if (flags == null) flags = new string[] { };
                if (flagesNot == null) flagesNot = new string[] { };

                var phrases = Db.getBotPhrase(key, flags, flagesNot);
                string phraseRes = null;
                if (phrases.Length > 0)
                {

                    var rundomInt = RandomNum.getNumber(phrases.Length);
                    phraseRes = phrases[rundomInt];

                }
                else
                {
                 //   throw new botphraseException();
                }


                phraseRes = formateVars(phraseRes, textVar);

                return phraseRes.Split('|');
            
        }

        private string formateVars(string phraseRes,string textVar)
        {
            if (StudySession == null)
            {
                //studySession = new StudySession();

            StudySession = new StudySession();
            }

            if (User == null)
            {
                User = new User();
                User.UserName = "";
                User.UserGender = "masculine";
            }

            phraseRes = phraseRes.Replace("<genderGuf>", getGufSecond());
            phraseRes = phraseRes.Replace("<genderPostfixH>",ifGufFemenin("ה"));
            phraseRes = phraseRes.Replace("<genderPostfixT>", ifGufFemenin("ת"));
            phraseRes = phraseRes.Replace("<text>", textVar);
            phraseRes = phraseRes.Replace("<subject>", StudySession.Category);
            phraseRes = phraseRes.Replace("<numOfQuestions>", StudySession.SessionLength + "");
            phraseRes = phraseRes.Replace("<questionNum>", (StudySession.QuestionAsked.Count + 1) +"");
            phraseRes = phraseRes.Replace("<userName>", User.UserName);
            phraseRes = phraseRes.Replace("<botName>", BOT_NAME);
            phraseRes = phraseRes.Replace("<botSubject>", BOT_SUBJECT);
            phraseRes = phraseRes.Replace("<genderPostfixY>", ifGufFemenin("י"));
            phraseRes = phraseRes.Replace("<genderMany>", getGenderName("many"));
            phraseRes = phraseRes.Replace("<!genderMany>", getGenderOpositeName("many"));
            phraseRes = phraseRes.Replace("<timeOfday>", getTimeOfDay());
            phraseRes = phraseRes.Replace("<questionsLeft>", (StudySession.SessionLength - StudySession.QuestionAsked.Count).ToString());


            phraseRes = phraseRes.Replace("נ ", "ן ");
            phraseRes = phraseRes.Replace("מ ", "מ ");
            phraseRes = phraseRes.Replace("צ ", "צ ");
            phraseRes = phraseRes.Replace("כ ", "ך ");
            phraseRes = phraseRes.Replace("פ ", "ף ");

            //formatEmuji emoji
            if (phraseRes.Contains("<e:")){
                phraseRes = formatEmuji(phraseRes);
            }


            //example <יודע#יודעת>
            while (phraseRes.Contains("<") && phraseRes.Contains(">"))
            {

                if (phraseRes.Contains("#"))
                {
                    var start = phraseRes.IndexOf("<");
                    var end = phraseRes.IndexOf(">");
                    var replace = phraseRes.Substring(start + 1, end - start -1).Split('#');
                    var gender = User.UserGender == "feminine" ? replace[1] : replace[0];
                    phraseRes = phraseRes.Remove(start, end - start + 1);
                    phraseRes = phraseRes.Insert(start, gender);
                }


            //    throw new PhraseFormatException();
            }

            return phraseRes;

        }

        private string getTimeOfDay()
        {
            var now = DateTime.Now.Hour;
            if(now > 5 && now < 11)
            {
                return "בוקר";
            }
            else if (now > 11 && now < 18)
            {
                return "יום";
            }
              else 
                {
                    return "לילה";
                }

            }

        internal IEnumerable<string> getYesNoOptions()
        {
            throw new NotImplementedException();
        }

        private char getGenderName(string userGender, string v)
        {
            throw new NotImplementedException();
        }

        private string ifGufFemenin(string v)
        {
            if (User.UserGender == "feminine") return v;
            else return "";
        }

        private string formatEmuji(string phraseRes)
        {
            while (phraseRes.Contains("<e:")){
                int startIdx = phraseRes.IndexOf("<e:");
                int endIdx = phraseRes.IndexOf(">", startIdx);
                var emojiFormat = phraseRes.Substring(startIdx, endIdx - startIdx + 1);
                var emoji = emojiFormat.Substring(3, emojiFormat.Length - 4);
                var emojiUnicode = Emoji.get(emoji);
                phraseRes = phraseRes.Replace(emojiFormat, emojiUnicode);
            }
            return phraseRes;
        }

        

    }



}
