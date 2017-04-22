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


        string language = "heb";
        private int[][] previusParses;

        public string Language
        {
            get
            {
                return language;
            }

            set
            {
                language = value;
            }
        }

        public int[][] PreviusParses {
            get
            {
                if (previusParses == null)
                    previusParses =  new int[100][]; //TODO fix number
                return previusParses;
            }
            set
            {
                previusParses = value;
            }
        }

        public DateTime? LastSeen { get ; set ; }
        public string TimeConnected { get ; set ; }

        public bool Equals(User other)
        {

            return this.UserID == other.UserID;
        }

    }

}
