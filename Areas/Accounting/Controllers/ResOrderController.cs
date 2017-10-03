using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Linq.Dynamic;
using WebApp.Areas.Accounting.Models;
using System.IO;
using WebApp.Areas.Accounting.Services;

namespace WebApp.Areas.Accounting.Controllers
{
    public class ResOrderController : AppAccountingListController
    {
        WebApp.Areas.Accounting.DAL.ResOrder _dataobject;
        string posadmin = "posview.posadmin.poscash"; 
        protected override IActionInvoker CreateActionInvoker()
        {
            _dataobject = new WebApp.Areas.Accounting.DAL.ResOrder(Request);
            this.InitData(_dataobject);

            this._deleteview = "DeleteOrder";
            return base.CreateActionInvoker();
        }

        public ActionResult AutoComplete()
        {
            WebApp.Areas.Accounting.DAL.ResOrder _dataobject = new WebApp.Areas.Accounting.DAL.ResOrder(Request);
            return Json(_dataobject.AutoComplete(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult FieldChange()
        {
            WebApp.Areas.Accounting.DAL.ResOrder _dataobject = new WebApp.Areas.Accounting.DAL.ResOrder(Request);
            return Json(_dataobject.FieldChange(), JsonRequestBehavior.AllowGet);
        }

        //[ChildActionOnly]
        public ActionResult GetTable(int TableId)
        {
            var model = _dataobject.GetTable(TableId);
            if (model == null)
            {
                model = _dataobject.GetNew(null, TableId);
            }
            return PartialView(model);
        }

        public ActionResult TakeawayOrder(int OrderId)
        {
            var model = _dataobject.GetById(OrderId);
            if (model.OrderStatusId>0 )
            {
                return PartialView(model);
            }

            return Content("");
        }

        public ActionResult GetTakeaway()
        {
            var model = _dataobject.GetTakeaway();
            return PartialView(model);
        }



        //[ChildActionOnly]
        public ActionResult GetChart(string datefrom, string dateto,int userid)
        {
            var _datefrom = Convert.ToDateTime(datefrom);
            var _dateto = Convert.ToDateTime(dateto).EndOfDay();
            ViewBag.datefrom = _datefrom;
            ViewBag.dateto = _dateto;
            ViewBag.userid = userid;
            return PartialView();
        }

        public ActionResult GetDataChart(string datefrom, string dateto, int userid)
        {
            int topproduct = 10;
            var _datefrom = Convert.ToDateTime(datefrom);
            var _dateto = Convert.ToDateTime(dateto).EndOfDay();

            object rows = null;
            object rows2 = null;
            object rows3 = null;
            object results = null;

            if ((GlobalVariant.GetAppUser().SysRole.IsAdmin || posadmin.Contains(GlobalVariant.GetAppUser().SysRole.Name.ToLower())) && userid == 0)
            {

                rows = _dataobject._db.Database.SqlQuery<RpResRevenue>(string.Format("sp_RpResRevenue '{0}','{1}'", _datefrom.ToString("yyyy-MM-dd HH:mm:ss"), _dateto.ToString("yyyy-MM-dd HH:mm:ss"))).ToList();
                rows2 =
                    (
                      from row in _dataobject._db.ResOrderItemViews
                      join master in _dataobject._db.ResOrders on row.OrderId equals master.OrderId
                      where master.OrderDate >= _datefrom && master.OrderDate <= _dateto && master.OrderStatusId == 0
                      group row by new
                      {
                          row.ItemId,
                          row.ItemName
                      } into g
                      select new DoughNutChart()
                      {
                          label = g.Key.ItemName,
                          value = g.Sum(x => x.Amount)
                      }
                    ).OrderByDescending(m => m.value).Take(topproduct).ToList();

                rows3 =
                   (
                     from row in _dataobject._db.ResOrderItemViews
                     join master in _dataobject._db.ResOrders on row.OrderId equals master.OrderId
                     where master.OrderDate >= _datefrom && master.OrderDate <= _dateto && master.OrderStatusId == 0
                     group row by new
                     {
                         row.ItemId,
                         row.ItemName
                     } into g
                     select new DoughNutChart()
                     {
                         label = g.Key.ItemName,
                         value = g.Sum(x => x.Quantity)
                     }
                   ).OrderByDescending(m => m.value).Take(topproduct).ToList();
            }
            else
            {
                int _userid = GlobalVariant.GetAppUser().UserID;
                if ((GlobalVariant.GetAppUser().SysRole.IsAdmin || posadmin.Contains(GlobalVariant.GetAppUser().SysRole.Name.ToLower())) && userid > 0) _userid = userid;

                rows = _dataobject._db.Database.SqlQuery<RpResRevenue>(string.Format("sp_RpResRevenueByUser '{0}','{1}',{2}", _datefrom.ToString("yyyy-MM-dd HH:mm:ss"), _dateto.ToString("yyyy-MM-dd HH:mm:ss"), _userid)).ToList();

                rows2 =
                    (
                      from row in _dataobject._db.ResOrderItemViews
                      join master in _dataobject._db.ResOrders on row.OrderId equals master.OrderId
                      where master.OrderDate >= _datefrom && master.OrderDate <= _dateto && master.OrderStatusId == 0 && master.ModifiedBy.Value == _userid
                      group row by new
                      {
                          row.ItemId,
                          row.ItemName
                      } into g
                      select new DoughNutChart()
                      {
                          label = g.Key.ItemName,
                          value = g.Sum(x => x.Amount)
                      }
                    ).OrderByDescending(m => m.value).Take(topproduct).ToList();

                rows3 =
                   (
                     from row in _dataobject._db.ResOrderItemViews
                     join master in _dataobject._db.ResOrders on row.OrderId equals master.OrderId
                     where master.OrderDate >= _datefrom && master.OrderDate <= _dateto && master.OrderStatusId == 0 && master.ModifiedBy.Value == _userid
                     group row by new
                     {
                         row.ItemId,
                         row.ItemName
                     } into g
                     select new DoughNutChart()
                     {
                         label = g.Key.ItemName,
                         value = g.Sum(x => x.Quantity)
                     }
                   ).OrderByDescending(m => m.value).Take(topproduct).ToList();
            }

            results = (new { rows = rows, rows2 = rows2, rows3 = rows3 });
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetReportResItem(string datefrom, string dateto,int userid, Models.Paging page)
        {
            var _datefrom = Convert.ToDateTime(datefrom);
            var _dateto = Convert.ToDateTime(dateto).EndOfDay();

            

            object rows = null;
            object results = null;

            int _userid = GlobalVariant.GetAppUser().UserID;
            if ((GlobalVariant.GetAppUser().SysRole.IsAdmin || posadmin.Contains(GlobalVariant.GetAppUser().SysRole.Name.ToLower())) && userid > 0) _userid = userid;
            var query0 = from row in _dataobject._db.ResOrderItemViews
                         join item in _dataobject._db.AppItemTables on row.ItemId equals item.ItemID
                         join master in _dataobject._db.ResOrders on row.OrderId equals master.OrderId
                         where master.OrderDate >= _datefrom && master.OrderDate <= _dateto && master.OrderStatusId == 0 && master.ModifiedBy==_userid
                         orderby item.ItemGroupID, row.ItemName
                         group row by new { row.ItemId, row.ItemName }
                             into g
                             select new ReportResItem()
                             {
                                 ItemId = g.Key.ItemId,
                                 ItemName = g.Key.ItemName,
                                 Quantity = g.Sum(x => x.Quantity),
                                 Amount = g.Sum(x => x.Amount)
                             };

            if ((GlobalVariant.GetAppUser().SysRole.IsAdmin || posadmin.Contains(GlobalVariant.GetAppUser().SysRole.Name.ToLower())) && userid == 0)
            {
                query0 = from row in _dataobject._db.ResOrderItemViews
                         join item in _dataobject._db.AppItemTables on row.ItemId equals item.ItemID
                         join master in _dataobject._db.ResOrders on row.OrderId equals master.OrderId
                         where master.OrderDate >= _datefrom && master.OrderDate <= _dateto && master.OrderStatusId == 0
                         orderby item.ItemGroupID, row.ItemName
                         group row by new { row.ItemId, row.ItemName }
                             into g
                             select new ReportResItem()
                             {
                                 ItemId = g.Key.ItemId,
                                 ItemName = g.Key.ItemName,
                                 Quantity = g.Sum(x => x.Quantity),
                                 Amount = g.Sum(x => x.Amount)
                             };

            }

            var query = (from row in query0
                         join item in _dataobject._db.AppItemTables on row.ItemId equals item.ItemID
                         orderby item.ItemGroupID, row.ItemName
                         select row);
            if (page != null)
            {
                page.TotalRows = query.Count();
                page.SetPaging(page.TotalRows, page.PageCurrent);
                rows = query.Skip(page.Skip).Take(page.PageSize).ToList();
            }
            else
            {
                page = new Models.Paging();
                rows = query.ToList();
            }

            results = (new { rows = rows, page = page });
            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index(int? TableId)
        {
            return PartialView(_dataobject.GetData(TableId));
        }
        public ActionResult Index2(int? OrderStatusId)
        {
            return PartialView(this._indexview, _dataobject.GetData2(OrderStatusId));
        }



        public ActionResult ChangeTable(int TableId)
        {
            return PartialView(TableId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeTable(int TableId, int TableId2)
        {

            // TODO: Add insert logic here
            try
            {
                if (TableId2 > 0)
                {
                    var table2 = _dataobject._db.ResTables.Where(m => m.TableId == TableId2).SingleOrDefault();
                    //ViewBag.TableId2 = TableId2;

                    var model = _dataobject.GetTable(TableId);
                    if (model == null) throw new Exception("Phải nhập bàn cần chuyển");

                    var model2 = _dataobject.GetTable(TableId2);
                    if (model2 == null)
                    {
                        //trường hợp bàn 1 active bàn 2 không active
                        model2 = _dataobject.GetNew(null,TableId);
                        _dataobject.MapView2Table(model, model2);
                        model2.ResOrderItemViews = new System.Collections.ObjectModel.Collection<Models.ResOrderItemView>();
                        model2.ResOrderItemViewz = new System.Collections.ObjectModel.Collection<int>();

                        foreach (var item in model.ResOrderItemViews)
                        {
                            var _item = new Models.ResOrderItemView();
                            //_dataobject.MapView2Table(_item, item);

                            //_item.OrderId = model2.OrderId;
                            _item.ItemId = item.ItemId;
                            _item.Quantity = item.Quantity;
                            _item.QuantityProcess = item.Quantity;//tạm thời khi order coi như bếp làm xong

                            _item.Price = item.Price;
                            _item.Amount = item.Amount;
                            _item.ItemNote = item.ItemNote;

                            _item.CreateDate = item.CreateDate;

                            model2.ResOrderItemViews.Add(_item);
                            model2.ResOrderItemViewz.Add(0);
                        }


                        model.OrderStatusId = -1;
                        model.Note = string.Format("Chuyển sang bàn {0}",table2.Name);

                        model2.TableId = TableId2;
                        _dataobject.Insert(model2);
                    }
                    else
                    {
                        //trường hợp 2 bàn đang active
                        //trường hợp bàn 1 active bàn 2 không active

                        var model3 = _dataobject.GetNew(null, TableId2);
                        _dataobject.MapView2Table(model2, model3);

                        model3.ResOrderItemViewz = new System.Collections.ObjectModel.Collection<int>();

                        
                        foreach (var item in model2.ResOrderItemViews)
                        {
                            var _item = new Models.ResOrderItemView();
                            //_dataobject.MapView2Table(_item, item);

                            _item.OrderId = item.OrderId;
                            _item.OrderItemId = item.OrderItemId;

                            _item.ItemId = item.ItemId;
                            _item.Quantity = item.Quantity;
                            _item.QuantityProcess = item.Quantity;//tạm thời khi order coi như bếp làm xong

                            _item.Price = item.Price;
                            _item.Amount = item.Amount;
                            _item.ItemNote = item.ItemNote;

                            _item.CreateDate = item.CreateDate;

                            model3.ResOrderItemViews.Add(_item);
                            model3.ResOrderItemViewz.Add(item.OrderItemId);
                        }

                        foreach (var item in model.ResOrderItemViews)
                        {
                            var _item = new Models.ResOrderItemView();
                            //_dataobject.MapView2Table(_item, item);

                            _item.OrderId = model2.OrderId;

                            _item.ItemId = item.ItemId;
                            _item.Quantity = item.Quantity;
                            _item.QuantityProcess = item.Quantity;//tạm thời khi order coi như bếp làm xong

                            _item.Price = item.Price;
                            _item.Amount = item.Amount;
                            _item.ItemNote = item.ItemNote;

                            _item.CreateDate = item.CreateDate;

                            model3.ResOrderItemViews.Add(_item);
                            model3.ResOrderItemViewz.Add(0);
                        }




                        model3.OrderSubtotal = model3.ResOrderItemViews.Sum(m => m.Amount);
                        if (model2.OrderDiscountPercentage!=0)
                        {
                            model3.OrderDiscount = model3.OrderSubtotal * model2.OrderDiscountPercentage;
                        }
                        model3.OrderQuantity = model3.ResOrderItemViews.Sum(m => m.Quantity);
                        model3.OrderQuantityProcess = model3.ResOrderItemViews.Sum(m => m.QuantityProcess);
                        model3.OrderTotal = model3.OrderSubtotal - model3.OrderDiscount;
                        
                        //model2.TableId = TableId2;
                        model.OrderStatusId = -1;
                        model.Note = string.Format("Chuyển sang bàn {0}", table2.Name);

                        _dataobject.Update(model3);

                    }
                }

                //return RedirectToAction("Index", "ResHome");
                return PartialView("UpdateSuccess", TableId2);
            }
            catch (Exception ex)
            {
                WebApp.Areas.Accounting.Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(this._updateview, TableId);
            }

        }


        public ActionResult Create(int? Id, int TableId)
        {
            return PartialView(this._updateview, _dataobject.GetNew(Id, TableId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(WebApp.Areas.Accounting.Models.ResOrderView collection)
        {

            // TODO: Add insert logic here
            try
            {
                long outputId = _dataobject.Insert(collection);
                ViewBag.OrderId = outputId;
                return PartialView("UpdateSuccess", collection.TableId);
            }
            catch (Exception ex)
            {
                WebApp.Areas.Accounting.Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(this._updateview, collection);
            }

        }

        public ActionResult Edit2(int Id)
        {
            if (!_dataobject.IsEdit(Id)) return PartialView(this._roleview);
            return PartialView("Update2", _dataobject.GetEdit(Id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit2(int Id, WebApp.Areas.Accounting.Models.ResOrderView collection)
        {
            try
            {
                int outputId = _dataobject.Update(collection);
                if (collection.ResOrderItemViews.Count(m => m.OrderItemId == 0) > 0)
                {
                    ViewBag.OrderId = outputId;
                }
                return PartialView("UpdateSuccess2", outputId);
            }
            catch (Exception ex)
            {
                WebApp.Areas.Accounting.Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView("Update2", _dataobject.GetEdit(Id));
            }
        }


        public ActionResult Create2(int? Id)
        {
            return PartialView("Update2", _dataobject.GetNew(Id, 0));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create2(WebApp.Areas.Accounting.Models.ResOrderView collection)
        {

            // TODO: Add insert logic here
            try
            {
                int outputId = _dataobject.Insert(collection);
                ViewBag.OrderId = outputId;
                return PartialView("UpdateSuccess2", outputId);
            }
            catch (Exception ex)
            {
                WebApp.Areas.Accounting.Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView("Update2", collection);
            }

        }

        public ActionResult PrintOrder(int Id)
        {

            var master = _dataobject.GetEdit(Id);
            if (master != null)
            {
                //var linesum = master.ResOrderItemViews.
                List<Accounting.Models.ResOrderItemView> linesum =
                                (
                                  from row in master.ResOrderItemViews
                                  join item in _dataobject._db.AppItemTables on row.ItemId equals item.ItemID
                                  orderby item.ItemGroupID, item.Name
                                  group row by new { row.ItemId, row.ItemName, row.Price }
                                      into g
                                      select new ResOrderItemView()
                                      {
                                          ItemId = g.Key.ItemId,
                                          ItemName = g.Key.ItemName,
                                          Quantity = g.Sum(x => x.Quantity),
                                          Price = g.Key.Price,
                                          Amount = g.Sum(x => x.Amount)
                                      }
                                ).ToList();
                ViewBag.linesum = linesum;
            }



            return View(_dataobject.GetEdit(Id));
        }

        public ActionResult PrintOrder2(int Id, int TableId)
        {
            //in hoa don bep nếu không chỉ định order id thì chọn order sau cùng của bàn mà có trạng thái khác với 0: hoàn thành và -1: hủy
            if (Id == 0)
            {
                var resorder = _dataobject._db.ResOrders.Where(m => m.TableId == TableId && m.OrderStatusId > 0).OrderByDescending(m => m.OrderId).Take(1).SingleOrDefault();
                if (resorder != null)
                {
                    Id = resorder.OrderId;
                }
            }

            var master = _dataobject.GetEdit(Id);
            if (master != null)
            {
                var maxline = master.ResOrderItemViews.OrderByDescending(m => m.OrderItemId).Take(1).SingleOrDefault();

                List<Accounting.Models.ResOrderItemView> linesum =
                                (
                                  from row in master.ResOrderItemViews
                                  //join item in _dataobject._db.AppItemTables on row.ItemId equals item.ItemID
                                  where row.CreateDate == maxline.CreateDate
                                  orderby row.OrderItemId
                                  select row
                                ).ToList();
                ViewBag.linesum = linesum;
            }
            else
            {
                return View(master);
            }

            return View(_dataobject.GetEdit(Id));
        }

        public ActionResult PrintKitchen(int Id, int TableId)
        {
            //in hoa don bep nếu không chỉ định order id thì chọn order sau cùng của bàn mà có trạng thái khác với 0: hoàn thành và -1: hủy
            if (Id == 0)
            {
                var resorder = _dataobject._db.ResOrders.Where(m => m.TableId == TableId && m.OrderStatusId > 0).OrderByDescending(m => m.OrderId).Take(1).SingleOrDefault();
                if (resorder != null)
                {
                    Id = resorder.OrderId;
                }
            }

            var master = _dataobject.GetEdit(Id);
            if (master != null)
            {
                var maxline = master.ResOrderItemViews.OrderByDescending(m => m.OrderItemId).Take(1).SingleOrDefault();

                List<Accounting.Models.ResOrderItemView> linesum =
                                (
                                  from row in master.ResOrderItemViews
                                  //join item in _dataobject._db.AppItemTables on row.ItemId equals item.ItemID
                                  where row.CreateDate == maxline.CreateDate
                                  orderby row.OrderItemId
                                  select row
                                ).ToList();
                ViewBag.linesum = linesum;
            }
            else
            {
                return PartialView(master);
            }

            return PartialView(_dataobject.GetEdit(Id));
        }

        public ActionResult PayOrder(int Id, int status)
        {
            try
            {
                var _data = _dataobject.GetById2(Id);
                _data.OrderStatusId = status;
                _data.ModifiedBy = GlobalVariant.GetAppUser().UserID;
                _data.ModifiedDateTime = DateTime.Now;
                _dataobject._db.Entry(_data).State = System.Data.Entity.EntityState.Modified;
                _dataobject._db.SaveChanges();

                return Json(new { ketqua = 1, tableid =_data.TableId??0 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(new { ketqua = 0, tableid = 0 }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult QuantityProcess(int Id, DateTime createdate)
        {
            try
            {
                _dataobject.UpdateLineQuantityProcess(Id, createdate);
                return Json(new { ketqua = 1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                return Json(new { ketqua = 0 }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult Edit(int Id)
        {
            if (!_dataobject.IsEdit(Id)) return PartialView(this._roleview);
            return PartialView(this._updateview, _dataobject.GetEdit(Id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int Id, WebApp.Areas.Accounting.Models.ResOrderView collection)
        {
            try
            {
                int outputId = _dataobject.Update(collection);
                if (collection.ResOrderItemViews.Count(m=>m.OrderItemId==0)>0)
                {
                    ViewBag.OrderId = outputId;
                }
                return PartialView("UpdateSuccess", collection.TableId);
            }
            catch (Exception ex)
            {
                WebApp.Areas.Accounting.Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(this._updateview, _dataobject.GetEdit(Id));
            }
        }


        public ActionResult Delete(int Id)
        {
            if (!_dataobject.IsDelete(Id)) return PartialView(this._roleview);
            return PartialView(this._deleteview, _dataobject.GetDelete(Id));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int Id, FormCollection collection)
        {
            try
            {
                var tableid = _dataobject.GetById2(Id).TableId;
                _dataobject.Delete(Id);
                return RedirectToAction(this.ActionReturn(), new { TableId=tableid });
            }
            catch (Exception ex)
            {
                WebApp.Areas.Accounting.Services.GlobalErrors.Parse(ModelState, _dataobject.Errors, ex);
                return PartialView(this._deleteview, _dataobject.GetDelete(Id));
            }
        }


        public ActionResult ExportToExcel2(string datefrom, string dateto)
        {
            byte[] bytes = null;
            Dictionary<string, Models.SysTableDetailView> columns = new Dictionary<string, SysTableDetailView>(); ;


            var _datefrom = Convert.ToDateTime(datefrom);
            var _dateto = Convert.ToDateTime(dateto).EndOfDay();

            var query0 = from row in _dataobject._db.ResOrderItemViews
                         join item in _dataobject._db.AppItemTables on row.ItemId equals item.ItemID
                         join master in _dataobject._db.ResOrders on row.OrderId equals master.OrderId
                         where master.OrderDate >= _datefrom && master.OrderDate <= _dateto && master.OrderStatusId == 0
                         orderby item.ItemGroupID, row.ItemName
                         group row by new { row.ItemId, row.ItemName }
                             into g
                             select new ReportResItem()
                             {
                                 ItemId = g.Key.ItemId,
                                 ItemName = g.Key.ItemName,
                                 Quantity = g.Sum(x => x.Quantity),
                                 Amount = g.Sum(x => x.Amount)
                             };



            var query = (from row in query0
                         join item in _dataobject._db.AppItemTables on row.ItemId equals item.ItemID
                         orderby item.ItemGroupID, row.ItemName
                         select row);

            columns.Add("ItemName", new SysTableDetailView()
            {
                ColumnName = "ItemName",
                Des = "Tên hàng",
                DATA_TYPE = "nvarchar",
                GridViewShow = true
            });

            columns.Add("Quantity", new SysTableDetailView()
            {
                ColumnName = "Quantity",
                Des = "Số lượng",
                DATA_TYPE = "decimal",
                GridViewShow = true,
                FormatType = "Customer",
                FormatValue = "n",
                CultureInfo = "CICC"
            });

            columns.Add("Amount", new SysTableDetailView()
            {
                ColumnName = "Amount",
                Des = "Tiền",
                DATA_TYPE = "decimal",
                GridViewShow = true,
                FormatType = "Customer",
                FormatValue = "c"
            });

            using (var stream = new MemoryStream())
            {
                Export.ExportToXlsx(stream, query.ToList(), columns);
                bytes = stream.ToArray();
            }
            string FileName = string.Format("lotusviet{0}.xlsx", DateTime.Now.ToString("yyyyMMddhhmmss"));
            return File(bytes, "text/xls", FileName);
        }


    }
}