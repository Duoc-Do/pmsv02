using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;

//Tiện ích chứng từ
namespace WebApp.Areas.Accounting.Services
{
    public static class Voucher
    {
        public static string GetControllerName(int VoucherID)
        {
            var db = new Models.WebAppAccEntities(Services.GlobalVariant.GetConnection());
            string _ret = "";
            var voucher = db.AppVoucherViews.Where(m => m.VoucherID == VoucherID).SingleOrDefault();
            _ret = voucher.AssemblyName.Replace(".exe", "");
            return _ret;
        }

        public static string GetControllerName(string VoucherCode)
        {
            var db = new Models.WebAppAccEntities(Services.GlobalVariant.GetConnection());
            string _ret = "";
            var voucher = db.AppVoucherViews.Where(m => m.VoucherCode == VoucherCode).SingleOrDefault();
            _ret = voucher.AssemblyName.Replace(".exe", "");
            return _ret;
        }

        public static int GetVoucherID(string VoucherCode)
        {
            var db = new Models.WebAppAccEntities(Services.GlobalVariant.GetConnection());
            int _GetVoucherID = 0;
            var row = db.AppVoucherTables.Where(m => m.VoucherCode == VoucherCode).SingleOrDefault();
            if (row != null)
            {
                _GetVoucherID = row.VoucherID;
            }
            return _GetVoucherID;
        }

        public static string GetVoucherNumber(int VoucherID)
        {
            var db = new Models.WebAppAccEntities(Services.GlobalVariant.GetConnection());
            string _GetVoucherNumber = "";
            int _IntegerPart = 0;
            var row = db.AppVoucherTables.Where(m => m.VoucherID == VoucherID).SingleOrDefault();

            if (row != null)
            {
                _IntegerPart = int.Parse(row.IntegerPart.ToString()) + 1;
                _GetVoucherNumber = row.AlphaPart + _IntegerPart.ToString();
            }
            return _GetVoucherNumber;
        }

        public static string GetPostStoreProcedure(int VoucherID)
        {
            var db = new Models.WebAppAccEntities(Services.GlobalVariant.GetConnection());
            string _GetPostStoreProcedure = "";
            var row = db.AppVoucherTables.Where(m => m.VoucherID == VoucherID).SingleOrDefault();

            if (row != null)
            {
                _GetPostStoreProcedure = row.PostStoreProcedure;
            }
            return _GetPostStoreProcedure;
        }

        public static int PostStoreProcedure(int VoucherID, long DocumentID)
        {
            string storename = GetPostStoreProcedure(VoucherID);
            if (!string.IsNullOrEmpty(storename))
            {
                storename = string.Format("{0} {1}", storename, DocumentID);
                var db = new Models.WebAppAccEntities(Services.GlobalVariant.GetConnection());
                return db.Database.ExecuteSqlCommand(storename);
            }
            return 0;
        }

        public static bool CheckLockDate(long DocumentID, DateTime VoucherDate)
        {
            //kiểm tra ngày khóa sổ có 2 trường hợp
            //trường hợp sửa nếu chứng từ đang sửa trước ngày khóa sổ thì không được phép
            //trường hợp tạo mới hoặc sửa chứng từ có ngày sau ngày và người dùng nhập ngày chứng từ nhỏ hơn ngày khóa sổ thì cũng không được phép
            var db = new Models.WebAppAccEntities(Services.GlobalVariant.GetConnection());
            var lockdate = DateTime.Parse(Services.GlobalVariant.GetSysOption()["LockDate"].ToString());
            var result = db.Database.SqlQuery<DateTime>(string.Format("Select value=VoucherDate From AppDocumentTable Where DocumentID={0}", DocumentID)).ToList();
            if (result != null && result.Count > 0) VoucherDate = result[0];
            return VoucherDate > lockdate;
        }

        public static IQueryable<T> RoleFilter<T>(IQueryable<T> data)
        {
            if (!GlobalVariant.GetAppUser().SysRole.IsAdmin && GlobalVariant.GetSysOption()["RoleFilter"].ToString().Split(',').Contains(GlobalVariant.GetAppUser().SysRole.Name))
            {
                string strfilter = string.Format(" CreatedBy=={0} ", GlobalVariant.GetAppUser().UserID);
                return data.Where(strfilter).AsQueryable();
            }
            return data;
        }


