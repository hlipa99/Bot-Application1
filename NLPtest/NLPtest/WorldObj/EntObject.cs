

namespace NLPtest.WorldObj
{
    internal class EntObject : WorldObject
    {

        public override IWorldObject Clone()
        {
            EntObject res = new EntObject();
            cloneBase(res);
            return res;
        }


        public EntObject()
        {
            
        }
    }
}