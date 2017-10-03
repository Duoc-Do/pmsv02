
//------------------------------------------------------------------------------
// <auto-generated>
// gen by lotusviet.vn
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApp.Areas.Accounting.Models 
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("ResOrder")]
    public partial class ResOrder
    {
        public ResOrder()
        {
            this.ResOrderItems = new HashSet<ResOrderItem>();
        }

        public virtual ICollection<ResOrderItem> ResOrderItems { get; set; }

        [Key]
        public int OrderId { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDateTime { get; set; }
        public string Note { get; set; }
        public System.DateTime OrderDate { get; set; }
        public decimal OrderDiscount { get; set; }
        public decimal OrderDiscountPercentage { get; set; }
        public string OrderNumber { get; set; }
        public decimal OrderQuantity { get; set; }
        public decimal OrderQuantityProcess { get; set; }
        public int OrderStatusId { get; set; }
        public decimal OrderSubtotal { get; set; }
        public decimal OrderTotal { get; set; }
        public int PaymentId { get; set; }
        public Nullable<int> TableId { get; set; }

        public string CardNumber { get; set; }

        public Nullable<decimal> CustomerPay { get; set; }
        public Nullable<decimal> CustomerReturn { get; set; }
        public Nullable<decimal> OrderSurcharge { get; set; }

    }
}