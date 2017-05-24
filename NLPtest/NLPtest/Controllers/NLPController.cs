using NLP;
using System.Collections.Generic;
using System;
using NLP.view;
using NLP.WorldObj;
using NLP.NLP;
using Model.dataBase;
using static NLP.HebWords.WordObject;
using NLP.HebWords;
using System.Runtime.Caching;
using System.Collections;
using System.Linq;
using Model.Models;

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

        public void updateEntities(IEnumerable<IentityBase> enumerable)
        {
            ma.updateEntities(enumerable);
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
            if (target == matchString) return 100;
            else
            {
                var targetAnlz = Ma.meniAnalize(target, false);
                var matchStringAnlz = Ma.meniAnalize(matchString, true);
                var score = 0;
                var points = 100 / (target.Split(' ').Length);
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

        public UserIntent getUserIntent(string str, string context)
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

        public virtual List<IentityBase> findMatchingEntities(string text)
        {
            return Ma.findMatchingEntities(text);
        }


        public virtual List<WorldObject> Analize(string text, string systemQuestionText)
        {
            ObjectCache cache = MemoryCache.Default;
            var cachedItem = cache.Get(text + systemQuestionText) as WorldObject[];

            if (cachedItem == null || cachedItem.Length == 0 || true)
            {


                // var context = new TextContext();
                var textAnlz = Ma.meniAnalize(text, systemQuestionText != null);
                List<WorldObject> input = new List<WorldObject>();
                List<WorldObject> sentence = new List<WorldObject>();
                List<WorldObject> last = new List<WorldObject>();
                List<List<ITemplate>> sentences;
                List<ITemplate> context = new List<ITemplate>();


                if (systemQuestionText != null && textAnlz.FindAll(x => x.FindAll(y => y.isA(WordType.gufWord)).Count > 0).Count > 0)
                {
                    //add mising data to entity DB
                    var contextAnlz = Ma.meniAnalize(systemQuestionText, true);

                    //create context 
                    sentences = sa.findGufContext(textAnlz, contextAnlz[0].Cast<ITemplate>().ToList());
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
                var exp = new CacheItemPolicy();
                exp.SlidingExpiration = (new TimeSpan(1, 0, 0, 1));
                cache.Set(text + systemQuestionText, input.ToArray(), exp);
                return input;
            }
            else
            {
                return new List<WorldObject>(cachedItem);
            }
        }
        internal void updateEntityTable()
        {
            Ma.searchAllAnswerForentities();
            //Ma.updateEntities();
        }

        public string getClass(string text)
        {
            return ma.getClass(text);
        }
    }
}
	
