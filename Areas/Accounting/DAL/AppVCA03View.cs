﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using WebApp.Areas.Accounting.Services;

namespace WebApp.Areas.Accounting.DAL
{
    public partial class AppVCA03View : SenVietVoucherObject
    {
        public AppVCA03View(HttpRequestBase request) : base(request) { }
        public override void InitObject()
        {
            this._businesscode = "AppVCA03ViewEdit";
            this._keyfieldmaster = "DocumentID";
            this._keyfieldline = "DocumentLineID";
            this._keyfieldvat = "DocumentVATID";

            this._metaname = "AppVCA03View";
            this._metalinename = "AppVCA03LineView";
            this._metavatname = "AppVCA03VATView";


            this._storeNameI = @"sp_AppVCA03ViewI @DocumentID OUTPUT,@ParentID,@VoucherID,@CurrencyID,@ExchangeRate,@VoucherDate,@VoucherNumber,@CustomerID,@Address,@Contact,@Description,@PostType,@AccountDebitID,@AccountCreditID,@CreatedBy,@CreatedDateTime,@ModifiedBy,@ModifiedDateTime";
            this._storeNameU = @"sp_AppVCA03ViewU @DocumentID OUTPUT,@ParentID,@VoucherID,@CurrencyID,@ExchangeRate,@VoucherDate,@VoucherNumber,@CustomerID,@Address,@Contact,@Description,@PostType,@AccountDebitID,@AccountCreditID,@CreatedBy,@CreatedDateTime,@ModifiedBy,@ModifiedDateTime,@Original_DocumentID";
            this._storeNameD = @"sp_AppVCA03ViewD {0}";
            this._storeNameLineI = @"sp_AppVCA03LineViewI @DocumentID,@DocumentLineID OUTPUT,@DocumentLineType,@AccountDebitLineID,@AccountCreditLineID,@Debit,@Credit,@ExchangeRateLine,@DescriptionLine,@ProductID,@Amount,@AmountFC,@ExObject01ID,@ExObject02ID,@ExObject03ID,@ExObject04ID,@ExObject05ID,@ExObject06ID,@ExObject07ID,@ExObject08ID,@ExObject09ID,@ExObject10ID,@ExDate01,@ExDate02,@ExDate03,@ExDate04,@ExDate05,@ExDate06,@ExString01,@ExString02,@ExString03,@ExString04,@ExString05,@ExString06,@ExNumeric01,@ExNumeric02,@ExNumeric03,@ExNumeric04,@ExNumeric05,@ExNumeric06,@ExNumeric07,@ExNumeric08,@ExNumeric09,@ExNumeric10,@ExObject11ID,@ExObject12ID,@ExObject13ID,@ExObject14ID,@ExObject15ID,@ExObject16ID,@ExObject17ID,@ExObject18ID,@ExObject19ID,@ExObject20ID";
            this._storeNameLineU = @"sp_AppVCA03LineViewU @DocumentID,@DocumentLineID OUTPUT,@DocumentLineType,@AccountDebitLineID,@AccountCreditLineID,@Debit,@Credit,@ExchangeRateLine,@DescriptionLine,@ProductID,@Amount,@AmountFC,@ExObject01ID,@ExObject02ID,@ExObject03ID,@ExObject04ID,@ExObject05ID,@ExObject06ID,@ExObject07ID,@ExObject08ID,@ExObject09ID,@ExObject10ID,@ExDate01,@ExDate02,@ExDate03,@ExDate04,@ExDate05,@ExDate06,@ExString01,@ExString02,@ExString03,@ExString04,@ExString05,@ExString06,@ExNumeric01,@ExNumeric02,@ExNumeric03,@ExNumeric04,@ExNumeric05,@ExNumeric06,@ExNumeric07,@ExNumeric08,@ExNumeric09,@ExNumeric10,@ExObject11ID,@ExObject12ID,@ExObject13ID,@ExObject14ID,@ExObject15ID,@ExObject16ID,@ExObject17ID,@ExObject18ID,@ExObject19ID,@ExObject20ID,@Original_DocumentLineID";
            this._storeNameLineD = @"sp_AppVCA03LineViewD {0}";
            this._storeNameVATI = @"sp_AppVCA03VATViewI @DocumentID,@DocumentVATID OUTPUT,@DocumentVATType,@VATDate,@VATNumber,@VATSerial,@VATTemplate,@CustomerName,@CustomerAddress,@TaxCode,@ItemName,@AmountVAT,@AmountVATFC,@Percentage,@Amount,@AmountFC,@DescriptionVAT,@SalesTaxID,@AccountDebitLineID,@AccountCreditLineID,@ProductID,@ExObject01ID,@ExObject02ID,@ExObject03ID,@ExObject04ID,@ExObject05ID,@ExObject06ID,@ExObject07ID,@ExObject08ID,@ExObject09ID,@ExObject10ID,@ExDate01,@ExDate02,@ExDate03,@ExDate04,@ExDate05,@ExDate06,@ExString01,@ExString02,@ExString03,@ExString04,@ExString05,@ExString06,@ExNumeric01,@ExNumeric02,@ExNumeric03,@ExNumeric04,@ExNumeric05,@ExNumeric06,@ExNumeric07,@ExNumeric08,@ExNumeric09,@ExNumeric10,@ExObject11ID,@ExObject12ID,@ExObject13ID,@ExObject14ID,@ExObject15ID,@ExObject16ID,@ExObject17ID,@ExObject18ID,@ExObject19ID,@ExObject20ID";
            this._storeNameVATU = @"sp_AppVCA03VATViewU @DocumentID,@DocumentVATID OUTPUT,@DocumentVATType,@VATDate,@VATNumber,@VATSerial,@VATTemplate,@CustomerName,@CustomerAddress,@TaxCode,@ItemName,@AmountVAT,@AmountVATFC,@Percentage,@Amount,@AmountFC,@DescriptionVAT,@SalesTaxID,@AccountDebitLineID,@AccountCreditLineID,@ProductID,@ExObject01ID,@ExObject02ID,@ExObject03ID,@ExObject04ID,@ExObject05ID,@ExObject06ID,@ExObject07ID,@ExObject08ID,@ExObject09ID,@ExObject10ID,@ExDate01,@ExDate02,@ExDate03,@ExDate04,@ExDate05,@ExDate06,@ExString01,@ExString02,@ExString03,@ExString04,@ExString05,@ExString06,@ExNumeric01,@ExNumeric02,@ExNumeric03,@ExNumeric04,@ExNumeric05,@ExNumeric06,@ExNumeric07,@ExNumeric08,@ExNumeric09,@ExNumeric10,@ExObject11ID,@ExObject12ID,@ExObject13ID,@ExObject14ID,@ExObject15ID,@ExObject16ID,@ExObject17ID,@ExObject18ID,@ExObject19ID,@ExObject20ID,@Original_DocumentVATID";
            this._storeNameVATD = @"sp_AppVCA03VATViewD {0}";

            base.InitObject();
        }

