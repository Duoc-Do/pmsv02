using System;
using System.Collections.Generic;
using System.Linq;

using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.IO;
//Tiện ích chứng từ
namespace WebApp.Services
{
    public static class Data
    {
        public static object GetByKeyword(System.Web.HttpRequestBase request)
        {
            string fieldname = request.Params["fieldname"].ToString();
            string keyword = request.Params["keyword"].ToString();
            string tablename = request.Params["tablename"] != null ? request.Params["tablename"].ToString() : "";
            return GetByKeyword(request, fieldname, keyword, tablename);
        }
        public static object GetByKeyword(System.Web.HttpRequestBase request, string fieldname, string keyword, string tablename = "")
        {
            Models.SenContext db = new Models.SenContext();
            //int maxRows = 10;
            Models.PagingAutocomplete paging = new Models.PagingAutocomplete(request);

            object rows = null;
            object results = null;

            keyword = keyword.Trim();

            switch (fieldname)
            {

                //case "RoleName":
                //    paging.SetPaging(db.aspnet_Roles.Where(m => (m.RoleName + m.Description).Contains(keyword)).Count());
                //    rows = (from m in db.aspnet_Roles
                //            where (m.RoleName + m.Description).Contains(keyword)
                //            orderby m.RoleName
                //            select new { label = m.RoleName + " " + m.Description, value = m.RoleName, id = m.RoleId, name = m.RoleName })
                //                                        .Skip(paging.Skip)
                //            .Take(paging.PageSize).ToList();
                //    break;
                default:
                    break;
            }
            results = (new { rows, paging });
            return results;
        }
        public static object GetByCode(System.Web.HttpRequestBase request)
        {
            string fieldname = request.Params["fieldname"].ToString();
            string keyword = request.Params["keyword"].ToString();
            string tablename = request.Params["tablename"] != null ? request.Params["tablename"].ToString() : "";
            return GetByCode(fieldname, keyword, tablename);
        }
        public static object GetByCode(string fieldname, string keyword, string tablename = "")
        {
            Models.SenContext db = new Models.SenContext();
            object results = null;

            switch (fieldname)
            {
                case "ApplicationName":
                    var ApplicationName = db.Database.SqlQuery<Models.SenApplication>("Select * from SenApplication Where Name={0}", keyword).SingleOrDefault();
                    results = (new { rows = ApplicationName });
                    break;
                default:
                    break;
            }
            return results;
        }
    }

    public static class Export
    {
        public static void ExportToXlsx(Stream stream, IEnumerable<dynamic> datasource, Dictionary<string, Models.SysTableDetailView> columns)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            // ok, we can run the real code of the sample now
            using (var xlPackage = new ExcelPackage(stream))
            {

                // uncomment this line if you want the XML written out to the outputDir
                //xlPackage.DebugMode = true; 

                // get handle to the existing worksheet
                var worksheet = xlPackage.Workbook.Worksheets.Add("Sheet1");
                //Create Headers and format them
                int colheader = 0;
                worksheet.Cells[1, colheader + 1].Value = "Stt";
                worksheet.Cells[1, colheader + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[1, colheader + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                worksheet.Cells[1, colheader + 1].Style.Font.Bold = true;
                colheader++;
                foreach (var item in columns)
                {
                    if (item.Value.GridViewShow)
                    {
                        worksheet.Cells[1, colheader + 1].Value = item.Value.Des;
                        worksheet.Cells[1, colheader + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[1, colheader + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                        worksheet.Cells[1, colheader + 1].Style.Font.Bold = true;
                        colheader++;
                    }

                }
                if (datasource != null)
                {

                    int row = 2;
                    foreach (dynamic data in datasource)
                    {
                        int colCount = 1;
                        worksheet.Cells[row, colCount].Value = row - 1;
                        colCount++;
                        foreach (var item in columns)
                        {
                            if (item.Value.GridViewShow)
                            {
                                var value = data.GetType().GetProperty(item.Value.ColumnName).GetValue(data, null) ?? String.Empty;

                                string datatype = Services.ExConvert.Sqltype2Systemtype(item.Value.DATA_TYPE);
                                if (datatype == "datetime")
                                {
                                    string _value = Services.ExConvert.Data2String(value, datatype, item.Value.FormatValue, item.Value.CultureInfo);
                                    worksheet.Cells[row, colCount].Value = _value;
                                }
                                else
                                {
                                    worksheet.Cells[row, colCount].Value = value;
                                }

                                colCount++;
                            }
                        }
                        row++;
                    }
                }


                // we had better add some document properties to the spreadsheet 

                // set some core property values
                //var storeName = _storeInformationSettings.StoreName;
                //var storeUrl = _storeInformationSettings.StoreUrl;
                //xlPackage.Workbook.Properties.Title = string.Format("{0} customers", storeName);
                //xlPackage.Workbook.Properties.Author = storeName;
                //xlPackage.Workbook.Properties.Subject = string.Format("{0} customers", storeName);
                //xlPackage.Workbook.Properties.Keywords = string.Format("{0} customers", storeName);
                //xlPackage.Workbook.Properties.Category = "Customers";
                //xlPackage.Workbook.Properties.Comments = string.Format("{0} customers", storeName);

                // set some extended property values
                //xlPackage.Workbook.Properties.Company = storeName;
                //xlPackage.Workbook.Properties.HyperlinkBase = new Uri(storeUrl);

                // save the new spreadsheet
                xlPackage.Save();
            }
        }

    }
}