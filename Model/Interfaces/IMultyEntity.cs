using System.Collections.Generic;

namespace Model.Models
{

        using System;
        using System.Collections.Generic;

    public interface IMultyEntity : IentityBase
    {

        string parts { get; set; }
        string singleValue { get; set; }
      
    }
}