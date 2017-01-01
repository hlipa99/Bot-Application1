using NLPtest.WorldObj;

namespace NLPtest
{
    internal class gufObject :WorldObject
    {
        private string amount;
        private string guf;
        private string time;
        private string word;

        public gufObject(string word)
        {
            this.word = word;
        }

        public gufObject(string word,string time,string amount,string guf)
        {
            this.time = time;
            this.guf = guf;
            this.amount = amount;
        }
    }
}