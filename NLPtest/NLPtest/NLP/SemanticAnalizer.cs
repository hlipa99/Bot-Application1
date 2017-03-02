
using NLPtest.WorldObj;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vohmm.application;
using NLPtest.HebWords;
using NLPtest.Controllers;
using NLPtest.MorfObjects;
using NLPtest.view;
using NLPtest.Exceptions;
using static NLPtest.HebWords.WordObject.WordType;

namespace NLPtest.NLP
{

   
    public class SemanticAnalizer
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
        private readonly int Word_Type = 0;

        public List<WorldObject> findTemplate(ITemplate[] original,out string log)
        {
            tc.loadTemplates();//debug
            List<ITemplate> resualtObjList = original.ToList();

            do {
                original = resualtObjList.ToArray();
                for(int k =0;k<= tc.TEMPLATE_RANGE; k++) { 
                for (int i = 1; i <= 3; i++)
                {
                        for (int j = resualtObjList.Count - i; j >= 0; j--)
                        {
                            if (i == 1)
                            {
                                //template of 1 words
                                var testPart = new ITemplate[] { resualtObjList[j] };
                                ITemplate o;
                                if ((o = tc.checkObjects(testPart,k)) != null)
                                {
                                    resualtObjList.RemoveAt(j);
                                    resualtObjList.Insert(j, o);
                                    k = 0;
                                }
                            }
                            else if (i == 2)
                            {
                                //template of 2 words
                                var testPart = new ITemplate[] { resualtObjList[j], resualtObjList[j + 1] };
                                ITemplate o;
                                if ((o = tc.checkObjects(testPart,k)) != null)
                                {
                                    resualtObjList.RemoveAt(j);
                                    resualtObjList.RemoveAt(j);
                                    resualtObjList.Insert(j, o);
                                    k = 0;
                                    //      j = j + 1;
                                }

                            }

                            else
                            {
                                //template of 3 words
                                var testPart = new ITemplate[] { resualtObjList[j], resualtObjList[j + 1], resualtObjList[j + 2] };
                                ITemplate o;
                                if ((o = tc.checkObjects(testPart,k)) != null)
                                {
                                    resualtObjList.RemoveAt(j);
                                    resualtObjList.RemoveAt(j);
                                    resualtObjList.RemoveAt(j);
                                    resualtObjList.Insert(j, o);
                                    k = 0;
                                    //   j = j + 2;
                                }

                            }
                        }
                    }
                   
                }

            } while (resualtObjList.Count < original.Count());


            List<WorldObject> res = new List<WorldObject>();
            foreach(var o in resualtObjList)
            {
                if(o.ObjectType() == 1) //worldobject
                {
                    res.Add(o as WorldObject);
                }else
                { //TODO take care of unknown objects
                 //   res.Add(new WorldObject(o.ToString()));
                }
            }
            log = tc.log;
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





        public List<List<ITemplate>> findGufContext(List<List<WordObject>> all, List<ITemplate> context)
        {
            List<List<ITemplate>> res = new List<List<ITemplate>>();
            foreach (var s in all)
            {
                List<ITemplate> words = new List<ITemplate>();
                words.AddRange(s);
                var newS = findGufContextHlpr(words, words);
                newS = findGufContextHlpr(newS, context);
                context = newS;
                res.Add(newS);
            }

            return res;
        }

        //public List<WorldObject> findGufContext(List<WordObject> objects)
        //{
        //    return findGufContextHlpr(objects, objects);
        //}

        private List<ITemplate> findGufContextHlpr(List<ITemplate> target, List<ITemplate> context)
        {
            List<ITemplate> res = new List<ITemplate>();

            foreach (var o in target)
            {
                if (o.ObjectType() == Word_Type)
                {
                    var word = o as WordObject;
                    if (word.isA(gufWord))
                    {
                        var g = getGuf(word, context);
                        if(g != null)
                        {
                            res.Add(g);
                        }else
                        {
                            res.Add(o);
                        }
                    }
                    else
                    {
                        res.Add(o);
                    }
                }
            }
                return res;
            
        }

        private ITemplate getGuf(WordObject gufObject, List<ITemplate> context)
        {
            if(gufObject.Person == personObject.personType.First)
            {
                if (gufObject.Amount == personObject.amountType.singular)
                {
                    return new UserObject("<userName>");//TODO getUserObjectname
                }else
                {
                    return new multyPersoneObject(new WorldObject[] { new UserObject("<userName>"), new BotObject("<botName>") }); //TODO det
                }
              
            }else if(gufObject.Person == NLPtest.personObject.personType.Second)
            {
                if (gufObject.Amount == NLPtest.personObject.amountType.singular)
                {
                    return new BotObject("botName");//TODO getUserObjectname
                }
                else
                {
                    throw new GufException(gufObject.Text);
                }
            }else //third person
            {
                foreach (var o in context)
                {
                    if (o.ObjectType() == Word_Type)
                    {
                        var w = o as WordObject;
                        if (w.isA(personWord))
                        {
                            return o;
                        }
                        else if (w.isA(orginazationWord) && gufObject.Amount == personObject.amountType.plural)
                        {
                            return o;
                        }
                        else if (w.isA(nounWord) && gufObject.Gender == w.Gender)
                        {
                            return o;
                        }
                    }
                    else
                    {

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

        OuterAPIController ac = new OuterAPIController();

        public UserIntent getUserIntent(string input,string context)  //tempfunction!! TODO
        {
            UserIntent intent;
            try
            {
                var intentStr = ac.getIntentApiAi(input, context);
                intent = (UserIntent) Enum.Parse(typeof(UserIntent), intentStr.Replace(" ",""));
            }catch(Exception ex)
            {
                return UserIntent.unknown;
            }

            return intent;

        }

    }

    public enum UserIntent
    {
        DefaultFallbackIntent,
        dontKnow,
        stopSession,
        answer,
        question,
        introduction,
        howAreYou,
        unknown,
        historyAnswer,
        yes,
        no,
    }
}


