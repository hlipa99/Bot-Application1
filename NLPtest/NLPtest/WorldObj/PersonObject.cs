using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLPtest.WorldObj
{


    public class PersonObject : NounObject
    {

        private string name;

        public PersonObject(string pers) : base(pers)
        {

        }

        internal gufObject.genderType Gender { get; set; }

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
