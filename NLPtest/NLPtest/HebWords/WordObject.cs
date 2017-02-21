using System;
using static NLPtest.WorldObj.PrepRelObject;
using NLPtest.WorldObj;
using vohmm.corpus;
using static NLPtest.WordObject.WordType;
using static NLPtest.personObject;
using Newtonsoft.Json;

namespace NLPtest
{
    public class WordObject : ITemplate
    {
        private WorldObject worldObject = null;
        private AnalysisInterface analysisInterface;

        WordType wordType = unknownWord;
        private string lemma;
        string text;
        private personType person = personType.unspecified;
        private timeType time = timeType.unspecified;
        private amountType amount = amountType.unspecified;
        private genderType gender = genderType.unspecified;

        private String ner;

        private String polarity;
        private String pos;
        private String posType;
        private String[] prefixes;
        private String tense;
        private String suffixFunction;

        private bool isDefinite;

        [JsonConstructor]
        public WordObject(string ner, string text, string gender, string number, string person, string polarity, string pos, string posType, string[] prefixes, string tense, string suffixFunction, string suffixGender, string suffixNumber, string suffixPerson, bool isDefinite,String lemma)
        {
            try
            {
                this.Ner = ner;
                this.text = text;
                this.polarity = polarity;
                this.Pos = pos;
                this.posType = posType;
                this.Prefixes = prefixes;
                this.tense = tense;
                this.suffixFunction = suffixFunction;
                this.Lemma = lemma;

                this.Gender = (genderType)Enum.Parse(typeof(genderType), gender);
                this.Amount = (amountType)Enum.Parse(typeof(amountType), number);
                this.Person = (personType)Enum.Parse(typeof(personType), person);
                this.Gender = this.Gender == genderType.unspecified & suffixGender != null ? (genderType)Enum.Parse(typeof(genderType), suffixGender) : this.Gender;
                this.Amount = this.Amount == amountType.unspecified & suffixNumber != null ? (amountType)Enum.Parse(typeof(amountType), suffixNumber) : this.Amount;
                this.Person = this.Person == personType.unspecified & suffixPerson != null ? (personType)Enum.Parse(typeof(personType), suffixPerson) : this.Person;
                this.Amount = (amountType)Enum.Parse(typeof(amountType), number);
                this.Person = (personType)Enum.Parse(typeof(personType), person);

                this.IsDefinite = isDefinite;
                findObjectType();
            }catch(Exception ex)
            {

            }
        }

