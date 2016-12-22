using System;
using HebMorph;
using NLPtest.WorldObj;

namespace NLPtest
{
    internal class NounWord// : Word
    {
        private HebrewToken ht;

        public NounWord(string word) : base(word)
        {
        }

        public NounWord(HebrewToken ht ) : base(ht.Text)
        {
            this.ht = ht;

        }

      

        public object WorldObject { get; internal set; }

  
    }
}