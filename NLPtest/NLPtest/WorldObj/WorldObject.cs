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
            this.word = word;
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

        public void addRelation(RelationObject type)
        {
            relations.Add(type);
        }


        public override string ToString()
        {
            var n = negat ? "!" : "";
                return word + "(" + GetType() + ")" + n ;
        }

  
    }




}
