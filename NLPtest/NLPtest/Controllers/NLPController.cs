using NLPtest;
using System.Collections.Generic;
using System;
using NLPtest.view;
using NLPtest.WorldObj;
using NLPtest.NLP;
using Model.dataBase;
using static NLPtest.HebWords.WordObject;
using NLPtest.HebWords;

namespace NLPtest.Controllers
{
    public class NLPControler
    {

      //  static NLPControler instance;
       // static object syncLock = new object();
        MorfAnalizer ma = null;
        SemanticAnalizer sa = new SemanticAnalizer();

        public MorfAnalizer Ma
        {
            get
            {
                return ma;
            }

            set
            {
                ma = value;
            }
        }

        public NLPControler()
        {
            this.Ma = new MorfAnalizer();
        }

        //public static NLPControler getInstence()
        //{
        //    lock (syncLock)
        //    {
        //        if (instance != null)
        //        {
        //            return instance;
        //        }
        //        else
        //        {
        //            var nlp = new NLPControler();
        //            //     var nlp = new NLPControlerTestStub();
        //            //   nlp.Initialize();
        //            return nlp;
        //        }
        //    }

        //}

        public List<WorldObject> testAnalizer(string inputText, out string log)
        {
            log = "";
            //    var a = MorfAnalizer.createSentence(inputText);
            // var context = new TextContext();

            //  sa.findGufContext(sen);
            log += "intent:" + getUserIntent(inputText, "QuestionDialog") + Environment.NewLine;
            return Analize(inputText);
        }

        public UserIntent getUserIntent(string str,string context)
        {
            return sa.getUserIntent(str, context);
        }


        //public string getClass(string text)
        //{
        //    return ma.getClass(text);
        //}



        //public string getName(string inputText)
        //{
        //    return ma.getName(inputText);

        //}

        //public string GetGender(string text)
        //{
        //    return ma.GetGender(text);
        //}

        //public string GetGeneralFeeling(string text)
        //{
        //    return ma.GetGeneralFeeling(text);
        //}


        public virtual List<WorldObject> Analize(string text)
        {
            return Analize(text, null);
        }

        public virtual List<WorldObject> Analize(string text, string systemAnswerText)
        {
            // var context = new TextContext();
            var textAnlz = Ma.meniAnalize(text,true);
            List<WorldObject> input = new List<WorldObject>();
            List<WorldObject> sentence = new List<WorldObject>();
            List<WorldObject> last = new List<WorldObject>();
            List<List<ITemplate>> sentences;
            List<ITemplate> context = new List<ITemplate>();


            if (systemAnswerText != null)
            {
                //add mising data to entity DB
                var contextAnlz = Ma.meniAnalize(systemAnswerText,false);


                //create context 
                var contextSentences = sa.findGufContext(contextAnlz, context);
                contextSentences.ForEach(x => context.AddRange(x));
                sentences = sa.findGufContext(textAnlz, context);
            }
            else
            {
                sentences = sa.findGufContext(textAnlz, context);
            }

            string logTemp;

            foreach (var s in sentences)
            {
                sentence = sa.findTemplate(s.ToArray(), out logTemp);
                last = sentence;
                input.AddRange(sentence);
            }

            return input;
        }

        internal void updateEntityTable()
        {
            Ma.searchAllAnswerForentities();
        }
    }
}
	
