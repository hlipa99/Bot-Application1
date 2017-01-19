using static NLPtest.WordObject.WordType;
using NLPtest.WorldObj;
using NLPtest.WorldObj.ConversationFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vohmm.application;
using NLPtest.WorldObj.ObjectsWrappers;
using NLPtest.HebWords;
using NLPtest.Controllers;

namespace NLPtest.view
{

   
    class SemanticAnalizer
    {

        //public ContentTurn tagWords2(Sentence sentence, ref TextContext context)
        //{
        //    var contextTurn = new ContentTurn();
        //    //deal with questions
        //    //     var first = sentence.Words.FirstOrDefault();
        //    WorldObject prevObj = null;
        //    while (sentence.Words.Count > 0)
        //    {

        //        var newObj = Tag2(ref prevObj, ref sentence, ref contextTurn);
        //        if (newObj != null)
        //        {
        //            prevObj = newObj;
        //            contextTurn.Add(prevObj);
        //        }

        //    }

        //    return contextTurn;
        //}

        //public WorldObject Tag2(ref WorldObject prevObj, ref Sentence sentence, ref ContentTurn context)
        //{

        //    //   var word = sentence.Words.FirstOrDefault();
        //    var word = sentence.Words.FirstOrDefault();

        //    if (word != null)
        //    {
        //        sentence.Words.RemoveAt(0);


        //        if (word.isA(gufWord))
        //        {
        //            return word.WorldObject;
        //        }
        //        else if (word.isA(nounWord))
        //        {
        //            if (word.prefix == null)
        //            {
        //                if (word.WorldObject != null) return word.WorldObject;
        //                else
        //                {
        //                    return new NounObject(word.Word);
        //                }
        //            }
        //            else
        //            {

        //                WorldObject obj = new NounObject(word.Word);
        //                return obj;
        //                if (word.ha)
        //                {
        //                    obj.DefiniteArticle = true;
        //                }

        //                if (word.le)
        //                {
        //                    prevObj.addRelation(new PrepRelObject(new NounObject(word.Word), PrepType.toPrep));

        //                    return null;
        //                }
        //                else if (word.me)
        //                {
        //                    return obj;
        //                }

        //                return obj;
        //                //        throw new SemanticException("unknown prefix");
        //            }
        //        }
        //        else if (word.isA(personWord))
        //        {
        //            return word.WorldObject;
        //        }
        //        else if (word.isA(verbWord))
        //        {
        //            return word.WorldObject;
        //        }
        //        else
        //        {
        //            return null;
        //        }

        //    }
        //    else {
        //        return null;
        //    }
        //}

        //public ContentTurn findRelations(ContentTurn objects)
        //{
        //    var newTurn = new ContentTurn();
        //    bool changed = false;
        //    while (!objects.empty())
        //    {
        //        WorldObject newObject;
        //        var first = objects.Get(0);
        //        var second = objects.Get(1);
        //        var third = objects.Get(2);
        //        if ((newObject = findRelations(first, second, third)) != null)
        //        {
        //            changed = true;
        //            newTurn.Add(newObject);
        //            objects.pop();
        //            objects.pop();
        //            objects.pop();
        //        }
        //        else
        //        {
        //            newTurn.Add(objects.pop());
        //        }
        //    }

        //    if(changed)
        //    {
        //        return findRelations(newTurn);
        //    }
        //    else
        //    {
        //         return newTurn;
        //    }

        //}


