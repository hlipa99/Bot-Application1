

using hebrewNER;
using NLPtest.Models;
using NLPtest.view;
using NLPtest.WorldObj;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using vohmm.application;
using yg.chunker;
using yg.sentence;
using Model.dataBase;
using Model;

namespace NLPtest
{


    public class ConversationController
    {


         MessageComposer composer;
         Responder responder;
         SemanticAnalizer sa;
        Dictionary<string, string[]> PraseDictionary;
        private ContentTurn last;
        INLPControler nlpControler;
        UserObject user;
        private Users user1;

        public ConversationController(string userName,string userGender)
        {
            composer = new MessageComposer();
            responder = new Responder();
            sa = new SemanticAnalizer();
            PraseDictionary = loadDictionary();
            nlpControler = NLPControler.getInstence();
            user = new UserObject(userName, userGender);
        }

        public ConversationController(Users user1)
        {
            this.user1 = user1;
        }

        public  string[] NotImplamented()
        {
            return new string[]
            {
                    "אני מצטער  :(" + user.getUserName() + " אבל התכונה הזאת עדיין בפיתוח",
                    "אני עוד בוט קטן, אולי כשאני אגדל אני אבין"
            };
        }

        public  string chooseStudyUnits()
        {

            return "יאללה לבחור יחידת לימוד";

          
        }

        public  string[] areYouSure()
        {
            return new string[]
{
                             "אתה בטוח שזה מה שאתה רוצה לעשות? :("
};
        }


        public  T FindMatchFromOptions<T>(string str, IEnumerable<T> options)
        {
            var res = "";
           foreach(var o in options as IEnumerable<string>)
            {

                if (str.Contains(o)) res =  o;
                if (o.Contains(str)) res =  o;
            }


           //bypass to keep the type T
            foreach (var o in options)
            {
               if (o.Equals(res)) return o;
            }

            return default(T);
        }

        public  string[] areUReaddyToLearn( string subject)
        {
            return new string[]
                {
                    "מוכן ללמוד קצת " +subject + "?"
                };
        }

        public  string[] beforAskQuestion(StudySession studySession)
        {
            return new string[]
                 {
                    "אוקיי, שאלה מס " +(studySession.questionAsked.Count + 1) +"'" + " מתוך "+ studySession.sessionLength
                 };
        }

        public  string[] chooseSubjectForLearn()
        {
            return new string[]
{ "אוקיי, מה תרצה ללמוד?"
};
        }

        public  string[] Happy()
        {
            return new string[]
{
                             "איזה כיף, הבהלת אותי לרגע"
};
        }

        public  string[] MissionDone()
        {
            return new string[]
{
                             "טוב, סיימתי"
};
        }


