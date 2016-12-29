using System;

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
    }
}

