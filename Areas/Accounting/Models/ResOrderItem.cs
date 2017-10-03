
//------------------------------------------------------------------------------
// <auto-generated>
// gen by lotusviet.vn
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApp.Areas.Accounting.Models
{

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("ResOrderItem")]
    public partial class ResOrderItem
    {
        [Key]
        public int OrderItemId { get; set; }
        public decimal Amount { get; set; }
        public int ItemId { get; set; }
        public int OrderId { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal QuantityProcess { get; set; }
        public string ItemNote { get; set; }

        public Nullable<System.DateTime> CreateDate { get; set; }
    }
}