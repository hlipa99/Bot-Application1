

namespace NLPtest.WorldObj
{
    internal class LocationObject : EntityObject
    {
   

        public LocationObject(string word) : base(word)
        {
     
        }

        public override IWorldObject Clone()
        {
            LocationObject res = new LocationObject(Word);
            res.Copy(this);
            return res;
        }
    }
}