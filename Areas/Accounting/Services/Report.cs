using Microsoft.Reporting.WebForms;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


//Tiện ích chứng từ
namespace WebApp.Areas.Accounting.Services.Report
{

    public static class Print
    {
        //public static void MapView2Table(dynamic view, dynamic table, string businesscode)
        //{
        //    var metatable = Services.GlobalMeta.GetMetaObject(businesscode).MetaTable;//lấy meta của bảng gốc

        //    foreach (var item in metatable)
        //    {
        //        string fieldmap = string.Format("{0}", item.ColumnName.ToString());
        //        var value = view.GetType().GetProperty(fieldmap).GetValue(view, null);
        //        table.GetType().GetProperty(fieldmap).SetValue(table, value, null);
        //    }


        //}

        public static ActionResult PrintVoucher<T>(Controller controller, long documentid, int sysreportid, string reporttype = "PDF", System.Collections.Hashtable ReportParameter = null)
        {

            LocalReport localReport = new LocalReport();

            var reportinfo = Services.Report.Uti.GetReport(sysreportid);

            //localReport.ReportPath = controller.Server.MapPath(string.Format("~/Areas/Accounting/Reports/{0}", reportinfo.ReportName)); //Server.MapPath("~/Areas/ACC/Reports/RpInventorySummary1Report.rdlc");

            if (!string.IsNullOrEmpty(reportinfo.ReportPath))
            {
                localReport.ReportPath = controller.Server.MapPath(string.Format("~/Areas/Accounting/Reports/{0}/{1}",reportinfo.ReportPath, reportinfo.ReportName));
            }
            else
            {
                localReport.ReportPath = controller.Server.MapPath(string.Format("~/Areas/Accounting/Reports/{0}", reportinfo.ReportName)); //Server.MapPath("~/Areas/ACC/Reports/RpInventorySummary1Report.rdlc");    
            }

            var db = new Models.WebAppAccEntities(Services.GlobalVariant.GetConnection());
            string storeprocedure = string.Format("{0} @Key", reportinfo.ProcedureName);

            //var reportdatasource = db.Database.SqlQuery<T>(storeprocedure, new System.Data.SqlClient.SqlParameter { ParameterName = "@Key", Value = string.Format("DocumentID='{0}'", documentid) }).ToList();

            var reportdatasource = db.Database.SqlQuery<T>(storeprocedure, new System.Data.SqlClient.SqlParameter { ParameterName = "@Key", Value = string.Format("DocumentID='{0}'", documentid) }).ToList();

            ReportDataSource reportDataSource = new ReportDataSource("DSPrint", reportdatasource);

            localReport.DataSources.Add(reportDataSource);

            //Gán giá trị tham số
            Services.Report.ReportUI.InitUIReport(localReport, reportinfo.TableName);

            if (ReportParameter["ReportTitle"] != null)
            {
                ReportParameter.Remove("ReportTitle");
            }
            ReportParameter.Add("ReportTitle", reportinfo.Title);

            Services.Report.ReportUI.SetReportParameter(localReport, ReportParameter);


            string deviceInfo = Services.Report.Uti.GetDeviceInfo(localReport);

            string reportType = reporttype;// Excel, PDF, Word, and Image.
            string mimeType;
            string encoding;
            string fileNameExtension;


            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            //Render the report
            renderedBytes = localReport.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            System.Web.Mvc.FileContentResult F = new FileContentResult(renderedBytes, mimeType);

            return F;
        }

