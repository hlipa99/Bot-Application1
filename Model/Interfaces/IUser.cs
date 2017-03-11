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
        DateTime? UserCreated { get; set; }
        DateTime? UserLastSession { get; set; }
  
    }
}