        private void findObjectType()
        {
            if (Ner != "O")
            {
                if (Ner.Contains("ORG"))
                {
                    wordType = orginazationWord | nounWord;
                }
                else if (Ner.Contains("MISC__AFF"))
                {
                    wordType = identityWord | nounWord;
                }
                else if (Ner.Contains("PERS"))
                {
                    wordType = personWord | nounWord;

                }
                else if (Ner.Contains("MISC_EVENT"))
                {
                    wordType = eventWord;

                }
                else if (Ner.Contains("LOC"))
                {
                    wordType = locationWord | nounWord;
                }
                else if (Ner.Contains("DATE"))
                {
                    wordType = dateWord;
                }
                else if (Ner.Contains("Time"))
                {
                    wordType = timeWord;

                }
                else if (Ner.Contains("MONEY"))
                {
                    wordType = moneyWord;
                }
                else if (Ner.Contains("PERCENT"))
                {
                    wordType = precentWord;

                }
                else
                {
                    throw new Exception("unknown NRI");
                }


            }


            else if (Pos == "interrogative" || Pos == "quantifier")
            {
                wordType = questionWord;
                //if(bitmaskResolver.getPOSType == "PRONOUN" "PROADVERB" "PRODET" "PRODET" "YESNO")

            }
            else if (Pos == "interjection")
            {
                throw new DictionaryException(text);
            }
            else if (Pos == "verb")
            {
                wordType = verbWord;
            }
            else if (Pos == "adverb")
            {
                wordType = adverbWord;
            }
            else if (Pos == "noun")
            {
                wordType = nounWord;
            }
            else if (Pos == "negation")
            {
                wordType = negationWord;
            }
            else if (Pos == "pronoun" && posType == "personal")
            {
                wordType = gufWord;
            }
            else if (Pos == "preposition")
            {
                //             throw new DictionaryException(text);
                wordType = prepWord;
            }
            else if (Pos == "punctuation")
            {

                if (posType == "question-mark")
                {
                    wordType = questionWord;
                }
                else if (posType == "mark")
                {
                    wordType = markWord;
                }
                else if (Text == "-" || Text == "–")
                {
                    wordType = hyphenWord;
                }
                else
                {
                    wordType = unknownWord;
                }
            }
            else if (Pos == "copula")
            {
                wordType = copulaWord;
            }
            else if (Pos == "conjunction")
            {
                wordType = conjunctionWord;
            }
            else if (Pos == "adjective")
            {
                wordType = adjectiveWord;

            } else if (Pos == "participle"){
                wordType = participleWord;
            }
            else if (Pos == "numeral")
            {
                wordType = numeralWord;

            }
            else if (Pos == "propername")
            {
                wordType = properNameWord;
            }
            else 
            {
                wordType = unknownWord;
            }

        }

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
               }else if (isA(copulaWord) || isA(gufWord))
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
                        worldObject = new LocationObject(Text);
                    }
                    else if (isA(markWord))
                    {
                        worldObject = new toneObject("mark");
                    }
                    else if (isA(adjectiveWord))
                    {
                        worldObject = new AdjObject(Text);
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
                        worldObject = new WorldObject(Text);
                    }

                }







                return worldObject;
            }

            set
            {
                worldObject = value;
            }
        }

        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                text = value;
            }
        }

        public string[] Prefixes
        {
            get
            {
                return prefixes;
            }

            set
            {
                prefixes = value;
            }
        }

        public string Ner
        {
            get
            {
                return ner;
            }

            set
            {
                ner = value;
            }
        }

        public string Pos
        {
            get
            {
                return pos;
            }

            set
            {
                pos = value;
            }
        }

        public string Lemma
        {
            get
            {
                return lemma;
            }

            set
            {
                lemma = value;
            }
        }

        internal personType Person
        {
            get
            {
                return person;
            }

            set
            {
                person = value;
            }
        }

        internal timeType Time
        {
            get
            {
                return time;
            }

            set
            {
                time = value;
            }
        }

        internal amountType Amount
        {
            get
            {
                return amount;
            }

            set
            {
                amount = value;
            }
        }

        internal genderType Gender
        {
            get
            {
                return gender;
            }

            set
            {
                gender = value;
            }
        }

        public bool IsDefinite
        {
            get
            {
                return isDefinite;
            }

            set
            {
                isDefinite = value;
            }
        }

        private WorldObject getVerbFromWord( )
        {
            var res = new VerbObject(Text);
            return res;
        }

        private WorldObject getTimeFromWord( )
        {
            var res = new TimeObject(Text);
            return res;
        }

        private WorldObject getQuestionFromWord( )
        {
            var res = new QuestionObject(Text);
            return res;
        }

        private WorldObject getPrecentageFromWord( )
        {
            throw new NotImplementedException();
        }

        private WorldObject getPersonFromWord( )
        {
            var res = new PersonObject(Text);
            return res;
        }

        private WorldObject getOrginazationFromWord( )
        {
            var res = new OrginazationObject(Text);
            return res;
        }

        private WorldObject getNounFromWord( )
        {
            var res = new NounObject(Text);
            return res;
        }

        private WorldObject getMoneyFromWord( )
        {
            throw new NotImplementedException();
        }

        private WorldObject getLocationWord( )
        {
            var res = new LocationObject(Text);
            return res;
        }

        private WorldObject getHelloFromWord( )
        {
            var res = new HelloObject(Text);
            return res;
        }

        private WorldObject getadverbFromWord( )
        {
            var res = new VerbObject(Text);
            return res;
        }

        private WorldObject getGufFromWord( )
        {
            var gufObj = new personObject(Amount, Person, Time, Gender, Text);
            return gufObj;
        }

        private WorldObject getEventFromWord( )
        {
            var res = new EventObject(Text);
            return res;
        }

        private WorldObject getDateFromWord( )
        {
            var res = new TimeObject(Text);
            return res;
        }



        public WordObject(string word, WordType t)
        {
            this.Text = word;
            this.WordT = t;

        }

        public WordObject(string word, WordType t, WorldObject worldObject) : this(word, t)
        {
            this.WorldObject = worldObject;
        }

        
        public override string ToString()
        {
            return Text + "(" + Enum.GetName(typeof(WordType), WordT) + ")";

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
            negationWord = 16777216,
            participleWord = 33554432
        }

        internal void setGender(string gen)
        {
            if (gen == "masculine") Gender =  genderType.masculine;
            else if(gen == "feminine") Gender = genderType.feminine;
            else Gender = genderType.unspecified;
        }

        internal void setAmount(string am)
        {
            if (am == "singular") Amount = amountType.singular;
            else if (am == "plural") Amount = amountType.plural;
            else Amount = amountType.unspecified;
        }

        internal void setTime(string time)
        {
            if (time == "future") this.Time = timeType.future;
            else if (time == "past") this.Time = timeType.past;
            else if (time == "present") this.Time = timeType.present;
            else this.Time = timeType.unspecified;
        }

        internal void setGuf(string guf)
        {
            if (guf == "1") this.Person = personType.First;
            else if (guf == "2") this.Person = personType.Second;
            else if (guf == "3") this.Person = personType.Third;
            else this.Person = personType.unspecified;
        }


        public new int ObjectType()
        {
            return 0;
        }

        public bool haveTypeOf(ITemplate template)
        {
            if (template.ObjectType() != ObjectType()) return false;
            var w = template as WordObject;
            if (WordT.HasFlag(w.WordT)) return true;
            return false;
        }
    }
}