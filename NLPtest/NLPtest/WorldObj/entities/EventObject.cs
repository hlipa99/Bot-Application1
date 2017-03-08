using NLPtest.WorldObj;

namespace NLPtest
{
    public class EventObject : EntityObject
    {

        public override IWorldObject Clone()
        {
            EventObject res = new EventObject( Word);
            res.Copy(this);
            return res;
        }
        public EventObject(string word) : base(word)
        {
           
        }
    }
}