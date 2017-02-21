

namespace NLPtest.WorldObj
{
    internal class EntObject : WorldObject
    {

        public override IWorldObject Clone()
        {
            EntObject res = new EntObject();
            res.Copy(this);
            return res;
        }


        public EntObject()
        {
            
        }
    }
}