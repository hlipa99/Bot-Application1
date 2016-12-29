using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLPtest.WorldObj.ObjectsWrappers
{

    class ObjectWrapper:WorldObject
    {
        WorldObject objective;
        public ObjectWrapper(WorldObject objective)
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

            return GetType() +"";
        }
    }
}
