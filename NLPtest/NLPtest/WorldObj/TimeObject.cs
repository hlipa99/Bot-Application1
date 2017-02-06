

namespace NLPtest.WorldObj
{
    internal class TimeObject : WorldObject
    {


        public TimeObject()
        {
          
        }

        public TimeObject(string word) : base(word)
        {
          
        }

        public override IWorldObject Clone()
        {
            TimeObject res = new TimeObject(Word);
            cloneBase(res);
            return res;
        }
    }
}