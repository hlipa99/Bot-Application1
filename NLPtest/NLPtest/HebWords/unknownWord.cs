using System;
using HebMorph;
using NLPtest.WorldObj;

namespace NLPtest
{
    internal class unknownWord : Word
    {
        private HebrewToken ht;

        public unknownWord(string word)// : base(word)
        {
        }

        public unknownWord(HebrewToken ht) : base(ht.Text)
        {
            this.ht = ht;
        }

       

      
       
    }
}