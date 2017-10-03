using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;


namespace WebApp.Areas.Admin.Models
{

    public partial class LookupObject
    {

        public int ID { get; set; }
        public string Code { get; set; }
        public string LookUp { get; set; }

    }

    public class MetaObject
    {
        //chứa mô tả giao diện
        public List<WebApp.Areas.Admin.Models.SysTableDetailView> MetaTable { get; set; }

        //chứa phân trang đối tượng
        public Paging Paging { get; set; }

        public Models.SysTableDetailView GetMetaByColumnName(string columnname)
        {
            return MetaTable.Where(m => m.ColumnName == columnname).SingleOrDefault();
        }

        public Dictionary<string, Models.SysTableDetailView> GetMetaTable()
        {
            return MetaTable.OrderBy(m => m.GridViewOrder).ToDictionary(m => m.ColumnName);
        }

        public void ResetSummary()
        {
            foreach (var item in MetaTable)
            {
                item.Summary = 0;
            }
        }
    }

    //public class AppLoginModel
    //{
    //    public int ServiceId { get; set; }
    //    public string UserName { get; set; }
    //    public string Password { get; set; }

    //}

    public class AppAjaxOption
    {
        public AppAjaxOption()
        {
            ajaxoption = new Dictionary<string, string>();
            ajaxoption.Add("ajaxloadingid", "ajaxloadingelementid");
        }
        public Dictionary<string, string> ajaxoption { get; set; }

        public string ajaxstring()
        {
            string ajaxstring = string.Format("data-ajax-update=#{0} ", ajaxoption["ajaxupdateid"]);
            ajaxstring += "data-ajax-mode=replace ";
            ajaxstring += "data-ajax-method=GET ";
            ajaxstring += string.Format("data-ajax-loading=#{0} ", ajaxoption["ajaxloadingid"]);
            ajaxstring += "data-ajax=true ";
            return ajaxstring;
        }
    }

    public class Paging
    {

        [DisplayName("Tổng số dòng")]
        public int TotalRows { get; set; }

        [DisplayName("số dòng trong 1 trang")]
        public int PageSize { get; set; }

        [DisplayName("Tổng số trang")]
        public int PageCount { get; set; }

        [DisplayName("Trang hiện hành")]
        public int PageCurrent { get; set; }

        [DisplayName("Số trang bỏ qua")]
        public int Skip { get; set; }

        [DisplayName("Chuỗi tìm kiếm hiện hành")]
        public string Search { get; set; }

        [DisplayName("Các trường tìm kiếm hiện hành")]
        public string FieldsSearch { get; set; }

        [DisplayName("Chuỗi sắp xếp hiện hành")]
        public string Sort { get; set; }

        [DisplayName("Có trang trước")]
        public bool HasPreviousPage { get; set; }

        [DisplayName("Có trang sau")]
        public bool HasNextPage { get; set; }

        //sẽ bỏ
        [DisplayName("Ẩn hộp thoại tìm kiếm")]
        public bool HideSearchBox { get; set; }

        //sẽ bỏ
        public void SetFieldsSearch(object Metadata, string FieldsList)
        {
            Dictionary<string, Dictionary<string, string>> _Metadata = (Dictionary<string, Dictionary<string, string>>)Metadata;
            string[] Fields = FieldsList.Split(',');
            string result = "";
            int arrlen = 0;
            foreach (string field in Fields)
            {
                result += _Metadata["DisplayName"][field];
                arrlen++;
                if (arrlen < Fields.Length)
                {
                    result += ", ";
                }
            }
            this.FieldsSearch = "Tìm kiếm: " + result;
        }

        public void SetPaging(int totalrows, int pagecurrent)
        {

            this.TotalRows = totalrows;


            #region PageCount

            if ((totalrows % this.PageSize == 0))
            {
                this.PageCount = totalrows / this.PageSize;
            }
            else
            {
                this.PageCount = (totalrows / this.PageSize) + 1;
            }
            #endregion

            if (pagecurrent >= 0)
            {
                this.PageCurrent = this.PageCount < pagecurrent ? this.PageCount : pagecurrent;
            }
            else
            {
                this.PageCurrent = 1;
            }

            this.HasPreviousPage = (this.PageCurrent - 1) > 0;
            if (this.PageCount > 1)
            {
                this.HasNextPage = (this.PageCurrent + 1) <= this.PageCount;
            }
            else
            {
                this.HasNextPage = false;
            }

            if (this.PageCurrent == 0)
            {
                this.PageCurrent = 1;

            }

            this.Skip = (this.PageCurrent - 1) * this.PageSize;

        }

