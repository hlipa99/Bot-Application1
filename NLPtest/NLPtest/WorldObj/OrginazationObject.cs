using NLPtest.WorldObj;

namespace NLPtest
{
    internal class OrginazationObject : NounObject
    {
       

        public OrginazationObject(string word) : base(word)
        {
        
        }
        public override IWorldObject Clone()
        {
            OrginazationObject res = new OrginazationObject(Word);
           res.cloneBase(this);
            return res;
        }


    }
}