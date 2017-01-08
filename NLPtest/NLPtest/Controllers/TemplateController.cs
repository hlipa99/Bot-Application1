using NLPtest.HebWords;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLPtest.WorldObj;
using static NLPtest.Word.WordType;
using NLPtest.view;

namespace NLPtest.Controllers
{
    class TemplateController
    {
        HebTemplate[] templateList;
        private string[] templatsStrings;

       public TemplateController()
        {
            loadTemplates();
        }

        public void loadTemplates()
        {




            templateList = new HebTemplate[] {
    
                
                //one object template
                new HebTemplate(new WorldObject[] { new Word("",nounWord) },new NounObject("1"),1),
                new HebTemplate(new WorldObject[] { new Word("",verbWord) },new VerbObject("1"),1),
                new HebTemplate(new WorldObject[] { new Word("",personWord) },new PersonObject("1"),2),
                new HebTemplate(new WorldObject[] { new Word("",locationWord) },new LocationObject("1"),2),


                //two object template

                 new HebTemplate(new WorldObject[] { new NounObject(""), new Word("", adjectiveWord) },
                 relate(new NounObject("0"),new adjectiveRelObject(new AdjObject("1"))),-1),
                  new HebTemplate(new WorldObject[] {new Word("", negationWord) , new VerbObject("")},
                 negate(new VerbObject("1")),-1),



                //three object template
                new HebTemplate(new WorldObject[] { new NounObject(""), new VerbObject(""),new NounObject("") },
                 relate(new NounObject("0"),new VerbRelObject(relate(new VerbObject("1"), new VerbObjRelObject(new NounObject("2"))))),0),

                 new HebTemplate(new WorldObject[] { new PersonObject(""), new VerbObject(""),new NounObject("") },
                 relate(new PersonObject("0"),new VerbRelObject(relate(new VerbObject("1"), new VerbObjRelObject(new NounObject("2"))))),0),


            };









        
        }



        private WorldObject negate(VerbObject verbObject)
        {
            verbObject.Negat = true;
            return verbObject;
        }

        private WorldObject relate(WorldObject obj, RelationObject adjectiveRelObject)
        {
            obj.addRelation(adjectiveRelObject);
            return obj;
        }

      

        internal WorldObject checkObjects(WorldObject[] objects)
        {
            WorldObject res = null;

            foreach (var t in templateList)
            {
               if((res = t.tryMatch(objects)) != null)
                {
                    break;
                }
            }

            return res;
        }
    }
}