        public Paging()
        {
            this.PageSize = 10;
        }

        public int GetCurrentRowBegin()
        {
            if (TotalRows <= 0) return 0;
            return 1 + (PageCurrent - 1) * PageSize;
        }
        public int GetCurrentRowEnd()
        {
            if (TotalRows <= 0) return 0;
            if (PageCurrent == PageCount) return TotalRows;

            return PageCurrent * PageSize;
        }
        public int GetPreviousPage()
        {
            if (PageCurrent > 0)
            {
                return PageCurrent - 1;
            }
            else
            {
                return 0;
            }
        }
        public int GetNextPage()
        {
            if (PageCurrent < PageCount)
            {
                return PageCurrent + 1;
            }
            else
            {
                return PageCount;
            }
        }
    }

    public class PagingAutocomplete
    {
        [DisplayName("Tổng số dòng")]
        public int TotalRows { get; set; }

        [DisplayName("số dòng trong 1 trang")]
        public int PageSize { get; set; }

        [DisplayName("Tổng số trang")]
        public int PageCount { get; set; }

        [DisplayName("Trang hiện hành")]
        public int PageCurrent { get; set; }

        [DisplayName("Số trang bỏ qua")]
        public int Skip { get; set; }

        [DisplayName("Có trang trước")]
        public bool HasPreviousPage { get; set; }

        [DisplayName("Có trang sau")]
        public bool HasNextPage { get; set; }


        public void SetPaging(int totalrows)
        {

            this.TotalRows = totalrows;


            #region PageCount

            if ((totalrows % this.PageSize == 0))
            {
                this.PageCount = totalrows / this.PageSize;
            }
            else
            {
                this.PageCount = (totalrows / this.PageSize) + 1;
            }
            #endregion

            if (this.PageCurrent >= 0)
            {
                this.PageCurrent = this.PageCount < this.PageCurrent ? this.PageCount : this.PageCurrent;
            }

            this.HasPreviousPage = (this.PageCurrent - 1) > 0;
            if (this.PageCount > 1)
            {
                this.HasNextPage = (this.PageCurrent + 1) <= this.PageCount;
            }
            else
            {
                this.HasNextPage = false;
            }

            if (this.PageCurrent == 0)
            {
                this.PageCurrent = 1;

            }

            this.Skip = (this.PageCurrent - 1) * this.PageSize;

        }

        public PagingAutocomplete(System.Web.HttpRequestBase request)
        {
            this.PageCurrent = request.Params["paging.pagecurrent"] == null ? -1 : int.Parse(request.Params["paging.pagecurrent"]);
            this.PageSize = request.Params["paging.pagesize"] == null ? 10 : int.Parse(request.Params["paging.pagesize"]);
        }

    }

    public class GiftCardType
    {
        public SelectList selectlist;
        public GiftCardType()
        {
            selectlist = new SelectList(
        new List<SelectListItem>
            {
                new SelectListItem { Selected = true, Text = "Ảo", Value = "0"},
                new SelectListItem { Selected = false, Text = "Vật lí", Value = "1"} 
            }, "Value", "Text");
        }
    }


    public class LeadSource
    {
        public SelectList selectlist;
        public LeadSource()
        {
            selectlist = new SelectList(
        new List<SelectListItem>
            {
                new SelectListItem { Selected = true, Text = "", Value = ""},
                new SelectListItem { Selected = false, Text = "Quan hệ", Value = "Quan hệ"},
                new SelectListItem { Selected = false, Text = "Khác", Value = "Khác"}
            }, "Value", "Text");
        }
    }

    public class ContractStatus
    {
        public SelectList selectlist;
        public ContractStatus()
        {
            selectlist = new SelectList(
        new List<SelectListItem>
            {
                new SelectListItem { Selected = false, Text = "Chưa bắt đầu", Value = "1"},
                new SelectListItem { Selected = true, Text = "Đang tiến hành", Value = "2"},
                new SelectListItem { Selected = false, Text = "Hoàn tất", Value = "0"},
                new SelectListItem { Selected = false, Text = "Hủy", Value = "-1"}
            }, "Value", "Text");
        }
    }

}