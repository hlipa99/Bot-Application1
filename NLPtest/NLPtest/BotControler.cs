
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
    public static class BotControler
    {


        static MessageComposer composer;
        static Responder responder;
        static MorfAnalizer ma;
        static SemanticAnalizer sa;



        static public void initialize()
        {
            composer = new MessageComposer();
            responder = new Responder();
            ma = new MorfAnalizer();
            sa = new SemanticAnalizer();

        }

        public static string[] areYouSure()
        {
            return new string[]
{
                             "אתה בטוח שזה מה שאתה רוצה לעשות? :("
};
        }

        static internal List<string> sendMessage(string inputText)
        {

            //    var a = ma.createSentence(inputText);
            var context = new Context();
            var sen = ma.meniAnalize(inputText);

            ContentTurn input = new ContentTurn();
            foreach (var s in sen) {
                input.Add(sa.tagWords(s, ref context));
            }



            var output = responder.respone(input, context);

            var outMessage = composer.compose(output, new User("יוחאי"));


            return outMessage;

        }

        public static string[] Happy()
        {
            return new string[]
{
                             "איזה כיף, הבהלת אותי לרגע"
};
        }

        public static string[] MissionDone()
        {
            return new string[]
{
                             "טוב, סיימתי"
};
        }

        static public String getName(string inputText)
        {

            //    var a = ma.createSentence(inputText);
            var context = new Context();
            var sen = ma.meniAnalize(inputText);

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


        public static string getGeneralFeeling(string text)   //TODO: real feeling
        {
            //    var a = ma.createSentence(inputText);
            if (text.Contains("לא טוב") || text.Contains("רע") || text.Contains("גרוע") || text.Contains("על הפנים"))
            {
                return "good";
            }else if (text.Contains("טוב") || text.Contains("סבבה") || text.Contains("מצויין") || text.Contains("אחלה"))
            {
                return "bad";
            }
            else
            {
                return "netural";
            }
        }


        public static string[] greetings(User user)
        {
                        return new string[]
               {
                                      "היי " + user + " בחיי שהתגעגעתי ",
               };
        }

      

        public static string[] selfIntroduction()
        {
            return new string[]
     {
                             "בוט ללימוד היסטוריה,H שלום, אני מר",
     };
        }

        public static string[] SoSorry(User user)
        {
            return new string[]
{
                               "מצטער לשמוע :/"
};
         
        }

        public static string[] OK(User user)
        {
            return new string[]
{
                               "סבבה"
};
        }

        public static string[] veryGood(User user)
        {
            return new string[]
{
                             "נפלא",
};
        }

        public static string[] letsLearn()
        {

            return new string[]
{
                             "אז קדימה, בוא נתחיל ללמוד",
};


        }

        public static string[] howAreYou(User user)
        {
            return new string[]
{
                             "מה שלומך היום?",
};
        }

        public static string[] NewUserGreeting(string username)
        {
            return new string[]
          {
                            "היי " + username +  " אני חושב שעוד לא הכרנו ,",
                           '\u263a' + "נחמד לפגוש אותך",
          };
        }

        public static string[] NewUserGetName()
        {
            return new string[]
               {
                             "לפני שנתחיל, רק כמה פרטים",
                                "איך קוראים לך?"
               };
        }

        public static string[] NewUserGetGender()
        {
            return new string[]
               {
                             "בן או בת?"
               };
        }

        public static string[] MissingUserInfo(string v)
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

        public static string[] GenderAck(string gender)
        {
           if(gender == "feminine")
            {
                return new string[]
      {
              
                        ":P תתפלאי, אבל כבר הקרתי בנים עם השם הזה"
            };
               
            }else
            {
                return new string[]
             {
                      ":P תתפלא, אבל כבר הכרתי בנות עם השם הזה"
            };
            }
        }
        
        public static string[] NewUserGetClass(User user)
        {

            return new string[]
        {
                     "עוד משהו.. באיזה כיתה " + getGufSecond(user) + "?"
            };
        }

        private static string getGufSecond(User user)
        {
            if(user.getGender() == "feminine")
            {
                return "את";
            }else
            {
                return "אתה";
            }
        }

        public static string[] GeneralAck()
        {
            return new string[]
         {
                     "אין בעיה, רשמתי"
         };
        }


        public static string[] GeneralAck(string value)
        {
            return new string[]
         {
                     "אין בעיה, אני רושם  " + value
         };
        }


        public static string[] LetsStart()
        {
        return new string[]
        {
                "אוקיי.. מתחילים"
       };
        }


        public static string getGender(string text)
        {
            //    var a = ma.createSentence(inputText);
            var context = new Context();
            var sen = ma.meniAnalize(text);

            ContentTurn input = new ContentTurn();
            foreach (var s in sen)
            {
                foreach (var w in s.Words)
                {
                    if (w.word == "בן")
                    {
                        return "masculine";
                    }
                    if (w.gender != null & w.gender != "" & w.gender != "unspecified")
                    {
                        return w.gender;
                    }
                }
            }

            return null;
        }


        public static string getClass(string text)
        {
            //    var a = ma.createSentence(inputText);
            var context = new Context();
            var sen = ma.meniAnalize(text);

            ContentTurn input = new ContentTurn();
            foreach (var s in sen)
            {
                foreach (var w in s.Words)
                {
                    switch  (w.word){
                        case "א":
                        case "'א":
                        case "אלף":
               
                            return "א";
                            break;
                        case "ב":
                        case "'ב":
                        case "בית":
             
                            return "ב";
                            break;
                        case "ג":
                        case "'ג":
                        case "גימל":
                            return "ג";

                            break;
                        case "ד":
                        case "'ד":
                        case "דלת":
                            return "ד";

                            break;
                        case "ה":
                        case "'ה":
                        case "הי":
                            return "ה";

                            break;
                        case "ו":
                        case "'ו":
                        case "וו":
         
                            return "ו";
                            break;

                        case "ז":
                        case "'ז":
                        case "זוד":
                        case "שזשזת":
                            return "ז";
                            break;

                        case "ח":
                        case "'ח":
                        case "חית":
                            return "ח";
                            break;

                        case "ט":
                        case "'ט":
                        case "טוד":
                        case "חמישית":
                        case "חמשוש":
                        case "חמשושית":
                            return "ט";
                            break;




                        case "י":
                        case "'י":
                        case "יוד":
                        case "שישית":
                        case "שישיסט":
                        case "שישיסטית":
                            return "י";
                            break;

                        case "יא":
                        case "י\"א":
                        case "יא'":
                        case "'יא":
                        case "שביעית":
                        case "שביעיסט":
                        case "שביעיסטית":
                            return "יא";
                            break;

                        case "יב":
                        case "י\"ב":
                        case "יב'":
                        case "'יב":
                        case "שמינית":
                            return "יב";
                            break;


                    }
                }
            }

            return null;
        }
    }

   
}
