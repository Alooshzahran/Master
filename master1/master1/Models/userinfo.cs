//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace master1.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class userinfo
    {
        public int Id { get; set; }
        public string Userid { get; set; }
        public string Username { get; set; }
        public Nullable<int> Phone { get; set; }
        public string Address { get; set; }
        public string Bio { get; set; }
        public string Email { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
    }
}