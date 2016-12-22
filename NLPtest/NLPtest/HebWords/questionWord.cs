using NLPtest.WorldObj;
using System;

namespace NLPtest
{
    internal class questionWord// : Word
    {
        private string s;
        private QuestionObject q;

     

        internal enum Question { What, When, Why ,Whom, Where, HowMatch, How, IsIt };

        public questionWord(string s) : base(s)
        {
        }

        public questionWord(string s, Question q) : this(s)
        {
            this.q = new QuestionObject(q);
            this.s = s;
        }


    }
}