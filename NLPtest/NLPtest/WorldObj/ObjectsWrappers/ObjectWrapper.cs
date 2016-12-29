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
            this.objective = objective;
        }


        public override string ToString()
        {

            return GetType() + "(" +objective +")";
        }
    }
}
