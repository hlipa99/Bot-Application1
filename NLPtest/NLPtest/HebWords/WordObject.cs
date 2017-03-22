using System;
using static NLP.WorldObj.PrepRelObject;
using NLP.WorldObj;
using vohmm.corpus;
using static NLP.HebWords.WordObject.WordType;
using Newtonsoft.Json;
using NLP.Exceptions;
using System.Collections.Generic;
using Model.dataBase;
using static NLP.WorldObj.personObject;

namespace NLP.HebWords
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

        internal string TypefromEnum(WordType wordT) {
            string ret = "";
            try { 
            ret = Enum.GetName(typeof(WordType), wordT);
            }catch(Exception ex)
            {
                ret = "unknownWord";
            }
            return ret;
        }

        private String ner;

        private String polarity;
        private String pos;
        private String posType;
        private String[] prefixes;
        private String tense;
        private String suffixFunction;

        private bool isDefinite;

        public WordObject() {
            this.Amount =amountType.unspecified;
            this.Person = personType.unspecified;
            this.Gender = genderType.unspecified;
                }
                               
        [JsonConstructor]      
        public WordObject(string ner, string text, string gender, string number, string person, string polarity, string pos, string posType, string[] prefixes, string tense, string suffixFunction, string suffixGender, string suffixNumber, string suffixPerson, bool isDefinite, String lemma)
        {
            try
            {
                this.Ner = ner;
                this.text = text;
                this.polarity = polarity;
                this.Pos = pos.ToLower();
                this.posType = posType;
                this.Prefixes = prefixes;
                this.tense = tense;
                this.suffixFunction = suffixFunction;
                this.Lemma = getLemma(lemma,text);

                this.IsDefinite = isDefinite;
                findObjectType();


                this.Gender = (genderType)Enum.Parse(typeof(genderType), gender.Replace(" ", ""));
                this.Amount = (amountType)Enum.Parse(typeof(amountType), number);
                this.Person = (personType)Enum.Parse(typeof(personType), person);
                this.Amount = (amountType)Enum.Parse(typeof(amountType), number);
                this.Person = (personType)Enum.Parse(typeof(personType), person);
                this.Time = (timeType)Enum.Parse(typeof(timeType), tense);
                this.Gender = this.Gender == genderType.unspecified & suffixGender != null ? (genderType)Enum.Parse(typeof(genderType), suffixGender.Replace(" ", "")) : this.Gender;
                this.Amount = this.Amount == amountType.unspecified & suffixNumber != null ? (amountType)Enum.Parse(typeof(amountType), suffixNumber) : this.Amount;
                this.Person = this.Person == personType.unspecified & suffixPerson != null ? (personType)Enum.Parse(typeof(personType), suffixPerson) : this.Person;


            } catch (Exception ex)
            {

            }
        }

        public string getLemma(string lemma,string text)
        {
            var chars = new List<char>();
            foreach (var c in Prefixes)
            {
                text = text.Substring(1);
            }

            if (lemma == null || lemma == "###NUMBER###" || lemma.Length <= 1) return text;

            if (text.Split(' ').Length > 1) return text;

            if (lemma.StartsWith("CARD")) return lemma.Remove(0, 4);
            if (lemma.StartsWith("ORD")) return lemma.Remove(0, 3);
            
            return lemma;
        }

        public WordObject(string word, WordType t)
        {
            this.Text = word;
            this.WordT = t;
            Ner = "O";
            this.Pos = Enum.GetName(typeof(WordType), t);
            this.posType = "";
            this.Prefixes = new string[0]; ;
            this.tense = "";
            this.suffixFunction = "";
            this.Lemma = getLemma(lemma, word);

        }

        public WordObject(string word, WordType t, WorldObject worldObject) : this(word, t)
        {
            this.WorldObject = worldObject;
        }

        private void findObjectType()
        {
            if (Ner != "O")
            {
                if (Ner.Contains("ORG"))
                {
                    wordType = organizationWord;
                }
                else if (Ner.Contains("MISC__AFF"))
                {
                    wordType = identityWord;
                }
                else if (Ner.Contains("PERS"))
                {
                    wordType = personWord;

                }
                else if (Ner.Contains("MISC_EVENT"))
                {
                    wordType = eventWord;

                }
                else if (Ner.Contains("LOC"))
                {
                    wordType = locationWord;
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
            else if (Pos == "negation" || Pos == "negative")
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
                else if (Text == "-" || Text == "–" || Text == ":")
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

            } else if (Pos == "participle") {
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
            else if (Pos == "wprefix")
            {
                wordType = wPrefixWord;
            }
            else if (Pos == "pronoun")
            {
                wordType = wPrefixWord;
            }
            else if (Pos == "modal")
            {
                wordType = modalWord;
            }
            else if (Pos == "existential")
            {
                wordType = existentialWord;
            }
            else if (text.Length == 1)
            {
                wordType = prefixWord;
            }
            else
            {
                wordType = unknownWord;
            }

        }

        internal WordObject clone()
        {
            var newWord = new WordObject(ner, text, "unspecified", "unspecified", "unspecified", polarity, pos, posType, prefixes, tense, suffixFunction, "unspecified", "unspecified", "unspecified", isDefinite, lemma);
                newWord.amount = amount;
            newWord.gender = gender;
            newWord.person = person;
            newWord.lemma = getLemma(lemma,text);
            return newWord;

        }

        internal static WordType typeFromString(string entityType)
        {
            try
            {
                var word = (WordType)Enum.Parse(typeof(WordType), entityType);
                return word;
            }catch(Exception ex)
            {
                return unknownWord;
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

        public virtual WordType WordT
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
                    } else if (isA(copulaWord) || isA(gufWord))
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
                        worldObject = new WorldObject(this);
                    }
                    else if (isA(adjectiveWord))
                    {
                        worldObject = new AdjObject(Text);
                    }
                    else if (isA(conjunctionWord))
                    {
                        worldObject = new ConjunctionRelObject(null);
                    }

                    else if (isA(conceptWord))
                    {
                        worldObject = new ConceptObject(text);
                    }
                    else if (isA(organizationWord))
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
                    } else
                    {
                        worldObject = new WorldObject(this);
                    }

                }


               var ent = new entity();
                ent.entityValue = text;
                ent.entityType = Enum.GetName(typeof(WordType), WordT);
                ent.entityValue = text;
                ent.entitySynonimus = ";" + text + ";";
                worldObject.Entity = ent;

                return worldObject;
            }

            set
            {
                worldObject = value;
            }
        }

        public bool isEntity()
        {
            return WordT == organizationWord || WordT == conceptWord ||
                 WordT == eventWord || WordT == personWord ||
                  WordT == timeWord || WordT == numeralWord || WordT == nounWord || WordT == verbWord;
        }

        public virtual string Text
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

        public personType Person
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

        public timeType Time
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

        public virtual amountType Amount
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

        public virtual genderType Gender
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

        private WorldObject getPersonFromWord( )
        {
            var res = new PersonObject(Text);
            return res;
        }

        private WorldObject getOrginazationFromWord( )
        {
            var res = new OrganizationObject(Text);
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




        
        public override string ToString()
        {
            return Text + "(" + Enum.GetName(typeof(WordType), WordT) + ")";

        }
        public bool isA(WordType t)
        {
            return WordT == t;
        }


        public enum WordType
        {
            unknownWord,
            dateWord,
            eventWord,
            gufWord,
            adverbWord,
            helloWord ,
            identityWord ,
            locationWord ,
            markWord ,
            adjectiveWord ,
            moneyWord ,
            nounWord ,
            organizationWord,
            personWord,
            precentWord ,
            prepWord ,
            questionWord,
            timeWord ,
           
            verbWord,
            copulaWord ,
            conjunctionWord ,
            numeralWord,
            properNameWord ,
            hyphenWord ,
            negationWord ,
            participleWord ,
            wPrefixWord ,
            pronoun,
            modalWord,
            conceptWord,
            existentialWord,
            prefixWord
     
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


        public int ObjectType()
        {
            return 0;
        }

        public virtual bool haveTypeOf(ITemplate template)
        {
            if (template.ObjectType() != ObjectType()) return false;
            var w = template as WordObject;
            if (w.isA(WordT)) return true;
            return false;
        }

        public override bool Equals(Object obj)
        {


            var objres = obj as WordObject;
            if (objres == null) return false;
            if (!(this.Text == objres.Text &&
            this.WordT == objres.WordT &&
             this.gender == objres.gender &&
              this.amount == objres.amount &&
                this.person == objres.person &&
                       this.Time == objres.Time )) return false;


            return true;
        }
    }
}