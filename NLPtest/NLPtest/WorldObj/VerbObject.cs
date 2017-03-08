

namespace NLPtest.WorldObj
{
    public class VerbObject : WorldObject
    {
        private string type;
        public VerbObject()
        {
          
        }

        public VerbObject(string v) : base(v)
        {
        
        }

        public override IWorldObject Clone()
        {
            VerbObject res = new VerbObject(Word);
            res.Copy(this);
            return res;
        }
    }
}