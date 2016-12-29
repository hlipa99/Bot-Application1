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

        public void addRelation(RelationObject type)
        {
            relations.Add(type);
        }


        public override string ToString()
        {
            var res = "";
            res += GetType() + "(" + word + ") {" + Environment.NewLine + "\t";
                foreach (var r in relations)
                {
                  res += r + Environment.NewLine ;
                }
            return res + "}" ;
        }




    }




}