        public static FileContentResult PrintReport<T>(Controller controller, List<T> reportdatasource, int sysreportid, string reporttype = "PDF", System.Collections.Hashtable ReportParameter = null)
        {
            LocalReport localReport = new LocalReport();

            var reportinfo = Services.Report.Uti.GetReport(sysreportid);

            localReport.ReportPath = controller.Server.MapPath(string.Format("~/Areas/Accounting/Reports/{0}", reportinfo.ReportName)); //Server.MapPath("~/Areas/ACC/Reports/RpInventorySummary1Report.rdlc");

            ReportDataSource reportDataSource = new ReportDataSource("DSPrint", reportdatasource);

            localReport.DataSources.Add(reportDataSource);

            //Gán giá trị tham số
            Services.Report.ReportUI.InitUIReport(localReport, reportinfo.TableName);

            if (ReportParameter["ReportTitle"] != null)
            {
                ReportParameter.Remove("ReportTitle");
            }
            ReportParameter.Add("ReportTitle", reportinfo.Title);

            Services.Report.ReportUI.SetReportParameter(localReport, ReportParameter);


            string deviceInfo = Services.Report.Uti.GetDeviceInfo(localReport);

            string reportType = reporttype;// Excel, PDF, Word, and Image.
            string mimeType;
            string encoding;
            string fileNameExtension;

            //= ".pdf";
            //switch (reporttype)
            //{
            //    case "EXCEL":
            //        fileNameExtension = ".xls";
            //        break;
            //    case "WORD":
            //        fileNameExtension = ".doc";
            //        break;
            //    case "IMAGE":
            //        fileNameExtension = ".jpg";
            //        break;
            //    default:

            //        break;
            //}

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            //Render the report
            renderedBytes = localReport.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings);

            System.Web.Mvc.FileContentResult F = new FileContentResult(renderedBytes, mimeType);
            switch (reporttype)
            {
                case "EXCEL":
                    F.FileDownloadName = "Print.xls";
                    break;
                case "WORD":
                    F.FileDownloadName = "Print.doc";
                    break;
                case "IMAGE":
                    F.FileDownloadName = "Print.jpg";
                    break;
                default:
                    F.FileDownloadName = "Print.pdf";
                    break;
            }

