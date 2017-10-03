using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace WebApp.Services.Helpers
{

    public static class HtmlHelpers
    {




        public static MvcHtmlString bsLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return bsLabelFor(html, expression, null);
        }

        public static MvcHtmlString bsLabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {

            var htmllabel = System.Web.Mvc.Html.LabelExtensions.LabelFor(html, expression, htmlAttributes);
            return htmllabel;

        }


        public static MvcHtmlString bsDropDownListFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectlist)
        {
            TagBuilder div = new TagBuilder("div");
            #region tạo label for
            MvcHtmlString strlabel = bsLabelFor(html, expression);
            #endregion
            div.AddCssClass("form-group");
            MvcHtmlString dropdown = System.Web.Mvc.Html.SelectExtensions.DropDownListFor(html, expression, selectlist, new { @class = "form-control" });

            div.InnerHtml = strlabel.ToString();
            div.InnerHtml += dropdown.ToString();
            return MvcHtmlString.Create(div.ToString());
        }

        public static MvcHtmlString bsEditorFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return bsEditorFor(html, expression, null);
        }

        public static MvcHtmlString bsEditorFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {

            #region tạo label for
            MvcHtmlString strlabel = bsLabelFor(html, expression);
            #endregion

            var builder = new TagBuilder("input");

            //((List<SenViet.Models.SysTableDetail>)html.ViewData[metadata.ContainerType.Name]).Where(m => m.ColumnName == metadata.PropertyName).ToArray();

            //var AllowDBNull = arrMeta.AllowDBNull == false ? " (*):" : ":";
            //var IsValid = arrMeta.IsValid;

            //var DisplayName = arrMeta.Des + AllowDBNull;
            //var ReadOnly = arrMeta.ReadOnly;

            var fullfieldname = ExpressionHelper.GetExpressionText(expression); // trả về tên đầy đủ field bao gồm Prefix ví dụ AppTourView.TourGroupID

            //Set lookup

            // Kết thúc set lookup

            //builder.MergeAttribute("fieldName", metadata.PropertyName, true);
            builder.AddCssClass("text-box single-line");
            builder.MergeAttribute("type", "text");
            builder.MergeAttribute("name", fullfieldname, true);
            builder.AddCssClass("form-control");

            builder.GenerateId(fullfieldname);
            var attemptedValue = System.Web.Mvc.Html.ValueExtensions.ValueFor(html, expression);

            ModelState modelState = null;
            html.ViewData.ModelState.TryGetValue(fullfieldname, out modelState);

            builder.MergeAttribute("value", attemptedValue.ToString(), true);

            if (htmlAttributes != null)
            {
                builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            }


            // If there are any errors for a named field, we add the css attribute.
            string errormessage = "";
            if (modelState != null)
            {
                if (modelState.Errors.Count > 0)
                {
                    builder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
                    errormessage = string.Format("<span class='label label-warning'>{0}</span>", modelState.Errors[0].ErrorMessage);
                }
            }

            // Create tag builder  
            var builderdiv = new TagBuilder("div");
            builderdiv.AddCssClass("form-group");
            builderdiv.InnerHtml = strlabel.ToString();
            #region nếu có autocomplete thì cho phép nút thêm
                builderdiv.InnerHtml += builder.ToString();
            #endregion

            builderdiv.InnerHtml += errormessage;
            if (!string.IsNullOrEmpty(errormessage))
            {
                builderdiv.AddCssClass("form-group has-error");
            }
            return MvcHtmlString.Create(builderdiv.ToString());
        }

        public static MvcHtmlString bsTextAreaFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            return bsTextAreaFor(html, expression, null);
        }

        public static MvcHtmlString bsTextAreaFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {

            #region tạo label for
            MvcHtmlString strlabel = bsLabelFor(html, expression);
            #endregion

            var builder = new TagBuilder("textarea");

            var fullfieldname = ExpressionHelper.GetExpressionText(expression); // trả về tên đầy đủ field bao gồm Prefix ví dụ AppTourView.TourGroupID

            //Set lookup

            // Kết thúc set lookup

            //builder.MergeAttribute("fieldName", metadata.PropertyName, true);
            //builder.AddCssClass("text-box single-line");
            //builder.MergeAttribute("type", "text");
            builder.MergeAttribute("name", fullfieldname, true);
            builder.AddCssClass("form-control");



            builder.GenerateId(fullfieldname);
            var attemptedValue = System.Web.Mvc.Html.ValueExtensions.ValueFor(html, expression);

            ModelState modelState = null;
            html.ViewData.ModelState.TryGetValue(fullfieldname, out modelState);

            //builder.MergeAttribute("value", attemptedValue, true); // do textarea không có value
            builder.SetInnerText(attemptedValue.ToString());


            if (htmlAttributes != null)
            {
                builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            }


            // If there are any errors for a named field, we add the css attribute.
            string errormessage = "";
            if (modelState != null)
            {
                if (modelState.Errors.Count > 0)
                {
                    builder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
                    errormessage = string.Format("<span class='label label-warning'>{0}</span>", modelState.Errors[0].ErrorMessage);
                }
            }

            // Create tag builder  
            var builderdiv = new TagBuilder("div");
            builderdiv.AddCssClass("form-group");
            builderdiv.InnerHtml = strlabel.ToString();
            builderdiv.InnerHtml += builder.ToString();
            builderdiv.InnerHtml += errormessage;
            if (!string.IsNullOrEmpty(errormessage))
            {
                builderdiv.AddCssClass("form-group has-error");
            }
         



            return MvcHtmlString.Create(builderdiv.ToString());
        }

        public static MvcHtmlString svGridList(this HtmlHelper html, IEnumerable<dynamic> source, string keyfield)
        {
            if (source == null || source.Count() == 0)
            {
                return new MvcHtmlString(string.Empty);
            }
            string tablename = "";
            foreach (object item in source)
            {
                tablename = item.GetType().Name;
                break;
            }
            return svGridList(html, source, keyfield, tablename);

        }

        //lưới liệt kê danh sách có phân trang
        public static MvcHtmlString svGridList(this HtmlHelper html, IEnumerable<dynamic> source, string keyfield, string tablename)
        {

            //string tablename = "";
            //foreach (object item in source)
            //{
            //    tablename = item.GetType().Name;
            //    break;
            //}

            var metaobject = Services.GlobalMeta.GetMetaObject(tablename);
            var columns = metaobject.GetMetaTable();

            TagBuilder divcontainer = new TagBuilder("div");
            divcontainer.AddCssClass("sv-grid-container");
            #region bảng phân trang
            TagBuilder tablepaging = new TagBuilder("table");
            tablepaging.AddCssClass("sv-grid-paging");
            TagBuilder tablepagingtr = new TagBuilder("tr");
            TagBuilder tablepagingtd = new TagBuilder("td");
            MvcHtmlString divpaging = System.Web.Mvc.Html.PartialExtensions.Partial(html, "_PagingPartial", metaobject.Paging);
            tablepagingtd.InnerHtml = divpaging.ToString();
            tablepagingtr.InnerHtml = tablepagingtd.ToString();
            tablepaging.InnerHtml = tablepagingtr.ToString();

            divcontainer.InnerHtml = tablepaging.ToString();

            #endregion

            #region border nội dung
            TagBuilder divbordertable = new TagBuilder("div");
            divbordertable.AddCssClass("sv-border-table");
            #region bảng nội dung
            TagBuilder table = new TagBuilder("table");

            table.MergeAttribute("tablename", tablename);
            table.AddCssClass("sv-gv-table");


            table.MergeAttribute("data-senviet-keys", keyfield);

            #region tạo header và filter

            string tagheader = "";
            string tagfilter = "";
            foreach (var item in columns)
            {
                tagheader += svHeaderColFor(html, item, tablename);
                tagfilter += svFilterColFor(html, item, tablename);
            }
            TagBuilder tableheader = new TagBuilder("tr");
            TagBuilder tablefilter = new TagBuilder("tr");
            tableheader.InnerHtml = tagheader;
            tablefilter.InnerHtml = tagfilter;

            table.InnerHtml = tableheader.ToString();
            table.InnerHtml += tablefilter.ToString();

            #endregion

            #region tạo dòng nội dung
            if (source != null && source.Count() > 0)
            {

                foreach (object item in source)
                {
                    if (item != null)
                    {

                        var arrkey = keyfield.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToArray();
                        string strpara = "";

                        foreach (var keyname in arrkey)
                        {
                            var value = item.GetType().GetProperty(keyname).GetValue(item, null) ?? String.Empty;
                            strpara += string.Format("&{0}={1}", keyname, value);
                        }

                        var tagcontent = new TagBuilder("tr");
                        tagcontent.AddCssClass("sv-gv-datarow");
                        tagcontent.MergeAttribute("sv-gv-datarow-para", strpara.ToString());
                        tagcontent.InnerHtml = svDataRow(html, item, columns).ToString();
                        table.InnerHtml += tagcontent.ToString();


                        //var value = item.GetType().GetProperty(keyfield).GetValue(item, null) ?? String.Empty;

                        //var tagcontent = new TagBuilder("tr");
                        //tagcontent.AddCssClass("sv-gv-datarow");
                        //tagcontent.MergeAttribute("sv-gv-datarow-para", value.ToString());
                        //tagcontent.InnerHtml = svDataRow(html, item, columns).ToString();
                        //table.InnerHtml += tagcontent.ToString();
                    }
                }
            }

            #endregion

            #endregion
            divbordertable.InnerHtml = table.ToString();
            divcontainer.InnerHtml += divbordertable.ToString();
            #endregion

            return new MvcHtmlString(divcontainer.ToString());
        }

        public static MvcHtmlString svHeaderColFor(this HtmlHelper htmlHelper, Models.SysTableDetailView item, string metaname, string des = "")
        {
            KeyValuePair<string, Models.SysTableDetailView> _item = new KeyValuePair<string, Models.SysTableDetailView>("A", item);

            return svHeaderColFor(htmlHelper, _item, metaname, des);
        }

        public static MvcHtmlString svHeaderColFor(this HtmlHelper htmlHelper, KeyValuePair<string, Models.SysTableDetailView> item, string metaname, string des = "")
        {
            TagBuilder tagheadercol = new TagBuilder("td");
            tagheadercol.AddCssClass("sv-gv-header");
            tagheadercol.MergeAttribute("id", string.Format("{0}.sort.{1}", metaname, item.Value.ColumnName));

            //thiết lập độ rộng của cột
            tagheadercol.MergeAttribute("style", string.Format("width:{0}px;", item.Value.GridViewWidth + 30));
            //kết thúc thiết lập độ rộng của cột

            TagBuilder table = new TagBuilder("table");
            table.MergeAttribute("cellspacing", "0");
            table.MergeAttribute("cellpadding", "0");
            table.MergeAttribute("style", "width:100%;border-collapse:collapse;");
            TagBuilder table_tr = new TagBuilder("tr");
            TagBuilder table_td = new TagBuilder("td");
            table_td.MergeAttribute("style", "width:100%;");

            table_td.SetInnerText(item.Value.Des);
            if (!string.IsNullOrWhiteSpace(des))
            {
                table_td.SetInnerText(des);
            }

            table_tr.InnerHtml = table_td.ToString();



            if (!string.IsNullOrWhiteSpace(item.Value.OrderExpression))
            {
                tagheadercol.MergeAttribute("OrderExpression", (item.Value.OrderExpression.Contains("asc") ? "asc" : "desc"));
                TagBuilder table_tdorder = new TagBuilder("td");
                table_tdorder.MergeAttribute("style", "padding-left: 5px;");
                table_tdorder.SetInnerText(item.Value.OrderExpression);

                TagBuilder imgsort = new TagBuilder("div");
                imgsort.AddCssClass(string.Format("sv-gv-header-{0}", (item.Value.OrderExpression.Contains("asc") ? "asc" : "desc")));

                table_tdorder.InnerHtml = imgsort.ToString();

                table_tr.InnerHtml += table_tdorder.ToString();
            }


            table.InnerHtml = table_tr.ToString();

            tagheadercol.InnerHtml = table.ToString();
            if (item.Value.GridViewShow != true)
            {
                tagheadercol.MergeAttribute("style", "display: none;", true);
            }

            return MvcHtmlString.Create(tagheadercol.ToString());
        }

        public static MvcHtmlString svFilterColFor(this HtmlHelper htmlHelper, KeyValuePair<string, Models.SysTableDetailView> item, string metaname)
        {

            string datatype = Services.ExConvert.Sqltype2Systemtype(item.Value.DATA_TYPE);

            TagBuilder filtercol = new TagBuilder("td");
            filtercol.AddCssClass("sv-gv-filter");
            filtercol.MergeAttribute("datatype", datatype);

            if ("numeric.datetime".Contains(datatype))
            {

                char[] delimiterChars = { ';' };
                string[] arrexpression = item.Value.FilterExpression == null ? null : item.Value.FilterExpression.Split(delimiterChars);

                for (int i = 0; i < 3; i++)
                {

                    TagBuilder div = new TagBuilder("div");

                    div.AddCssClass("sv-gv-filter-box");

                    TagBuilder span = new TagBuilder("span");
                    span.SetInnerText(i == 0 ? "=" : i == 1 ? ">" : "<");
                    var col = new TagBuilder("input");
                    string name = "";
                    switch (i)
                    {
                        case 0:
                            name = string.Format("{0}.filter.{1}.eq", metaname, item.Value.ColumnName);
                            break;
                        case 1:
                            name = string.Format("{0}.filter.{1}.gt", metaname, item.Value.ColumnName);
                            break;
                        case 2:
                            name = string.Format("{0}.filter.{1}.lt", metaname, item.Value.ColumnName);
                            break;
                        default:
                            break;
                    }

                    //
                    if (datatype == "numeric")
                    {
                        col.MergeAttribute("style", "text-align:right;");
                        col.MergeAttribute("decimaldigits", Services.ExConvert.SetDecimalDigits(item.Value.FormatValue, item.Value.CultureInfo).ToString());
                    }
                    col.AddCssClass(GetClassNameByType(datatype, item.Value.FormatValue));
                    string attemptedValue = Services.ExConvert.Data2String(arrexpression == null ? "" : arrexpression[i] == null ? "" : arrexpression[i], datatype, item.Value.FormatValue, item.Value.CultureInfo);
                    //
                    col.MergeAttribute("name", name);
                    col.MergeAttribute("type", "text");
                    col.MergeAttribute("value", attemptedValue);
                    col.MergeAttribute("fieldname", name, false);

                    div.InnerHtml = span.ToString();
                    div.InnerHtml += col.ToString();

                    filtercol.InnerHtml += div.ToString();
                }

                if (item.Value.GridViewShow != true)
                {
                    filtercol.MergeAttribute("style", "display: none;", false);
                }

            }
            else
            {
                var col = new TagBuilder("input");
                string name = string.Format("{0}.filter.{1}", metaname, item.Value.ColumnName);
                col.MergeAttribute("name", name);

                col.MergeAttribute("type", "text");
                col.MergeAttribute("value", item.Value.FilterExpression);

                col.MergeAttribute("fieldname", name, false);
                if (item.Value.GridViewShow != true)
                {
                    filtercol.MergeAttribute("style", "display: none;", false);
                }
                TagBuilder div = new TagBuilder("div");
                div.InnerHtml = col.ToString();
                filtercol.InnerHtml += div.ToString();
            }
            return MvcHtmlString.Create(filtercol.ToString());

        }

        public static MvcHtmlString svDataRow<TModel>(this HtmlHelper htmlHelper, TModel row, Dictionary<string, Models.SysTableDetailView> columns)
        {
            string datarow = "";

            foreach (var column in columns)
            {
                var value = row.GetType().GetProperty(column.Value.ColumnName).GetValue(row, null) ?? String.Empty;
                var td = new TagBuilder("td");
                td.AddCssClass("sv-gv");
                td.MergeAttribute("sv-fieldname", column.Value.ColumnName, false);

                string datatype = Services.ExConvert.Sqltype2Systemtype(column.Value.DATA_TYPE);
                if (datatype == "boolean")
                {
                    var col = new TagBuilder("input");
                    col.MergeAttribute("type", "checkbox", true);
                    col.MergeAttribute("disabled", "disabled", true);

                    col.AddCssClass("check-box");
                    if (value.ToString() != "")
                    {
                        if ((bool)value)
                        {
                            col.MergeAttribute("checked", "checked");
                        }
                    }
                    td.InnerHtml = col.ToString();
                }
                else
                {
                    if (datatype == "numeric")
                    {
                        td.MergeAttribute("align", "right", false);
                    }
                    string _value = string.Format("<span title=\"{0}\">{0}</span>", Services.ExConvert.Data2String(value, datatype, column.Value.FormatValue, column.Value.CultureInfo));
                    td.InnerHtml = _value;
                }



                if (column.Value.GridViewShow != true)
                {
                    td.MergeAttribute("style", "display: none;", false);
                }

                datarow += td;
            } //kết thúc quét cột


            return MvcHtmlString.Create(datarow);
        }

        private static string GetClassNameByType(string datatype, string formatvalue)
        {
            switch (datatype)
            {
                case "System.Nullable`1[System.Boolean]":
                    return "boolean";
                case "System.Boolean":
                    return "boolean";
                case "System.Nullable`1[System.DateTime]":
                    if (formatvalue == "g")
                    {
                        return "datetime-g";
                    }
                    return "datetime";
                case "System.DateTime":
                    if (formatvalue == "g")
                    {
                        return "datetime-g";
                    }
                    return "datetime";
                case "System.Nullable`1[System.Decimal]":
                    return "numeric";
                case "System.Nullable`1[System.Int32]":
                    return "numeric";
                case "numeric":
                    return "numeric";
                case "datetime":
                    if (formatvalue == "g")
                    {
                        return "datetime-g";
                    }
                    return "datetime";
                default: // nếu là string
                    return "";
            }

        }

    }

    public static class Uti
    {
        public static string GetClassNameByType(string datatype, string formatvalue)
        {
            switch (datatype)
            {
                case "System.Nullable`1[System.Boolean]":
                    return "boolean";
                case "System.Boolean":
                    return "boolean";
                case "System.Nullable`1[System.DateTime]":
                    if (formatvalue == "g")
                    {
                        return "datetime-g";
                    }
                    return "datetime";
                case "System.DateTime":
                    if (formatvalue == "g")
                    {
                        return "datetime-g";
                    }
                    return "datetime";
                case "System.Nullable`1[System.Decimal]":
                    return "numeric";
                case "System.Nullable`1[System.Int32]":
                    return "numeric";
                case "numeric":
                    return "numeric";
                case "datetime":
                    if (formatvalue == "g")
                    {
                        return "datetime-g";
                    }
                    return "datetime";
                default: // nếu là string
                    return "";
            }

        }

        public static class Date
        {
            public static void SetDateRange(HttpRequestBase request, out  DateTime DateFrom, out DateTime DateTo)
            {

                #region xử lý điều kiện ngày
                try
                {

                    var culture = ((System.Globalization.CultureInfo)Services.GlobalVariant.GetCultureInfo()["CICC"]).DateTimeFormat;
                    DateFrom = DateTime.Now.Date; // Default min giờ.
                    if (!string.IsNullOrEmpty(request.Params["DateFrom"])) DateFrom = DateTime.Parse(request.Params["DateFrom"], culture);

                    DateTo = DateTime.Now.Date;
                    if (!string.IsNullOrEmpty(request.Params["DateTo"])) DateTo = DateTime.Parse(request.Params["DateTo"], culture);

                    DateTo = DateTo.Date.AddDays(1).AddTicks(-1); // Set Max giờ.

                    //DateFrom = DateTime.Parse(request.Params["DateFrom"] ?? DateTime.Now.Date.ToShortDateString()); // Default min giờ.
                    //DateTo = DateTime.Parse(request.Params["DateTo"] ?? DateTime.Now.Date.ToShortDateString());
                    //DateTo = DateTo.Date.AddDays(1).AddTicks(-1); // Set Max giờ.


                    //controller.ViewBag.DateFrom = DateFrom.ToShortDateString();
                    //controller.ViewBag.DateTo = DateTo.ToShortDateString();
                    //controller.ViewBag.DateRangeType = request.Params["DateRangeType"] ?? "day";

                }
                catch (Exception)
                {
                    DateFrom = DateTime.Now.Date;
                    DateTo = DateTime.Now.Date;
                    //controller.ViewBag.DateFrom = DateFrom.ToShortDateString();
                    //controller.ViewBag.DateTo = DateTo.ToShortDateString();
                    //controller.ViewBag.DateRangeType = request.Params["DateRangeType"] ?? "day";
                }

                #endregion
            }

            //public static void SetDateRange(HttpRequestBase request, out  DateTime DateFrom, out DateTime DateTo)
            //{

            //    #region xử lý điều kiện ngày
            //    try
            //    {
            //        //((System.Globalization.CultureInfo)SenViet.GlobeVariant.CultureInfo["CICC"]).DateTimeFormat

            //        DateFrom = DateTime.Parse(request.Params["DateFrom"] ?? DateTime.Now.Date.ToShortDateString()); // Default min giờ.
            //        DateTo = DateTime.Parse(request.Params["DateTo"] ?? DateTime.Now.Date.ToShortDateString());
            //        DateTo = DateTo.Date.AddDays(1).AddTicks(-1); // Set Max giờ.

            //        //controller.ViewBag.DateFrom = DateFrom.ToShortDateString();
            //        //controller.ViewBag.DateTo = DateTo.ToShortDateString();
            //        //controller.ViewBag.DateRangeType = request.Params["DateRangeType"] ?? "day";

            //    }
            //    catch (Exception)
            //    {
            //        DateFrom = DateTime.Now.Date;
            //        DateTo = DateTime.Now.Date;
            //        //controller.ViewBag.DateFrom = DateFrom.ToShortDateString();
            //        //controller.ViewBag.DateTo = DateTo.ToShortDateString();
            //        //controller.ViewBag.DateRangeType = request.Params["DateRangeType"] ?? "day";
            //    }

            //    #endregion
            //}

            public static int GetMonthRange(DateTime DateFrom, DateTime DateTo)
            {
                if (DateFrom > DateTo)
                {
                    return 0;
                }
                DateTime DateTemp = DateFrom;
                int MonthRange = 0;
                while ((DateTemp.Year < DateTo.Year) || (DateTemp.Year == DateTo.Year && DateTemp.Month < DateTo.Month))
                {
                    MonthRange++;
                    DateTemp = DateTemp.AddMonths(1);
                }
                return MonthRange;
            }

            public static int GetDayRange(DateTime DateFrom, DateTime DateTo)
            {
                if (DateFrom > DateTo)
                {
                    return 0;
                }
                TimeSpan ts = DateTo - DateFrom;
                // Difference in days.
                int DayRange = ts.Days;
                return DayRange;
            }
        }

        public static string GetId()
        {
            string _filename = string.Format("{0}", DateTime.UtcNow.ToString("yyyyMMddhhss"));
            return _filename;
        }
    }
}