

namespace NLPtest.WorldObj
{
    internal class LocationObject : NounObject
    {
   

        public LocationObject(string word) : base(word)
        {
     
        }

        public override IWorldObject Clone()
        {
            LocationObject res = new LocationObject(Word);
            cloneBase(res);
            return res;
        }
    }
}