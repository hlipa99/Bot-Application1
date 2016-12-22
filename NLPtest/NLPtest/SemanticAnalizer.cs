using static NLPtest.Word.WordType;
using NLPtest.WorldObj;
using NLPtest.WorldObj.ConversationFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vohmm.application;

namespace NLPtest.view
{
    class SemanticAnalizer
    {
      

        public ContentTurn tagWords(Sentence sentence, ref NLPtest.Context context)
        {
            var contextTurn = new ContentTurn();
            //deal with questions
            var first = sentence.Words.FirstOrDefault();
            WorldObject prevObj = null;
            while (sentence.Words.Count > 0)
            {

                 var newObj = Tag(ref prevObj,ref sentence);
                    if(newObj != null)
                    {
                        prevObj = newObj;
                        contextTurn.Add(prevObj);
                    }

            }

            return contextTurn;
        }


        public WorldObject Tag(ref WorldObject prevObj,ref Sentence sentence) {

            var word = sentence.Words.FirstOrDefault();

            if (word != null)
            {
                sentence.Words.RemoveAt(0);
                if (word.isA(helloWord))
                {
                    return (HelloObject)word.WorldObject;
                }
                else if (word.isA(questionWord))
                {
                    return (QuestionObject)word.WorldObject;

                }
                else if (word.isA(gufWord))
                {
                    return word.WorldObject;
                }
                else if (word.isA(nounWord))
                {
                    if (word.WorldObject != null) return word.WorldObject;
                    else
                    {
                        return new NounObject(word.word);
                    }
                }
                else if (word.isA(personWord))
                {
                    if (word.WorldObject != null) return word.WorldObject;
                    else
                    {
                        return new PersonObject(word.word);
                    }
                }
                else if (word.isA(verbWord))
                {
                    return new VerbObject(word.word);
                }
                else if (word.isA(timeWord))
                {
                    if (word.WorldObject != null) return word.WorldObject;
                    else
                    {
                        return new TimeObject(word.word);
                    }

                }
                else if (word.isA(adjectiveWord | adverbWord))
                {
                    prevObj.addRelation(new adjectiveRelObject(), new AdjObject(word.word));
                    return Tag(ref prevObj, ref sentence);
                }
                else if (word.isA(prepWord))
                {
                    prevObj.addRelation((RelationObject)word.WorldObject, Tag(ref prevObj, ref sentence));
                    return Tag(ref prevObj, ref sentence);
                }
                else if (word.isA(conjunction))
                {
                    prevObj.addRelation((RelationObject)word.WorldObject, Tag(ref prevObj, ref sentence));
                    return Tag(ref prevObj, ref sentence);
                }

                else
                {
                    return null;
                    //      throw new NotImplementedException();
                }
            }
            else
            {
                return null;
            }

         

        }

        internal bool isAName(Word w)
        {
            return w.isA(personWord) || w.isA(properName);
        }

        private Sentence nose(Sentence sentence, Word word)
        {
            sentence.Nose = word.WorldObject;
            return sentence;
        }



    }







}
