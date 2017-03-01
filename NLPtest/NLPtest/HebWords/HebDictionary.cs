using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLPtest.WorldObj;

using static NLPtest.HebWords.WordObject.WordType;
namespace NLPtest.HebWords
{
     class HebDictionary
    {
         Dictionary<string,WordObject> hebDic = new Dictionary<string, WordObject>();
        internal enum Guf { Rishon, Sheny, Shlishi };
        internal enum Kamot { Yahid, Rabim};
        internal enum Min { Zahar, Nekeva , Both, None };
     
        public  bool loadWords()
        {



            addHello("הי", new HelloObject("intergection"));
            addHello("היי", new HelloObject("intergection"));
            addHello("הייי", new HelloObject("intergection"));
            addHello("היייי",new HelloObject("intergection"));
            addHello("הייייי", new HelloObject("intergection"));
            addHello("שלום", new HelloObject("intergection"));
            addHello("שלומ", new HelloObject("intergection"));
            addHello("הלו", new HelloObject("intergection"));

            addHello("מה אתה עושה", new HelloObject("inquisive"));
            addHello("מה עושה", new HelloObject("inquisive"));
            addHello("מה שלומך", new HelloObject("inquisive"));
            addHello("מה קורה", new HelloObject("inquisive"));
            addHello("מה נשמע", new HelloObject("inquisive"));
            addHello("מה הולך", new HelloObject("inquisive"));
            addHello("מה המצב", new HelloObject("inquisive"));
            addHello("מה העניינים", new HelloObject("inquisive"));
            addHello("איך הולך", new HelloObject("inquisive"));
            addHello("ממצב", new HelloObject("inquisive"));

            addHello("בוקר טוב", new HelloObject("blessing"));
            addHello("יום טוב", new HelloObject("blessing"));
            addHello("ערב טוב", new HelloObject("blessing"));
            addHello("לילה טוב", new HelloObject("blessing"));
            addHello("צהריים טובים", new HelloObject("blessing"));









            addTime("שעה");
            addTime("שעתיים");
            addTime("שעות");
            addTime("דקה");
            addTime("שניה");
            addTime("שנייה");
            addTime("יום");
            addTime("אתמול");
            addTime("שלשום");
            addTime("שבוע");
            addTime("חודש");
            addTime("שנה");
            addTime("שנת");
            addTime("בתקופת");
            addTime("בזמן");
            addTime("בתאריך");
            addTime("מחר");
            addTime("מחורתיים");
            addTime("מחרתיים");
            addTime("מחורתים");
            addTime("יום ראשון");
            addTime("יום שני");
            addTime("יום שלישי");
            addTime("יום רביעי");
            addTime("יום חמישי");
            addTime("יום שישי");
            addTime("יום שבת");
            addTime("שבת");
            addTime("מוצש");
            addTime("מוצ\"ש");

            //addPrep("ל", new PrepForObject());
            //addPrep("של", new PrepOfObject());
            //addPrep("את", new PrepNoneObject());
            //addPrep("עם", new PrepWithObject());
            //addPrep("ב", new PrepInObject());
            //addPrep("מ", new PrepFromObject());
            //addPrep("אצל", new PrepAtObject());
            //addPrep("בשביל", new PrepForObject());
            //addPrep("בגלל", new PrepBecauseOfObject());
            //addPrep("במשך", new PrepDurationObject());
            //addPrep("לאחר", new PrepAfterObject());
            //addPrep("כמו", new PrepLikeObject());
            //addPrep("על", new PrepAboutObject());
            //addPrep("אל", new PrepNoneObject());
            //addPrep("לפני", new PrepositionObject());
            //addPrep("אחרי", new PrepositionObject());
            //addPrep("מעל", new PrepositionObject());
            //addPrep("מתחת", new PrepositionObject());
            //addPrep("", new PrepositionObject());

            //addQuestion("מה", Question.What);
            //addQuestion("למה", Question.Why);
            //addQuestion("לאן", Question.Where);
            //addQuestion("כמה", Question.HowMatch);
            //addQuestion("איך", Question.How);
            //addQuestion("מדוע", Question.Why);
            //addQuestion("האם", Question.IsIt);
            //addQuestion("מתי", Question.When);

            //addGuf("אני", "אתה",Guf.Rishon,Kamot.Yahid,Min.Both);
            //addGuf("אנחנו", "אתם", Guf.Rishon, Kamot.Rabim, Min.Both);

            //addGuf("אתה", "אני", Guf.Sheny, Kamot.Yahid, Min.Zahar);
            //addGuf("את", "אני", Guf.Sheny, Kamot.Yahid, Min.Zahar);
            //addGuf("אתם", "אנחנו", Guf.Sheny, Kamot.Rabim, Min.Both);
            //addGuf("אתן", "אנחנו", Guf.Sheny, Kamot.Rabim, Min.Both);

            //addGuf("הוא", "הוא", Guf.Shlishi, Kamot.Yahid, Min.Zahar);
            //addGuf("היא", "היא", Guf.Shlishi, Kamot.Yahid, Min.Zahar);
            //addGuf("הם", "הם", Guf.Shlishi, Kamot.Rabim, Min.Both);
            //addGuf("הן", "הן", Guf.Shlishi, Kamot.Rabim, Min.Both);


            //addGuf("זה", "זה", Guf.Shlishi, Kamot.Yahid, Min.Zahar);
            //addGuf("זאת", "זאת", Guf.Shlishi, Kamot.Yahid, Min.Nekeva);
            //addGuf("אלו", "אלו", Guf.Shlishi, Kamot.Rabim, Min.Both);
            //addGuf("הללו", "הללו", Guf.Shlishi, Kamot.Rabim, Min.Both);



            return true;
        }


        private void addPrep(string v, RelationObject prepositionObject)
        {
            hebDic.Add(v, new WordObject(v, prepWord, prepositionObject));
        }

        //private  void addGuf(string v1, string v2, Guf rishon, Kamot yahid, Min both)
        //{
        //    hebDic.Add(v1, new Word( v1,  v2,  rishon,  yahid,  both));
        //}

        private  void addHello(string s,WorldObject obj) { 
            hebDic.Add(s, new WordObject(s,helloWord,obj));
        
        }


        //private  void addQuestion(string s, QuestionObject.QuestionType q)
        //{
        //    hebDic.Add(s, new Word(s,questionWord));
        //}


        private  void addTime(string s)
        {
            hebDic.Add(s, new WordObject(s,timeWord));
        }





        internal  bool contains(string lemma)
        {
            return hebDic.ContainsKey(lemma);
        }

        internal  WordObject get(string lemma)
        {
            if (hebDic.ContainsKey(lemma))
            {
                return hebDic[lemma];
            }else { return null; }
        }

    }

}