        public override object FieldChange()
        {
            string fieldname = this._request.Params["fieldname"].ToString();
            string keyword = this._request.Params["keyword"].ToString();
            int type = int.Parse(this._request.Params["type"].ToString());
            DateTime voucherdate = this._request.Params["voucherdate"] != null ? DateTime.Parse(this._request["voucherdate"].ToString()) : DateTime.Today;
            string customercode = this._request.Params["customercode"] != null ? this._request.Params["customercode"].ToString() : "";

            object results = null;

            #region valid ch?ng t?

            if (type == 0)
            {
                switch (fieldname)
                {
                    case "IsoCode":
                        var appcurrencytable = this._db.AppCurrencyTables.Where(m => m.IsoCode == keyword).SingleOrDefault();
                        if (appcurrencytable != null)
                        {
                            decimal exchangerate = Services.Voucher.GetExchangeRate(appcurrencytable.CurrencyID, voucherdate);
                            results = (new { rows = new { ExchangeRate = exchangerate, CurrencyID = appcurrencytable.CurrencyID } });
                        }
                        break;

                    default:
                        results = Services.Data.GetByCode(fieldname, keyword);
                        break;
                }
            }
            #endregion

            #region valid h?ch toán
            if (type == 1)
            {
                switch (fieldname)
                {
                    case "DisplayNumberLineCredit":
                        var appaccountview2 = this._db.AppAccountView2.Where(m => m.DisplayNumber == keyword).SingleOrDefault();
                        if (appaccountview2 != null)
                        {
                            decimal exchangerateline = Services.Voucher.GetExchangeRateLine(voucherdate, customercode, keyword);
                            results = (new { rows = new { ExchangeRateLine = exchangerateline, AccountID = appaccountview2.AccountID } });
                        }
                        break;
                    default:
                        results = Services.Data.GetByCode(fieldname, keyword);
                        break;
                }
            }
            #endregion

            #region valid thu?
            if (type == 2)
            {
                switch (fieldname)
                {
                    case "VATDate":
                        results = (new { rows = new { VATNumber = Services.Voucher.GetVATNumber(voucherdate), VATSerial = Services.Voucher.GetVATSerial(voucherdate) } });
                        break;
                    default:
                        results = Services.Data.GetByCode(fieldname, keyword);
                        break;
                }
            }
            #endregion

            //base.FieldChange();
            return results;
        }

