
//------------------------------------------------------------------------------
// <auto-generated>
// gen by lotusviet.vn
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApp.Areas.Accounting.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("AppItemExTable")]
    public partial class AppItemExTable
    {
        [Key, Column(Order = 0)]
        public int ItemID { get; set; }
        [Key, Column(Order = 1)]
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }
        public System.DateTime LastUpdatedDate { get; set; }
    }

}