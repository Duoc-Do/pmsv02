using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using WebApp.Areas.Accounting.Services;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class AppFixedAssetTable : SenVietListObject
    {
        #region Tham số
        public string _paramnamemasteroutput;
        public string _paramnamemasterupdate;

        public string _paramnamelineoutput;
        public string _paramnamelineupdate;

        public string _paramnamevatoutput;
        public string _paramnamevatupdate;



        public string _keyfieldmaster;
        public string _keyfieldline;
        public string _keyfieldvat;



        public string _metalinename;
        public string _metavatname;


        public string _storeNameLineI;
        public string _storeNameLineU;
        public string _storeNameLineD;

        public string _storeNameVATI;
        public string _storeNameVATU;
        public string _storeNameVATD;

        #endregion

        public AppFixedAssetTable(HttpRequestBase request) : base(request) { }

        public override void InitObject()
        {

            this._businesscode = "AppFixedAssetTable";
            this._keyfield = "FixedAssetID";


            this._keyfieldmaster = "FixedAssetID";
            this._keyfieldline = "FixedAssetLineID";
            this._keyfieldvat = "EquipmentID";

            this._metaname = "AppFixedAssetView";
            this._metalinename = "AppFixedAssetLineTable";
            this._metavatname = "AppEquipmentTable";


            this._storeNameI = @"sp_AppFixedAssetTableI @FixedAssetID OUTPUT,@FixedAssetCode,@Name,@FixedAssetTypeID,@FixedAssetGroupID,@DepartmentID,@VoucherNumberDecrease,@VoucherDateDecrease,@DescriptionDecrease,@FixedAssetNumber,@MadeIn,@YearProduction,@YearUse,@Power,@SuspensionDate,@DescriptionSuspension,@Note,@PeriodOfDepreciation,@AccountID,@AccountCreditID,@AccountDebitID,@IsActive,@CreatedBy,@CreatedDateTime,@ModifiedBy,@ModifiedDateTime";
            this._storeNameU = @"sp_AppFixedAssetTableU @FixedAssetID OUTPUT,@FixedAssetCode,@Name,@FixedAssetTypeID,@FixedAssetGroupID,@DepartmentID,@VoucherNumberDecrease,@VoucherDateDecrease,@DescriptionDecrease,@FixedAssetNumber,@MadeIn,@YearProduction,@YearUse,@Power,@SuspensionDate,@DescriptionSuspension,@Note,@PeriodOfDepreciation,@AccountID,@AccountCreditID,@AccountDebitID,@IsActive,@CreatedBy,@CreatedDateTime,@ModifiedBy,@ModifiedDateTime,@Original_FixedAssetID";
            this._storeNameD = @"sp_AppFixedAssetTableD {0}";

            this._storeNameLineI = @"sp_AppFixedAssetLineTableI @FixedAssetLineID OUTPUT,@FixedAssetID,@VoucherDate,@VoucherNumber,@Quantity,@Amount,@Amount2,@AmountEnd,@AmountDepreciation,@DescriptionLine";
            this._storeNameLineU = @"sp_AppFixedAssetLineTableU @FixedAssetLineID OUTPUT,@FixedAssetID,@VoucherDate,@VoucherNumber,@Quantity,@Amount,@Amount2,@AmountEnd,@AmountDepreciation,@DescriptionLine,@Original_FixedAssetLineID";
            this._storeNameLineD = @"sp_AppFixedAssetLineTableD {0}";

            this._storeNameVATI = @"sp_AppEquipmentTableI @EquipmentID OUTPUT,@FixedAssetID,@EquipmentName,@UOMCode,@Quantity,@Amount,@AmountFC";
            this._storeNameVATU = @"sp_AppEquipmentTableU @EquipmentID OUTPUT,@FixedAssetID,@EquipmentName,@UOMCode,@Quantity,@Amount,@AmountFC,@Original_EquipmentID";
            this._storeNameVATD = @"sp_AppEquipmentTableD {0}";


            this._paramnamemasteroutput = this._keyfieldmaster;
            this._paramnamemasterupdate = string.Format("Original_{0}", this._keyfieldmaster);

            this._paramnamelineoutput = this._keyfieldline;
            this._paramnamelineupdate = string.Format("Original_{0}", this._keyfieldline);

            this._paramnamevatoutput = this._keyfieldvat;
            this._paramnamevatupdate = string.Format("Original_{0}", this._keyfieldvat);


            base.InitObject();
        }

        public  void ValidateMaster<T>(T data)
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
                        this.Errors.Add(fieldmap, string.Format("phải nhập {0}", item.Des.ToLower()));
                    }
                }
            }
            #endregion

            #endregion

            if (this.Errors.Count > 0) throw new InvalidOperationException("lỗi nhập số liệu");

        }

        public  void ValidateLine<T>(ICollection<T> data, ICollection<int> dataz)
        {
            //nếu không có chi tiết thì không được lưu
            if (data == null)
            {
                this.Errors.Add(this._metalinename, "Phải nhập chi tiết");
            }
            else
            {
                var line = data.ToList();
                var linez = dataz.ToList();

                bool notline = true;//ngầm định chi tiết đã bị xóa hết
                for (int i = 0; i < line.Count; i++)
                {
                    var itemz = linez[i];
                    if (itemz != -1)
                    {
                        var dataline = line[i];
                        foreach (var item in this._metaobject.MetaTable)
                        {
                            string fieldmap = string.Format("{0}", item.ColumnName.ToString());
                            if (item.AllowDBNull != true)
                            {
                                var value = dataline.GetType().GetProperty(fieldmap).GetValue(dataline, null) ?? String.Empty;

                                if (string.IsNullOrEmpty(value.ToString()))
                                {
                                    string fielderror = string.Format("{0}s[{1}].{2}", this._metalinename, i, item.ColumnName);
                                    this.Errors.Add(fielderror, string.Format("phải nhập {0}", item.Des.ToLower()));
                                }
                            }
                        }
                        notline = false;
                    }

                }
                if (notline)
                {
                    this.Errors.Add(this._metalinename, "Phải nhập chi tiết");
                }
            }
        }

        public  void ValidateVAT<T>(ICollection<T> data, ICollection<int> dataz)
        {
            if (data != null)
            {
                var vat = data.ToList();
                var vatz = dataz.ToList();

                for (int i = 0; i < vat.Count; i++)
                {
                    var itemz = vatz[i];
                    if (itemz != -1)
                    {
                        var datavat = vat[i];
                        foreach (var item in this._metaobject.MetaTable)
                        {
                            string fieldmap = string.Format("{0}", item.ColumnName.ToString());
                            if (item.AllowDBNull != true)
                            {
                                var value = datavat.GetType().GetProperty(fieldmap).GetValue(datavat, null) ?? String.Empty;

                                if (string.IsNullOrEmpty(value.ToString()))
                                {
                                    string fielderror = string.Format("{0}s[{1}].{2}", this._metavatname, i, item.ColumnName);
                                    this.Errors.Add(fielderror, string.Format("phải nhập {0}", item.Des.ToLower()));
                                }
                            }
                        }
                    }

                }

            }
        }

        public void Validate(Models.AppFixedAssetView data)
        {
            #region kiểm tra chi tiết

            this._metaobject = Services.GlobalMeta.GetMetaObject(this._metalinename);
            this.ValidateLine(data.AppFixedAssetLineTables, data.AppFixedAssetLineTablez);

            #endregion

            #region kiểm tra thuế

            this._metaobject = Services.GlobalMeta.GetMetaObject(this._metavatname);
            this.ValidateVAT(data.AppEquipmentTables, data.AppEquipmentTablez);
            #endregion

            #region Business logic validate
            #region Object validate
            this._metaobject = Services.GlobalMeta.GetMetaObject(this._metaname);
            this.ValidateMaster(data);
            #endregion

            #endregion

            //if (Errors.Count > 0) throw new InvalidOperationException("lỗi nhập số liệu");

        }

        public List<Models.AppFixedAssetView> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, this._db.AppFixedAssetViews);
            return model;
        }

        public Models.AppFixedAssetView GetById(int Id)
        {
            return this._db.AppFixedAssetViews.SingleOrDefault(m => m.FixedAssetID == Id);
        }

        public Models.AppFixedAssetView GetNew(int? id)
        {
            if (id != null) return GetById(id ?? 0);
            return new Models.AppFixedAssetView() { IsActive=true};
        }

        public Models.AppFixedAssetView GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.AppFixedAssetView GetDelete(int id)
        {
            return GetById(id);
        }

        public int Insert(Models.AppFixedAssetView data)
        {
            try
            {
                this.Validate(data);

                data.CreatedBy = GlobalVariant.GetAppUser().UserID;
                data.CreatedDateTime = DateTime.Now;

                SqlParameter[] parameters = ExConvert.Data2SqlParam(data, this._metaobject, this._paramnameoutput).ToArray();
                this._db.Database.ExecuteSqlCommand(this._storeNameI, parameters);

                #region Xử lý line
                this._metaobject = Services.GlobalMeta.GetMetaObject(this._metalinename);

                var AppFixedAssetLineTables = data.AppFixedAssetLineTables.ToList();
                var AppFixedAssetLineTablez = data.AppFixedAssetLineTablez.ToList();
                for (int i = 0; i < AppFixedAssetLineTables.Count; i++)
                {
                    var itemz = AppFixedAssetLineTablez[i];
                    if (itemz != -1)
                    {
                        var item = AppFixedAssetLineTables[i];
                        item.FixedAssetID = data.FixedAssetID;
                        this.InsertLine(item);
                    }
                }

                #endregion

                #region Xử lý VAT
                if (data.AppEquipmentTables != null)
                {

                    this._metaobject = Services.GlobalMeta.GetMetaObject(this._metavatname);

                    var AppEquipmentTables = data.AppEquipmentTables.ToList();
                    var AppEquipmentTablez = data.AppEquipmentTablez.ToList();
                    for (int i = 0; i < AppEquipmentTables.Count; i++)
                    {

                        var itemz = AppEquipmentTablez[i];
                        if (itemz != -1)
                        {
                            var item = AppEquipmentTables[i];
                            item.FixedAssetID = data.FixedAssetID;
                            this.InsertVAT(item);
                        }
                    }
                }

                #endregion

                return (int)parameters.GetValueSqlParam(this._paramnameoutput);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int InsertLine(Models.AppFixedAssetLineTable data)
        {
            try
            {
                SqlParameter[] parameters = ExConvert.Data2SqlParam(data, this._metaobject, this._paramnamelineoutput).ToArray();
                this._db.Database.ExecuteSqlCommand(this._storeNameLineI, parameters);
                data.FixedAssetLineID = (int)parameters.GetValueSqlParam(this._paramnamelineoutput);

                return data.FixedAssetLineID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int InsertVAT(Models.AppEquipmentTable data)
        {
            try
            {
                SqlParameter[] parameters = ExConvert.Data2SqlParam(data, this._metaobject, this._paramnamevatoutput).ToArray();
                this._db.Database.ExecuteSqlCommand(this._storeNameVATI, parameters);
                data.EquipmentID = (int)parameters.GetValueSqlParam(this._paramnamevatoutput);

                return data.EquipmentID;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Update(Models.AppFixedAssetView data)
        {
            try
            {
                this.Validate(data);

                data.ModifiedBy = GlobalVariant.GetAppUser().UserID;
                data.ModifiedDateTime = DateTime.Now;

                SqlParameter pOriginal = ExConvert.ParseSqlParam(data, this._metaobject.GetMetaByColumnName(this._paramnameoutput), this._paramnameupdate);
                var parameters = ExConvert.Data2SqlParam(data, this._metaobject, pOriginal).ToArray();
                this._db.Database.ExecuteSqlCommand(this._storeNameU, parameters);

                #region Xử lý line
                this._metaobject = Services.GlobalMeta.GetMetaObject(this._metalinename);


                var AppFixedAssetLineTables = data.AppFixedAssetLineTables.ToList();
                var AppFixedAssetLineTablez = data.AppFixedAssetLineTablez.ToList();
                for (int i = 0; i < AppFixedAssetLineTables.Count; i++)
                {

                    var itemz = AppFixedAssetLineTablez[i];
                    var item = AppFixedAssetLineTables[i];

                    if (itemz != -1)
                    {
                        //có 2 trường hợp: 1 - thêm mới thường DocumentLineID==0, 2 - sửa dữ liệu cũ DocumentLineID<>0
                        item.FixedAssetID = data.FixedAssetID;

                        if (item.FixedAssetLineID == 0)
                        {
                            this.InsertLine(item);
                        }
                        else
                        {
                            this.UpdateLine(item);
                        }
                    }
                    else
                    {
                        //nếu xóa có 2 trường hợp : 1 - xóa dữ liệu có trước DocumentLineID<>0, 2 - xóa dữ liệu mới thêm DocumentLineID ==0
                        if (item.FixedAssetLineID > 0)
                        {
                            this.DeleteLine(item.FixedAssetLineID);
                        }
                    }
                }


                #endregion


                #region Xử lý thuế
                if (data.AppEquipmentTables != null)
                {


                    this._metaobject = Services.GlobalMeta.GetMetaObject(this._metavatname);


                    var AppEquipmentTables = data.AppEquipmentTables.ToList();
                    var AppEquipmentTablez = data.AppEquipmentTablez.ToList();
                    for (int i = 0; i < AppEquipmentTables.Count; i++)
                    {

                        var itemz = AppEquipmentTablez[i];
                        var item = AppEquipmentTables[i];

                        if (itemz != -1)
                        {
                            //có 2 trường hợp: 1 - thêm mới thường DocumentLineID==0, 2 - sửa dữ liệu cũ DocumentLineID<>0
                            item.FixedAssetID = data.FixedAssetID;

                            if (item.EquipmentID == 0)
                            {
                                this.InsertVAT(item);
                            }
                            else
                            {
                                this.UpdateVAT(item);
                            }
                        }
                        else
                        {
                            //nếu xóa có 2 trường hợp : 1 - xóa dữ liệu có trước DocumentLineID<>0, 2 - xóa dữ liệu mới thêm DocumentLineID ==0
                            if (item.EquipmentID > 0)
                            {
                                this.DeleteVAT(item.EquipmentID);
                            }
                        }
                    }
                }


                #endregion

                return data.FixedAssetID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int UpdateLine(Models.AppFixedAssetLineTable data)
        {
            try
            {

                SqlParameter pOriginal = ExConvert.ParseSqlParam(data, this._metaobject.GetMetaByColumnName(this._paramnamelineoutput), this._paramnamelineupdate);
                var parameters = ExConvert.Data2SqlParam(data, this._metaobject, pOriginal).ToArray();
                this._db.Database.ExecuteSqlCommand(this._storeNameLineU, parameters);

                return data.FixedAssetLineID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int UpdateVAT(Models.AppEquipmentTable data)
        {
            try
            {

                SqlParameter pOriginal = ExConvert.ParseSqlParam(data, this._metaobject.GetMetaByColumnName(this._paramnamevatoutput), this._paramnamevatupdate);
                var parameters = ExConvert.Data2SqlParam(data, this._metaobject, pOriginal).ToArray();
                this._db.Database.ExecuteSqlCommand(this._storeNameVATU, parameters);

                return data.EquipmentID;
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

        public void DeleteLine(int Id)
        {
            try
            {
                this._db.Database.ExecuteSqlCommand(this._storeNameLineD, Id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteVAT(int Id)
        {
            try
            {
                this._db.Database.ExecuteSqlCommand(this._storeNameVATD, Id);
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
    }
}