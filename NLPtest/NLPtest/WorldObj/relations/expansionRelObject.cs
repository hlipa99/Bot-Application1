using NLPtest.WorldObj;

namespace NLPtest.view
{
    internal class expansionRelObject : RelationObject
    {
        public expansionRelObject(IWorldObject objective) : base( objective)
        {
        }
        public override IWorldObject Clone()
        {
            expansionRelObject res = new expansionRelObject(Objective);
           res.cloneBase(this);
            return res;
        }

    }


    
}