        public  bool isStopSession(string answer)
        {
            var stop =  new string[]
         {
                "מספיק",
                "די",
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

        public  string[] stopLearningSession()
        {
            return new string[]
         {
                "עבודה יפה, נמשיך בפעם אחרת",
             
         };
        }


        public string[] goodAnswer()
        {
            return new string[]
           {
                "תשובה טובה"
           };
        }


        public  string[] MyAnswerToQuestion()
        {
            return new string[]
           {
                ":התשובה לשאלה היא"
           };
        }

        public string[] goodbye()
        {
                return new string[]
             {
                     "ביי, נתראה בקרוב"
            };
           
        }

        public  string[] wrongOption()
        {
            return new string[]
           {
                "מצטער, זו אף פעם לא הייתה אופציה"
           };
        }

        public  string[] MainMenuOptions()
        {
            
            return new string[]
            {
                "בוא נלמד" +   Emoji.get("student") ,
                "כלום. אין לי כח ללמוד",

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

        public  string MainMenuText()
        {
           return "טוב, " + user.getUserName() + " אז מה עושים היום? ";
        }



        public string[] notAnAnswer()
        {
            return new string[]
           {
               Emoji.get("dizzy") +  "טוב, אני אגיד לך" 
           };
        }

        public string[] partialAnswer()
        {
            return new string[]
           {
                "אוקיי, תשובה מעניינת"
           };
        }


        public  string[] greetings()
        {
                        return new string[]
               {
                                      "היי " + user.getUserName() + " בחיי שהתגעגעתי ",
               };
        }

        public string[] moveToNextQuestion()
        {
            return new string[]
                 {
                            "שנמשיך לשאלה הבאה" + "?",
                 };
        }

        public  string[] selfIntroduction()
        {
            return new string[]
     {
                            "שלום, אני " + " מיסטר אייצ" +"'" + ", " + "בוט ללימוד היסטוריה" + Emoji.get("robot")
     };
        }

        public  string[] SoSorry()
        {
            return new string[]
{
                               "מצטער לשמוע :/"
};
         
        }

        public  string[] OK()
        {
            return new string[]
{
                               "סבבה"
};
        }

        public  string[] veryGood()
        {
            return new string[]
{
                             "נפלא",
};
        }

        public string[] giveYourFeedback()
        {
            return new string[]
{
                             "איזה ציון היית נותן לעצמך על התשובה שענית" + "?"  + Emoji.get("sunglasses")
};
        }

        public  string[] letsLearn()
        {

            return new string[]
{
                             "אז קדימה, בוא נתחיל ללמוד",
};


        }

        public string[] endOfSession(Users user)
        {
            return new string[]
{
                             "היה נפלא :) להתראות מחר",
};
        }

        public  string[] howAreYou()
        {
            return new string[]
{
                             "מה שלומך היום?",
};
        }

        public string[] endOfSession(Users user, StudySession studySession)
        {
            if (studySession.questionAsked.Count <= 1)
            {
                return new string[]
               {
                            "מה? כבר הולכים" + Emoji.get("crying")
                };
            } else
            {
                var average = 0;
                foreach (var q in studySession.questionAsked)
                {
                    average += q.answerScore / studySession.questionAsked.Count;
                }

                if (average > 60)
                {
                    return new string[]
           {
                            "טוב, היה סבב מוצלח",
                            "הממוצע שלך היה " + average  + Emoji.get("smiling")
                };
            }else
                {
                    return new string[]
           {
                            "אני רואה שאתה עובד קשה, בסיבוב הבא אני בטוח שתקבל יותר מ" + average  + Emoji.get("grinning")

            };
                }


              
            }
        }

        public  string[] NewUserGreeting(string username)
        {
            return new string[]
          {
                            "היי " + username +  " אני חושב שעוד לא הכרנו ,",
                         Emoji.get("smiling") + "נחמד לפגוש אותך",
          };
        }

        public string[] notNumber()
        {
            return new string[]
                {
                             "זה לא מספר.. משהו בין 0 ל 100"
                };
        }

        public int getNum(string conv)
        {
            int res = -1;
            if(int.TryParse(conv, out res))
            {
                if (res <= 10) return res * 10;
                else if (res <= 100) return res;
                else return -1;
            }
            return -1;

            //  return nlpControler.getNum(text);
        }

        public  string[] NewUserGetName()
        {
            return new string[]
               {
                             "לפני שנתחיל, רק כמה פרטים",
                                "איך קוראים לך?"
               };
        }

        public  string[] NewUserGetGender()
        {
            return new string[]
               {
                             "בן או בת?"
               };
        }

        public  string[] MissingUserInfo(string v)
        {
            var info = "";
            switch (v){
                case "name":
                    info = "שם";
                    break;
                case "gender":
                    info = "מין";
                    break;
                case "class":
                    info = "כיתה";
                    break;

            }

            return new string[]
           {
                         '\u263a'  + "אני מצטער, אבל בלי ה" + info + " שלך לא נוכל להמשיך " 
           };


        }

        public  string[] GenderAck(string gender)
        {
           if(gender == "feminine")
            {
                return new string[]
      {
              
                        ":P תתפלאי, אבל כבר הכרתי בנים עם השם הזה"
            };
               
            }else
            {
                return new string[]
             {
                      ":P תתפלא, אבל כבר הכרתי בנות עם השם הזה"
            };
            }
        }
        
        public  string[] NewUserGetClass(Users user)
        {

            return new string[]
        {
                     "עוד משהו.. באיזה כיתה " + getGufSecond(user) + "?"
            };
        }

        private  string getGufSecond(Users user)
        {
            if(user.UserGender == "feminine")
            {
                return "את";
            }else
            {
                return "אתה";
            }
        }

        public  string[] GeneralAck()
        {
            return new string[]
         {
                     "אין בעיה, רשמתי"
         };
        }


        public  string[] GeneralAck(string value)
        {
            return new string[]
         {
                     "אין בעיה, אני רושם  " + value
         };
        }


        public  string[] LetsStart()
        {
        return new string[]
        {
                "אוקיי.. מתחילים"
       };
        }

        public string getName(string text)
        {
            return text;
          //  return nlpControler.getName(text);
        }

        public string getGender(string text)
        {
            if (text == "בן")
            {
                return "masculine";
            }
            else
            {
                return "feminine";
            }
           // return nlpControler.GetGender(text);
        }

        public  string getClass(string text)
        {
            return text;
          //  return nlpControler.getClass(text);
        }

        public string getGeneralFeeling(string text)   //TODO: real feeling
        {
            return nlpControler.GetGeneralFeeling(text);
        }


        internal ContentTurn testAnalizer(string inputText)
        {

            //    var a = MorfAnalizer.createSentence(inputText);
            var context = new TextContext();
            var sen = nlpControler.Analize(inputText);

            ContentTurn input = new ContentTurn();
            foreach (var s in sen)
            {
                input.Add(sa.tagWords2(s, ref context));
         //       input = sa.findGufContext(input);
            }


          //  input = sa.findRelations(input);

            if (last != null)
            {
       //         input = sa.findGufContext(last, input);
            }


            //       var output = responder.respone(input, context);

            //      var outMessage = composer.compose(output, new User("יוחאי"));


            //  return outMessage;

            last = input;
            return input;

        }



        private Dictionary<string, string[]> loadDictionary()  //TODO
        {
            var d = new Dictionary<string, string[]>();
            d.Add("", new string[] {"" });

            return d;
        }

    }


}
