using System;
using HebMorph;
using NLPtest.WorldObj;

namespace NLPtest
{
    internal class VerbWord// : Word
    {
        private HebrewToken ht;

        public VerbWord(string word) : base(word)
        {
        }

        public VerbWord(HebrewToken ht)  : base(ht.Text)
        {
            this.ht = ht;
        }

       

        public object WorldObject { get; internal set; }

       
    }
}