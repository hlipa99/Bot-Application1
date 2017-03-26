﻿


using NLP.Models;
using NLP.view;
using NLP.WorldObj;
using System;
using System.Collections.Generic;

using Model.dataBase;
using Model;
using Bot_Application1.Controllers;
using NLP;
using Model.Models;
using Bot_Application1.Exceptions;
using Bot_Application1.Models;
using NLP.NLP;
using NLP.Controllers;
using Microsoft.Bot.Connector;
using System.IO;
using Bot_Application1.YAndex;

namespace Bot_Application1.Controllers
{


    public class ConversationController
    {


        MessageComposer composer;
        // Dictionary<string, string[]> PraseDictionary;
        DataBaseController db = new DataBaseController();
        EducationController ec;

        //private ContentList last;
        NLPControler nlpControler = new NLPControler();
        IUser user;
        IStudySession studySession;
  

        public static string BOT_NAME = "מיסטר אייצ" + "'";

        public static string BOT_SUBJECT = "היסטוריה";
        private readonly int SUCCESS_THRESHHOLD = 35;


        public virtual DataBaseController Db
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

        public string FindMatchFromOptions(string[] options, string matchString)
        {
            var best = "";
            var bestInt = -1;
            foreach (var o in options)
            {
                int precent = NlpControler.matchStrings(o, matchString);
                if(precent > bestInt)
                {
                    bestInt = precent;
                    best = o;
                }
            }

            if(bestInt > SUCCESS_THRESHHOLD)
            {
                return best;
            }else
            {
                return null;
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

        public NLPControler NlpControler
        {
            get
            {
                return nlpControler;
            }

            set
            {
                nlpControler = value;
            }
        }

        internal UserIntent getUserIntente(string res, UserContext userContext)
        {
            return NlpControler.getUserIntent(res, userContext.dialog);
        }

        public ConversationController(){}

        public ConversationController(IUser user, IStudySession studySession)
        {
            this.User = user;
            this.StudySession = studySession;
            this.db = db;
            ec = new EducationController(user, studySession, this);
        }


        //public  T FindMatchFromOptions<T>(string str, IEnumerable<T> options)
        //{
        //    var res = "";
        //    if (str != "")
        //    {
        //        foreach (var o in options as IEnumerable<string>)
        //        {

        //            if (str.Contains(o)) res = o;
        //            if (o.Contains(str)) res = o;
        //        }


        //        //bypass to keep the type T
        //        foreach (var o in options)
        //        {
        //            if (o.Equals(res)) return o;
        //        }
        //    }
        //    return default(T);
        //}

        internal string[] MainMenuOptions()
        {
            return new string[]
            {
                getPhrase(Pkey.MenuLearn)[0],getPhrase(Pkey.MenuNotLearn)[0]
            };
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

            var average = 0;
            foreach (var q in StudySession.QuestionAsked)
            {
                average += q.AnswerScore / StudySession.QuestionAsked.Count;
            }
            if(StudySession.QuestionAsked.Count < StudySession.SessionLength)
            {
                return getPhrase(Pkey.earlyDiparture);

            }
            if (average > 60)
            {
                return getPhrase(Pkey.goodSessionEnd, textVar: average + "");
            }
            else
            {
                return getPhrase(Pkey.badSessionEnd, textVar: average + "");

            }

          //  return getPhrase(Pkey.endOfSession);


        
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

                if (text != null && text.Split(' ').Length == 1 && text.Length > 1)
                    return text;

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

        internal string[] mergeText(string[] v1, string v2)
        {
            if (v1.Length == 0) return new string[] { v2};
            var space = v1[v1.Length - 1].Length == 1 ? "" : " ";
            v1[v1.Length - 1] += space + v2;
            return v1;
        }

        internal string[] mergeText(string[] v1, string[] v2)
        {
            var list1 = new List<string>(v1);
            list1.AddRange(v2);
            return list1.ToArray();
        }

        internal string[] mergeText(string v1, string[] v2)
        {
            if (v2.Length == 0) return new string[] { v1 };

            var space = v1.Length == 1 ? "" : " ";
            v2[0] = v1 + space + v2[0];
            return v2;
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
            if (context.dialog == "LerningDialog" || context.dialog == "QuestionDialog")
            {
                return ec.createReplayToUser(text, answerIntent);
            }

            else if (context.dialog == "GreetingDialog")
            {

                switch (answerIntent)
                {
                   
                    case UserIntent.howAreYou:
                        return mergeText(getPhrase(Pkey.greetings), getPhrase(Pkey.IAmFine));
                        break;

                    case UserIntent.bot_questions:
                        return getPhrase(Pkey.unknownQuestion);
                        break;

                    case UserIntent.hello:
                    case UserIntent.DefaultFallbackIntent:
                    default:
                        return getPhrase(Pkey.greetings);
                        break;
                }
            }
            else
            {
                throw new ContextException();
            }
            return null;
        }

        public bool isEnglish(string text)
        {
            foreach (var c in text)
                {
                    if((c <= 90 && c >= 65) || (c <= 122 && c >= 97))
                    {
                         return true;
                    }
                }
            return false;
        }

        public bool isHebrew(string text)
        {
            foreach (var c in text)
            {
                if (c <= 0x0005EA && c >= 0x0005D0)
                {
                    return true;
                }
            }
            return false;
        }



        public virtual string[] getPhrase(Pkey key,string[] flags = null, string[] flagesNot = null, string textVar = null)
        {
            try
            {
                Logger.addLog("Bot: " + Enum.GetName(typeof(Pkey), key));

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

                if (phraseRes != null)
                {
                    phraseRes = formateVars(phraseRes, textVar);

                    if (user.Language == "en")
                    {
                        phraseRes = ControlerTranslate.TranslateToEng(phraseRes);

                    }

                    return phraseRes.Split('|');
                }
                else
                {
                    return new string[] { };
                }
            }catch(Exception ex)
            {
                return new string[] { };
            }
   
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
