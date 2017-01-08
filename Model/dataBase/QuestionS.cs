using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.dataBase
{

    [Serializable]
    public partial class Question
    {

        public override bool Equals(object other)
        {
            if (other is Question)
            {
                var q = other as Question;
                return this.ID == q.ID;
            }
            else
            {
                return false;
            }
        }

            public override int GetHashCode()
            {
                   return this.ID.GetHashCode();
             }
        
    }
}
