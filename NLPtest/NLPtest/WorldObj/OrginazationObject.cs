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
            cloneBase(res);
            return res;
        }


    }
}