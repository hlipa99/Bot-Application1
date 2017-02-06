using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace NLPtest.WorldObj
{


    public class WorldObject : IWorldObject
    {
       
        List<RelationObject> relations = new List<RelationObject>();
        private string word;
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
            var n = negat ? "not" : "";
                return Word + "(" + GetType() + ")" + n ;
        }

        internal void Copy(WorldObject obj)
        {
            word = obj.word;
            negat = negat || obj.negat;
            DefiniteArticle = DefiniteArticle || obj.DefiniteArticle;
        }

        public int ObjectType()
        {
            return 0;
        }

        public virtual IWorldObject Clone()
        {
            WorldObject res = new WorldObject(word);
            cloneBase(res);
            return res;
        }

        internal void cloneBase(WorldObject res)
        {
            foreach (var r in relations)
            {
                res.Relations.Add((RelationObject)r.Clone());
            }
            res.negat = negat;
            res.DefiniteArticle = DefiniteArticle;
        }

         public virtual void CopyFromTemplate(ITemplate[] objects)
        {
            var index = int.Parse(Word);
            var obj = objects[index];

            if (obj is WorldObject)
            {
                Copy(obj as WorldObject);
                foreach (var r in relations)
                {
                    r.CopyFromTemplate(objects);
                }
            }
            else
            {
                var word = obj as WordObject;
                this.word = word.Text;
                this.DefiniteArticle = word.IsDefinite;
            }


        }
    }




}
