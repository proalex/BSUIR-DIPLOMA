//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ControllerServer.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class Reports
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Reports()
        {
            this.Points = new HashSet<Points>();
        }
    
        public int Id { get; set; }
        public System.DateTime Time { get; set; }
        public int VirtualUsers { get; set; }
        public int Timeout { get; set; }
        public int RequestDuration { get; set; }
        public int Duration { get; set; }
        public byte Strategy { get; set; }
        public int UserId { get; set; }
        public int UrlGroup { get; set; }
    
        public virtual Users User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Points> Points { get; set; }
    }
}