using System;
using static NLPtest.WorldObj.PrepRelObject;
using NLPtest.WorldObj;
using vohmm.corpus;
using static NLPtest.Word.WordType;
namespace NLPtest
{
    public class Word
    {
        private WorldObject worldObject;

        public string root;
        internal string guf;
        internal string time;
        internal string amount;
        internal string gender;
        internal object prefix;
        internal bool le;
        internal bool me;
        internal bool ce;
        internal bool be;
        internal bool ha;
        internal bool kshe;
        internal bool ve;
        internal bool sh;


        WordType wordType;
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

                    switch (wordType)
                    {
                        case dateWord:
                            worldObject = getDateFromWord(word);
                            break;
                        case copulaWord:
                            worldObject = getGufFromWord(word);
                            break;
                        case eventWord:
                            worldObject = getEventFromWord(word);
                            break;
                        case gufWord | nounWord:
                            worldObject = getGufFromWord(word);
                            break;
                        case adverbWord:
                            worldObject = getadverbFromWord(word);
                            break;
                        case helloWord:
                            worldObject = getHelloFromWord(word);
                            break;
                        case identityWord:
                            throw new NotImplementedException();
                            break;
                        case   locationWord | nounWord:
                            worldObject =  new LocationObject(word);
                            break;
                        case   markWord:
                            worldObject =  new toneObject("mark");
                            break;
                        case   adjectiveWord:
                            worldObject =  new AdjObject(word);
                            break;
                        case   moneyWord:
                            worldObject = getMoneyFromWord(word);
                            break;
                        case conjunction:
                            worldObject = new ConjunctionRelObject(null);
                            break;
                        case   nounWord:
                            worldObject = getNounFromWord(word);
                            break;
                        case orginazationWord | nounWord:
                            worldObject = getOrginazationFromWord(word);
                            break;
                        case personWord | nounWord:
                            worldObject = getPersonFromWord(word);
                            break;
                        case   precentWord:
                            worldObject = getPrecentageFromWord(word);
                            break;
                        case   prepWord:
                            worldObject = new PrepRelObject(null);
                            break;
                        case   questionWord:
                            worldObject = getQuestionFromWord(word);
                            break;
                        case   timeWord:
                            worldObject = getTimeFromWord(word);
                            break;
                        case   unknownWord:
                            worldObject = new WorldObject(word);
                            break;
                        case   verbWord:
                            worldObject = getVerbFromWord(word);
                            break;

                        default:
                            break;
                    }

                    if(worldObject == null)
                    {
                      //  throw new WorldObjectException(this);
                    }

                }







                return worldObject;
            }

            set
            {
                worldObject = value;
            }
        }


        private WorldObject getVerbFromWord(string word)
        {
            var res = new VerbObject(word);
            return res;
        }

        private WorldObject getTimeFromWord(string word)
        {
            var res = new TimeObject(word);
            return res;
        }

        private WorldObject getQuestionFromWord(string word)
        {
            var res = new QuestionObject(word);
            return res;
        }

        private WorldObject getPrecentageFromWord(string word)
        {
            throw new NotImplementedException();
        }

        private WorldObject getPersonFromWord(string word)
        {
            var res = new PersonObject(word);
            return res;
        }

        private WorldObject getOrginazationFromWord(string word)
        {
            var res = new OrginazationObject(word);
            return res;
        }

        private WorldObject getNounFromWord(string word)
        {
            var res = new NounObject(word);
            return res;
        }

        private WorldObject getMoneyFromWord(string word)
        {
            throw new NotImplementedException();
        }

        private WorldObject getLocationWord(string word)
        {
            var res = new LocationObject(word);
            return res;
        }

        private WorldObject getHelloFromWord(string word)
        {
            var res = new HelloObject(word);
            return res;
        }

        private WorldObject getadverbFromWord(string word)
        {
            var res = new VerbObject(word);
            return res;
        }

        private WorldObject getGufFromWord(string word)
        {
            var gufObj = new gufObject(word, time,amount, guf);
            return gufObj;
        }

        private WorldObject getEventFromWord(string word)
        {
            var res = new EventObject(word);
            return res;
        }

        private WorldObject getDateFromWord(string word)
        {
            var res = new TimeObject(word);
            return res;
        }

        internal string word;
        private AnalysisInterface analysisInterface;




        public Word(string word, WordType t)
        {
            this.word = word;
            this.WordT = t;
        }

        public Word(string word, WordType t, WorldObject worldObject) : this(word, t)
        {
            this.WorldObject = worldObject;
        }

        public override string ToString()
        {
            return word;

        }
        public bool isA(WordType t)
        {
            return (WordT & t) > 0;
        }


        public enum WordType
        {
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
            conjunction = 1048576,
            numeralWord = 2097152,
            properName = 4194304,
            hyphenWord = 8388608,

        }


    }
}