using NLPtest.WorldObj;

namespace NLPtest.view
{
    internal class copulaRelObject : RelationObject
    {
        private gufObject guf;

        public copulaRelObject(WorldObject objective,gufObject guf) : base( objective)
        {
            this.guf = guf;
        }
    }
}
