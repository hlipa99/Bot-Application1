using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLPtest.HebWords
{
    class NERWord// : Word
    {
        Boolean second = false;
        public NERWord(string word) : base(word)
        {
        }

        public bool Second
        {
            get
            {
                return second;
            }

            set
            {
                second = value;
            }
        }
    }
}
