
using hebrewNER;
using NLPtest.view;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using vohmm.application;
using yg.chunker;
using yg.sentence;


namespace NLPtest
{
    public class ConversationController
    {


         MessageComposer composer;
         Responder responder;
         SemanticAnalizer sa;
        Dictionary<string, string[]> PraseDictionary;


         public void initialize()
        {
            composer = new MessageComposer();
            responder = new Responder();
            sa = new SemanticAnalizer();
            PraseDictionary = loadDictionary();

        }

       

        public  string[] NotImplamented(User user)
        {
            return new string[]
            {
                    "אני מצטער  :(" + user.getUserName() + " אבל התכונה הזאת עדיין בפיתוח",
                    "אני עוד בוט קטן, אולי כשאני אגדל אני אבין"
            };
        }

        public  string chooseStudyUnits(User user)
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

        public  string[] areUReaddyToLearn(User user, string subject)
        {
            return new string[]
                {
                    "אוקיי, מתחילים. מוכן ללמוד קצת " +subject + "?"
                };
        }

        public  string[] beforAskQuestion(User user, int questionAsked)
        {
            return new string[]
                 {
                    "אוקיי, שאלה מס " +questionAsked + "'"
                 };
        }

        public  string chooseSubjectForLearn(User user)
        {
            return "אוקיי, מה תרצה ללמוד?";
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

         public String getName(string inputText)
        {

            //    var a = MorfAnalizer.createSentence(inputText);
            var context = new Context();
            var sen = MorfAnalizer.meniAnalize(inputText);

            if(sen.Count == 1 && sen[0].Words.Count == 1)
            {
                return sen[0].Words[0].word;
            }

            ContentTurn input = new ContentTurn();
            foreach (var s in sen)
            {
                foreach (var w in s.Words)
                {
                    if (sa.isAName(w))
                    {
                        return w.word;
                    }
                }
            }

            return null;

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

        public  string[] stopLearningSession(User user)
        {
            throw new NotImplementedException();
        }

        public  string[] MyAnswerToQuestion()
        {
            return new string[]
           {
                "תשובה יפה",
                " התשובה שלי לשאולה היא"
           };
        }

        public  string[] wrongOption()
        {
            return new string[]
           {
                "יפה, התשובה שלי לשאולה היא" +":"
           };
        }

        public  string[] MainMenuOptions()
        {
            return new string[]
            {
                "בוא נלמד",
                "עריכת פרופיל",
                "כלום. אין לי כח ללמוד",
            };
        }

        public  string MainMenuText(User user)
        {
           return "טוב, " + user.userName + " אז מה עושים היום? ";
        }

      

        public  string[] greetings(User user)
        {
                        return new string[]
               {
                                      "היי " + user.userName + " בחיי שהתגעגעתי ",
               };
        }

      

        public  string[] selfIntroduction()
        {
            return new string[]
     {
                            " שלום, אני" + ", 'מיסטר אייצ " + " בוט ללימוד היסטוריה",
     };
        }

        public  string[] SoSorry(User user)
        {
            return new string[]
{
                               "מצטער לשמוע :/"
};
         
        }

        public  string[] OK(User user)
        {
            return new string[]
{
                               "סבבה"
};
        }

        public  string[] veryGood(User user)
        {
            return new string[]
{
                             "נפלא",
};
        }

        public  string[] letsLearn()
        {

            return new string[]
{
                             "אז קדימה, בוא נתחיל ללמוד",
};


        }

        public  string[] howAreYou(User user)
        {
            return new string[]
{
                             "מה שלומך היום?",
};
        }

        public  string[] NewUserGreeting(string username)
        {
            return new string[]
          {
                            "היי " + username +  " אני חושב שעוד לא הכרנו ,",
                           '\u263a' + "נחמד לפגוש אותך",
          };
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
        
        public  string[] NewUserGetClass(User user)
        {

            return new string[]
        {
                     "עוד משהו.. באיזה כיתה " + getGufSecond(user) + "?"
            };
        }

        private  string getGufSecond(User user)
        {
            if(user.getGender() == "feminine")
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


        public  string getGender(string text)
        {
            return MorfAnalizer.GetGender(text);
        }


        public  string getClass(string text)
        {
            return MorfAnalizer.getClass(text);
        }

        public string getGeneralFeeling(string text)   //TODO: real feeling
        {
            return MorfAnalizer.GetGeneralFeeling(text);
        }


        internal List<string> testAnalizer(string inputText)
        {

            //    var a = MorfAnalizer.createSentence(inputText);
            var context = new Context();
            var sen = MorfAnalizer.meniAnalize(inputText);

            ContentTurn input = new ContentTurn();
            foreach (var s in sen)
            {
                input.Add(sa.tagWords(s, ref context));
            }



            var output = responder.respone(input, context);

            var outMessage = composer.compose(output, new User("יוחאי"));


            return outMessage;

        }


        private Dictionary<string, string[]> loadDictionary()  //TODO
        {
            var d = new Dictionary<string, string[]>();
            d.Add("", new string[] {"" });

            return d;
        }

    }


}
