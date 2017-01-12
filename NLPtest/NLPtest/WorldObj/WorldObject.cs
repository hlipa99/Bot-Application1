using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLPtest.WorldObj
{


    public class WorldObject
    {
       
        List<RelationObject> relations = new List<RelationObject>();
        string word;
        private bool negat;

        public WorldObject(){}

        public WorldObject(string word) 
        {
            this.Word = word;
        }

        public List<RelationObject> Relations
        {
            get
            {
                return relations;
            }

            set
            {
                relations = value;
            }
        }

        public bool Negat
        {
            get
            {
                return negat;
            }

            set
            {
                negat = value;
            }
        }

        public bool DefiniteArticle { get; internal set; }

        public string Word
        {
            get
            {
                return word;
            }

            set
            {
                word = value;
            }
        }

        public void addRelation(RelationObject relation)
        {if (relation != null)
            {
                relations.Add(relation);
            }
        }


        public override string ToString()
        {
            var n = negat ? "!" : "";
                return Word + "(" + GetType() + ")" + n ;
        }

        internal void Copy(WorldObject first)
        {
            word = first.word;
            negat = first.negat;
            DefiniteArticle = first.DefiniteArticle;
        }


        internal void CopyFromTemplate(WorldObject[] objects)
        {
            var index = int.Parse(word);
            Copy(objects[index]);
            foreach (var r in relations)
            {
                r.CopyFromTemplate(objects);
            }
        }

        internal int ObjectType()
        {
            return 0;
        }


    }




}
