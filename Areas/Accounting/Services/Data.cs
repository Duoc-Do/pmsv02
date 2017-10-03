using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using WebApp.Areas.Accounting.Models;

using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.IO;
//Tiện ích chứng từ
namespace WebApp.Areas.Accounting.Services
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
            Models.WebAppAccEntities db = new Models.WebAppAccEntities(Services.GlobalVariant.GetConnection());
            //int maxRows = 10;
            Models.PagingAutocomplete paging = new Models.PagingAutocomplete(request);

            object rows = null;
            object results = null;

            keyword = keyword.Trim();

            switch (fieldname)
            {
                case "ConstructionCode":
                    paging.SetPaging(db.AppConstructionTables.Where(m => (m.ConstructionCode + m.Name).Contains(keyword)).Count());
                    rows = (from m in db.AppConstructionTables
                            where (m.ConstructionCode + m.Name).Contains(keyword)
                            orderby m.ConstructionCode
                            select new { label = m.ConstructionCode + " " + m.Name, value = m.ConstructionCode, id = m.ConstructionID, name = m.Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "EmployeeCode":
                    paging.SetPaging(db.AppEmployeeTables.Where(m => (m.EmployeeCode + m.Name).Contains(keyword)).Count());
                    rows = (from m in db.AppEmployeeTables
                            where (m.EmployeeCode+ m.Name).Contains(keyword)
                            orderby m.EmployeeCode
                            select new { label =m.EmployeeCode + " " + m.Name, value = m.EmployeeCode, id = m.EmployeeID, name = m.Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "BusinessCode":
                    paging.SetPaging(db.SysBusinesses.Where(m => (m.BusinessCode + " " + m.Des).Contains(keyword)).Count());
                    rows = (from m in db.SysBusinesses
                            where (m.BusinessCode + " " + m.Des).Contains(keyword)
                            orderby m.BusinessCode
                            select new { label = m.BusinessCode + " " + m.Des, value = m.BusinessCode, id = m.BusinessCode, name = m.BusinessCode })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "RoleName":
                    paging.SetPaging(db.SysRoles.Where(m => (m.Name).Contains(keyword)).Count());
                    rows = (from m in db.SysRoles
                            where (m.Name).Contains(keyword)
                            orderby m.Name
                            select new { label = m.Name, value = m.Name, id = m.RoleID, name = m.Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "DepartmentCode":
                    paging.SetPaging(db.AppDepartmentTables.Where(m => (m.DepartmentCode + m.Name).Contains(keyword)).Count());
                    rows = (from m in db.AppDepartmentTables
                            where (m.DepartmentCode + m.Name).Contains(keyword)
                            orderby m.DepartmentCode
                            select new { label = m.DepartmentCode + " " + m.Name, value = m.DepartmentCode, id = m.DepartmentID, name = m.Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "ExpenseCode":
                    paging.SetPaging(db.AppExpenseTables.Where(m => (m.ExpenseCode + m.Name).Contains(keyword)).Count());
                    rows = (from m in db.AppExpenseTables
                            where (m.ExpenseCode + m.Name).Contains(keyword)
                            orderby m.ExpenseCode
                            select new { label = m.ExpenseCode + " " + m.Name, value = m.ExpenseCode, id = m.ExpenseID, name = m.Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "ExObject01Code":
                    paging.SetPaging(db.AppExObject01Views.Where(m => (m.ExObject01Code + m.ExObject01Name).Contains(keyword)).Count());
                    rows = (from m in db.AppExObject01Views
                            where (m.ExObject01Code + m.ExObject01Name).Contains(keyword)
                            orderby m.ExObject01Code
                            select new { label = m.ExObject01Code + m.ExObject01Name, value = m.ExObject01Code, id = m.ExObject01ID, name = m.ExObject01Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "ExObject02Code":
                    paging.SetPaging(db.AppExObject02Views.Where(m => (m.ExObject02Code + m.ExObject02Name).Contains(keyword)).Count());
                    rows = (from m in db.AppExObject02Views
                            where (m.ExObject02Code + m.ExObject02Name).Contains(keyword)
                            orderby m.ExObject02Code
                            select new { label = m.ExObject02Code + m.ExObject02Name, value = m.ExObject02Code, id = m.ExObject02ID, name = m.ExObject02Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "ExObject03Code":
                    paging.SetPaging(db.AppExObject03Views.Where(m => (m.ExObject03Code + m.ExObject03Name).Contains(keyword)).Count());
                    rows = (from m in db.AppExObject03Views
                            where (m.ExObject03Code + m.ExObject03Name).Contains(keyword)
                            orderby m.ExObject03Code
                            select new { label = m.ExObject03Code + m.ExObject03Name, value = m.ExObject03Code, id = m.ExObject03ID, name = m.ExObject03Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "ExObject04Code":
                    paging.SetPaging(db.AppExObject04Views.Where(m => (m.ExObject04Code + m.ExObject04Name).Contains(keyword)).Count());
                    rows = (from m in db.AppExObject04Views
                            where (m.ExObject04Code + m.ExObject04Name).Contains(keyword)
                            orderby m.ExObject04Code
                            select new { label = m.ExObject04Code + m.ExObject04Name, value = m.ExObject04Code, id = m.ExObject04ID, name = m.ExObject04Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "ExObject05Code":
                    paging.SetPaging(db.AppExObject05Views.Where(m => (m.ExObject05Code + m.ExObject05Name).Contains(keyword)).Count());
                    rows = (from m in db.AppExObject05Views
                            where (m.ExObject05Code + m.ExObject05Name).Contains(keyword)
                            orderby m.ExObject05Code
                            select new { label = m.ExObject05Code + m.ExObject05Name, value = m.ExObject05Code, id = m.ExObject05ID, name = m.ExObject05Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "ExObject06Code":
                    paging.SetPaging(db.AppExObject06Views.Where(m => (m.ExObject06Code + m.ExObject06Name).Contains(keyword)).Count());
                    rows = (from m in db.AppExObject06Views
                            where (m.ExObject06Code + m.ExObject06Name).Contains(keyword)
                            orderby m.ExObject06Code
                            select new { label = m.ExObject06Code + m.ExObject06Name, value = m.ExObject06Code, id = m.ExObject06ID, name = m.ExObject06Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "ExObject07Code":
                    paging.SetPaging(db.AppExObject07Views.Where(m => (m.ExObject07Code + m.ExObject07Name).Contains(keyword)).Count());
                    rows = (from m in db.AppExObject07Views
                            where (m.ExObject07Code + m.ExObject07Name).Contains(keyword)
                            orderby m.ExObject07Code
                            select new { label = m.ExObject07Code + m.ExObject07Name, value = m.ExObject07Code, id = m.ExObject07ID, name = m.ExObject07Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "ExObject08Code":
                    paging.SetPaging(db.AppExObject08Views.Where(m => (m.ExObject08Code + m.ExObject08Name).Contains(keyword)).Count());
                    rows = (from m in db.AppExObject08Views
                            where (m.ExObject08Code + m.ExObject08Name).Contains(keyword)
                            orderby m.ExObject08Code
                            select new { label = m.ExObject08Code + m.ExObject08Name, value = m.ExObject08Code, id = m.ExObject08ID, name = m.ExObject08Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "ExObject09Code":
                    paging.SetPaging(db.AppExObject09Views.Where(m => (m.ExObject09Code + m.ExObject09Name).Contains(keyword)).Count());
                    rows = (from m in db.AppExObject09Views
                            where (m.ExObject09Code + m.ExObject09Name).Contains(keyword)
                            orderby m.ExObject09Code
                            select new { label = m.ExObject09Code + m.ExObject09Name, value = m.ExObject09Code, id = m.ExObject09ID, name = m.ExObject09Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "ExObject10Code":
                    paging.SetPaging(db.AppExObject10Views.Where(m => (m.ExObject10Code + m.ExObject10Name).Contains(keyword)).Count());
                    rows = (from m in db.AppExObject10Views
                            where (m.ExObject10Code + m.ExObject10Name).Contains(keyword)
                            orderby m.ExObject10Code
                            select new { label = m.ExObject10Code + m.ExObject10Name, value = m.ExObject10Code, id = m.ExObject10ID, name = m.ExObject10Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "ExObject11Code":
                    paging.SetPaging(db.AppExObject11Views.Where(m => (m.ExObject11Code + m.ExObject11Name).Contains(keyword)).Count());
                    rows = (from m in db.AppExObject11Views
                            where (m.ExObject11Code + m.ExObject11Name).Contains(keyword)
                            orderby m.ExObject11Code
                            select new { label = m.ExObject11Code + m.ExObject11Name, value = m.ExObject11Code, id = m.ExObject11ID, name = m.ExObject11Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "ExObject12Code":
                    paging.SetPaging(db.AppExObject12Views.Where(m => (m.ExObject12Code + m.ExObject12Name).Contains(keyword)).Count());
                    rows = (from m in db.AppExObject12Views
                            where (m.ExObject12Code + m.ExObject12Name).Contains(keyword)
                            orderby m.ExObject12Code
                            select new { label = m.ExObject12Code + m.ExObject12Name, value = m.ExObject12Code, id = m.ExObject12ID, name = m.ExObject12Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "ExObject13Code":
                    paging.SetPaging(db.AppExObject13Views.Where(m => (m.ExObject13Code + m.ExObject13Name).Contains(keyword)).Count());
                    rows = (from m in db.AppExObject13Views
                            where (m.ExObject13Code + m.ExObject13Name).Contains(keyword)
                            orderby m.ExObject13Code
                            select new { label = m.ExObject13Code + m.ExObject13Name, value = m.ExObject13Code, id = m.ExObject13ID, name = m.ExObject13Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "ExObject14Code":
                    paging.SetPaging(db.AppExObject14Views.Where(m => (m.ExObject14Code + m.ExObject14Name).Contains(keyword)).Count());
                    rows = (from m in db.AppExObject14Views
                            where (m.ExObject14Code + m.ExObject14Name).Contains(keyword)
                            orderby m.ExObject14Code
                            select new { label = m.ExObject14Code + m.ExObject14Name, value = m.ExObject14Code, id = m.ExObject14ID, name = m.ExObject14Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "ExObject15Code":
                    paging.SetPaging(db.AppExObject15Views.Where(m => (m.ExObject15Code + m.ExObject15Name).Contains(keyword)).Count());
                    rows = (from m in db.AppExObject15Views
                            where (m.ExObject15Code + m.ExObject15Name).Contains(keyword)
                            orderby m.ExObject15Code
                            select new { label = m.ExObject15Code + m.ExObject15Name, value = m.ExObject15Code, id = m.ExObject15ID, name = m.ExObject15Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "ExObject16Code":
                    paging.SetPaging(db.AppExObject16Views.Where(m => (m.ExObject16Code + m.ExObject16Name).Contains(keyword)).Count());
                    rows = (from m in db.AppExObject16Views
                            where (m.ExObject16Code + m.ExObject16Name).Contains(keyword)
                            orderby m.ExObject16Code
                            select new { label = m.ExObject16Code + m.ExObject16Name, value = m.ExObject16Code, id = m.ExObject16ID, name = m.ExObject16Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "ExObject17Code":
                    paging.SetPaging(db.AppExObject17Views.Where(m => (m.ExObject17Code + m.ExObject17Name).Contains(keyword)).Count());
                    rows = (from m in db.AppExObject17Views
                            where (m.ExObject17Code + m.ExObject17Name).Contains(keyword)
                            orderby m.ExObject17Code
                            select new { label = m.ExObject17Code + m.ExObject17Name, value = m.ExObject17Code, id = m.ExObject17ID, name = m.ExObject17Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "ExObject18Code":
                    paging.SetPaging(db.AppExObject18Views.Where(m => (m.ExObject18Code + m.ExObject18Name).Contains(keyword)).Count());
                    rows = (from m in db.AppExObject18Views
                            where (m.ExObject18Code + m.ExObject18Name).Contains(keyword)
                            orderby m.ExObject18Code
                            select new { label = m.ExObject18Code + m.ExObject18Name, value = m.ExObject18Code, id = m.ExObject18ID, name = m.ExObject18Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "ExObject19Code":
                    paging.SetPaging(db.AppExObject19Views.Where(m => (m.ExObject19Code + m.ExObject19Name).Contains(keyword)).Count());
                    rows = (from m in db.AppExObject19Views
                            where (m.ExObject19Code + m.ExObject19Name).Contains(keyword)
                            orderby m.ExObject19Code
                            select new { label = m.ExObject19Code + m.ExObject19Name, value = m.ExObject19Code, id = m.ExObject19ID, name = m.ExObject19Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "ExObject20Code":
                    paging.SetPaging(db.AppExObject20Views.Where(m => (m.ExObject20Code + m.ExObject20Name).Contains(keyword)).Count());
                    rows = (from m in db.AppExObject20Views
                            where (m.ExObject20Code + m.ExObject20Name).Contains(keyword)
                            orderby m.ExObject20Code
                            select new { label = m.ExObject20Code + m.ExObject20Name, value = m.ExObject20Code, id = m.ExObject20ID, name = m.ExObject20Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;

                case "FixedAssetGroupCode":
                    paging.SetPaging(db.AppFixedAssetGroupTables.Where(m => (m.FixedAssetGroupCode + m.Name).Contains(keyword)).Count());
                    rows = (from m in db.AppFixedAssetGroupTables
                            where (m.FixedAssetGroupCode + m.Name).Contains(keyword)
                            orderby m.FixedAssetGroupCode
                            select new { label = m.FixedAssetGroupCode + " " + m.Name, value = m.FixedAssetGroupCode, id = m.FixedAssetGroupID, name = m.Name })
                            .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;

                case "FixedAssetTypeCode":
                    paging.SetPaging(db.AppFixedAssetTypeTables.Where(m => (m.FixedAssetTypeCode + m.Name).Contains(keyword)).Count());
                    rows = (from m in db.AppFixedAssetTypeTables
                            where (m.FixedAssetTypeCode + m.Name).Contains(keyword)
                            orderby m.FixedAssetTypeCode
                            select new { label = m.FixedAssetTypeCode + " " + m.Name, value = m.FixedAssetTypeCode, id = m.FixedAssetTypeID, name = m.Name })
                            .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;

                case "DocumentVATTypeName":
                    paging.SetPaging(db.AppDocumentVATTypeTables.Where(m => (m.EnumName).Contains(keyword)).Count());
                    rows = (from m in db.AppDocumentVATTypeTables
                            where (m.EnumName).Contains(keyword)
                            orderby m.EnumName
                            select new { label = m.EnumName, value = m.EnumName, id = m.EnumValue, name = m.EnumName })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "VATTemplate":
                    var appdocumentvattempview = db.Database.SqlQuery<LookupObject>("sp_AppDocumentVATTemplateTableLookUp").ToList();
                    paging.SetPaging(appdocumentvattempview.Where(m => (m.LookUp).Contains(keyword)).Count());
                    rows = (from m in appdocumentvattempview
                            where (m.LookUp).Contains(keyword)
                            orderby m.Code
                            select new { label = m.LookUp, value = m.Code, id = m.ID, name = m.LookUp })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "SalesPriceGroupCode":
                    paging.SetPaging(db.AppSalesPriceGroupTables.Where(m => (m.SalesPriceGroupCode + m.Name).Contains(keyword)).Count());
                    rows = (from m in db.AppSalesPriceGroupTables
                            where (m.SalesPriceGroupCode + m.Name).Contains(keyword)
                            orderby m.SalesPriceGroupCode
                            select new { label = m.SalesPriceGroupCode + " " + m.Name, value = m.SalesPriceGroupCode, id = m.SalesPriceGroupID, name = m.Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "WarehouseCode":
                case "WarehouseLineCode":
                    paging.SetPaging(db.AppWarehouseTables.Where(m => (m.WarehouseCode + m.Name).Contains(keyword)).Count());
                    rows = (from m in db.AppWarehouseTables
                            where (m.WarehouseCode + m.Name).Contains(keyword)
                            orderby m.WarehouseCode
                            select new { label = m.WarehouseCode + " " + m.Name, value = m.WarehouseCode, id = m.WarehouseID, name = m.Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "IsoCode":
                    paging.SetPaging(db.AppCurrencyTables.Where(m => (m.IsoCode + m.Name).Contains(keyword)).Count());
                    rows = (from m in db.AppCurrencyTables
                            where (m.IsoCode + m.Name).Contains(keyword)
                            orderby m.IsoCode
                            select new { label = m.IsoCode + " " + m.Name, value = m.IsoCode, id = m.CurrencyID, name = m.Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "DisplayNumberDebit":
                case "DisplayNumberCredit":
                case "DisplayNumberLineDebit":
                case "DisplayNumberLineCredit":
                case "DisplayNumberLine1Debit":
                case "DisplayNumberLine1Credit":
                case "DisplayNumber":
                    if (tablename == "AppAccountTable")
                    {
                        paging.SetPaging(db.AppAccountTables.Where(m => (m.DisplayNumber + m.Name).Contains(keyword)).Count());
                        rows = (from m in db.AppAccountTables
                                where (m.DisplayNumber + m.Name).Contains(keyword)
                                orderby m.DisplayNumber
                                select new { label = m.DisplayNumber + " " + m.Name, value = m.DisplayNumber, id = m.AccountID, name = m.DisplayNumber })
                                                            .Skip(paging.Skip)
                                .Take(paging.PageSize).ToList();
                        break;
                    }

                    paging.SetPaging(db.AppAccountView2.Where(m => (m.DisplayNumber + m.Name).Contains(keyword)).Count());
                    rows = (from m in db.AppAccountView2
                            where (m.DisplayNumber + m.Name).Contains(keyword)
                            orderby m.DisplayNumber
                            select new { label = m.DisplayNumber + " " + m.Name, value = m.DisplayNumber, id = m.AccountID, name = m.DisplayNumber })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "ParentDisplayNumber":
                    paging.SetPaging(db.AppAccountViews.Where(m => (m.DisplayNumber + m.Name).Contains(keyword)).Count());
                    rows = (from m in db.AppAccountViews
                            where (m.DisplayNumber + m.Name).Contains(keyword)
                            orderby m.DisplayNumber
                            select new { label = m.DisplayNumber + " " + m.Name, value = m.DisplayNumber, id = m.AccountID, name = m.DisplayNumber })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;

                case "CustomerCode":

                    paging.SetPaging(db.AppCustomerViews.Where(m => (m.CustomerCode + m.Name).Contains(keyword)).Count());
                    rows = (from m in db.AppCustomerViews
                            where (m.CustomerCode + m.Name).Contains(keyword)
                            orderby m.CustomerCode
                            select new { label = m.CustomerCode + " " + m.Name, value = m.CustomerCode, id = m.CustomerID, name = m.Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;

                case "CustomerGroupCode":
                    paging.SetPaging(db.AppCustomerGroupTables.Where(m => (m.CustomerGroupCode + m.Name).Contains(keyword)).Count());
                    rows = (from m in db.AppCustomerGroupTables
                            where (m.CustomerGroupCode + m.Name).Contains(keyword)
                            orderby m.CustomerGroupCode
                            select new { label = m.CustomerGroupCode + " " + m.Name, value = m.CustomerGroupCode, id = m.CustomerGroupID, name = m.Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;

                case "ProductCode":
                    paging.SetPaging(db.AppItemViews.Where(m => (m.ItemCode + m.Name).Contains(keyword)).Count());
                    rows = (from m in db.AppItemViews
                            where (m.ItemCode + m.Name).Contains(keyword) && m.ItemType==3
                            orderby m.ItemCode
                            select new { label = m.ItemCode + " " + m.Name, value = m.ItemCode, id = m.ItemID, name = m.Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "ItemCode":
                    paging.SetPaging(db.AppItemViews.Where(m => (m.ItemCode + m.Name).Contains(keyword)).Count());
                    rows = (from m in db.AppItemViews
                            where (m.ItemCode + m.Name).Contains(keyword)
                            orderby m.ItemCode
                            select new { label = m.ItemCode + " " + m.Name, value = m.ItemCode, id = m.ItemID, name = m.Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "UOMLinkCode":
                case "UOMCode":
                    int count = db.AppUnitOfMeasureTables.Where(m => m.UOMCode.Contains(keyword)).Count();
                    paging.SetPaging(count);

                    rows = (from m in db.AppUnitOfMeasureTables
                            where (m.UOMCode).Contains(keyword)
                            orderby m.UOMCode
                            select new { label = m.UOMCode + " " + m.Name, value = m.UOMCode, id = m.UOMID, name = m.Name })
                            .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "ItemTypeName":
                    paging.SetPaging(db.AppItemTypeTables.Where(m => (m.EnumName).Contains(keyword)).Count());
                    rows = (from m in db.AppItemTypeTables
                            where (m.EnumName).Contains(keyword)
                            orderby m.EnumName
                            select new { label = m.EnumName, value = m.EnumName, id = m.EnumValue, name = m.EnumName })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "ItemGroupCode":
                    paging.SetPaging(db.AppItemGroupTables.Where(m => (m.ItemGroupCode + m.Name).Contains(keyword)).Count());
                    rows = (from m in db.AppItemGroupTables
                            where (m.ItemGroupCode + m.Name).Contains(keyword)
                            orderby m.ItemGroupCode
                            select new { label = m.ItemGroupCode + " " + m.Name, value = m.ItemGroupCode, id = m.ItemGroupID, name = m.Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "ItemMethodTypeName":
                    paging.SetPaging(db.AppItemMethodTypeTables.Where(m => (m.EnumName).Contains(keyword)).Count());
                    rows = (from m in db.AppItemMethodTypeTables
                            where (m.EnumName).Contains(keyword)
                            orderby m.EnumName
                            select new { label = m.EnumName, value = m.EnumName, id = m.EnumValue, name = m.EnumName })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;


                case "PurchaseTaxCode":
                    paging.SetPaging(db.AppPurchaseTaxTables.Where(m => (m.PurchaseTaxCode).Contains(keyword)).Count());
                    rows = (from m in db.AppPurchaseTaxTables
                            where (m.PurchaseTaxCode).Contains(keyword)
                            orderby m.PurchaseTaxCode
                            select new { label = m.PurchaseTaxCode + " - " + m.Name, value = m.PurchaseTaxCode, id = m.PurchaseTaxID, name = m.Name })
                                                        .Skip(paging.Skip)
                            .Take(paging.PageSize).ToList();
                    break;
                case "SalesTaxCode":
                    paging.SetPaging(db.AppSalesTaxTables.Where(m => (m.SalesTaxCode).Contains(keyword)).Count());
                    rows = (from m in db.AppSalesTaxTables
                            where (m.SalesTaxCode).Contains(keyword)
                            orderby m.SalesTaxCode
                            select new { label = m.SalesTaxCode + " - " + m.Name, value = m.SalesTaxCode, id = m.SalesTaxID, name = m.Name })
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
            Models.WebAppAccEntities db = new Models.WebAppAccEntities(Services.GlobalVariant.GetConnection());
            object results = null;

            switch (fieldname)
            {
                case "ExObject01Code":
                    var exobject01code = db.Database.SqlQuery<Models.AppExObject01View>("Select * from AppExObject01View Where ExObject01Code={0}", keyword).SingleOrDefault();
                    results = (new { rows = exobject01code });
                    break;
                case "ExObject02Code":
                    var exobject02code = db.Database.SqlQuery<Models.AppExObject02View>("Select * from AppExObject02View Where ExObject02Code={0}", keyword).SingleOrDefault();
                    results = (new { rows = exobject02code });
                    break;
                case "ExObject03Code":
                    var exobject03code = db.Database.SqlQuery<Models.AppExObject03View>("Select * from AppExObject03View Where ExObject03Code={0}", keyword).SingleOrDefault();
                    results = (new { rows = exobject03code });
                    break;
                case "ExObject04Code":
                    var exobject04code = db.Database.SqlQuery<Models.AppExObject04View>("Select * from AppExObject04View Where ExObject04Code={0}", keyword).SingleOrDefault();
                    results = (new { rows = exobject04code });
                    break;
                case "ExObject05Code":
                    var exobject05code = db.Database.SqlQuery<Models.AppExObject05View>("Select * from AppExObject05View Where ExObject05Code={0}", keyword).SingleOrDefault();
                    results = (new { rows = exobject05code });
                    break;
                case "ExObject06Code":
                    var exobject06code = db.Database.SqlQuery<Models.AppExObject06View>("Select * from AppExObject06View Where ExObject06Code={0}", keyword).SingleOrDefault();
                    results = (new { rows = exobject06code });
                    break;
                case "ExObject07Code":
                    var exobject07code = db.Database.SqlQuery<Models.AppExObject07View>("Select * from AppExObject07View Where ExObject07Code={0}", keyword).SingleOrDefault();
                    results = (new { rows = exobject07code });
                    break;
                case "ExObject08Code":
                    var exobject08code = db.Database.SqlQuery<Models.AppExObject08View>("Select * from AppExObject08View Where ExObject08Code={0}", keyword).SingleOrDefault();
                    results = (new { rows = exobject08code });
                    break;
                case "ExObject09Code":
                    var exobject09code = db.Database.SqlQuery<Models.AppExObject09View>("Select * from AppExObject09View Where ExObject09Code={0}", keyword).SingleOrDefault();
                    results = (new { rows = exobject09code });
                    break;
                case "ExObject10Code":
                    var exobject10code = db.Database.SqlQuery<Models.AppExObject10View>("Select * from AppExObject10View Where ExObject10Code={0}", keyword).SingleOrDefault();
                    results = (new { rows = exobject10code });
                    break;
                case "ExObject11Code":
                    var exobject11code = db.Database.SqlQuery<Models.AppExObject11View>("Select * from AppExObject11View Where ExObject11Code={0}", keyword).SingleOrDefault();
                    results = (new { rows = exobject11code });
                    break;
                case "ExObject12Code":
                    var exobject12code = db.Database.SqlQuery<Models.AppExObject12View>("Select * from AppExObject12View Where ExObject12Code={0}", keyword).SingleOrDefault();
                    results = (new { rows = exobject12code });
                    break;
                case "ExObject13Code":
                    var exobject13code = db.Database.SqlQuery<Models.AppExObject13View>("Select * from AppExObject13View Where ExObject13Code={0}", keyword).SingleOrDefault();
                    results = (new { rows = exobject13code });
                    break;
                case "ExObject14Code":
                    var exobject14code = db.Database.SqlQuery<Models.AppExObject14View>("Select * from AppExObject14View Where ExObject14Code={0}", keyword).SingleOrDefault();
                    results = (new { rows = exobject14code });
                    break;
                case "ExObject15Code":
                    var exobject15code = db.Database.SqlQuery<Models.AppExObject15View>("Select * from AppExObject15View Where ExObject15Code={0}", keyword).SingleOrDefault();
                    results = (new { rows = exobject15code });
                    break;
                case "ExObject16Code":
                    var exobject16code = db.Database.SqlQuery<Models.AppExObject16View>("Select * from AppExObject16View Where ExObject16Code={0}", keyword).SingleOrDefault();
                    results = (new { rows = exobject16code });
                    break;
                case "ExObject17Code":
                    var exobject17code = db.Database.SqlQuery<Models.AppExObject17View>("Select * from AppExObject17View Where ExObject17Code={0}", keyword).SingleOrDefault();
                    results = (new { rows = exobject17code });
                    break;
                case "ExObject18Code":
                    var exobject18code = db.Database.SqlQuery<Models.AppExObject18View>("Select * from AppExObject18View Where ExObject18Code={0}", keyword).SingleOrDefault();
                    results = (new { rows = exobject18code });
                    break;
                case "ExObject19Code":
                    var exobject19code = db.Database.SqlQuery<Models.AppExObject19View>("Select * from AppExObject19View Where ExObject19Code={0}", keyword).SingleOrDefault();
                    results = (new { rows = exobject19code });
                    break;
                case "ExObject20Code":
                    var exobject20code = db.Database.SqlQuery<Models.AppExObject20View>("Select * from AppExObject20View Where ExObject20Code={0}", keyword).SingleOrDefault();
                    results = (new { rows = exobject20code });
                    break;

                case "FixedAssetGroupCode":
                    var fixedassetgroupcode = db.Database.SqlQuery<Models.AppFixedAssetGroupTable>("Select * from AppFixedAssetGroupTable Where FixedAssetGroupCode={0}", keyword).SingleOrDefault();
                    results = (new { rows = fixedassetgroupcode });
                    break;

                case "FixedAssetTypeCode":
                    var fixedassettypecode = db.Database.SqlQuery<Models.AppFixedAssetTypeTable>("Select * from AppFixedAssetTypeTable Where FixedAssetTypeCode={0}", keyword).SingleOrDefault();
                    results = (new { rows = fixedassettypecode });
                    break;

                case "DocumentVATTypeName":
                    var documentvattypename = db.Database.SqlQuery<Models.AppDocumentVATTypeTable>("Select * from AppDocumentVATTypeTable Where EnumName={0}", keyword).SingleOrDefault();
                    results = (new { rows = documentvattypename });
                    break;

                case "SalesPriceGroupCode":
                    var salespricegroupcode = db.Database.SqlQuery<Models.AppSalesPriceGroupTable>("Select * from AppSalesPriceGroupTable Where SalesPriceGroupCode={0}", keyword).SingleOrDefault();
                    results = (new { rows = salespricegroupcode });
                    break;
                case "WarehouseLineCode":
                case "WarehouseCode":
                    var warehousecode = db.Database.SqlQuery<Models.AppWarehouseTable>("Select * from AppWarehouseTable Where WarehouseCode={0}", keyword).SingleOrDefault();
                    results = (new { rows = warehousecode });
                    break;
                case "IsoCode":
                    var isocode = db.Database.SqlQuery<Models.AppCurrencyTable>("Select * from AppCurrencyTable Where IsoCode={0}", keyword).SingleOrDefault();
                    results = (new { rows = isocode });
                    break;
                case "DisplayNumberLineCredit":
                case "DisplayNumberLineDebit":
                case "DisplayNumberLine1Debit":
                case "DisplayNumberLine1Credit":
                case "DisplayNumberCredit":
                case "DisplayNumberDebit":
                case "DisplayNumber":
                    var displaynumber = db.Database.SqlQuery<Models.AppAccountView2>("Select * from AppAccountView2 Where DisplayNumber={0}", keyword).SingleOrDefault();
                    results = (new { rows = displaynumber });
                    break;
                case "ParentDisplayNumber":
                    var parentdisplaynumber = db.Database.SqlQuery<Models.AppAccountView>("Select * from AppAccountView Where DisplayNumber={0}", keyword).SingleOrDefault();
                    results = (new { rows = parentdisplaynumber });
                    break;
                case "CustomerCode":
                    var customercode = db.Database.SqlQuery<Models.AppCustomerView>("Select * from AppCustomerView Where CustomerCode={0}", keyword).SingleOrDefault();
                    results = (new { rows = customercode });
                    break;
                case "CustomerGroupCode":
                    var customergroupcode = db.Database.SqlQuery<Models.AppCustomerGroupView>("Select * from AppCustomerGroupView Where CustomerGroupCode={0}", keyword).SingleOrDefault();
                    results = (new { rows = customergroupcode });
                    break;

                case "PurchaseTaxCode":
                    var purchasetaxcode = db.Database.SqlQuery<Models.AppPurchaseTaxView>("Select * from AppPurchaseTaxView Where PurchaseTaxCode={0}", keyword).SingleOrDefault();
                    results = (new { rows = purchasetaxcode });
                    break;

                case "SalesTaxCode":
                    var salestaxcode = db.Database.SqlQuery<Models.AppSalesTaxView>("Select * from AppSalesTaxView Where SalesTaxCode={0}", keyword).SingleOrDefault();
                    results = (new { rows = salestaxcode });
                    break;

                case "ItemGroupCode":
                    var itemgroupcode = db.Database.SqlQuery<Models.AppItemGroupTable>("Select * from AppItemGroupTable Where ItemGroupCode={0}", keyword).SingleOrDefault();
                    results = (new { rows = itemgroupcode });
                    break;
                case "ProductCode":
                case "ItemCode":
                    var itemcode = db.Database.SqlQuery<Models.AppItemView>("Select * from AppItemView Where ItemCode={0}", keyword).SingleOrDefault();
                    results = (new { rows = itemcode });
                    break;
                case "UOMLinkCode":
                case "UOMCode":
                    var uomcode = db.Database.SqlQuery<Models.AppUnitOfMeasureTable>("Select * from AppUnitOfMeasureTable Where UOMCode={0}", keyword).SingleOrDefault();
                    results = (new { rows = uomcode });
                    break;
                case "ItemTypeName":
                    var itemtypename = db.Database.SqlQuery<Models.AppItemTypeTable>("Select * from AppItemTypeTable Where EnumName={0}", keyword).SingleOrDefault();
                    results = (new { rows = itemtypename });
                    break;
                case "ItemMethodTypeName":
                    var itemmethodtypename = db.Database.SqlQuery<Models.AppItemMethodTypeTable>("Select * from AppItemMethodTypeTable Where EnumName={0}", keyword).SingleOrDefault();
                    results = (new { rows = itemmethodtypename });
                    break;
                case "ExpenseCode":
                    var expensecode = db.Database.SqlQuery<Models.AppExpenseTable>("Select * from AppExpenseTable Where ExpenseCode={0}", keyword).SingleOrDefault();
                    results = (new { rows = expensecode });
                    break;
                case "DepartmentCode":
                    var departmentcode = db.Database.SqlQuery<Models.AppDepartmentTable>("Select * from AppDepartmentTable Where DepartmentCode={0}", keyword).SingleOrDefault();
                    results = (new { rows = departmentcode });
                    break;
                case "RoleName":
                    var rolename = db.Database.SqlQuery<Models.SysRole>("Select * from SysRole Where Name={0}", keyword).SingleOrDefault();
                    results = (new { rows = rolename });
                    break;
                case "BusinessCode":
                    var businesscode = db.Database.SqlQuery<Models.SysBusiness>("Select * from SysBusiness Where BusinessCode={0}", keyword).SingleOrDefault();
                    results = (new { rows = businesscode });
                    break;
                case "EmployeeCode":
                    var employeecode = db.Database.SqlQuery<Models.AppEmployeeTable>("Select * from AppEmployeeTable Where EmployeeCode={0}", keyword).SingleOrDefault();
                    results = (new { rows = employeecode });
                    break;
                case "ConstructionCode":
                    var Constructioncode = db.Database.SqlQuery<Models.AppConstructionTable>("Select * from AppConstructionTable Where ConstructionCode={0}", keyword).SingleOrDefault();
                    results = (new { rows = Constructioncode });
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