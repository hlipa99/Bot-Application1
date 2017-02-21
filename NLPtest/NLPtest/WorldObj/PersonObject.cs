using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLPtest.WorldObj
{


    public class PersonObject : EntityObject
    {
        public override IWorldObject Clone()
        {
            PersonObject res = new PersonObject(Word);
            res.Copy(this);
            return res;
        }


        private string name;

        public PersonObject(string pers) : base(pers)
        {

        }

        internal personObject.genderType Gender { get; set; }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }
    }
}
