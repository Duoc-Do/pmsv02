using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebApp.Areas.Accounting.Models;
using WebApp.Areas.Accounting.Services;



namespace WebApp.Areas.Accounting.DAL
{
    public partial class ResOrder : SenVietListObject
    {
        public ResOrder(HttpRequestBase request) : base(request) { }
        

        public override void InitObject()
        {
            this._businesscode = "ResOrder";
            this._metaname = "ResOrderView";
            this._keyfield = "OrderId";
            this.appajaxoption.ajaxoption.Add("title", "Đặt món ăn");
            base.InitObject();
        }

        public List<Models.ReportResItem> GetReportResItem(DateTime datefrom, DateTime dateto)
        {
            var query0 = from row in this._db.ResOrderItemViews
                         join item in this._db.AppItemTables on row.ItemId equals item.ItemID
                         join master in this._db.ResOrders on row.OrderId equals master.OrderId
                         where master.OrderDate >= datefrom && master.OrderDate <= dateto && master.OrderStatusId == 0
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
                         join item in this._db.AppItemTables on row.ItemId equals item.ItemID
                         orderby item.ItemGroupID, row.ItemName
                         select row).ToList();


            return query;

        }

        public List<Models.ResOrderView> GetData(int? TableId)
        {
            if (TableId != null)
            {
                var query = this._db.ResOrderViews.Where(m => m.TableId == TableId);

                return Services.GridHelper.GetResults(this._request, this._metaname, query);
            }
            return Services.GridHelper.GetResults(this._request, this._metaname, this._db.ResOrderViews);

        }

        public List<Models.ResOrderView> GetData2(int? OrderStatusId)
        {
            if (OrderStatusId != null)
            {
                var query = this._db.ResOrderViews.Where(m => m.OrderStatusId == OrderStatusId);

                return Services.GridHelper.GetResults(this._request, this._metaname, query);
            }
            return Services.GridHelper.GetResults(this._request, this._metaname, this._db.ResOrderViews);

        }

        public List<Models.ResOrderView> GetTakeaway()
        {
                var query = this._db.ResOrderViews.Where(m => m.OrderStatusId >0 && m.TableId == null);
                return query.OrderBy(m=>m.OrderDate).ToList();
        }


        public Models.ResOrderView GetById(int Id)
        {
            return this._db.ResOrderViews.SingleOrDefault(m => m.OrderId == Id);
        }

        public Models.ResOrderView GetTable(int TableId)
        {
            int status = 0;
            return this._db.ResOrderViews.Where(m => m.OrderStatusId > status && m.TableId == TableId).Take(1).SingleOrDefault();
        }

        public Models.ResOrderItem GetByOrderItemId2(int OrderItemId)
        {
            return this._db.ResOrderItems.SingleOrDefault(m => m.OrderItemId == OrderItemId);
        }

        public Models.ResOrder GetById2(int Id)
        {
            return this._db.ResOrders.SingleOrDefault(m => m.OrderId == Id);
        }

        public Models.ResOrderView GetNew(int? id, int tableid)
        {
            if (id != null) return GetById(id ?? 0);

            var model = new Models.ResOrderView() { OrderDate = DateTime.Now, CreatedDateTime = DateTime.Now, OrderStatusId = 1 };
            var postable = this._db.ResTables.SingleOrDefault(m => m.TableId == tableid);
            if (postable != null)
            {
                model.TableId = postable.TableId;
                model.TableName = postable.Name;
            }
            return model;
        }

        public Models.ResOrderView GetEdit(int id)
        {
            return GetById(id);
        }

        public Models.ResOrderView GetDelete(int id)
        {
            return GetById(id);
        }

        public Models.AppEmployeeTable GetEmployeeByUserId(int Id)
        {
            return this._db.AppEmployeeTables.SingleOrDefault(m => m.UserID == Id);
        }

