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
    
    public partial class Profiles
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int UrlGroup { get; set; }
        public int VirtualUsers { get; set; }
        public int Timeout { get; set; }
        public int RequestDuration { get; set; }
        public int Duration { get; set; }
        public byte Strategy { get; set; }
        public string Name { get; set; }
    
        public virtual Users User { get; set; }
    }
}