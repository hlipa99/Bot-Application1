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
    
    public partial class SubQuestion
    {
        public string questionID { get; set; }
        public string subQuestionID { get; set; }
        public string questionText { get; set; }
        public string answerText { get; set; }
        public string flags { get; set; }
    
        public virtual Question Question { get; set; }
    }
}