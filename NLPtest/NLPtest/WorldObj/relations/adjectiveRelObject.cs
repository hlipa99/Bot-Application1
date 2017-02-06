using NLPtest.WorldObj;

namespace NLPtest.view
{
    internal class adjectiveRelObject : RelationObject
    {
        public adjectiveRelObject( WorldObject objective) : base(objective)
        {
        }



        public override IWorldObject Clone()
        {
            adjectiveRelObject res = new adjectiveRelObject(Objective);
            cloneBase(res);
            return res;
        }
    }
}