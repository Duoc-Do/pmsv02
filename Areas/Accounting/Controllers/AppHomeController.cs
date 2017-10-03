using System;
using System.Linq;
//using System.Linq.Dynamic;
using System.Web.Mvc;
using WebApp.Areas.Accounting.Services;

namespace WebApp.Areas.Accounting.Controllers
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

        public ActionResult ACSOSB()
        {
            return View();
        }

        public ActionResult wSales()
        {
            var _dataobject = new DAL.SenVietGeneralObject(Request, "");
            string[] vouchercode = new string[] { "SO01", "SO06" };
            var query = from q in _dataobject._db.AppDocumentLineViews
                        where q.VoucherDate.Value.Year == DateTime.Today.Year && vouchercode.Contains(q.VoucherCode)
                        group q by new { q.VoucherDate.Value.Year, q.VoucherDate.Value.Month } into g
                        orderby g.Key.Year, g.Key.Month
                        select new { Year = g.Key.Year, Month = g.Key.Month, AmountSell = g.Sum(rl => rl.AmountSell) };
            var rows = query.ToList();
            for (int i = 1; i < 13; i++)
            {
                if (rows.Exists(m => m.Month == i))
                {
                }
                else
                {
                    var row = rows.Select(m => new { Year = m.Year, Month = i, AmountSell = (decimal?)0 }).FirstOrDefault();
                    rows.Add(row);
                }
            }
            var total = query.Sum(m => m.AmountSell);
            return Json((new { rows = rows.OrderBy(m => m.Month), total = total.HasValue ? total.Value.Currency() : "" }), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GLSummary(int year,
            string debits, string credits,
            string debits1, string credits1,
            string debits2, string credits2
            )
        {
            int lenchar = 3;
            var debit = debits.Split(new char[] { ',' });
            var credit = credits.Split(new char[] { ',' });

            var debit1 = debits1.Split(new char[] { ',' });
            var credit1 = credits1.Split(new char[] { ',' });

            var debit2 = debits2.Split(new char[] { ',' });
            var credit2 = credits2.Split(new char[] { ',' });

            var _dataobject = new DAL.SenVietGeneralObject(Request, "");


            var query0 = from q in _dataobject._db.AppGLJournalViews where q.VoucherDate.Value.Year == year select q;
            if (!string.IsNullOrEmpty(debit[0])) query0 = from q in query0 where debit.Contains(q.DisplayNumberDebit.Substring(0, lenchar)) select q;
            if (!string.IsNullOrEmpty(credit[0])) query0 = from q in query0 where credit.Contains(q.DisplayNumberCredit.Substring(0, lenchar)) select q;
            var _query0 = from q in query0
                          group q by new { q.VoucherDate.Value.Year, q.VoucherDate.Value.Month } into g
                          orderby g.Key.Year, g.Key.Month
                          select new { Year = g.Key.Year, Month = g.Key.Month, Amount = g.Sum(rl => rl.Amount), Amount1 = (decimal)0, Amount2 = (decimal)0 };

            var rows0 = _query0.ToList();

            var query1 = from q in _dataobject._db.AppGLJournalViews where q.VoucherDate.Value.Year == year select q;
            if (!string.IsNullOrEmpty(debit1[0])) query1 = from q in query1 where debit1.Contains(q.DisplayNumberDebit.Substring(0, lenchar)) select q;
            if (!string.IsNullOrEmpty(credit1[0])) query1 = from q in query1 where credit1.Contains(q.DisplayNumberCredit.Substring(0, lenchar)) select q;
            var _query1 = from q in query1
                          group q by new { q.VoucherDate.Value.Year, q.VoucherDate.Value.Month } into g
                          orderby g.Key.Year, g.Key.Month
                          select new { Year = g.Key.Year, Month = g.Key.Month, Amount = (decimal)0, Amount1 = g.Sum(rl => rl.Amount), Amount2 = (decimal)0 };

            var rows1 = _query1.ToList();

            var query2 = from q in _dataobject._db.AppGLJournalViews where q.VoucherDate.Value.Year == year select q;
            if (!string.IsNullOrEmpty(debit2[0])) query2 = from q in query2 where debit2.Contains(q.DisplayNumberDebit.Substring(0, lenchar)) select q;
            if (!string.IsNullOrEmpty(credit2[0])) query2 = from q in query2 where credit2.Contains(q.DisplayNumberCredit.Substring(0, lenchar)) select q;
            var _query2 = from q in query2
                          group q by new { q.VoucherDate.Value.Year, q.VoucherDate.Value.Month } into g
                          orderby g.Key.Year, g.Key.Month
                          select new { Year = g.Key.Year, Month = g.Key.Month, Amount = (decimal)0, Amount1 = (decimal)0, Amount2 = g.Sum(rl => rl.Amount) };

            var rows2 = _query2.ToList();

            rows0.AddRange(rows1);
            rows0.AddRange(rows2);

            var query = (from q in rows0
                         group q by new { q.Year, q.Month } into g
                         orderby g.Key.Year, g.Key.Month
                         select new { Year = g.Key.Year, Month = g.Key.Month, Amount = g.Sum(rl => rl.Amount), Amount1 = g.Sum(rl => rl.Amount1), Amount2 = g.Sum(rl => rl.Amount2) }
                            );
            var rows = query.ToList();

            for (int i = 1; i < 13; i++)
            {
                if (rows.Count > 0)
                {
                    if (rows.Exists(m => m.Month == i))
                    {
                        continue;
                    }
                }


                var row = new { Year = year, Month = i, Amount = (decimal)0, Amount1 = (decimal)0, Amount2 = (decimal)0 };
                rows.Add(row);

            }

            var total = rows.Sum(m => m.Amount);
            var total1 = rows.Sum(m => m.Amount1);
            var total2 = rows.Sum(m => m.Amount2);
            var total3 = total - total1 - total2;

            return Json(
                (new
                {
                    rows = rows.OrderBy(m => m.Month).Select(m => new { Year = m.Year, Month=m.Month, Amount = m.Amount, Amount1 = m.Amount1, Amount2 = m.Amount2, Amount3 = m.Amount - m.Amount1 - m.Amount2 }).ToList(),
                    total = total.Currency(),
                    total1 = total1.Currency(),
                    total2 = total2.Currency(),
                    total3 = total3.Currency()
                }), JsonRequestBehavior.AllowGet);

        }


    }
}