        public int Insert(Models.ResOrderView data)
        {
            try
            {
                var tablewait = this.GetTable(data.TableId ?? 0);
                if (tablewait != null) throw new Exception("Bàn này đã có người rồi!");

                //if (data.ResOrderItemViewz == null) throw new Exception("Phải nhập chi tiết!");
                //var countline = data.ResOrderItemViewz.Where(m => m != -1).Count();
                //if (countline == 0) throw new Exception("Phải nhập chi tiết!");

                this.Validate(data);


                data.CreatedBy = GlobalVariant.GetAppUser().UserID;
                data.CreatedDateTime = DateTime.Now;

                data.ModifiedBy = GlobalVariant.GetAppUser().UserID;
                data.ModifiedDateTime = DateTime.Now;

                var _data = new Models.ResOrder();
                this.MapView2Table(data, _data);
                this._db.ResOrders.Add(_data);
                this._db.SaveChanges();

                if (data.ResOrderItemViewz == null) return _data.OrderId;
                var countline = data.ResOrderItemViewz.Where(m => m != -1).Count();
                if (countline == 0) return _data.OrderId;

                var lines = data.ResOrderItemViews.ToList();
                var linez = data.ResOrderItemViewz.ToList();

                DateTime _curDate = DateTime.Now;
                for (int i = 0; i < lines.Count; i++)
                {

                    var itemz = linez[i];
                    if (itemz != -1)
                    {
                        var item = lines[i];
                        item.OrderId = _data.OrderId;
                        item.CreateDate = _curDate;
                        this.InsertLine(item);
                    }
                }

                return _data.OrderId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int InsertLine(Models.ResOrderItemView data)
        {
            try
            {
                var _data = new Models.ResOrderItem();
                //this.MapView2Table(data, _data);

                _data.OrderId = data.OrderId;
                _data.ItemId = data.ItemId;
                _data.Quantity = data.Quantity;
                _data.QuantityProcess = 0;// _data.Quantity;//tạm thời khi order coi như bếp làm xong

                _data.Price = data.Price;
                _data.Amount = data.Amount;
                _data.ItemNote = data.ItemNote;

                _data.CreateDate = data.CreateDate;


                this._db.ResOrderItems.Add(_data);
                this._db.SaveChanges();
                return _data.OrderItemId;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int UpdateLineQuantityProcess(int orderid,DateTime createdate)
        {
            var master = GetById2(orderid);

            foreach (var item in master.ResOrderItems)
            {
                if (item.CreateDate.Value.ToString("yyyyMMddhms") == createdate.ToString("yyyyMMddhms"))
                {
                    item.QuantityProcess = item.Quantity;
                }
            }
            this._db.Entry(master).State = System.Data.Entity.EntityState.Modified;
            this._db.SaveChanges();
            return 1;

        }

        public int UpdateLine(Models.ResOrderItemView data)
        {
            try
            {
                var _data = this.GetByOrderItemId2(data.OrderItemId);
                //this.MapView2Table(data, _data);

                _data.OrderId = data.OrderId;
                _data.ItemId = data.ItemId;
                _data.Quantity = data.Quantity;
                //_data.QuantityProcess = _data.QuantityProcess;//tạm thời khi order coi như bếp làm xong

                _data.Price = data.Price;
                _data.Amount = data.Amount;
                _data.ItemNote = data.ItemNote;

                this._db.Entry(_data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();
                return _data.OrderItemId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteLine(int OrderItemId)
        {
            try
            {
                var _rec = this.GetByOrderItemId2(OrderItemId);
                this._db.Entry(_rec).State = System.Data.Entity.EntityState.Deleted;
                this._db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public int Update(Models.ResOrderView data)
        {
            try
            {

                if (data.ResOrderItemViewz == null) throw new Exception("Phải nhập chi tiết!");
                var countline = data.ResOrderItemViewz.Where(m => m != -1).Count();
                if (countline == 0) throw new Exception("Phải nhập chi tiết!");

                this.Validate(data);


                var _data = this.GetById2(data.OrderId);

                data.ModifiedBy = GlobalVariant.GetAppUser().UserID;
                data.ModifiedDateTime = DateTime.Now;
                data.CreatedBy = _data.CreatedBy;
                data.CreatedDateTime = _data.CreatedDateTime;

                this.MapView2Table(data, _data);

                this._db.Entry(_data).State = System.Data.Entity.EntityState.Modified;
                this._db.SaveChanges();

                #region Xử lý line


                var lines = data.ResOrderItemViews.ToList();
                var linez = data.ResOrderItemViewz.ToList();

                DateTime _curDate = DateTime.Now;
                for (int i = 0; i < lines.Count; i++)
                {

                    var itemz = linez[i];
                    var item = lines[i];

                    if (itemz != -1)
                    {
                        //có 2 tru?ng h?p: 1 - thêm m?i thu?ng DocumentOrderItemId==0, 2 - s?a d? li?u cu DocumentOrderItemId<>0

                        item.OrderId = _data.OrderId;


                        if (item.OrderItemId == 0)
                        {
                            item.CreateDate = _curDate;
                            this.InsertLine(item);
                        }
                        else
                        {
                            this.UpdateLine(item);
                        }
                    }
                    else
                    {
                        //n?u xóa có 2 tru?ng h?p : 1 - xóa d? li?u có tru?c DocumentOrderItemId<>0, 2 - xóa d? li?u m?i thêm DocumentOrderItemId ==0
                        if (item.OrderId > 0)
                        {
                            this.DeleteLine(item.OrderItemId);
                        }
                    }
                }


                #endregion

                return data.OrderId;
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
                var _rec = GetById2(Id);
                this._db.Entry(_rec).State = System.Data.Entity.EntityState.Deleted;
                this._db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
        private string posadmin = "posadmin.systems.administrators";
        public bool IsEdit(int id)
        {
            var data = GetById(id);
            var OrderDate = data.OrderDate; //data.GetType().GetProperty("OrderDate").GetValue(data, null) ?? String.Empty;
            var begindate = DateTime.Now.AddHours(-12);

            var IsAdmin = Roles.IsUserInRole("Admin") || Roles.IsUserInRole("POSAdmin") || this.posadmin.Contains(GlobalVariant.GetAppUser().SysRole.Name.ToLower()) ;

            if ((DateTime)OrderDate < begindate && !IsAdmin)
            {
                return false;
            }

            return true;
        }

        public bool IsDelete(int id)
        {
            return Roles.IsUserInRole("Admin") || this.posadmin.Contains(GlobalVariant.GetAppUser().SysRole.Name.ToLower());
        }



    }
}