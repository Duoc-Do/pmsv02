using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using WebApp.Areas.Accounting.Services;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class AppUnitOfMeasureRateTable : SenVietListObject
    {
        public AppUnitOfMeasureRateTable(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "AppUnitOfMeasureRateTable";
            this._metaname = "AppUnitOfMeasureRateView";
            this._keyfield = "UnitOfMeasureRateID";
            this._storeNameI = @"sp_AppUnitOfMeasureRateTableI @UnitOfMeasureRateID OUTPUT,@UOMID,@UOMLinkID,@Value,@IsActive,@CreatedBy,@CreatedDateTime,@ModifiedBy,@ModifiedDateTime";
            this._storeNameU = @"sp_AppUnitOfMeasureRateTableU @UnitOfMeasureRateID OUTPUT,@UOMID,@UOMLinkID,@Value,@IsActive,@CreatedBy,@CreatedDateTime,@ModifiedBy,@ModifiedDateTime,@Original_UnitOfMeasureRateID";
            this._storeNameD = @"sp_AppUnitOfMeasureRateTableD {0}";
            base.InitObject();
        }

        public List<Models.AppUnitOfMeasureRateView> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.AppUnitOfMeasureRateViews);
            return model;
        }

        public Models.AppUnitOfMeasureRateView GetById(int Id)
        {
            return this._db.AppUnitOfMeasureRateViews.SingleOrDefault(m => m.UnitOfMeasureRateID == Id);
        }

        public Models.AppUnitOfMeasureRateView GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.AppUnitOfMeasureRateView() { IsActive = true };
        }

        public Models.AppUnitOfMeasureRateView GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.AppUnitOfMeasureRateView GetDelete(int id)
        {
            return GetById(id);
        }

        public int Insert(Models.AppUnitOfMeasureRateView data)
        {
            try
            {
                this.Validate(data);

                data.CreatedBy = GlobalVariant.GetAppUser().UserID;
                data.CreatedDateTime = DateTime.Now;

                SqlParameter[] parameters = ExConvert.Data2SqlParam(data, this._metaobject, this._paramnameoutput).ToArray();
                this._db.Database.ExecuteSqlCommand(this._storeNameI, parameters);

                return (int)parameters.GetValueSqlParam(this._paramnameoutput);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.AppUnitOfMeasureRateView data)
        {
            try
            {
                this.Validate(data);
                //data.UOMLinkCode
                //data.UOMLinkID    
                data.ModifiedBy = GlobalVariant.GetAppUser().UserID;
                data.ModifiedDateTime = DateTime.Now;

                SqlParameter pOriginal = ExConvert.ParseSqlParam(data, this._metaobject.GetMetaByColumnName(this._paramnameoutput), this._paramnameupdate);
                var parameters = ExConvert.Data2SqlParam(data, this._metaobject, pOriginal).ToArray();
                this._db.Database.ExecuteSqlCommand(this._storeNameU, parameters);

                return data.UnitOfMeasureRateID;
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
                this._db.Database.ExecuteSqlCommand(this._storeNameD, Id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region phần  import/export excel

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

        public override void MapCode2Id(dynamic data)
        {
            var metatable = this._metaobject.MetaTable;
            foreach (var item in metatable)
            {
                string fieldmap = string.Format("{0}", item.ColumnName.ToString());
                var value = data.GetType().GetProperty(fieldmap).GetValue(data, null);
                string _value = "";
                if (fieldmap == "UOMCode" && !string.IsNullOrEmpty(value))
                {
                    _value = value.ToString();
                    var UOMID = this._db.AppUnitOfMeasureTables.Where(m => m.UOMCode == _value).SingleOrDefault();
                    if (UOMID != null)
                    {
                        data.GetType().GetProperty("UOMID").SetValue(data, UOMID.UOMID, null);
                    }
                }

                if (fieldmap == "UOMLinkCode" && !string.IsNullOrEmpty(value))
                {
                    _value = value.ToString();
                    var UOMLinkID = this._db.AppUnitOfMeasureTables.Where(m => m.UOMCode == _value).SingleOrDefault();
                    if (UOMLinkID != null)
                    {
                        data.GetType().GetProperty("UOMLinkID").SetValue(data, UOMLinkID.UOMID, null);
                    }
                }
            }

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

        #endregion

    }
}