        TemplateController tc = new TemplateController();
        public List<WorldObject> findTemplate(ITemplate[] objects)
        {

            List<ITemplate> obj = objects.ToList();
            do {
                objects = obj.ToArray();
                for (int i = 1; i <= 3; i++)
                {
                    for (int j = 0; j + i < objects.Length; j++)
                    {
                        if (i == 1)
                        {
                            objects = new ITemplate[] { objects[j] };
                            ITemplate o;
                            if ((o = tc.checkObjects(objects)) != null) {
                                objects[j] = o;
                            }
                        }
                        else if (i == 2)
                        {
                            objects = new ITemplate[] { objects[j], objects[j + 1] };
                            ITemplate o;
                            if ((o = tc.checkObjects(objects)) != null)
                            {
                                objects[j] = o;
                                objects[j + 1] = null;
                            }
                        }

                        else
                        {
                            objects = new ITemplate[] { objects[j], objects[j + 1], objects[j + 2] };
                            ITemplate o;
                            if ((o = tc.checkObjects(objects)) != null)
                            {
                                objects[j] = o;
                                objects[j + 1] = null;
                                objects[j + 2] = null;
                            }
                        }

                    }
                }

                foreach (var o in objects)
                {
                    obj.Add(o);
                }

            } while (obj.Count != objects.Count());


            List<WorldObject> res = new List<WorldObject>();
            foreach(var o in obj)
            {
                res.Add(o as WorldObject);
            }

            return res;
        }
            






            



        //public ContentTurn tagWords(Sentence sentence, ref TextContext context)
        //{
        //    var contextTurn = new ContentTurn();
        //    //deal with questions
        //    //     var first = sentence.Words.FirstOrDefault();
        //    WorldObject prevObj = null;
        //    while (sentence.Words.Count > 0)
        //    {

        //        var newObj = Tag(ref prevObj, ref sentence, ref contextTurn);
        //        if (newObj != null)
        //        {
        //            prevObj = newObj;
        //            contextTurn.Add(prevObj);
        //        }

        //    }

        //    return contextTurn;
        //}





        public List<WorldObject> findGufContext(List<WorldObject> last, List<WorldObject> current)
        {
            return findGufContextHlpr(last, current);
        }

        public List<WorldObject> findGufContext(List<WorldObject> objects)
        {
            return findGufContextHlpr(objects, objects);
        }

        private List<WorldObject> findGufContextHlpr(List<WorldObject> context, List<WorldObject> target)
        {
            foreach (var o in target)
            {
                if(o is gufObject)
                {
                    var g = getGuf(o as gufObject, context);
                    var i = target.IndexOf(o);
                    target.RemoveAt(i);
                    target.Insert(i, g);
                }
            }
            return target;
        }