        public static bool CheckVoucherNumber(long DocumentID, int VoucherID, string VoucherNumber, DateTime VoucherDate)
        {
            //CheckVoucherNumber	int	0: Trùng cho lưu, 1: Trùng không cho lưu
            //CheckVoucherNumberBy	int	0: Không kiểm tra, 1: Kiểm tra toàn thời gian, 2: Theo năm, 3: Theo tháng

            var db = new Models.WebAppAccEntities(Services.GlobalVariant.GetConnection());
            bool _ret = true;
            var checkvouchernumberby = Services.GlobalVariant.GetSysOption()["CheckVoucherNumberBy"].ToString();
            List<string> result = null;
            if (checkvouchernumberby == "1")
            {
                result = db.Database.SqlQuery<string>(string.Format("Select top 1 value=VoucherNumber From AppDocumentTable Where DocumentID<>{0} And VoucherNumber='{1}' And VoucherID={2}", DocumentID, VoucherNumber, VoucherID)).ToList();
            }

            if (checkvouchernumberby == "2")
            {
                result = db.Database.SqlQuery<string>(string.Format("Select top 1 value=VoucherNumber From AppDocumentTable Where DocumentID<>{0} And Year(VoucherDate)={1} And VoucherNumber='{2}' And VoucherID={3}", DocumentID, VoucherDate.Year, VoucherNumber, VoucherID)).ToList();
            }

            if (checkvouchernumberby == "3")
            {
                result = db.Database.SqlQuery<string>(string.Format("Select top 1 value=VoucherNumber From AppDocumentTable Where DocumentID<>{0} And Year(VoucherDate)={1} And Month(VoucherDate)={2} And VoucherNumber='{3}' And VoucherID={4}", DocumentID, VoucherDate.Year, VoucherDate.Month, VoucherNumber, VoucherID)).ToList();
            }
            if (result != null) _ret = result.Count == 0;

            return _ret;
        }

        public static void SaveVoucherNumber(int VoucherID, string VoucherNumber)
        {

            DAL.AppVoucherTable _dataobject = new DAL.AppVoucherTable(null);

            var row = _dataobject.GetById(VoucherID);
            if (row != null)
            {

                string IntegerPart = "";
                for (int i = 0; i < VoucherNumber.Length; i++)
                {
                    if (char.IsDigit(VoucherNumber[i]))
                    {
                        IntegerPart += VoucherNumber[i];
                    }
                }
                row.IntegerPart = int.Parse(IntegerPart);
                // Phải Cập nhật vào dữ liệu                
                //db.SaveChanges();
                _dataobject.Update(row);
            }

        }

        public static int GetDocumentID(string TableName, string StoredProcedureName, string Key)
        {
            int _result = 0;

            //LotusLib.Forms.FormCreateFromLine frm = new LotusLib.Forms.FormCreateFromLine();
            //frm.strStoredProcedure = StoredProcedureName;
            //frm.strStoredProcedureKey = Key;
            //frm.strTableName = TableName;
            //frm.ShowDialog();
            //_result = frm.DocumentID;


            return _result;
        }

        public static string GetIsoCode()
        {
            return Services.GlobalVariant.GetSysOption()["IsoCode"].ToString();
        }

        public static int GetCurrencyIDByIsoCode(string IsoCode)
        {
            var db = new Models.WebAppAccEntities(Services.GlobalVariant.GetConnection());
            int _result = 0;
            var result = db.AppCurrencyTables.Where(m => m.IsoCode == IsoCode).SingleOrDefault();
            if (result != null)
            {
                _result = result.CurrencyID;
            }
            return _result;
        }

        public static decimal GetExchangeRate(int CurrencyID, DateTime VoucherDate)
        {
            var db = new Models.WebAppAccEntities(Services.GlobalVariant.GetConnection());
            decimal _result = 1;
            var result = db.AppExchangeRateViews.Where(m => m.CurrencyID == CurrencyID && m.FromDate <= VoucherDate).OrderByDescending(m => m.FromDate).Take(1).SingleOrDefault();
            if (result != null)
            {
                _result = result.Value;
            }

            return _result;
        }

        public static decimal GetExchangeRateLine(DateTime VoucherDate, string CustomerCode, string DisplayNumber)
        {
            var db = new Models.WebAppAccEntities(Services.GlobalVariant.GetConnection());
            decimal _result = 1;
            var result = db.Database.SqlQuery<decimal>(string.Format("select value=dbo.fc_GetExchangeRateLine('{0}','{1}','{2}')", VoucherDate.ToString("yyyyMMdd"), CustomerCode, DisplayNumber)).ToList();
            if (result != null)
            {
                _result = result[0];
            }

            return _result;
        }

        public static decimal GetMeasureRate(int? UOMID, int? ItemID)
        {
            var db = new Models.WebAppAccEntities(Services.GlobalVariant.GetConnection());
            decimal _Result = 1;
            var _result = db.Database.SqlQuery<decimal>(string.Format("sp_FcGetMeasureRate '{0}','{1}'", UOMID, ItemID)).ToList();
            if (_result != null)
            {
                _Result = _result[0];
            }

            //_Result = _result.SingleOrDefault();
            //_Result = decimal.Parse(s);
            return _Result;
        }

