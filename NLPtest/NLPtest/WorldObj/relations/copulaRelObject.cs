using NLPtest.WorldObj;

namespace NLPtest.view
{
    internal class copulaRelObject : RelationObject
    {
    //    private personObject guf;

        public copulaRelObject(WorldObject objective) : base( objective)
        {
        //    this.guf = guf;
        }

        public override IWorldObject Clone()
        {
            expansionRelObject res = new expansionRelObject(Objective);
            res.Copy(this);
            return res;
        }
    }
}
