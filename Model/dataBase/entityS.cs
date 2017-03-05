using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bot_Application1.dataBase
{
    public partial class entity : Ientity
    {
        public int EntityID { get; set; }

        public string EntitySynonimus { get; set; }

        public string EntityType { get; set; }

        public string EntityValue{ get; set; }
    }
}