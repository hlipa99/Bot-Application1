using System;
using HebMorph;
using NLPtest.WorldObj;

namespace NLPtest
{
    internal class PrepWord //: Word
    {
        private HebrewToken ht;


        public PrepWord(string word) : base(word)
        {
        }

        public PrepWord(HebrewToken ht) : base(ht.Text)
        {
            this.ht = ht;
        }

        public PrepWord(string word, PrepositionObject prepositionObject) : this(word)
        {
            this.worldObject = prepositionObject;
        }

        public object WorldObject { get; internal set; }

     

       
    }
}