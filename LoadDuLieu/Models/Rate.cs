//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LoadDuLieu.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Rate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<double> Buy { get; set; }
        public Nullable<double> Sell { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<System.TimeSpan> Time { get; set; }
    
        public virtual Symbol Symbol { get; set; }
    }
}
