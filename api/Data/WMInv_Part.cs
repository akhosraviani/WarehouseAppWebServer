//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace api.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class WMInv_Part
    {
        public WMInv_Part()
        {
            this.WMInv_Part1 = new HashSet<WMInv_Part>();
            this.WMInv_PartSerial = new HashSet<WMInv_PartSerial>();
        }
    
        public string Code { get; set; }
        public string Title { get; set; }
        public string Barcode { get; set; }
        public string CommercialTitle { get; set; }
        public string LatinTitle { get; set; }
        public string UnitOfMeasureCode { get; set; }
        public string PartTypeCode { get; set; }
        public string PartStatusCode { get; set; }
        public string SubstitutePartCode { get; set; }
        public Nullable<decimal> StandardCost { get; set; }
        public Nullable<bool> InventoryProperty { get; set; }
        public Nullable<bool> PurchasingProperty { get; set; }
        public Nullable<bool> EngineeringProperty { get; set; }
        public Nullable<bool> ManufacturingProperty { get; set; }
        public Nullable<bool> SalesProperty { get; set; }
        public Nullable<bool> QCProperty { get; set; }
        public string ConsumptionPlace { get; set; }
        public Nullable<decimal> UnitValue { get; set; }
        public Nullable<decimal> PackingBoxRatio { get; set; }
        public string CreatorCode { get; set; }
        public System.DateTime CreationDate { get; set; }
        public string Note { get; set; }
        public byte[] Picture { get; set; }
        public Nullable<System.Guid> Guid { get; set; }
    
        public virtual ICollection<WMInv_Part> WMInv_Part1 { get; set; }
        public virtual WMInv_Part WMInv_Part2 { get; set; }
        public virtual WMInv_PartInventoryProperty WMInv_PartInventoryProperty { get; set; }
        public virtual ICollection<WMInv_PartSerial> WMInv_PartSerial { get; set; }
    }
}
