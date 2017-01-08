using System;
using System.Collections.Generic;

namespace NLPtest.WorldObj
{
    public class RelationObject : WorldObject
    {
        WorldObject objective;

        public RelationObject(WorldObject objective)
        {
            this.Objective = objective;
        }

        public WorldObject Objective
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
            return "rel->(" + GetType().ToString() + ")";
        }

        internal void addObjective(WorldObject[] paramObjects)
        {
            Objective = paramObjects[0];
        }


        internal void Copy(RelationObject first)
        {
            base.Copy(first);
            objective = first.objective;
        }


        internal new void CopyFromTemplate(WorldObject[] objects)
        {
            var index = int.Parse(Word);
            Copy(objects[index]);
            objective.CopyFromTemplate(objects);
            foreach (var r in Relations)
            {
                r.CopyFromTemplate(objects);
            }
        }
    }
}