        public void Validate(Models.AppVCA03View data)
        {
            #region ki?m tra chi ti?t

            this._metaobject = Services.GlobalMeta.GetMetaObject(this._metalinename);
            this.ValidateLine(data.AppVCA03LineViews, data.AppVCA03LineViewz);

            #endregion

            #region ki?m tra thu?

            this._metaobject = Services.GlobalMeta.GetMetaObject(this._metavatname);
            this.ValidateVAT(data.AppVCA03VATViews, data.AppVCA03VATViewz);
            #endregion

            #region Business logic validate
            #region Object validate
            this._metaobject = Services.GlobalMeta.GetMetaObject(this._metaname);
            this.ValidateMaster(data);
            #endregion

            #endregion

            //if (Errors.Count > 0) throw new InvalidOperationException("l?i nh?p s? li?u");

        }

        public List<Models.AppVCA03View> GetData()
        {
            var model = Services.GridHelper.GetResults(this._request, this._metaname, Voucher.RoleFilter(this._db.AppVCA03Views));
            return model;
        }

        public Models.AppVCA03View GetById(long Id)
        {
            return this._db.AppVCA03Views.SingleOrDefault(m => m.DocumentID == Id);
        }

        public Models.AppVCA03View GetNew(long? id)
        {
            //ki?m tra quy?n n?u không cho thì ném exception
            if (id != null)
            {
                var model = GetById(id ?? 0);
                model.VoucherDate = DateTime.Today;
                model.VoucherNumber = Services.Voucher.GetVoucherNumber(model.VoucherID);
                return model;
            }
            return new Models.AppVCA03View();
        }

        public Models.AppVCA03View GetEdit(long id)
        {
            //ki?m tra quy?n n?u không cho thì ném exception
            return GetById(id);
        }

        public Models.AppVCA03View GetDelete(long id)
        {
            //ki?m tra quy?n n?u không cho thì ném exception
            return GetById(id);
        }

