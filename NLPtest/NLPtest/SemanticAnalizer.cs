using static NLPtest.Word.WordType;
using NLPtest.WorldObj;
using NLPtest.WorldObj.ConversationFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vohmm.application;
using NLPtest.WorldObj.ObjectsWrappers;

namespace NLPtest.view
{
    class SemanticAnalizer
    {


        public ContentTurn tagWords(Sentence sentence, ref NLPtest.Context context)
        {
            var contextTurn = new ContentTurn();
            //deal with questions
       //     var first = sentence.Words.FirstOrDefault();
            WorldObject prevObj = null;
            while (sentence.Words.Count > 0)
            {

                var newObj = Tag(ref prevObj, ref sentence, ref contextTurn);
                if (newObj != null)
                {
                    prevObj = newObj;
                    contextTurn.Add(prevObj);
                }

            }

            return contextTurn;
        }



     


        public WorldObject Tag(ref WorldObject prevObj,ref Sentence sentence, ref ContentTurn context) {

            //   var word = sentence.Words.FirstOrDefault();
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
                else if (word.isA(hyphenWord))
                {
                    var objective = Tag(ref prevObj, ref sentence, ref context);

                    if (objective != null)
                    {
                        prevObj.addRelation(new expansionRelObject(objective));
                        prevObj = objective;
                    }
                    else
                    {
                        throw new SemanticException("prep without objective");
                    }
                    return null;
                }
                else if (word.isA(gufWord))
                {
                    return word.WorldObject;
                }
                else if (word.isA(nounWord))
                {
                    if (word.prefix == null)
                    {
                        if (word.WorldObject != null) return word.WorldObject;
                        else
                        {
                            return new NounObject(word.word);
                        }
                    }else
                    {
                        if (word.le)
                        {
                            prevObj.addRelation(new PrepRelObject(new NounObject(word.word),PrepType.toPrep));
                            return null;
                        }else if (word.ha)
                        {
                            return new DefiniteArticleWrap(new NounObject(word.word));
                        }


                        throw new SemanticException("unknown prefix");
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
                    var objective = new VerbObject(word.word);
                    prevObj.addRelation(new VerbRelObject(objective));
                    return objective;
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
                    prevObj.addRelation(new adjectiveRelObject(new AdjObject(word.word)));
                    return null;
                }
                else if (word.isA(prepWord))
                {
                    var objective = Tag(ref prevObj, ref sentence, ref context);
                    if (objective != null) {
                        prevObj.addRelation(new PrepRelObject(objective, ((PrepRelObject)word.WorldObject).Type));
                        prevObj = objective;
                    }
                    else
                    {
                  //      throw new SemanticException("prep without objective");
                    }
                    return null;
                }
                else if (word.isA(locationWord))
                {
                    return word.WorldObject;
                }
                else if (word.isA(conjunction))
                {
                    var objective = Tag(ref prevObj, ref sentence, ref context);
                    if (objective != null)
                    {
                        prevObj.addRelation(new PrepRelObject(objective, ((PrepRelObject)word.WorldObject).Type));
                        prevObj = objective;
                    }
                    else
                    {
                   //     throw new SemanticException("prep without objective");
                    }
                    return null;
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
