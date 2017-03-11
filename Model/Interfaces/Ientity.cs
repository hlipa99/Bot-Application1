using System.Collections.Generic;

namespace Model.Models
{

        using System;
        using System.Collections.Generic;

        public interface Ientity
        {
             int entityID { get; set; }
             string entityValue { get; set; }
             string entityType { get; set; }
             string entitySynonimus { get; set; }

              bool Equals(Object obj);
        }
    

}