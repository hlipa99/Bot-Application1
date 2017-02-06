using NLPtest.WorldObj;

namespace NLPtest.view
{
    internal class expansionRelObject : RelationObject
    {
        public expansionRelObject(WorldObject objective) : base( objective)
        {
        }
        public override IWorldObject Clone()
        {
            expansionRelObject res = new expansionRelObject(Objective);
            cloneBase(res);
            return res;
        }

    }


    
}