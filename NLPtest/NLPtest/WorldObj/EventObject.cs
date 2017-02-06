using NLPtest.WorldObj;

namespace NLPtest
{
    internal class EventObject : WorldObject
    {

        public override IWorldObject Clone()
        {
            EventObject res = new EventObject( Word);
            cloneBase(res);
            return res;
        }
        public EventObject(string word) : base(word)
        {
           
        }
    }
}