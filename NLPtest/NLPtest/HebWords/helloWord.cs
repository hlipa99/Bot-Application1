using NLPtest.WorldObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLPtest.HebWords
{
    class helloWord// : Word
    {

        

            public helloWord(string word) 
            {
            }


        public helloWord(string word,WorldObject worldObj) : base(word)
        {
            this.worldObject = worldObject;
        }



    }
}
