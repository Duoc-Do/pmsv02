using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebApp.Areas.Accounting.Services;

namespace WebApp.Areas.Accounting.DAL
{

    public partial class AppVoucherTable
    {
        private Models.WebAppAccEntities _db;
        private HttpRequestBase _request;
        private Models.MetaObject _metaobject;
        public Dictionary<string, string> Errors = new Dictionary<string, string>();

        private string _metaname = "AppVoucherTable";
        private string _paramnameoutput = "VoucherID";
        private string _paramnameupdate = "Original_VoucherID";

        private string _storeNameI = @"sp_AppVoucherTableI @VoucherID OUTPUT,@VoucherCode,@VoucherName,@AlphaPart,@IntegerPart,@PostStoreProcedure,@BusinessCode,@CreatedBy,@CreatedDateTime,@ModifiedBy,@ModifiedDateTime";
        private string _storeNameU = @"sp_AppVoucherTableU @VoucherID OUTPUT,@VoucherCode,@VoucherName,@AlphaPart,@IntegerPart,@PostStoreProcedure,@BusinessCode,@CreatedBy,@CreatedDateTime,@ModifiedBy,@ModifiedDateTime,@Original_VoucherID";
        private string _storeNameD = @"sp_AppVoucherTableD {0}";

        public AppVoucherTable(HttpRequestBase request)
        {
            this._db = new Models.WebAppAccEntities(Services.GlobalVariant.GetConnection()); ;
            this._request = request;
            this._metaobject = Services.GlobalMeta.GetMetaObject(this._metaname);

        }

        public object AutoComplete()
        {
            return Data.GetByKeyword(this._request);
        }

        public object FieldChange()
        {
            return Services.Data.GetByCode(this._request);

        }

        public void Validate(dynamic data)
        {


            #region Business logic validate
            #region Object validate

            foreach (var item in this._metaobject.MetaTable)
            {
                string fieldmap = string.Format("{0}", item.ColumnName.ToString());
                if (item.AllowDBNull != true)
                {
                    var value = data.GetType().GetProperty(fieldmap).GetValue(data, null) ?? String.Empty;

                    if (string.IsNullOrEmpty(value.ToString()))
                    {
                        Errors.Add(fieldmap, string.Format("phải nhập {0}", item.Des.ToLower()));
                    }
                }
            }
            #endregion

            #endregion

            if (Errors.Count > 0) throw new InvalidOperationException("lỗi nhập số liệu");

        }

        public List<Models.AppVoucherTable> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.AppVoucherTables);
            return model;
        }

        public Models.AppVoucherTable GetById(int Id)
        {
            return this._db.AppVoucherTables.SingleOrDefault(m => m.VoucherID == Id);
        }

        public Models.AppVoucherTable GetNew(int? id)
        {
            //kiểm tra quyền nếu không cho thì ném exception
            if (id != null) return GetById(id ?? 0);
            return new Models.AppVoucherTable();
        }

        public Models.AppVoucherTable GetEdit(int id)
        {
            //kiểm tra quyền nếu không cho thì ném exception
            return GetById(id);
        }

        public Models.AppVoucherTable GetDelete(int id)
        {
            //kiểm tra quyền nếu không cho thì ném exception
            return GetById(id);
        }

        public int Insert(Models.AppVoucherTable data)
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

        public int Update(Models.AppVoucherTable data)
        {
            try
            {
                this.Validate(data);

                data.ModifiedBy = GlobalVariant.GetAppUser().UserID;
                data.ModifiedDateTime = DateTime.Now;

                SqlParameter pOriginal = ExConvert.ParseSqlParam(data, this._metaobject.GetMetaByColumnName(this._paramnameoutput), this._paramnameupdate);
                var parameters = ExConvert.Data2SqlParam(data, this._metaobject, pOriginal).ToArray();
                this._db.Database.ExecuteSqlCommand(this._storeNameU, parameters);

                return data.VoucherID;
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

        //public override byte[] ExportToExcel()
        //{
        //    byte[] bytes = null;
        //    using (var stream = new MemoryStream())
        //    {
        //        Export.ExportToXlsx(stream, this.GetData(), this._metaobject.GetMetaTable());
        //        bytes = stream.ToArray();
        //    }
        //    return bytes;
        //}

        //public override void ImportFromExcel(Stream stream)
        //{

        //    var metatable = this._metaobject.MetaTable;//lấy meta của bảng gốc

        //    // ok, we can run the real code of the sample now
        //    using (var xlPackage = new ExcelPackage(stream))
        //    {
        //        // get the first worksheet in the workbook
        //        var worksheet = xlPackage.Workbook.Worksheets.FirstOrDefault();
        //        if (worksheet == null)
        //            throw new Exception("No worksheet found");

        //        //the columns
        //        Dictionary<int, string> properties = new Dictionary<int, string>();

        //        int iCol = 1;
        //        #region lấy header column
        //        while (true)
        //        {
        //            bool allColumnsAreEmpty = true;
        //            if (worksheet.Cells[1, iCol].Value != null && !String.IsNullOrEmpty(worksheet.Cells[1, iCol].Value.ToString()))
        //            {
        //                properties.Add(iCol, worksheet.Cells[1, iCol].Value.ToString());
        //                allColumnsAreEmpty = false;
        //            }
        //            if (allColumnsAreEmpty) break;
        //            iCol++;
        //        }
        //        #endregion


        //        int iRow = 2;
        //        while (true)
        //        {
        //            var newitem = this.GetNew(null);
        //            int allColumnsAreEmpty = properties.Count;
        //            for (var i = 1; i <= properties.Count; i++)
        //            {
        //                if (worksheet.Cells[iRow, i].Value != null && !String.IsNullOrEmpty(worksheet.Cells[iRow, i].Value.ToString()))
        //                {
        //                    string fieldmap = properties[i];
        //                    var value = worksheet.Cells[iRow, i].Value.ToString();
        //                    var itemfield = metatable.Find(m => m.ColumnName == fieldmap || m.Des == fieldmap);
        //                    if (itemfield != null)
        //                    {
        //                        var objvalue = Services.ExConvert.String2Data(value, Services.ExConvert.Sqltype2Systemtype(itemfield.DATA_TYPE));
        //                        newitem.GetType().GetProperty(itemfield.ColumnName).SetValue(newitem, objvalue, null);
        //                    }
        //                }
        //                else
        //                {
        //                    allColumnsAreEmpty--;
        //                }
        //            }
        //            try
        //            {
        //                this.MapCode2Id(newitem);
        //                this.Insert(newitem);
        //            }
        //            catch (Exception)
        //            {
        //            }

        //            if (allColumnsAreEmpty <= 0) break;

        //            //next object
        //            iRow++;
        //        }
        //    }

        //}

    }

}