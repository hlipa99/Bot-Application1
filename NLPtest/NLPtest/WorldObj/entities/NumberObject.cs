using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLP.WorldObj
{


    public class NumberObject : EntityObject
    {
        int value;

        public int Value { get ; set ; }

        public override IWorldObject Clone()
        {
            NumberObject res = new NumberObject(Word);
            res.Copy(this);
            return res;
        }

        public NumberObject(string concept) : base(concept)
        {
            int intValue;
            int.TryParse(concept, out intValue);
            Value = intValue;

        }

    }
}
