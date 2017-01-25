using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.dataBase
{

    [Serializable]
    public partial class User : IUser
    {

        public bool Equals(User other)
        {

            return this.UserID == other.UserID;
        }

    }

}
