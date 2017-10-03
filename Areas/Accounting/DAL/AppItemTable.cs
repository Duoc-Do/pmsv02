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
    public partial class AppItemTable : SenVietListObject
    {
        public AppItemTable(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {
            this._businesscode = "AppItemTable";
            this._metaname = "AppItemView";
            this._keyfield = "ItemID";
            this._storeNameI = @"sp_AppItemTableI @ItemID OUTPUT,@ItemCode,@Name,@ItemGroupID,@UOMID,@ItemMethodType,@ItemType,@IsInventory,@IsActive,@CreatedBy,@CreatedDateTime,@ModifiedBy,@ModifiedDateTime,@AccountID,@AccountCreditID,@AccountDebitID,@QuantityMin,@QuantityMax,@Note,@Cost,@Price";
            this._storeNameU = @"sp_AppItemTableU @ItemID OUTPUT,@ItemCode,@Name,@ItemGroupID,@UOMID,@ItemMethodType,@ItemType,@IsInventory,@IsActive,@CreatedBy,@CreatedDateTime,@ModifiedBy,@ModifiedDateTime,@AccountID,@AccountCreditID,@AccountDebitID,@QuantityMin,@QuantityMax,@Note,@Cost,@Price,@Original_ItemID";
            this._storeNameD = @"sp_AppItemTableD {0}";
            base.InitObject();
        }

        public void UpdateExProperty(Models.AppItemView data)
        {

            this.SetExProperty(data.ItemID, "ItemPicture", data.ItemPicture);
            this.SetExProperty(data.ItemID, "CurrentQuantity", data.CurrentQuantity.HasValue? data.CurrentQuantity.GetValueOrDefault().ToString():null);
            this.SetExProperty(data.ItemID, "IsolationDay", data.IsolationDay.HasValue? data.IsolationDay.GetValueOrDefault().ToString():null);


        }
        public void MapExProperty(Models.AppItemView data)
        {

            data.ItemPicture = this.GetExProperty(data.ItemID, "ItemPicture");
            string currentquantity = this.GetExProperty(data.ItemID, "CurrentQuantity");
            if (!string.IsNullOrEmpty(currentquantity)){data.CurrentQuantity = Convert.ToDecimal(currentquantity);}
            string isolationday = this.GetExProperty(data.ItemID, "IsolationDay");
            if (!string.IsNullOrEmpty(isolationday)){data.IsolationDay = Convert.ToInt16(isolationday);}
        }
        public void MapExProperty(Models.AppItemTable data)
        {
            data.ItemPicture = this.GetExProperty(data.ItemID, "ItemPicture");
            string currentquantity = this.GetExProperty(data.ItemID, "CurrentQuantity");
            if (!string.IsNullOrEmpty(currentquantity)) { data.CurrentQuantity = Convert.ToDecimal(currentquantity); }
            string isolationday = this.GetExProperty(data.ItemID, "IsolationDay");
            if (!string.IsNullOrEmpty(isolationday)) { data.IsolationDay = Convert.ToInt16(isolationday); }
        }



        private void SetExProperty(int id, string pname, string pvalue)
        {

            var edit = this._db.AppItemExTables.Where(m => m.ItemID == id && m.PropertyName == pname).SingleOrDefault();
            if (edit == null)
            {
                if (!string.IsNullOrEmpty(pvalue))
                {
                    var add = new Models.AppItemExTable() { ItemID = id, PropertyName = pname, PropertyValue = pvalue ?? "", LastUpdatedDate = DateTime.Now };
                    this._db.AppItemExTables.Add(add);                    
                }
            }
            else
            {
                edit.PropertyValue = pvalue??"";
                edit.LastUpdatedDate = DateTime.Now;
                this._db.Entry(edit).State = System.Data.Entity.EntityState.Modified;
            }
            this._db.SaveChanges();
        }

        public string GetExProperty(int id, string pname)
        {

           var exproperty = this._db.AppItemExTables.Where(m => m.ItemID == id && m.PropertyName == pname).SingleOrDefault();
           if (exproperty != null)
            {
                return exproperty.PropertyValue;
            }

            return "";
        }

        public int GetIsolationDay(int id)
        {
            int i;
            Int32.TryParse(this.GetExProperty(id, "IsolationDay"), out i);
            return i;
        }

        


        public List<Models.AppItemView> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.AppItemViews);
            return model;
        }

        public List<Models.AppItemView> GetDataPos()
        {
            var query = this._db.AppItemViews.Where(m => (m.ItemType == 2 || m.ItemType==3));
            var model = Services.GridHelper.GetResults(this._request, this._metaname,query);
            return model;
        }

        public List<Models.AppItemView> GetDataByIds(int[] ids)
        {
            var model = this._db.AppItemViews.Where(m =>ids.Contains(m.ItemID)).ToList();
            return model;
        }

        public Models.AppItemView GetById(int Id)
        {
            var model = this._db.AppItemViews.SingleOrDefault(m => m.ItemID == Id);
            if (model != null) this.MapExProperty(model);
            return model;
        }

        public Models.AppItemView GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.AppItemView() { 
                IsActive=true,
                IsInventory=true,
                ItemType=2,
                ItemTypeName="Hàng hóa",
                ItemMethodType=1,
                ItemMethodTypeName="Trung bình tháng"
            };
        }

        public Models.AppItemView GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.AppItemView GetDelete(int id)
        {
            return GetById(id);
        }
        public int Insert(Models.AppItemView data)
        {
            try
            {
                this.Validate(data);

                data.CreatedBy = GlobalVariant.GetAppUser().UserID;
                data.CreatedDateTime = DateTime.Now;

                SqlParameter[] parameters = ExConvert.Data2SqlParam(data, this._metaobject, this._paramnameoutput).ToArray();
                this._db.Database.ExecuteSqlCommand(this._storeNameI, parameters);

                data.ItemID = (int)parameters.GetValueSqlParam(this._paramnameoutput);
                this.UpdateExProperty(data);

                return (int)parameters.GetValueSqlParam(this._paramnameoutput);
            }
            catch ( Exception)
            {
                throw;
            }
        }

        public int Update(Models.AppItemView data)
        {
            try
            {
                this.Validate(data);

                data.ModifiedBy = GlobalVariant.GetAppUser().UserID;
                data.ModifiedDateTime = DateTime.Now;

                SqlParameter pOriginal = ExConvert.ParseSqlParam(data, this._metaobject.GetMetaByColumnName(this._paramnameoutput), this._paramnameupdate);
                var parameters = ExConvert.Data2SqlParam(data, this._metaobject, pOriginal).ToArray();
                this._db.Database.ExecuteSqlCommand(this._storeNameU, parameters);

                this.UpdateExProperty(data);
                return data.ItemID;
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
                if (fieldmap == "UOMCode" && !string.IsNullOrEmpty(value))
                {
                    _value = value.ToString();
                    var UOMID = this._db.AppUnitOfMeasureTables.Where(m => m.UOMCode == _value).SingleOrDefault();
                    if (UOMID != null)
                    {
                        data.GetType().GetProperty("UOMID").SetValue(data, UOMID.UOMID, null);
                    }
                }


            }

        }
    }
}