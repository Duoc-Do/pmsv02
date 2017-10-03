using System.Web.Mvc;
using WebApp.Areas.Accounting;

namespace WebApp.Areas.Res.Controllers
{
    [Authorize]
    public class AppHomeController : AppAccountingController
    {
        //
        // GET: /Accounting/Home/
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult wSales()
        //{
        //    var _dataobject = new DAL.SenVietGeneralObject(Request, "");
        //    string[] vouchercode = new string[] { "SO01", "SO06" };
        //    var query = from q in _dataobject._db.AppDocumentLineViews
        //                where q.VoucherDate.Value.Year == DateTime.Today.Year && vouchercode.Contains(q.VoucherCode)
        //                group q by new { q.VoucherDate.Value.Year, q.VoucherDate.Value.Month } into g
        //                orderby g.Key.Year, g.Key.Month
        //                select new { Year = g.Key.Year, Month = g.Key.Month, AmountSell = g.Sum(rl => rl.AmountSell) };

        //    var total = query.Sum(m => m.AmountSell);
        //    return Json((new { rows = query.ToList(),total =  WebApp.Areas.Accounting.Services.ExConvert.Data2String(total, "numeric", "", "CICC") }), JsonRequestBehavior.AllowGet);
        //}
    }
}
