//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuanLyCayXanh.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public int IDuser { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Phonenumber { get; set; }
        public string Address { get; set; }
        public int Role { get; set; }
    
        public virtual Roledetail Roledetail { get; set; }
    }
}