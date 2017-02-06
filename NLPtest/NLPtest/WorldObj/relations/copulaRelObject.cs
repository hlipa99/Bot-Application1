using NLPtest.WorldObj;

namespace NLPtest.view
{
    internal class copulaRelObject : RelationObject
    {
        private personObject guf;

        public copulaRelObject(WorldObject objective,personObject guf) : base( objective)
        {
            this.guf = guf;
        }

        public override IWorldObject Clone()
        {
            expansionRelObject res = new expansionRelObject(Objective);
            cloneBase(res);
            return res;
        }
    }
}