        public static string GetVATNumber(DateTime VoucherDate)
        {
            var db = new Models.WebAppAccEntities(Services.GlobalVariant.GetConnection());
            string _result = "";
            var result = db.Database.SqlQuery<string>(string.Format("select vatnumber=dbo.fc_GetVATNumber('{0}')", VoucherDate.ToString("yyyyMMdd"))).ToList();
            if (result != null)
            {
                _result = result[0];
            }

            return _result;
        }

        public static string GetVATSerial(DateTime VoucherDate)
        {
            var db = new Models.WebAppAccEntities(Services.GlobalVariant.GetConnection());
            string _result = "";
            var result = db.Database.SqlQuery<string>(string.Format("select vatserial = dbo.fc_GetVATSerial('{0}')", VoucherDate.ToString("yyyyMMdd"))).ToList();
            if (result != null)
            {
                _result = result[0];
            }

            return _result;
        }

        public static Decimal GetUnitPriceSell(int CustomerID, int ItemID, DateTime VoucherDate)
        {
            Decimal _Result = 0;
            return _Result;
        }

        public static decimal GetUnitPriceSellFC(int CustomerID, int ItemID, DateTime VoucherDate)
        {
            decimal _Result = 0;
            return _Result;
        }

        public static decimal GetUnitPriceSell(int CustomerID, int ItemID, int UOMID, decimal Quantity0, DateTime VoucherDate)
        {
            var db = new Models.WebAppAccEntities(Services.GlobalVariant.GetConnection());

            decimal _result = 0;
            var result = db.Database.SqlQuery<decimal>(string.Format("sp_FcGetUnitPriceSell {0},{1},{2},{3},'{4}'", CustomerID, ItemID, UOMID, Quantity0, VoucherDate.ToString("yyyyMMdd"))).ToList();
            if (result != null)
            {
                _result = result[0];
            }

            return _result;
        }

        public static decimal GetUnitPriceSellFC(int CustomerID, int ItemID, int UOMID, decimal Quantity0, DateTime VoucherDate)
        {
            var db = new Models.WebAppAccEntities(Services.GlobalVariant.GetConnection());

            decimal _result = 0;
            var result = db.Database.SqlQuery<decimal>(string.Format("sp_FcGetUnitPriceSellFC {0},{1},{2},{3},'{4}'", CustomerID, ItemID, UOMID, Quantity0, VoucherDate.ToString("yyyyMMdd"))).ToList();
            if (result != null)
            {
                _result = result[0];
            }

            return _result;

        }

        public static decimal GetUnitPriceSell2(int CustomerID, int ItemID, int UOMID, decimal Quantity0)
        {
            var db = new Models.WebAppAccEntities(Services.GlobalVariant.GetConnection());

            decimal _result = 0;
            var result = db.Database.SqlQuery<decimal>(string.Format("sp_FcGetUnitPriceSell2 {0},{1},{2},{3}", CustomerID, ItemID, UOMID, Quantity0)).ToList();
            if (result != null)
            {
                _result = result[0];
            }
            return _result;

        }

        public static decimal GetUnitPriceSellFC2(int CustomerID, int ItemID, int UOMID, decimal Quantity0)
        {
            var db = new Models.WebAppAccEntities(Services.GlobalVariant.GetConnection());

            decimal _result = 0;
            var result = db.Database.SqlQuery<decimal>(string.Format("sp_FcGetUnitPriceSellFC2 {0},{1},{2},{3}", CustomerID, ItemID, UOMID, Quantity0)).ToList();
            if (result != null)
            {
                _result = result[0];
            }
            return _result;
        }


        public static decimal GetUnitPrice(int ItemID, int CustomerID, DateTime VoucherDate)
        {
            var db = new Models.WebAppAccEntities(Services.GlobalVariant.GetConnection());

            decimal _result = 0;
            var result = db.Database.SqlQuery<decimal>(string.Format("sp_FcGetUnitPrice {0},{1},'{2}'", CustomerID, ItemID, VoucherDate.ToString("yyyyMMdd"))).ToList();
            if (result != null)
            {
                _result = result[0];
            }
            return _result;
        }

        public static decimal GetUnitPriceFC(int ItemID, int CustomerID, DateTime VoucherDate)
        {
            var db = new Models.WebAppAccEntities(Services.GlobalVariant.GetConnection());

            decimal _result = 0;
            var result = db.Database.SqlQuery<decimal>(string.Format("sp_FcGetUnitPriceFC {0},{1},'{2}'", CustomerID, ItemID, VoucherDate.ToString("yyyyMMdd"))).ToList();
            if (result != null)
            {
                _result = result[0];
            }
            return _result;
        }


    }
}