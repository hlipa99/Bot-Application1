using NLPtest.HebWords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLPtest.WorldObj;
using static NLPtest.HebWords.WordObject.WordType;
using NLPtest.view;

namespace NLPtest.Controllers
{
    class TemplateController
    {
        public int TEMPLATE_RANGE = 10;

        HebTemplate[] templateList;
        private string[] templatsStrings;
        public string log = "";

       public TemplateController()
        {
            loadTemplates();
        }

        public void loadTemplates()
        {




            templateList = new HebTemplate[] {
    
                
                //one object template
                //new HebTemplate(new ITemplate[] { new WordObject("",nounWord) },new NounObject("0"),1),
                //new HebTemplate(new ITemplate[] { new WordObject("",verbWord) },new VerbObject("0"),1),
             //   new HebTemplate(new ITemplate[] { new WordObject("",gufWord) },new personObject("0"),1),
                new HebTemplate(new ITemplate[] { new WordObject("",locationWord) },new LocationObject("0"),1),
                new HebTemplate(new ITemplate[] { new WordObject("",personWord) },new PersonObject("0"),1),
               // new HebTemplate(new ITemplate[] { new WordObject("",adjectiveWord) },new AdjObject("0"),1),
               // new HebTemplate(new ITemplate[] { new WordObject("",participleWord) },new ParticipleObject("0"),1),
                new HebTemplate(new ITemplate[] { new WordObject("",conceptWord) },new ConceptObject("0"),1),
                new HebTemplate(new ITemplate[] { new WordObject("",orginazationWord) },new OrginazationObject("0"),1),
                new HebTemplate(new ITemplate[] { new WordObject("",eventWord) },new EventObject("0"),1),


                
                //two object template

                 //new HebTemplate(new ITemplate[] { new AdjObject(""), new NounObject("") },
                 //relate(new WorldObject("1"),new adjectiveRelObject(new AdjObject("0"))),0),

                 //new HebTemplate(new ITemplate[] { new NounObject(""),new AdjObject(""),  },
                 //relate(new WorldObject("0"),new adjectiveRelObject(new AdjObject("1"))),0),

                 ////אהב לאכול
                 //new HebTemplate(new ITemplate[] { new VerbObject(""),new VerbObject("")},
                 //relate(new VerbObject("0"),new VerbRelObject(new AdjObject("1"))),10),

                 ////לאכול חצילים
                 //    new HebTemplate(new ITemplate[] { new VerbObject(""),new WorldObject("")},
                 //relate(new VerbObject("1"),new VerbRelObject(new WorldObject("0"))),10),

                 //new HebTemplate(new ITemplate[] { new WorldObject(""),new VerbObject("")},
                 //relate(new WorldObject("0"),new VerbRelObject(new VerbObject("1"))),10),

                 //new HebTemplate(new ITemplate[] { new WorldObject(""), new WordObject("", adjectiveWord) },
                 //relate(new WorldObject("0"),new adjectiveRelObject(new AdjObject("1"))),2),

                 //לא עושה
                new HebTemplate(new ITemplate[] {new WordObject("", negationWord) , new WorldObject("")},
                 negate(new VerbObject("1")),2),

               ////לא רחוק
               //      new HebTemplate(new ITemplate[] {new WordObject("", negationWord) , new AdjObject("")},
               //  negate(new AdjObject("1")),2),
                  

               //  //אוהב לאכול
               //  new HebTemplate(new ITemplate[] { new ParticipleObject(""),new VerbObject("") },
               //  relate(new VerbObject("0"),new VerbRelObject(new VerbObject("1"))),4),


               // //three object template

               // //גרגמל גר ביער
               // new HebTemplate(new ITemplate[] { new NounObject(""), new VerbObject(""),new NounObject("") },
               //  relate(new NounObject("0"),new VerbRelObject(relate(new VerbObject("1"), new VerbObjRelObject(new NounObject("2"))))),8),

               //     new HebTemplate(new ITemplate[] { new NounObject(""), new WordObject("",copulaWord),new AdjObject("") },
               //  relate(new NounObject("0"),new copulaRelObject(new AdjObject("2"))),2),

              

                // new HebTemplate(new WorldObject[] { new AdjObject(""), new VerbObject(""),new NounObject("") },
                // relate(new NounObject("2"),(RelationObject) relate(new VerbRelObject(new VerbObject("1")),new adjectiveRelObject(new AdjObject("0")))),2),

                // new HebTemplate(new WorldObject[] {new NounObject(""), new AdjObject(""),new VerbObject("") },
                // relate(new NounObject("0"),(RelationObject) relate(new VerbRelObject(new VerbObject("2")),new adjectiveRelObject(new AdjObject("1")))),2),

                // //התגורר רחוק מהיער
                // new HebTemplate(new WorldObject[] {new VerbObject("") ,new AdjObject(""),new NounObject("") },
                // relate(new NounObject("2"),(RelationObject) relate(new VerbRelObject(new VerbObject("0")),new adjectiveRelObject(new AdjObject("1")))),1),


                //   new HebTemplate(new WorldObject[] {new NounObject(""), new VerbObject(""),new AdjObject(""), },
                // relate(new NounObject("0"),(RelationObject) relate(new VerbRelObject(new VerbObject("1")),new adjectiveRelObject(new AdjObject("2")))),2),

                //     new HebTemplate(new WorldObject[] {new AdjObject(""), new NounObject(""), new VerbObject("") },
                // relate(new NounObject("1"),(RelationObject) relate(new VerbRelObject(new VerbObject("2")),new adjectiveRelObject(new AdjObject("0")))),2),

            };
        }



        private WorldObject negate(WorldObject verbObject)
        {
            verbObject.Negat = true;
            return verbObject;
        }

        private WorldObject relate(WorldObject obj, RelationObject adjectiveRelObject)
        {
            obj.addRelation(adjectiveRelObject);
            return obj;
        }

      

        internal ITemplate checkObjects(ITemplate[] objects,int priority)
        {
            ITemplate res = null;

            foreach (var t in templateList.Where(x=> x.Count == objects.Count() && x.Priority == priority))
            {
               if((res = t.tryMatch(objects)) != null)
                {
                 
                    log += priority + ":"; //debuging
                    foreach(var o in objects)
                    {
                        log += o + ",";
                    }
                    log += "=>" + res + Environment.NewLine;

                    break;
                }
            }

            return res;
        }
    }
}
