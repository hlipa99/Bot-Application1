using NLPtest.WorldObj;

namespace NLPtest
{
    internal class EventObject : WorldObject
    {
        private string word;

        public EventObject(string word)
        {
            this.word = word;
        }
    }
}