        public long Insert(Models.AppVCA03View data)
        {
            try
            {
                data.DocumentID = 0;
                //Ki?m tra thêm m?i ch?ng t? nhu trùng s? ho?c ngày khóa s?
                this.ValidateInsert(data);
                //Ki?m tra ki?u nh?p d? li?u
                this.Validate(data);

                #region x? lý master

                data.CreatedBy = GlobalVariant.GetAppUser().UserID;
                data.CreatedDateTime = DateTime.Now;

                SqlParameter[] parameters = ExConvert.Data2SqlParam(data, this._metaobject, this._paramnamemasteroutput).ToArray();
                this._db.Database.ExecuteSqlCommand(this._storeNameI, parameters);
                data.DocumentID = (long)parameters.GetValueSqlParam(this._paramnamemasteroutput);

                #endregion

                #region X? lý line
                this._metaobject = Services.GlobalMeta.GetMetaObject(this._metalinename);

                var appvca03lineviews = data.AppVCA03LineViews.ToList();
                var appvca03lineviewz = data.AppVCA03LineViewz.ToList();
                for (int i = 0; i < appvca03lineviews.Count; i++)
                {

                    var itemz = appvca03lineviewz[i];
                    if (itemz != -1)
                    {
                        var item = appvca03lineviews[i];
                        item.DocumentID = data.DocumentID;
                        item.AccountDebitLineID = data.AccountDebitID;
                        if (data.IsoCode == Services.GlobalVariant.GetSysOption()["IsoCode"].ToString())
                        {
                            item.Debit = 0;
                            item.Credit = 0;
                            item.ExchangeRateLine = 0;
                            item.AmountFC = 0;
                        }

                        this.InsertLine(item);
                    }
                }

                #endregion

                #region X? lý VAT
                if (data.AppVCA03VATViews != null)
                {

                    this._metaobject = Services.GlobalMeta.GetMetaObject(this._metavatname);

                    var appvca03vatviews = data.AppVCA03VATViews.ToList();
                    var appvca03vatviewz = data.AppVCA03VATViewz.ToList();
                    for (int i = 0; i < appvca03vatviews.Count; i++)
                    {

                        var itemz = appvca03vatviewz[i];
                        if (itemz != -1)
                        {
                            var item = appvca03vatviews[i];
                            item.DocumentID = data.DocumentID;
                            item.AccountDebitLineID = data.AccountDebitID;
                            if (data.IsoCode == Services.GlobalVariant.GetSysOption()["IsoCode"].ToString())
                            {
                                item.AmountFC = 0;
                                item.AmountVATFC = 0;
                            }

                            this.InsertVAT(item);
                        }
                    }
                }

                #endregion

                this._metaobject = Services.GlobalMeta.GetMetaObject(this._metaname);

                //Post vào sổ sách
                Services.Voucher.PostStoreProcedure(data.VoucherID, data.DocumentID);
                //cập nhật số chứng từ
                Services.Voucher.SaveVoucherNumber(data.VoucherID, data.VoucherNumber);
                return data.DocumentID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long InsertLine(Models.AppVCA03LineView data)
        {
            try
            {
                SqlParameter[] parameters = ExConvert.Data2SqlParam(data, this._metaobject, this._paramnamelineoutput).ToArray();
                this._db.Database.ExecuteSqlCommand(this._storeNameLineI, parameters);
                data.DocumentLineID = (long)parameters.GetValueSqlParam(this._paramnamelineoutput);

                return data.DocumentLineID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long InsertVAT(Models.AppVCA03VATView data)
        {
            try
            {
                SqlParameter[] parameters = ExConvert.Data2SqlParam(data, this._metaobject, this._paramnamevatoutput).ToArray();
                this._db.Database.ExecuteSqlCommand(this._storeNameVATI, parameters);
                data.DocumentVATID = (long)parameters.GetValueSqlParam(this._paramnamevatoutput);

                return data.DocumentVATID;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public long Update(Models.AppVCA03View data)
        {
            try
            {
                this.ValidateUpdate(data);
                this.Validate(data);

                data.ModifiedBy = GlobalVariant.GetAppUser().UserID;
                data.ModifiedDateTime = DateTime.Now;

                SqlParameter pOriginal = ExConvert.ParseSqlParam(data, this._metaobject.GetMetaByColumnName(this._paramnamemasteroutput), this._paramnamemasterupdate);
                var parameters = ExConvert.Data2SqlParam(data, this._metaobject, pOriginal).ToArray();
                this._db.Database.ExecuteSqlCommand(this._storeNameU, parameters);

                #region X? lý line
                this._metaobject = Services.GlobalMeta.GetMetaObject(this._metalinename);


                var appvca03lineviews = data.AppVCA03LineViews.ToList();
                var appvca03lineviewz = data.AppVCA03LineViewz.ToList();
                for (int i = 0; i < appvca03lineviews.Count; i++)
                {

                    var itemz = appvca03lineviewz[i];
                    var item = appvca03lineviews[i];

                    if (itemz != -1)
                    {
                        //có 2 tru?ng h?p: 1 - thêm m?i thu?ng DocumentLineID==0, 2 - s?a d? li?u cu DocumentLineID<>0
                        item.DocumentID = data.DocumentID;
                        item.AccountDebitLineID = data.AccountDebitID;

                        if (data.IsoCode == Services.GlobalVariant.GetSysOption()["IsoCode"].ToString())
                        {
                            item.Debit = 0;
                            item.Credit = 0;
                            item.ExchangeRateLine = 0;
                            item.AmountFC = 0;
                        }


                        if (item.DocumentLineID == 0)
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
                        //n?u xóa có 2 tru?ng h?p : 1 - xóa d? li?u có tru?c DocumentLineID<>0, 2 - xóa d? li?u m?i thêm DocumentLineID ==0
                        if (item.DocumentLineID > 0)
                        {
                            this.DeleteLine(item.DocumentLineID);
                        }
                    }
                }


                #endregion

                #region X? lý thu?
                if (data.AppVCA03VATViews != null)
                {


                    this._metaobject = Services.GlobalMeta.GetMetaObject(this._metavatname);


                    var appvca03vatviews = data.AppVCA03VATViews.ToList();
                    var appvca03vatviewz = data.AppVCA03VATViewz.ToList();
                    for (int i = 0; i < appvca03vatviews.Count; i++)
                    {

                        var itemz = appvca03vatviewz[i];
                        var item = appvca03vatviews[i];

                        if (itemz != -1)
                        {
                            //có 2 tru?ng h?p: 1 - thêm m?i thu?ng DocumentLineID==0, 2 - s?a d? li?u cu DocumentLineID<>0
                            item.DocumentID = data.DocumentID;
                            item.AccountDebitLineID = data.AccountDebitID;
                            if (data.IsoCode == Services.GlobalVariant.GetSysOption()["IsoCode"].ToString())
                            {
                                item.AmountFC = 0;
                                item.AmountVATFC = 0;
                            }


                            if (item.DocumentVATID == 0)
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
                            //n?u xóa có 2 tru?ng h?p : 1 - xóa d? li?u có tru?c DocumentLineID<>0, 2 - xóa d? li?u m?i thêm DocumentLineID ==0
                            if (item.DocumentVATID > 0)
                            {
                                this.DeleteVAT(item.DocumentVATID);
                            }
                        }
                    }
                }


                #endregion

                this._metaobject = Services.GlobalMeta.GetMetaObject(this._metaname);

                Services.Voucher.PostStoreProcedure(data.VoucherID, data.DocumentID);

                return data.DocumentID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long UpdateLine(Models.AppVCA03LineView data)
        {
            try
            {

                SqlParameter pOriginal = ExConvert.ParseSqlParam(data, this._metaobject.GetMetaByColumnName(this._paramnamelineoutput), this._paramnamelineupdate);
                var parameters = ExConvert.Data2SqlParam(data, this._metaobject, pOriginal).ToArray();
                this._db.Database.ExecuteSqlCommand(this._storeNameLineU, parameters);

                return data.DocumentLineID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public long UpdateVAT(Models.AppVCA03VATView data)
        {
            try
            {

                SqlParameter pOriginal = ExConvert.ParseSqlParam(data, this._metaobject.GetMetaByColumnName(this._paramnamevatoutput), this._paramnamevatupdate);
                var parameters = ExConvert.Data2SqlParam(data, this._metaobject, pOriginal).ToArray();
                this._db.Database.ExecuteSqlCommand(this._storeNameVATU, parameters);

                return data.DocumentVATID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete(long Id)
        {
            try
            {
                this.ValidateDelete(Id);
                this._db.Database.ExecuteSqlCommand(this._storeNameD, Id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteLine(long Id)
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

        public void DeleteVAT(long Id)
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
    }
}
