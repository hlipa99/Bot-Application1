using System;
using static NLPtest.WorldObj.PrepRelObject;
using NLPtest.WorldObj;
using vohmm.corpus;
using static NLPtest.Word.WordType;
using static NLPtest.gufObject;

namespace NLPtest
{
    public class Word : WorldObject
    {
        private WorldObject worldObject = null;
        private AnalysisInterface analysisInterface;



        public string root;
        internal gufType guf = gufType.unspecified;
        internal timeType time = timeType.unspecified;
        internal amountType amount = amountType.unspecified;
        internal genderType gender = genderType.unspecified;
        internal object prefix;
        internal bool le;
        internal bool me;
        internal bool ce;
        internal bool be;
        internal bool ha;
        internal bool kshe;
        internal bool ve;
        internal bool sh;


        WordType wordType = unknownWord;
        public AnalysisInterface AnalysisInterface
        {
            get
            {
                return analysisInterface;
            }

            set
            {
                analysisInterface = value;
            }
        }

        public WordType WordT
        {
            get
            {
                return wordType;
            }

            set
            {
                wordType = value;
            }
        }

        public WorldObject WorldObject
        {
            get
            {
                WorldObject res = null;
           
                if (worldObject == null)
                {

                        if (isA(dateWord)) {
                        worldObject = getDateFromWord();
               }else if (isA(copulaWord))
                    {
                        worldObject = getGufFromWord();
                    }
                    else if (isA(eventWord))
                    {
                        worldObject = getEventFromWord();
                    }
                   
                    else if (isA(adverbWord))
                    {
                        worldObject = getadverbFromWord();
                    }
                    else if (isA(helloWord))
                    {
                        worldObject = getHelloFromWord();
                    }
                    else if (isA(locationWord & nounWord))
                    {
                        worldObject = new LocationObject(Word);
                    }
                    else if (isA(markWord))
                    {
                        worldObject = new toneObject("mark");
                    }
                    else if (isA(adjectiveWord))
                    {
                        worldObject = new AdjObject(Word);
                    }
                    else if (isA(conjunctionWord))
                    {
                        worldObject = new ConjunctionRelObject(null);
                    }
                  
                    else if (isA(orginazationWord))
                    {
                        worldObject = getOrginazationFromWord();
                    }
                    else if (isA(personWord))
                    {
                        worldObject = getPersonFromWord();
                    }
                    else if (isA(prepWord))
                    {
                        worldObject = new PrepRelObject(null);
                    }
                    else if (isA(questionWord))
                    {
                        worldObject = getQuestionFromWord();
                    }
                    else if (isA(timeWord))
                    {
                        worldObject = getTimeFromWord();
                    }
                    else if (isA(unknownWord))
                    {
                        worldObject = new WorldObject();
                    }
                    else if (isA(verbWord))
                    {
                        worldObject = getVerbFromWord();
                    }
                    else if (isA(nounWord))
                    {
                        worldObject = getNounFromWord();
                    }else
                    {
                        worldObject = new WorldObject(Word);
                    }

                }







                return worldObject;
            }

            set
            {
                worldObject = value;
            }
        }


        private WorldObject getVerbFromWord( )
        {
            var res = new VerbObject(Word);
            return res;
        }

        private WorldObject getTimeFromWord( )
        {
            var res = new TimeObject(Word);
            return res;
        }

        private WorldObject getQuestionFromWord( )
        {
            var res = new QuestionObject(Word);
            return res;
        }

        private WorldObject getPrecentageFromWord( )
        {
            throw new NotImplementedException();
        }

        private WorldObject getPersonFromWord( )
        {
            var res = new PersonObject(Word);
            return res;
        }

        private WorldObject getOrginazationFromWord( )
        {
            var res = new OrginazationObject(Word);
            return res;
        }

        private WorldObject getNounFromWord( )
        {
            var res = new NounObject(Word);
            return res;
        }

        private WorldObject getMoneyFromWord( )
        {
            throw new NotImplementedException();
        }

        private WorldObject getLocationWord( )
        {
            var res = new LocationObject(Word);
            return res;
        }

        private WorldObject getHelloFromWord( )
        {
            var res = new HelloObject(Word);
            return res;
        }

        private WorldObject getadverbFromWord( )
        {
            var res = new VerbObject(Word);
            return res;
        }

        private WorldObject getGufFromWord( )
        {
            var gufObj = new gufObject(amount, guf, time, gender, Word);
            return gufObj;
        }

        private WorldObject getEventFromWord( )
        {
            var res = new EventObject(Word);
            return res;
        }

        private WorldObject getDateFromWord( )
        {
            var res = new TimeObject(Word);
            return res;
        }



        public Word(string word, WordType t)
        {
            this.Word = word;
            this.WordT = t;

        }

        public Word(string word, WordType t, WorldObject worldObject) : this(word, t)
        {
            this.WorldObject = worldObject;
        }

        public override string ToString()
        {
            return Word;

        }
        public bool isA(WordType t)
        {
            return (WordT & t) > 0;
        }


        public enum WordType
        {
            everyword = int.MaxValue,
            dateWord = 1,
            eventWord = 2,
            gufWord = 4,
            adverbWord = 8,
            helloWord = 16,
            identityWord = 32,
            locationWord = 64,
            markWord = 128,
            adjectiveWord = 256,
            moneyWord = 512,
            nounWord = 1024,
            orginazationWord = 2048,
            personWord = 4096,
            precentWord = 8192,
            prepWord = 16384,
            questionWord = 32768,
            timeWord = 65536,
            unknownWord = 131072,
            verbWord = 262144,
            copulaWord = 524288,
            conjunctionWord = 1048576,
            numeralWord = 2097152,
            properNameWord = 4194304,
            hyphenWord = 8388608,
            negationWord = 16777216
        }

        internal void setGender(string gen)
        {
            if (gen == "masculine") gender =  genderType.masculine;
            else if(gen == "feminine") gender = genderType.feminine;
            else gender = genderType.unspecified;
        }

        internal void setAmount(string am)
        {
            if (am == "singular") amount = amountType.singular;
            else if (am == "plural") amount = amountType.plural;
            else amount = amountType.unspecified;
        }

        internal void setTime(string time)
        {
            if (time == "future") this.time = timeType.future;
            else if (time == "past") this.time = timeType.past;
            else if (time == "present") this.time = timeType.present;
            else this.time = timeType.unspecified;
        }

        internal void setGuf(string guf)
        {
            if (guf == "1") this.guf = gufType.First;
            else if (guf == "2") this.guf = gufType.Second;
            else if (guf == "3") this.guf = gufType.Third;
            else this.guf = gufType.unspecified;
        }


        internal new int ObjectType()
        {
            return (int)WordT;
        }
    }
}