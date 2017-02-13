﻿using NLPtest.WorldObj;

namespace NLPtest.view
{

   
    internal class BotObject : PersonObject
    {
        public BotObject(string word) : base(word)
        {
        }

        public override IWorldObject Clone()
        {
            BotObject res = new BotObject(Word);
           res.cloneBase(this);
            return res;
        }



    }
}