using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public interface IUser
    {
      bool Equals(object other);
      int Id { get; set; }
      string UserID { get; set; }
      string UserName { get; set; }
      string Channel { get; set; }
      System.DateTime created { get; set; }
      string Message { get; set; }
      string UserGender { get; set; }
      string UserClass { get; set; }
        
    }
}
