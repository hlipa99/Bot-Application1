using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public interface IUser
    {

        string UserID { get; set; }
        string UserName { get; set; }
        string UserGender { get; set; }
        string UserClass { get; set; }
        Nullable<System.DateTime> UserCreated { get; set; }
        Nullable<System.DateTime> UserLastSession { get; set; }
        string UserAddress { get; set; }
        int? UserTimesConnected { get; set; }
        String UserOverallTime { get; set; }

        int[][] PreviusParses { get; set; }
    }
}
