using NLP;
using System.Collections.Generic;
using System;
using NLP.view;
using NLP.WorldObj;
using NLP.NLP;
using Model.dataBase;
using static NLP.HebWords.WordObject;
using NLP.HebWords;

namespace NLP.Controllers
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

        public virtual int matchStrings(string target, string matchString)
        {
           if(target == matchString) return 100;
            else{
                var targetAnlz = Ma.meniAnalize(target, false);
                var matchStringAnlz = Ma.meniAnalize(matchString, true);
                var score = 0;
                var points = 100/(target.Split(' ').Length);
                foreach (var s in targetAnlz[0])
                {
                    if (matchStringAnlz.FindAll(x => x.FindAll(w => w.Lemma == s.Lemma).Count > 0).Count > 0)
                    {
                        score += points;
                    }
                }
                return score;
            }
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


            if (systemAnswerText != null && textAnlz.FindAll(x => x.FindAll(y=>y.isA(WordType.gufWord)).Count > 0).Count > 0)
            {
                //add mising data to entity DB
                var contextAnlz = Ma.meniAnalize(systemAnswerText,true);


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
	