        private WorldObject getGuf(gufObject gufObject, List<WorldObject> objects)
        {
            if(gufObject.Guf == gufObject.gufType.First)
            {
                if (gufObject.Amount == gufObject.amountType.singular)
                {
                    return new UserObject("");//TODO getUserObjectname
                }else
                {
                    return new multyPersoneObject(new WorldObject[] { new UserObject(""), new BotObject("") }); //TODO det
                }
              
            }else if(gufObject.Guf == gufObject.gufType.First)
            {
                if (gufObject.Amount == gufObject.amountType.singular)
                {
                    return new BotObject("");//TODO getUserObjectname
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

        //public WorldObject Tag(ref WorldObject prevObj, ref Sentence sentence, ref ContentTurn context)
        //{

        //    //   var word = sentence.Words.FirstOrDefault();
        //    var word = sentence.Words.FirstOrDefault();

        //    if (word != null)
        //    {
        //        sentence.Words.RemoveAt(0);
        //        if (word.isA(helloWord))
        //        {
        //            return (HelloObject)word.WorldObject;
        //        }
        //        else if (word.isA(questionWord))
        //        {
        //            return (QuestionObject)word.WorldObject;

        //        }
        //        else if (word.isA(hyphenWord))
        //        {
        //            var objective = Tag(ref prevObj, ref sentence, ref context);

        //            if (objective != null)
        //            {
        //                prevObj.addRelation(new expansionRelObject(objective));
        //                prevObj = objective;
        //            }
        //            else
        //            {
        //                throw new SemanticException("prep without objective");
        //            }
        //            return null;
        //        }
        //        else if (word.isA(copulaWord))
        //        {
        //            var objective = Tag(ref prevObj, ref sentence, ref context);

        //            if (objective != null)
        //            {
        //                var guf = (gufObject)word.WorldObject;
        //                prevObj.addRelation(new copulaRelObject(objective, guf));
        //                prevObj = objective;
        //                return null;
        //            }
        //            else
        //            {
        //                return prevObj;
        //              //  throw new SemanticException("prep without objective");
        //            }
        //            return null;
        //        }

        //        else if (word.isA(gufWord))
        //        {
        //            return word.WorldObject;
        //        }
        //        else if (word.isA(nounWord))
        //        {
        //            if (word.prefix == null)
        //            {
        //                if (word.WorldObject != null) return word.WorldObject;
        //                else
        //                {
        //                    return new NounObject(word.Word);
        //                }
        //            }
        //            else
        //            {

        //                WorldObject obj = new NounObject(word.Word);
        //                if (word.ha)
        //                {
        //                    obj.DefiniteArticle = true;
        //                }

        //                if (word.le)
        //                {
        //                    prevObj.addRelation(new PrepRelObject(new NounObject(word.Word), PrepType.toPrep));
        //                    return null;
        //                }
        //                else if (word.me)
        //                {
        //                    return obj;
        //                }

        //                return obj;
        //                //        throw new SemanticException("unknown prefix");
        //            }
        //        }
        //        else if (word.isA(personWord))
        //        {
        //            if (word.WorldObject != null) return word.WorldObject;
        //            else
        //            {
        //                return new PersonObject(word.Word);
        //            }
        //        }
        //        else if (word.isA(negationWord))
        //        {
        //            var objective = Tag(ref prevObj, ref sentence, ref context);
        //            objective.Negat = true;
        //            return objective;
        //        }
        //        else if (word.isA(verbWord))
        //        {
        //            if(prevObj != null)
        //            {
        //                var objective = new VerbObject(word.Word);
        //                prevObj.addRelation(new VerbRelObject(objective));
        //                prevObj = objective;
        //            }
        //            else
        //            {
        //                var prev = Tag(ref prevObj, ref sentence, ref context);
        //                var objective = new VerbObject(word.Word);
        //                prev.addRelation(new VerbRelObject(objective));
        //                return prevObj;
        //            }
                   
        //            return null;
        //        }
        //        else if (word.isA(timeWord))
        //        {
        //            if (word.WorldObject != null) return word.WorldObject;
        //            else
        //            {
        //                return new TimeObject(word.Word);
        //            }

        //        }
        //        else if (word.isA(adjectiveWord | adverbWord))
        //        {
        //            if (prevObj != null)
        //            {
        //                prevObj.addRelation(new adjectiveRelObject(new AdjObject(word.Word)));
        //                return null;
        //            }else
        //            {
        //                var objective = Tag(ref prevObj, ref sentence, ref context);
        //                objective.addRelation(new adjectiveRelObject(new AdjObject(word.Word)));
        //                return objective;
        //            }
        //        }
        //        else if (word.isA(prepWord))
        //        {
        //            var objective = Tag(ref prevObj, ref sentence, ref context);
        //            if (objective != null & prevObj != null)
        //            {
        //                prevObj.addRelation(new PrepRelObject(objective, ((PrepRelObject)word.WorldObject).Type));
        //                return prevObj;
        //            }
        //            else
        //            {
        //                var objective2 = Tag(ref prevObj, ref sentence, ref context);
        //                objective2.addRelation(new PrepRelObject(objective, ((PrepRelObject)word.WorldObject).Type));
        //                return objective2;
        //            }
                  
        //        }
        //        else if (word.isA(locationWord))
        //        {
        //            return word.WorldObject;
        //        }
        //        else if (word.isA(conjunctionWord))
        //        {
        //            var objective = Tag(ref prevObj, ref sentence, ref context);
        //            if (objective != null)
        //            {
        //                prevObj.addRelation(new PrepRelObject(objective, ((PrepRelObject)word.WorldObject).Type));
        //                prevObj = objective;
        //            }
        //            else
        //            {
        //                //     throw new SemanticException("prep without objective");
        //            }
        //            return null;
        //        }

        //        else
        //        {
        //            return null;
        //            //      throw new NotImplementedException();
        //        }
        //    }
        //    else
        //    {
        //        return null;
        //    }



        //}

        internal bool isAName(WordObject w)
        {
            return w.isA(personWord) || w.isA(properNameWord);
        }

        private Sentence nose(Sentence sentence, WordObject word)
        {
            sentence.Nose = word.WorldObject;
            return sentence;
        }



    }
}


