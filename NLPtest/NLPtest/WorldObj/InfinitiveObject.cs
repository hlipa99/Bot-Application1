

namespace NLPtest.WorldObj
{
    internal class InfinitiveObject : WorldObject
    {
        public InfinitiveObject()
        {
    
        }

        public InfinitiveObject(string word) : base(word)
        {
          
        }

        public override IWorldObject Clone()
        {
            InfinitiveObject res = new InfinitiveObject(Word);
            res.cloneBase(this);
            return res;
        }

    }
}