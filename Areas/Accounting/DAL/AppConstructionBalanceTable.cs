
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using WebApp.Areas.Accounting.Services;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class AppConstructionBalanceTable : SenVietListObject
    {
        public AppConstructionBalanceTable(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "AppConstructionBalanceTable";
            this._metaname = "AppConstructionBalanceView";
            this._keyfield = "ConstructionBalanceID";
            base.InitObject();
        }

        public List<Models.AppConstructionBalanceView> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.AppConstructionBalanceViews);
            return model;
        }

        public Models.AppConstructionBalanceView GetById(int Id)
        {
            return this._db.AppConstructionBalanceViews.SingleOrDefault(m => m.ConstructionBalanceID == Id);
        }
        public Models.AppConstructionBalanceTable GetById2(int Id)
        {
            return this._db.AppConstructionBalanceTables.SingleOrDefault(m => m.ConstructionBalanceID == Id);
        }

        public Models.AppConstructionBalanceView GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.AppConstructionBalanceView();
        }

        public Models.AppConstructionBalanceView GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.AppConstructionBalanceView GetDelete(int id)
        {
            return GetById(id);
        }
        public int Insert(Models.AppConstructionBalanceView data)
        {
            try
            {
                this.Validate(data);
                var _data = new Models.AppConstructionBalanceTable();
                this.MapView2Table(data, _data);
                this._db.AppConstructionBalanceTables.Add(_data);
                this._db.SaveChanges();
                return data.ConstructionBalanceID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.AppConstructionBalanceView data)
        {
            try
            {
                this.Validate(data);
                var _data = this.GetById2(data.ConstructionBalanceID);

                this.MapView2Table(data, _data);
                this._db.Entry(_data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                return data.ConstructionBalanceID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(int Id)
        {
            try
            {
                var _rec = GetById2(Id);
                this._db.Entry(_rec).State = System.Data.Entity.EntityState.Deleted;
                this._db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override byte[] ExportToExcel()
        {
            byte[] bytes = null;
            using (var stream = new MemoryStream())
            {
                Export.ExportToXlsx(stream, this.GetData(), this._metaobject.GetMetaTable());
                bytes = stream.ToArray();
            }
            return bytes;
        }

        public override void ImportFromExcel(Stream stream)
        {

            var metatable = this._metaobject.MetaTable;//lấy meta của bảng gốc

            // ok, we can run the real code of the sample now
            using (var xlPackage = new ExcelPackage(stream))
            {
                // get the first worksheet in the workbook
                var worksheet = xlPackage.Workbook.Worksheets.FirstOrDefault();
                if (worksheet == null)
                    throw new Exception("No worksheet found");

                //the columns
                Dictionary<int, string> properties = new Dictionary<int, string>();

                int iCol = 1;
                #region lấy header column
                while (true)
                {
                    bool allColumnsAreEmpty = true;
                    if (worksheet.Cells[1, iCol].Value != null && !String.IsNullOrEmpty(worksheet.Cells[1, iCol].Value.ToString()))
                    {
                        properties.Add(iCol, worksheet.Cells[1, iCol].Value.ToString());
                        allColumnsAreEmpty = false;
                    }
                    if (allColumnsAreEmpty) break;
                    iCol++;
                }
                #endregion


                int iRow = 2;
                while (true)
                {
                    var newitem = this.GetNew(null);
                    int allColumnsAreEmpty = properties.Count;
                    for (var i = 1; i <= properties.Count; i++)
                    {
                        if (worksheet.Cells[iRow, i].Value != null && !String.IsNullOrEmpty(worksheet.Cells[iRow, i].Value.ToString()))
                        {
                            string fieldmap = properties[i];
                            var value = worksheet.Cells[iRow, i].Value.ToString();
                            var itemfield = metatable.Find(m => m.ColumnName == fieldmap || m.Des == fieldmap);
                            if (itemfield != null)
                            {
                                var objvalue = Services.ExConvert.String2Data(value, Services.ExConvert.Sqltype2Systemtype(itemfield.DATA_TYPE));
                                newitem.GetType().GetProperty(itemfield.ColumnName).SetValue(newitem, objvalue, null);
                            }
                        }
                        else
                        {
                            allColumnsAreEmpty--;
                        }
                    }
                    try
                    {
                        this.MapCode2Id(newitem);
                        this.Insert(newitem);
                    }
                    catch (Exception)
                    {
                    }

                    if (allColumnsAreEmpty <= 0) break;

                    //next object
                    iRow++;
                }
            }

        }

        public override void MapCode2Id(dynamic data)
        {
            var metatable = this._metaobject.MetaTable;
            foreach (var item in metatable)
            {
                string fieldmap = string.Format("{0}", item.ColumnName.ToString());
                var value = data.GetType().GetProperty(fieldmap).GetValue(data, null);
                string _value = "";

                if (fieldmap == "ConstructionCode" && !string.IsNullOrEmpty(value))
                {
                    _value = value.ToString();
                    var ConstructionID = this._db.AppConstructionTables.Where(m => m.ConstructionCode == _value).SingleOrDefault();
                    if (ConstructionID != null)
                    {
                        data.GetType().GetProperty("ConstructionID").SetValue(data, ConstructionID.ConstructionID, null);
                    }
                }

                if (fieldmap == "CustomerCode" && !string.IsNullOrEmpty(value))
                {
                    _value = value.ToString();
                    var CustomerID = this._db.AppCustomerTables.Where(m => m.CustomerCode == _value).SingleOrDefault();
                    if (CustomerID != null)
                    {
                        data.GetType().GetProperty("CustomerID").SetValue(data, CustomerID.CustomerID, null);
                    }
                }
                if (fieldmap == "DisplayNumber" && !string.IsNullOrEmpty(value))
                {
                    _value = value.ToString();
                    var AccountID = this._db.AppAccountTables.Where(m => m.DisplayNumber == _value).SingleOrDefault();
                    if (AccountID != null)
                    {
                        data.GetType().GetProperty("AccountID").SetValue(data, AccountID.AccountID, null);
                    }
                }

            }

        }
    }
}