//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace VancouverSports
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductVisit
    {
        public string sessionID { get; set; }
        public int productID { get; set; }
        public Nullable<int> qtyOrdered { get; set; }
        public Nullable<System.DateTime> updated { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual Visit Visit { get; set; }
    }
}
