using NLPtest.WorldObj;

namespace NLPtest.view
{
    internal class adjectiveRelObject : RelationObject
    {
        public adjectiveRelObject( IWorldObject objective) : base(objective)
        {
        }



        public override IWorldObject Clone()
        {
            adjectiveRelObject res = new adjectiveRelObject(null);
            res.Copy(this);
            return res;
        }
    }
}