using System.Collections.Generic;

namespace Model.Models
{

        using System;
        using System.Collections.Generic;

        public interface Ientity
        {
             int EntityID { get; set; }
             string EntityValue { get; set; }
             string EntityType { get; set; }
             string EntitySynonimus { get; set; }
        }
    

}