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
    public partial class ResTable : SenVietListObject
    {
        public ResTable(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "ResTable";
            this._metaname = "ResTableView";
            this._keyfield = "TableId";
            this._storeNameI = @"sp_ResTableI @TableId OUTPUT,@Name,@IsActive,@WarehouseId,@CreatedBy,@CreatedDateTime,@ModifiedBy,@ModifiedDateTime,@EmployeeId";
            this._storeNameU = @"sp_ResTableU @TableId OUTPUT,@Name,@IsActive,@WarehouseId,@CreatedBy,@CreatedDateTime,@ModifiedBy,@ModifiedDateTime,@EmployeeId,@Original_TableId";
            this._storeNameD = @"sp_ResTableD {0}";
            base.InitObject();
        }


        public List<Models.ResTableView> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.ResTableViews);
            return model;
        }

        public Models.ResTableView GetById(int Id)
        {
            return this._db.ResTableViews.SingleOrDefault(m => m.TableId == Id);
        }

        public Models.ResTableView GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.ResTableView() { IsActive=true};
        }

        public Models.ResTableView GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.ResTableView GetDelete(int id)
        {
            return GetById(id);
        }
        public int Insert(Models.ResTableView data)
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

        public int Update(Models.ResTableView data)
        {
            try
            {
                this.Validate(data);

                data.ModifiedBy = GlobalVariant.GetAppUser().UserID;
                data.ModifiedDateTime = DateTime.Now;

                SqlParameter pOriginal = ExConvert.ParseSqlParam(data, this._metaobject.GetMetaByColumnName(this._paramnameoutput), this._paramnameupdate);
                var parameters = ExConvert.Data2SqlParam(data, this._metaobject, pOriginal).ToArray();
                this._db.Database.ExecuteSqlCommand(this._storeNameU, parameters);

                return data.TableId;
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

        public override void MapCode2Id(dynamic data)
        {
            var metatable = this._metaobject.MetaTable;
            foreach (var item in metatable)
            {
                string fieldmap = string.Format("{0}", item.ColumnName.ToString());
                var value = data.GetType().GetProperty(fieldmap).GetValue(data, null);
                string _value = "";
                if (fieldmap == "WarehouseCode" && !string.IsNullOrEmpty(value))
                {
                    _value = value.ToString();
                    var WarehouseCode = this._db.AppWarehouseTables.Where(m => m.WarehouseCode == _value).SingleOrDefault();
                    if (WarehouseCode != null)
                    {
                        data.GetType().GetProperty("WarehouseId").SetValue(data, WarehouseCode.WarehouseID, null);
                    }
                }

              
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

    }
}