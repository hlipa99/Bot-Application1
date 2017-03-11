

namespace NLP.WorldObj
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
            res.Copy(this);
            return res;
        }
    }
}