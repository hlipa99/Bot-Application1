using NLPtest.WorldObj;

namespace NLPtest
{
    public class OrganizationObject : EntityObject
    {
       

        public OrganizationObject(string word) : base(word)
        {
        
        }
        public override IWorldObject Clone()
        {
            OrganizationObject res = new OrganizationObject(Word);
            res.Copy(this);
            return res;
        }


    }
}