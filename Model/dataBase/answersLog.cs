//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model.dataBase
{
    using System;
    using System.Collections.Generic;
    
    public partial class answersLog
    {
        public int id { get; set; }
        public string question { get; set; }
        public string userAnswer { get; set; }
        public string entities { get; set; }
        public string missingEntities { get; set; }
        public Nullable<System.DateTime> time { get; set; }
    }
}
