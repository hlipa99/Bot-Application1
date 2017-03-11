
using System;

namespace NLP.WorldObj
{
 
    public class NounObject : WorldObject
    {

 
        public NounObject(string word) : base (word)
        {

        }

        public override IWorldObject Clone()
        {
            NounObject res = new NounObject(Word);
            res.Copy(this);
            return res;
        }
    }
}