using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using WebApp.Areas.Admin.Models;

using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.IO;
//Tiện ích chứng từ
namespace WebApp.Areas.Admin.Services
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
            Models.AdminContext db = new Models.AdminContext();
            //int maxRows = 10;
            Models.PagingAutocomplete paging = new Models.PagingAutocomplete(request);

            object rows = null;
            object results = null;

            keyword = keyword.Trim();

            switch (fieldname)
            {
                case "CustomerName":
                    paging.SetPaging(db.SenCustomers.Where(m => (m.CustomerCode + " - " + m.Name).Contains(keyword)).Count());
                    rows = (from m in db.SenCustomers
                            where (m.CustomerCode + " - " + m.Name).Contains(keyword)
                            orderby m.Name
                            select new { label = m.CustomerCode + " - " + m.Name, value = m.Name, id = m.CustomerId, name = m.Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "ProductName":
                    paging.SetPaging(db.SenProducts.Where(m => (m.ProductCode + " - " + m.Name).Contains(keyword)).Count());
                    rows = (from m in db.SenProducts
                            where (m.ProductCode + " - " + m.Name).Contains(keyword)
                            orderby m.Name
                            select new { label = m.ProductCode + " - " + m.Name, value = m.Name, id = m.ProductId, name = m.Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "CompanyName":
                    paging.SetPaging(db.SenCompanyViews.Where(m => (m.Name + " - " + m.UserName).Contains(keyword)).Count());
                    rows = (from m in db.SenCompanyViews
                            where (m.Name + " - " + m.UserName).Contains(keyword)
                            orderby m.Name
                            select new { label = m.Name + " - " + m.UserName, value = m.Name, id = m.CompanyId, name = m.Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "ApplicationName":
                    paging.SetPaging(db.SenApplications.Where(m => (m.Name).Contains(keyword)).Count());
                    rows = (from m in db.SenApplications
                            where (m.Name).Contains(keyword)
                            orderby m.Name
                            select new { label = m.Name, value = m.Name, id = m.ApplicationId, name = m.Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;

                case "UserName":
                    paging.SetPaging(db.vw_aspnet_MembershipUsers.Where(m => (m.UserName + m.Email).Contains(keyword)).Count());
                    rows = (from m in db.vw_aspnet_MembershipUsers
                            where (m.UserName + m.Email).Contains(keyword)
                            orderby m.UserName
                            select new { label = m.UserName + " " + m.Email, value = m.UserName, id = m.UserId, name = m.UserName })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "RoleName":
                    paging.SetPaging(db.aspnet_Roles.Where(m => (m.RoleName + m.Description).Contains(keyword)).Count());
                    rows = (from m in db.aspnet_Roles
                            where (m.RoleName + m.Description).Contains(keyword)
                            orderby m.RoleName
                            select new { label = m.RoleName + " " + m.Description, value = m.RoleName, id = m.RoleId, name = m.RoleName })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "TableName":
                    paging.SetPaging(db.SysTables.Where(m => (m.TableName + m.Des).Contains(keyword)).Count());
                    rows = (from m in db.SysTables
                            where (m.TableName + m.Des).Contains(keyword)
                            orderby m.TableName
                            select new { label = m.TableName + " " + m.Des, value = m.TableName, id = m.TableName, name = m.TableName })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "ColumnName":
                    paging.SetPaging(db.SysColumns.Where(m => (m.Name + m.Des).Contains(keyword)).Count());
                    rows = (from m in db.SysColumns
                            where (m.Name + m.Des).Contains(keyword)
                            orderby m.Name
                            select new { label = m.Name + " " + m.Des, value = m.Name, id = m.Name, name = m.Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
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
            Models.AdminContext db = new Models.AdminContext();
            object results = null;

            switch (fieldname)
            {
                case "CustomerName":
                    var CustomerName = db.Database.SqlQuery<Models.SenCustomer>("Select * from SenCustomer Where Name={0}", keyword).SingleOrDefault();
                    results = (new { rows = CustomerName });
                    break;
                case "ProductName":
                    var ProductName = db.Database.SqlQuery<Models.SenProduct>("Select * from SenProduct Where Name={0}", keyword).SingleOrDefault();
                    results = (new { rows = ProductName });
                    break;
                case "UserName":
                    var UserName = db.Database.SqlQuery<Models.aspnet_Users>("Select * from aspnet_Users Where UserName={0}", keyword).SingleOrDefault();
                    results = (new { rows = UserName });
                    break;
                case "RoleName":
                    var RoleName = db.Database.SqlQuery<Models.aspnet_Roles>("Select * from aspnet_Roles Where RoleName={0}", keyword).SingleOrDefault();
                    results = (new { rows = RoleName });
                    break;
                case "CompanyName":
                    var CompanyName = db.Database.SqlQuery<Models.SenCompany>("Select * from SenCompany Where Name={0}", keyword).SingleOrDefault();
                    results = (new { rows = CompanyName });
                    break;
                case "ApplicationName":
                    var ApplicationName = db.Database.SqlQuery<Models.SenApplication>("Select * from SenApplication Where Name={0}", keyword).SingleOrDefault();
                    results = (new { rows = ApplicationName });
                    break;
                case "TableName":
                    var TableName = db.Database.SqlQuery<Models.SysTable>("Select * from SysTable Where TableName={0}", keyword).SingleOrDefault();
                    results = (new { rows = TableName });
                    break;
                case "ColumnName":
                    var ColumnName = db.Database.SqlQuery<Models.SysColumn>("Select * from SysColumn Where Name={0}", keyword).SingleOrDefault();
                    results = (new { rows = ColumnName });
                    break;
                default:
                    break;
            }
            return results;
        }

        public static void CalSummary(this SysTableDetailView column, object row)
        {
            if (column.DATA_TYPE == "decimal")
            {
                var value = row.GetType().GetProperty(column.ColumnName).GetValue(row, null) ?? "0";
                column.Summary += decimal.Parse(value.ToString());
            }
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