            return F;
        }

    }

    public static class ReportUI
    {
        public static void InitUIReport(Microsoft.Reporting.WebForms.LocalReport localreport, string metaname)
        {
            #region Set giá trị cho tham số Report

            Microsoft.Reporting.WebForms.ReportParameterInfoCollection RPara = localreport.GetParameters();
            var Columns = Services.GlobalMeta.GetMetaObject(metaname).GetMetaTable();


            foreach (Microsoft.Reporting.WebForms.ReportParameterInfo item in RPara)
            {
                if (item.Name.Length > 6)
                {

                    string FieldName = item.Name.Substring(6);
                    if (Columns.ContainsKey(FieldName))
                    {
                        if (item.Name.IndexOf("Header") == 0)
                        {
                            localreport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter[] { new Microsoft.Reporting.WebForms.ReportParameter(item.Name, Columns[FieldName].Des) });
                        }
                        if (item.Name.IndexOf("Format") == 0)
                        {
                            string Format = "";
                            if (Columns[FieldName].FormatValue != null)
                            {
                                Format = Columns[FieldName].FormatValue.ToString();
                            }

                            if (Columns[FieldName].CultureInfo != null)
                            {
                                switch (Columns[FieldName].CultureInfo.ToString())
                                {
                                    case "CIFC":
                                        if (Format == "n")
                                        {
                                            //Format += Services.GlobalVariant.GetSysOption()["RoundAmountFC"].ToString();


                                            Format = "#,##0" + SetRound(int.Parse(Services.GlobalVariant.GetSysOption()["RoundAmountFC"].ToString()));
                                        }
                                        break;
                                    case "CIUPCC":
                                        if (Format == "n")
                                        {
                                            //Format += Services.GlobalVariant.GetSysOption()["RoundUnitPrice"].ToString();
                                            Format = "#,##0" + SetRound(int.Parse(Services.GlobalVariant.GetSysOption()["RoundUnitPrice"].ToString()));
                                        }
                                        break;
                                    case "CIUPFC":
                                        if (Format == "n")
                                        {
                                            //Format += Services.GlobalVariant.GetSysOption()["RoundUnitPriceFC"].ToString();
                                            Format = "#,##0" + SetRound(int.Parse(Services.GlobalVariant.GetSysOption()["RoundUnitPriceFC"].ToString()));
                                        }
                                        break;
                                    default:
                                        switch (Format)
                                        {
                                            case "c":
                                                //hiện format dấu tiền tệ
                                                //Format += Services.GlobalVariant.GetSysOption()["RoundAmount"].ToString();
                                                //Không hiện format dấu tiền tệ
                                                //Format = "n"+Services.GlobalVariant.GetSysOption()["RoundAmount"].ToString();
                                                Format = "#,##0" + SetRound(int.Parse(Services.GlobalVariant.GetSysOption()["RoundAmount"].ToString()));
                                                break;
                                            case "n":
                                                //Format += Services.GlobalVariant.GetSysOption()["RoundQuantity"].ToString();
                                                Format = "#,##0" + SetRound(int.Parse(Services.GlobalVariant.GetSysOption()["RoundQuantity"].ToString()));
                                                break;
                                            default:
                                                break;
                                        }

                                        break;
                                }
                            }
                            else
                            {
                                switch (Format)
                                {
                                    case "c":
                                        //hiện format dấu tiền tệ
                                        //Format += Services.GlobalVariant.GetSysOption()["RoundAmount"].ToString();
                                        //Không hiện format dấu tiền tệ
                                        //Format = "n"+Services.GlobalVariant.GetSysOption()["RoundAmount"].ToString();
                                        Format = "#,##0" + SetRound(int.Parse(Services.GlobalVariant.GetSysOption()["RoundAmount"].ToString()));
                                        break;
                                    case "n":
                                        //Format += Services.GlobalVariant.GetSysOption()["RoundQuantity"].ToString();
                                        Format = "#,##0" + SetRound(int.Parse(Services.GlobalVariant.GetSysOption()["RoundQuantity"].ToString()));
                                        break;
                                    default:
                                        break;
                                }
                            }

                            if (!string.IsNullOrEmpty(Format))
                            {
                                Format = string.Format("{0};-{1};{2}", Format, Format, "#.#");

                                //Format = string.Format("{0};{1};'-'", Format, Format);
                                localreport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter[] { new Microsoft.Reporting.WebForms.ReportParameter(item.Name, Format) });
                            }
                        }
                    }
                }

            }
            #endregion
        }

        private static string SetRound(int Round)
        {
            if (Round <= 0)
            {
                return "";
            }
            switch (Round)
            {
                case 1:
                    return ".#";
                case 2:
                    return ".##";
                case 3:
                    return ".###";
                case 4:
                    return ".####";
                case 5:
                    return ".#####";
                case 6:
                    return ".######";
                default:
                    return "";
            }

        }

        public static void SetReportParameter(Microsoft.Reporting.WebForms.LocalReport localreport, System.Collections.Hashtable Para)
        {
            Microsoft.Reporting.WebForms.ReportParameterInfoCollection RPara = localreport.GetParameters();

            foreach (Microsoft.Reporting.WebForms.ReportParameterInfo item in RPara)
            {

                switch (item.Name)
                {
                    case "CompanyName":
                        localreport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter[] { new Microsoft.Reporting.WebForms.ReportParameter(item.Name, Services.GlobalVariant.GetSysCompany()["CompanyName"].ToString()) });
                        break;
                    case "CompanyAddress":
                        localreport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter[] { new Microsoft.Reporting.WebForms.ReportParameter(item.Name, Services.GlobalVariant.GetSysCompany()["CompanyAddress"].ToString()) });
                        break;
                    case "CompanyPhone":
                        localreport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter[] { new Microsoft.Reporting.WebForms.ReportParameter(item.Name, Services.GlobalVariant.GetSysCompany()["CompanyPhone"].ToString()) });
                        break;
                    case "CompanyFax":
                        localreport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter[] { new Microsoft.Reporting.WebForms.ReportParameter(item.Name, Services.GlobalVariant.GetSysCompany()["CompanyFax"].ToString()) });
                        break;
                    case "CompanyTaxCode":
                        localreport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter[] { new Microsoft.Reporting.WebForms.ReportParameter(item.Name, Services.GlobalVariant.GetSysCompany()["CompanyTaxCode"].ToString()) });
                        break;
                    case "QDBTC":
                        localreport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter[] { new Microsoft.Reporting.WebForms.ReportParameter(item.Name, Services.GlobalVariant.GetSysOption()["QDBTC"].ToString()) });
                        break;
                    case "DirectorName":
                        localreport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter[] { new Microsoft.Reporting.WebForms.ReportParameter(item.Name, Services.GlobalVariant.GetSysOption()["DirectorName"].ToString()) });
                        break;
                    case "ChiefAccountantName":
                        localreport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter[] { new Microsoft.Reporting.WebForms.ReportParameter(item.Name, Services.GlobalVariant.GetSysOption()["ChiefAccountantName"].ToString()) });
                        break;
                    case "ReportCreatorName":
                        localreport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter[] { new Microsoft.Reporting.WebForms.ReportParameter(item.Name, Services.GlobalVariant.GetSysOption()["ReportCreatorName"].ToString()) });
                        break;
                    case "ReportFontSize":
                        localreport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter[] { new Microsoft.Reporting.WebForms.ReportParameter(item.Name, Services.GlobalVariant.GetSysOption()["ReportFontSize"].ToString()) });
                        break;
                    default:
                        if (Para[item.Name] != null)
                        {
                            localreport.SetParameters(new Microsoft.Reporting.WebForms.ReportParameter[] { new Microsoft.Reporting.WebForms.ReportParameter(item.Name, Para[item.Name].ToString()) });
                        }
                        break;
                }

            }
        }
    }

    public static class Printing
    {

    }

    public static class Uti
    {
        public static string GetDeviceInfo(LocalReport localReport)
        {

            //Body Width <= Page Width - (Left Margin + Right Margin)

            //Horizontal usable area:

            //X = Page.Width - (Left Margin + Right Margin + Column Spacing)

            //Vertical usable area:

            //Y = Page.Height - (Top Margin + Bottom Margin + Header Height + Footer Height)




            string deviceInfo =
  "<DeviceInfo>" +
  "  <OutputFormat>PDF</OutputFormat>" +
  string.Format("  <PageWidth>{0}in</PageWidth>", ((decimal)(localReport.GetDefaultPageSettings().PaperSize.Width) / 100).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))) +
  string.Format("  <PageHeight>{0}in</PageHeight>", ((decimal)(localReport.GetDefaultPageSettings().PaperSize.Height) / 100).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))) +
  string.Format("  <MarginTop>{0}in</MarginTop>", ((decimal)(localReport.GetDefaultPageSettings().Margins.Top) / 100).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))) +
  string.Format("  <MarginLeft>{0}in</MarginLeft>", ((decimal)(localReport.GetDefaultPageSettings().Margins.Left) / 100).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))) +
  string.Format("  <MarginRight>{0}in</MarginRight>", ((decimal)(localReport.GetDefaultPageSettings().Margins.Right) / 100).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))) +
  string.Format("  <MarginBottom>{0}in</MarginBottom>", ((decimal)(localReport.GetDefaultPageSettings().Margins.Bottom) / 100).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"))) +
  "</DeviceInfo>";
            return deviceInfo;
        }

        public static List<Models.SysReport> GetReports(string BusinessCode)
        {
            string sessionkeys = string.Format("sysreport.{0}", BusinessCode);
            if (HttpContext.Current.Session[sessionkeys] != null)
            {
                return (List<Models.SysReport>)HttpContext.Current.Session[sessionkeys];
            }

            var db = new Models.WebAppAccEntities(Services.GlobalVariant.GetConnection());
            var result = db.SysReports.Where(m => m.BusinessCode == BusinessCode).OrderByDescending(m => m.IsDefault).ToList();
            HttpContext.Current.Session[sessionkeys] = result;
            return result;
        }

        public static Models.SysReport GetReport(int SysReportID)
        {
            var db = new Models.WebAppAccEntities(Services.GlobalVariant.GetConnection());
            var result = db.SysReports.Where(m => m.SysReportID == SysReportID).SingleOrDefault();
            return result;
        }

    }
}