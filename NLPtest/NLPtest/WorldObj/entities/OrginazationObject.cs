using NLPtest.WorldObj;

namespace NLPtest
{
    internal class OrginazationObject : EntityObject
    {
       

        public OrginazationObject(string word) : base(word)
        {
        
        }
        public override IWorldObject Clone()
        {
            OrginazationObject res = new OrginazationObject(Word);
            res.Copy(this);
            return res;
        }


    }
}