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

    [Serializable]
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


        public ContentTurn findGufContext(ContentTurn last, ContentTurn current)
        {
            return findGufContextHlpr(last, current);
        }

        public ContentTurn findGufContext(ContentTurn objects)
        {
            return findGufContextHlpr(objects, objects);
        }

        private ContentTurn findGufContextHlpr(ContentTurn context, ContentTurn target)
        {
            foreach (var o in target)
            {
                if(o is gufObject)
                {
                    var g = getGuf(o as gufObject, context);
                    target.replace(o, g);
                }
            }
            return target;
        }

        private WorldObject getGuf(gufObject gufObject, ContentTurn objects)
        {
            if(gufObject.Guf == gufObject.gufType.First)
            {
                if (gufObject.Amount == gufObject.amountType.singular)
                {
                    return new UserObject("");//TODO get user name
                }else
                {
                    return new multyPersoneObject(new WorldObject[] { new UserObject(""), new BotObject("") }); //TODO det
                }
              
            }else if(gufObject.Guf == gufObject.gufType.First)
            {
                if (gufObject.Amount == gufObject.amountType.singular)
                {
                    return new BotObject("");//TODO get user name
                }
                else
                {
                    throw new GufException(gufObject);
                }
            }else
            {
                foreach (var o in objects)
                {
                    if(o is PersonObject)
                    {
                        var p = o as PersonObject;
                        if(p.Gender == gufObject.Gender)
                        {
                            return p;
                        }
                    }else if(o is OrginazationObject && gufObject.Amount == gufObject.amountType.plural)
                    {
                        return o;
                    }
                }
           //     throw new GufException(gufObject);
           return null;
            }
           
        }

        public WorldObject Tag(ref WorldObject prevObj, ref Sentence sentence, ref ContentTurn context)
        {

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
                else if (word.isA(copulaWord))
                {
                    var objective = Tag(ref prevObj, ref sentence, ref context);

                    if (objective != null)
                    {
                        var guf = (gufObject)word.WorldObject;
                        prevObj.addRelation(new copulaRelObject(objective, guf));
                        prevObj = objective;
                        return null;
                    }
                    else
                    {
                        return prevObj;
                      //  throw new SemanticException("prep without objective");
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
                    }
                    else
                    {
                        if (word.le)
                        {
                            prevObj.addRelation(new PrepRelObject(new NounObject(word.word), PrepType.toPrep));
                            return null;
                        }
                        else if (word.ha)
                        {
                            return new DefiniteArticleWrap(new NounObject(word.word));
                        }

                        return null;
                        //        throw new SemanticException("unknown prefix");
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
                else if (word.isA(negationWord))
                {
                    var objective = Tag(ref prevObj, ref sentence, ref context);
                    objective.Negat = true;
                    return objective;
                }
                else if (word.isA(verbWord))
                {
                    var objective = new VerbObject(word.word);
                    prevObj.addRelation(new VerbRelObject(objective));
                    prevObj = objective;
                    return null;
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
                    if (prevObj != null)
                    {
                        prevObj.addRelation(new adjectiveRelObject(new AdjObject(word.word)));
                        return null;
                    }else
                    {
                        var objective = Tag(ref prevObj, ref sentence, ref context);
                        objective.addRelation(new adjectiveRelObject(new AdjObject(word.word)));
                        return objective;
                    }
                }
                else if (word.isA(prepWord))
                {
                    var objective = Tag(ref prevObj, ref sentence, ref context);
                    if (objective != null & prevObj != null)
                    {
                        prevObj.addRelation(new PrepRelObject(objective, ((PrepRelObject)word.WorldObject).Type));
                        return prevObj;
                    }
                    else
                    {
                        var objective2 = Tag(ref prevObj, ref sentence, ref context);
                        objective2.addRelation(new PrepRelObject(objective, ((PrepRelObject)word.WorldObject).Type));
                        return objective2;
                    }
                  
                }
                else if (word.isA(locationWord))
                {
                    return word.WorldObject;
                }
                else if (word.isA(conjunctionWord))
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
            return w.isA(personWord) || w.isA(properNameWord);
        }

        private Sentence nose(Sentence sentence, Word word)
        {
            sentence.Nose = word.WorldObject;
            return sentence;
        }



    }
}


