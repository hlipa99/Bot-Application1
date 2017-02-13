

namespace NLPtest.WorldObj
{
    internal class EntObject : WorldObject
    {

        public override IWorldObject Clone()
        {
            EntObject res = new EntObject();
           res.cloneBase(this);
            return res;
        }


        public EntObject()
        {
            
        }
    }
}