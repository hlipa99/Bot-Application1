using System;
using System.Collections.Generic;

namespace NLP.WorldObj
{
    public class RelationObject : WorldObject
    {
        IWorldObject objective;


        public RelationObject(IWorldObject objective)
        {
            this.Objective = objective;
        }

        public IWorldObject Objective
        {
            get
            {
                return objective;
            }

            set
            {
                objective = value;
            }
        }

      
        public override string ToString()
        {
            return "rel->(" + GetType().ToString() + ")[" + objective + "]";
        }

        internal void addObjective(WorldObject[] paramObjects)
        {
            Objective = paramObjects[0];
        }


        internal void Copy(RelationObject first)
        {
            base.Copy(first);
            objective = first.objective.Clone();
        }


        public override void CopyFromTemplate(ITemplate[] objects)
        {

            objective.CopyFromTemplate(objects);
            foreach (var r in Relations)
            {
                r.CopyFromTemplate(objects);
            }
        }

        public override IWorldObject Clone()
        {
            RelationObject res = new RelationObject(null);
            res.Copy(this);
            return res;
        }
        


    }
}

