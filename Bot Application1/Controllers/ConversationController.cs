


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

namespace Bot_Application1.Controllers
{


    public class ConversationController
    {


        MessageComposer composer;
        // Dictionary<string, string[]> PraseDictionary;
        DataBaseController db = new DataBaseController();


        private ContentTurn last;
        INLPControler nlpControler;
        IUser user;
        IStudySession studySession;
  

        public static string BOT_NAME = "מיסטר אייצ" + "'";

        public static string BOT_SUBJECT = "היסטוריה";

        public DataBaseController Db
        {
            get
            {
                return db;
            }

            set
            {
                db = value;
            }
        }

        public ConversationController(IUser user, IStudySession studySession)
        {
            this.user = user;
            this.studySession = studySession;
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
            if (studySession.QuestionAsked.Count <= 1)
            {
                return getPhrase(Pkey.earlyDiparture);
             } else
            {
                var average = 0;
                foreach (var q in studySession.QuestionAsked)
                {
                    average += q.AnswerScore / studySession.QuestionAsked.Count;
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
            if(user.UserGender == "feminine")
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
                if(user.UserGender == "masculine")
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
                if (user.UserGender == "masculine")
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
                if (user.UserGender == "masculine")
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
                if (user.UserGender == "masculine")
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

        public string getGeneralFeeling(string text)   //TODO: real feeling
        {
            return nlpControler.GetGeneralFeeling(text);
        }




        //private Dictionary<string, string[]> loadDictionary()  //TODO
        //{
        //    var d = new Dictionary<string, string[]>();
        //    d.Add("", new string[] {"" });

        //    return d;
        //}

 
        public string[] getPhrase(Pkey key,string[] flags = null, string[] flagesNot = null, string textVar = null)
        {
          
                if (flags == null) flags = new string[] { };
                if (flagesNot == null) flagesNot = new string[] { };

                var phrases = Db.getBotPhrase(key, flags, flagesNot);
                string phraseRes;
                if (phrases.Length > 0)
                {

                    var rundomInt = RandomNum.getNumber(phrases.Length);
                    phraseRes = phrases[rundomInt];

                }
                else
                {
                    throw new botphraseException();
                }


                phraseRes = formateVars(phraseRes, textVar);

                return phraseRes.Split('|');
            
        }

        private string formateVars(string phraseRes,string textVar)
        {
            if (studySession == null)
            {
                //studySession = new StudySession();

            studySession = new StudySession();
            }

            if (user == null)
            {
                user = new User();
                user.UserName = "";
                user.UserGender = "masculine";
            }

            phraseRes = phraseRes.Replace("<genderGuf>", getGufSecond());
            phraseRes = phraseRes.Replace("<genderPostfixH>",ifGufFemenin("ה"));
            phraseRes = phraseRes.Replace("<genderPostfixT>", ifGufFemenin("ת"));
            phraseRes = phraseRes.Replace("<text>", textVar);
            phraseRes = phraseRes.Replace("<subject>", studySession.Category);
            phraseRes = phraseRes.Replace("<numOfQuestions>", studySession.SessionLength + "");
            phraseRes = phraseRes.Replace("<questionNum>", (studySession.QuestionAsked.Count + 1) +"");
            phraseRes = phraseRes.Replace("<userName>", user.UserName);
            phraseRes = phraseRes.Replace("<botName>", BOT_NAME);
            phraseRes = phraseRes.Replace("<botSubject>", BOT_SUBJECT);
            phraseRes = phraseRes.Replace("<genderPostfixY>", ifGufFemenin("י"));
            phraseRes = phraseRes.Replace("<genderMany>", getGenderName("many"));
            phraseRes = phraseRes.Replace("<!genderMany>", getGenderOpositeName("many"));
            phraseRes = phraseRes.Replace("<timeOfday>", getTimeOfDay());
            phraseRes = phraseRes.Replace("<questionsLeft>", (studySession.SessionLength - studySession.QuestionAsked.Count).ToString());


            phraseRes = phraseRes.Replace("נ ", "ן ");
            phraseRes = phraseRes.Replace("מ ", "מ ");
            phraseRes = phraseRes.Replace("צ ", "צ ");
            phraseRes = phraseRes.Replace("כ ", "ך ");
            phraseRes = phraseRes.Replace("פ ", "ף ");

            //formatEmuji emoji
            if (phraseRes.Contains("<e:")){
                phraseRes = formatEmuji(phraseRes);
            }

            if (phraseRes.Contains("<") || phraseRes.Contains(">"))
            {
                throw new PhraseFormatException();
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
            if (user.UserGender == "feminine") return v;
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
