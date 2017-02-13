

namespace NLPtest.WorldObj
{
    internal class ParticipleObject : VerbObject
    {


        public ParticipleObject()
        {
    
        }

        public ParticipleObject(string word) : base(word)
        {
          
        }

        public override IWorldObject Clone()
        {
            ParticipleObject res = new ParticipleObject(Word);
            res.cloneBase(this);
            return res;
        }

    }
}