using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLPtest.WorldObj
{


    public class ConceptObject : EntityObject
    {
        public override IWorldObject Clone()
        {
            ConceptObject res = new ConceptObject(Word);
            res.Copy(this);
            return res;
        }



        public ConceptObject(string concept) : base(concept)
        {

        }

    }
}
