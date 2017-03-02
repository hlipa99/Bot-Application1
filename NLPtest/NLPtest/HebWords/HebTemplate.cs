using NLPtest;
using NLPtest.WorldObj;
using static NLPtest.HebWords.WordObject.WordType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLPtest.view;


namespace NLPtest.HebWords
{

     public class HebTemplate
    {
        string templateText;
        string[] template;
        string result;
        string[] resultFlags;
        readonly int priority;
        readonly int count;
        readonly WorldObject resualtObject;
        readonly ITemplate[] objectTemplate;

        public int Count
        {
            get
            {
                return count;
            }


        }

        public int Priority
        {
            get
            {
                return priority;
            }


        }

        // negateWord objectWord $ objectWord $ negate
        // adjactiveWord nounwords $ AjdectiveWord $ 
        //objectWord AdjObject objectWord $ objectWord1^RelationObject^objectWord2 $ 

        public HebTemplate(string text)
        {
            templateText = text;
            var firstSplit = text.Split('$');
            var template = firstSplit[0].Split(' ');
            var result = firstSplit[1];
            var resultFlags = firstSplit[2].Split(' ');
        }



         public HebTemplate(ITemplate[] template,WorldObject result,int priority)
        {
            resualtObject = result;
            this.objectTemplate = template;
            count = template.Count();
            this.priority = priority;
        }




        public bool Equals(WordObject[] words)
        {
            if (template.Count() != words.Count()) return false;

            for (int i = 0; i < template.Count(); i++)
            {
                var s = words[i].WordT.ToString();
                if (s.Trim() != template[i].Trim()) return false;
            }
            return true;
        }

        //private WorldObject getResult(WordObject[] words)
        //{
        //    List<WorldObject> objList = new List<WorldObject>();
        //   foreach(var w in words)
        //    {
        //        objList.Add(w.WorldObject);
        //    }

        //    return getResult(objList.ToArray());
        //}

       

        //private WorldObject getResult(WorldObject[] objects)
        //{

        //    var templateRes = result.Split('^');
        //    WorldObject worldObject = null;
        //    WorldObject objective = null;
        //    RelationObject relation = null;


        //    foreach (var t in templateRes)
        //    {

        //        if (t.Contains(":"))
        //        {
        //            var r = t.Split(':');
        //            var idx = getIdx(r[0]);


        //            var paramObjects = new List<WorldObject>();
        //            foreach (var param in r[1].Split(',')){
        //                var idx2 = getIdx(r[1]);
        //                if (idx2 >= 0)
        //                {
        //                    paramObjects.Add(objects[idx2]);
        //                }
        //            }

        //            if (idx >= 0)
        //            {
        //                relation = (RelationObject)objects[idx];
        //                relation.addObjective(paramObjects.ToArray());
        //            }
        //            else
        //            {
        //                Type rt = Type.GetType(r[0], true);
        //                relation = (RelationObject)(Activator.CreateInstance(rt, paramObjects));
        //                rt = Type.GetType(r[1], true);
        //            }


        //        }
        //        else
        //        {
        //            var idx = getIdx(templateRes[0]);
        //            if (idx >= 0)
        //            {
        //                worldObject = objects[idx];
        //            }
        //        }

        //    }

        //    worldObject.addRelation(relation);

        //    worldObject = addFlages(worldObject);
        //    return worldObject;
        //}

        internal ITemplate tryMatch(ITemplate[] objects)
        {
            if (objectTemplate.Count() == objects.Count())
            {
               for(int i = 0; i < objectTemplate.Count(); i++)
                {
                    if(!objects[i].haveTypeOf(objectTemplate[i]))
                    {
                        //not match
                        return null;
                    }
                }


                //recursive copy
                var newObject = resualtObject.Clone();
                newObject.CopyFromTemplate(objects);
                return newObject;
            }
            return null;
        }
      

        private WorldObject addFlages(WorldObject result)
        {
            foreach (var f in resultFlags)
            {
                switch (f)
                {
                    case "negate":
                        result.Negat = true;
                        break;
                    case "definiteArticle":
                        result.DefiniteArticle = true;
                        break;
                }
            }

            return result;
        }

        private int getIdx(string t)
        {
            var charidx = t.First();
            int idx = -1;
            if (int.TryParse(charidx + "", out idx))
            {
                return idx;
            }
            else
            {
                return -1;
            }
        }


        //private WorldObject getResult2(Word[] words)
        //{
        //    var templateRes = result.Split('^');
        //    WorldObject worldObject = null;
        //    WorldObject objective = null;
        //    RelationObject relation = null;


        //    foreach (var t in templateRes.Reverse())
        //    {
        //        if (t.Contains("0")) {
        //            worldObject = words[0].WorldObject;
        //        }
        //        else if (t.Contains("1"))
        //        {
        //            objective = words[1].WorldObject;
        //        }
        //        else if (t.Contains("Rel"))
        //        {
        //            {
        //                try
        //                {
        //                    Type rt = Type.GetType(t, true);
        //                    relation = (RelationObject)(Activator.CreateInstance(rt));
        //                }
        //                catch (Exception ex)
        //                {
        //                    throw new TemplateException(templateText);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            throw new TemplateException(templateText);
        //        }
        //    }

        //    relation.Objective = objective;
        //    worldObject.addRelation(relation);
        //    return worldObject;

        //}

        //private WorldObject getResult3(Word[] words)
        //{
        //    var templateRes = result.Split('^');
        //    WorldObject worldObject = null;
        //    WorldObject objective = null;
        //    RelationObject relation = null;

        //    foreach (var r in templateRes)
        //    {
        //        if (r.Contains("0"))
        //        {
        //            worldObject = words[0].WorldObject;
        //        }
        //        else if (r.Contains("1")){
        //            objective = words[1].WorldObject;
        //        }
        //        else if (r.Contains("1"))
        //        {
        //            objective = words[1].WorldObject;
        //        }
        //        else if (r.Contains("Rel")){
        //            {
        //                try
        //                {
        //                    Type rt = Type.GetType(r, true);
        //                    relation = (RelationObject)(Activator.CreateInstance(rt));
        //                    relation.Objective = objective;
        //                    worldObject.addRelation(relation);
        //                }
        //                catch (Exception ex)
        //                {
        //                    throw new TemplateException(templateText);
        //                }
        //            }
        //        } else {
        //            throw new TemplateException(templateText);
        //        }



        //    }

        //    return worldObject;

        //}

    }





}
