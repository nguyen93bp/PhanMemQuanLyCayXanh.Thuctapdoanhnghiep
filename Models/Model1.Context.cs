﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class QuanLyCayXanhEntities : DbContext
    {
        public QuanLyCayXanhEntities()
            : base("name=QuanLyCayXanhEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Buy> Buys { get; set; }
        public virtual DbSet<BuyF> BuyFs { get; set; }
        public virtual DbSet<Fertilizer> Fertilizers { get; set; }
        public virtual DbSet<Greenery> Greeneries { get; set; }
        public virtual DbSet<Roledetail> Roledetails { get; set; }
        public virtual DbSet<User> Users { get; set; }
    }
}