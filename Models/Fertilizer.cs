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
    
    public partial class Fertilizer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Fertilizer()
        {
            this.BuyFs = new HashSet<BuyF>();
        }
    
        public int FertilizerID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }
        public string Location { get; set; }
        public string Image { get; set; }
        public string Nametree { get; set; }
        public string Water { get; set; }
        public string Fertilization { get; set; }
        public string Applyingfertilizer { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BuyF> BuyFs { get; set; }
    }
}
