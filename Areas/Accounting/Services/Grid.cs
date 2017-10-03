using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Data.Entity.Core.Objects;

using WebApp.Areas.Accounting.Models;

namespace WebApp.Areas.Accounting.Services
{
    public class GridHelper
    {

        public static List<T> GetResults<T>(HttpRequestBase request, string MetaName, ObjectQuery<T> query)
        {

            var MetaObject = Services.GlobalMeta.GetMetaObject(MetaName);

            List<T> objectsList;
            var exspressions = new List<string>();
            var parameters = new List<ObjectParameter>();
            string sort = "";
            string sorttempval = "";
            string sorttempcol = "";

            bool issortadd = false;
            var sortlist = MetaObject.MetaTable.Where(m => m.OrderExpression != "" && m.OrderExpression != null).OrderBy(m => m.OrderExpression).ToList();

            //gán lại giá trị filter
            foreach (var item in MetaObject.MetaTable)
            {
                // thiết lập biểu thức lọc
                string fieldfilter = string.Format("{0}.filter.{1}", MetaName, item.ColumnName);
                string datatype = Services.ExConvert.Sqltype2Systemtype(item.DATA_TYPE);

                if (request.Params[fieldfilter] != null)
                {
                    item.FilterExpression = request.Params[fieldfilter];
                }

                //Nếu kiểu dữ liệu là số hoặc ngày 
                if ("numeric.datetime".Contains(datatype))
                {
                    if (
                        request.Params[string.Format("{0}.filter.{1}.eq", MetaName, item.ColumnName)] != null ||
                        request.Params[string.Format("{0}.filter.{1}.gt", MetaName, item.ColumnName)] != null ||
                        request.Params[string.Format("{0}.filter.{1}.lt", MetaName, item.ColumnName)] != null
                        )
                    {

                        fieldfilter = string.Format("{0}.filter.{1}.eq", MetaName, item.ColumnName);
                        item.FilterExpression = string.Format("{0};", request.Params[fieldfilter] == null ? "" : request.Params[fieldfilter]);

                        fieldfilter = string.Format("{0}.filter.{1}.gt", MetaName, item.ColumnName);
                        item.FilterExpression += string.Format("{0};", request.Params[fieldfilter] == null ? "" : request.Params[fieldfilter]);

                        fieldfilter = string.Format("{0}.filter.{1}.lt", MetaName, item.ColumnName);
                        item.FilterExpression += string.Format("{0};", request.Params[fieldfilter] == null ? "" : request.Params[fieldfilter]);

                    }

                    //fieldfilter = string.Format("{0}.filter.{1}.eq", MetaName, item.ColumnName);
                    //item.FilterExpression = string.Format("{0};", request.Params[fieldfilter] == null ? "" : request.Params[fieldfilter]);

                    //fieldfilter = string.Format("{0}.filter.{1}.gt", MetaName, item.ColumnName);
                    //item.FilterExpression += string.Format("{0};", request.Params[fieldfilter] == null ? "" : request.Params[fieldfilter]);

                    //fieldfilter = string.Format("{0}.filter.{1}.lt", MetaName, item.ColumnName);
                    //item.FilterExpression += string.Format("{0};", request.Params[fieldfilter] == null ? "" : request.Params[fieldfilter]);
                }



                if (!string.IsNullOrEmpty(item.FilterExpression))
                {
                    var expression = new Filter(item).getExpression();
                    if (expression != null)
                    {
                        exspressions.Add(expression.Expression);
                        parameters.AddRange(expression.Parameters);

                    }
                }


                string fieldsort = string.Format("{0}.sort.{1}", MetaName, item.ColumnName);
                if (request.Params[fieldsort] != null)
                {
                    sorttempval = request.Params[fieldsort];
                    sorttempcol = item.ColumnName;
                    string fieldsortshiftkey = string.Format("{0}.sort.{1}.shiftkey", MetaName, item.ColumnName);
                    issortadd = (request.Params[fieldsortshiftkey] != null);
                }


            }


            if (!issortadd)// nếu không phải multi sort
            {
                if (!string.IsNullOrWhiteSpace(sorttempcol))// nếu tham số sắp xếp có giá trị
                {
                    foreach (var item in sortlist)
                    {
                        item.OrderExpression = "";
                    }
                    sort = string.Format("it.{0} {1}", sorttempcol, sorttempval);
                    MetaObject.GetMetaByColumnName(sorttempcol).OrderExpression = string.Format("101.{0}", sorttempval);
                }
                else
                {
                    if (sortlist.Count == 0)// nếu không có tham số khai báo sẳn thì lấy côt đầu
                    {
                        sort = string.Format("it.{0} asc", MetaObject.MetaTable[0].ColumnName); // Nếu không có tham số sắp xếp thì lấy ngầm định cột đầu tiên
                        MetaObject.MetaTable[0].OrderExpression = "101.asc";
                    }
                    else
                    {
                        foreach (var item in sortlist)
                        {
                            sort += string.Format(",it.{0} {1}", item.ColumnName, (item.OrderExpression.Contains("asc") ? "asc" : "desc")); // Nếu không có tham số sắp xếp thì lấy ngầm định cột đầu tiên                                                        
                        }

                    }
                }

            }
            else // nếu multi sort
            {
                if (!string.IsNullOrWhiteSpace(sorttempcol))// nếu tham số sắp xếp có giá trị
                {
                    bool issortold = false;
                    foreach (var item in sortlist)
                    {
                        if (sorttempcol == item.ColumnName)
                        {
                            sort += string.Format(",it.{0} {1}", sorttempcol, sorttempval);
                            if (!item.OrderExpression.Contains(sorttempval)) // nếu là côt sắp xếp cũ nhưng thay đổi trình tự thì gán lại
                            {
                                item.OrderExpression = item.OrderExpression.Replace("desc", sorttempval);
                                item.OrderExpression = item.OrderExpression.Replace("asc", sorttempval);
                            }


                            issortold = true;
                        }
                        else
                        {
                            sort += string.Format(",it.{0} {1} ", item.ColumnName, (item.OrderExpression.Contains("asc") ? "asc" : "desc")); // Nếu không có tham số sắp xếp thì lấy ngầm định cột đầu tiên                                                        
                        }
                    }
                    if (!issortold)
                    {
                        sort += string.Format(",it.{0} {1} ", sorttempcol, sorttempval);
                        MetaObject.GetMetaByColumnName(sorttempcol).OrderExpression = string.Format("{0}.{1}", 101 + sortlist.Count, sorttempval);
                    }
                }

            }
            //xóa dấu , thừa nếu có trong biểu thức sort
            if (sort.Substring(0, 1) == ",")
            {
                sort = sort.Substring(1, sort.Length - 1);
            }

            //thiết lập điều kiện lọc
            var exspression = string.Format("({0})", string.Join("AND", exspressions.ToArray()));
            //build the final expression
            if (exspression != "()")
            {
                query = query.Where(exspression, parameters.ToArray()); //filter agents collection on the expression
            }

            //thiết lập phân trang

            int page = request.Params["paging.page"] == null ? MetaObject.Paging.PageCurrent : int.Parse(request.Params["paging.page"]);
            int pagesize = request.Params["paging.pagesize"] == null ? MetaObject.Paging.PageSize : int.Parse(request.Params["paging.pagesize"]);
            MetaObject.Paging.PageSize = pagesize;

            MetaObject.Paging.SetPaging(query.Count(), page);

            objectsList = query.OrderBy(sort).Skip(MetaObject.Paging.Skip).Take(MetaObject.Paging.PageSize).ToList();

            return objectsList;
        }


        public static List<T> GetResults<T>(HttpRequestBase request, string MetaName, IQueryable<T> query)
        {

            var MetaObject = Services.GlobalMeta.GetMetaObject(MetaName);

            List<T> objectsList;
            var exspressions = new List<string>();
            var parameters = new List<ObjectParameter>();
            string sort = "";
            string sorttempval = "";
            string sorttempcol = "";

            bool issortadd = false;
            var sortlist = MetaObject.MetaTable.Where(m => m.OrderExpression != "" && m.OrderExpression != null).OrderBy(m => m.OrderExpression).ToList();


            //gán lại giá trị filter
            foreach (var item in MetaObject.MetaTable)
            {
                //nếu cột tự tạo thì tạm không can thiệp
                //if (item.UType) continue;


                // thiết lập biểu thức lọc
                string fieldfilter = string.Format("{0}.filter.{1}", MetaName, item.ColumnName);
                string datatype = Services.ExConvert.Sqltype2Systemtype(item.DATA_TYPE);

                if (request.Params[fieldfilter] != null)
                {
                    item.FilterExpression = request.Params[fieldfilter];
                }

                //Nếu kiểu dữ liệu là số hoặc ngày 
                if ("numeric.datetime".Contains(datatype))
                {
                    fieldfilter = string.Format("{0}.filter.{1}.eq", MetaName, item.ColumnName);
                    item.FilterExpression = string.Format("{0};", request.Params[fieldfilter] == null ? "" : request.Params[fieldfilter]);

                    fieldfilter = string.Format("{0}.filter.{1}.gt", MetaName, item.ColumnName);
                    item.FilterExpression += string.Format("{0};", request.Params[fieldfilter] == null ? "" : request.Params[fieldfilter]);

                    fieldfilter = string.Format("{0}.filter.{1}.lt", MetaName, item.ColumnName);
                    item.FilterExpression += string.Format("{0};", request.Params[fieldfilter] == null ? "" : request.Params[fieldfilter]);
                }

                //// thiết lập biểu thức lọc
                //string fieldfilter = string.Format("{0}.filter.{1}", MetaName, item.ColumnName);
                //if (request.Params[fieldfilter] != null)
                //{
                //    item.FilterExpression = request.Params[fieldfilter];
                //}

                if (!string.IsNullOrEmpty(item.FilterExpression))
                {
                    var expression = new Filter(item).getExpression2();
                    if (expression != null)
                    {
                        exspressions.Add(expression.Expression);
                        parameters.AddRange(expression.Parameters);
                    }
                }

                // thiết lập biểu thức sắp xếp
                string fieldsort = string.Format("{0}.sort.{1}", MetaName, item.ColumnName);
                if (request.Params[fieldsort] != null)
                {
                    sorttempval = request.Params[fieldsort];
                    sorttempcol = item.ColumnName;
                    string fieldsortshiftkey = string.Format("{0}.sort.{1}.shiftkey", MetaName, item.ColumnName);
                    issortadd = (request.Params[fieldsortshiftkey] != null);
                }


            }



            if (!issortadd)// nếu không phải multi sort
            {
                if (!string.IsNullOrWhiteSpace(sorttempcol))// nếu tham số sắp xếp có giá trị
                {
                    foreach (var item in sortlist)
                    {
                        item.OrderExpression = "";
                    }
                    sort = string.Format("it.{0} {1}", sorttempcol, sorttempval);
                    MetaObject.GetMetaByColumnName(sorttempcol).OrderExpression = string.Format("101.{0}", sorttempval);
                }
                else
                {
                    if (sortlist.Count == 0)// nếu không có tham số khai báo sẳn thì lấy côt đầu
                    {
                        sort = string.Format("it.{0} asc", MetaObject.MetaTable[0].ColumnName); // Nếu không có tham số sắp xếp thì lấy ngầm định cột đầu tiên
                        MetaObject.MetaTable[0].OrderExpression = "101.asc";
                    }
                    else
                    {
                        foreach (var item in sortlist)
                        {
                            sort += string.Format(",it.{0} {1}", item.ColumnName, (item.OrderExpression.Contains("asc") ? "asc" : "desc")); // Nếu không có tham số sắp xếp thì lấy ngầm định cột đầu tiên                                                        
                        }

                    }
                }

            }
            else // nếu multi sort
            {
                if (!string.IsNullOrWhiteSpace(sorttempcol))// nếu tham số sắp xếp có giá trị
                {
                    bool issortold = false;
                    foreach (var item in sortlist)
                    {
                        if (sorttempcol == item.ColumnName)
                        {
                            sort += string.Format(",it.{0} {1}", sorttempcol, sorttempval);
                            if (!item.OrderExpression.Contains(sorttempval)) // nếu là côt sắp xếp cũ nhưng thay đổi trình tự thì gán lại
                            {
                                item.OrderExpression = item.OrderExpression.Replace("desc", sorttempval);
                                item.OrderExpression = item.OrderExpression.Replace("asc", sorttempval);
                            }


                            issortold = true;
                        }
                        else
                        {
                            sort += string.Format(",it.{0} {1} ", item.ColumnName, (item.OrderExpression.Contains("asc") ? "asc" : "desc")); // Nếu không có tham số sắp xếp thì lấy ngầm định cột đầu tiên                                                        
                        }
                    }
                    if (!issortold)
                    {
                        sort += string.Format(",it.{0} {1} ", sorttempcol, sorttempval);
                        MetaObject.GetMetaByColumnName(sorttempcol).OrderExpression = string.Format("{0}.{1}", 101 + sortlist.Count, sorttempval);
                    }
                }

            }
            //xóa dấu , thừa nếu có trong biểu thức sort
            if (sort.Substring(0, 1) == ",")
            {
                sort = sort.Substring(1, sort.Length - 1);
            }

            var exspression = string.Format("({0})", string.Join("AND", exspressions.ToArray()));
            //build the final expression
            if (exspression != "()")
            {
                query = query.Where(exspression, parameters.ToArray()); //filter agents collection on the expression

            }

            //thiết lập phân trang

            int page = request.Params["paging.page"] == null ? MetaObject.Paging.PageCurrent : int.Parse(request.Params["paging.page"]);
            int pagesize = request.Params["paging.pagesize"] == null ? MetaObject.Paging.PageSize : int.Parse(request.Params["paging.pagesize"]);
            MetaObject.Paging.PageSize = pagesize;

            MetaObject.Paging.SetPaging(query.Count(), page);

            objectsList = query.OrderBy(sort).Skip(MetaObject.Paging.Skip).Take(MetaObject.Paging.PageSize).ToList();

            return objectsList;
        }

        public static List<T> GetResultAlls<T>(HttpRequestBase request, string MetaName, IQueryable<T> query)
        {

            var MetaObject = Services.GlobalMeta.GetMetaObject(MetaName);

            List<T> objectsList;
            var exspressions = new List<string>();
            var parameters = new List<ObjectParameter>();
            string sort = "";
            string sorttempval = "";
            string sorttempcol = "";

            bool issortadd = false;
            var sortlist = MetaObject.MetaTable.Where(m => m.OrderExpression != "" && m.OrderExpression != null).OrderBy(m => m.OrderExpression).ToList();


            //gán lại giá trị filter
            foreach (var item in MetaObject.MetaTable)
            {
                //// thiết lập biểu thức lọc
                //string fieldfilter = string.Format("{0}.filter.{1}", MetaName, item.ColumnName);
                //if (request.Params[fieldfilter] != null)
                //{
                //    item.FilterExpression = request.Params[fieldfilter];
                //}

                // thiết lập biểu thức lọc
                string fieldfilter = string.Format("{0}.filter.{1}", MetaName, item.ColumnName);
                string datatype = Services.ExConvert.Sqltype2Systemtype(item.DATA_TYPE);

                if (request.Params[fieldfilter] != null)
                {
                    item.FilterExpression = request.Params[fieldfilter];
                }

                //Nếu kiểu dữ liệu là số hoặc ngày 
                if ("numeric.datetime".Contains(datatype))
                {
                    fieldfilter = string.Format("{0}.filter.{1}.eq", MetaName, item.ColumnName);
                    item.FilterExpression = string.Format("{0};", request.Params[fieldfilter] == null ? "" : request.Params[fieldfilter]);

                    fieldfilter = string.Format("{0}.filter.{1}.gt", MetaName, item.ColumnName);
                    item.FilterExpression += string.Format("{0};", request.Params[fieldfilter] == null ? "" : request.Params[fieldfilter]);

                    fieldfilter = string.Format("{0}.filter.{1}.lt", MetaName, item.ColumnName);
                    item.FilterExpression += string.Format("{0};", request.Params[fieldfilter] == null ? "" : request.Params[fieldfilter]);
                }


                if (!string.IsNullOrEmpty(item.FilterExpression))
                {
                    var expression = new Filter(item).getExpression2();
                    if (expression != null)
                    {
                        exspressions.Add(expression.Expression);
                        parameters.AddRange(expression.Parameters);
                    }
                }

                string fieldsort = string.Format("{0}.sort.{1}", MetaName, item.ColumnName);
                if (request.Params[fieldsort] != null)
                {
                    sorttempval = request.Params[fieldsort];
                    sorttempcol = item.ColumnName;
                    string fieldsortshiftkey = string.Format("{0}.sort.{1}.shiftkey", MetaName, item.ColumnName);
                    issortadd = (request.Params[fieldsortshiftkey] != null);
                }


            }


            if (!issortadd)// nếu không phải multi sort
            {
                if (!string.IsNullOrWhiteSpace(sorttempcol))// nếu tham số sắp xếp có giá trị
                {
                    foreach (var item in sortlist)
                    {
                        item.OrderExpression = "";
                    }
                    sort = string.Format("it.{0} {1}", sorttempcol, sorttempval);
                    MetaObject.GetMetaByColumnName(sorttempcol).OrderExpression = string.Format("101.{0}", sorttempval);
                }
                else
                {
                    if (sortlist.Count == 0)// nếu không có tham số khai báo sẳn thì lấy côt đầu
                    {
                        sort = string.Format("it.{0} asc", MetaObject.MetaTable[0].ColumnName); // Nếu không có tham số sắp xếp thì lấy ngầm định cột đầu tiên
                        MetaObject.MetaTable[0].OrderExpression = "101.asc";
                    }
                    else
                    {
                        foreach (var item in sortlist)
                        {
                            sort += string.Format(",it.{0} {1}", item.ColumnName, (item.OrderExpression.Contains("asc") ? "asc" : "desc")); // Nếu không có tham số sắp xếp thì lấy ngầm định cột đầu tiên                                                        
                        }

                    }
                }

            }
            else // nếu multi sort
            {
                if (!string.IsNullOrWhiteSpace(sorttempcol))// nếu tham số sắp xếp có giá trị
                {
                    bool issortold = false;
                    foreach (var item in sortlist)
                    {
                        if (sorttempcol == item.ColumnName)
                        {
                            sort += string.Format(",it.{0} {1}", sorttempcol, sorttempval);
                            if (!item.OrderExpression.Contains(sorttempval)) // nếu là côt sắp xếp cũ nhưng thay đổi trình tự thì gán lại
                            {
                                item.OrderExpression = item.OrderExpression.Replace("desc", sorttempval);
                                item.OrderExpression = item.OrderExpression.Replace("asc", sorttempval);
                            }


                            issortold = true;
                        }
                        else
                        {
                            sort += string.Format(",it.{0} {1} ", item.ColumnName, (item.OrderExpression.Contains("asc") ? "asc" : "desc")); // Nếu không có tham số sắp xếp thì lấy ngầm định cột đầu tiên                                                        
                        }
                    }
                    if (!issortold)
                    {
                        sort += string.Format(",it.{0} {1} ", sorttempcol, sorttempval);
                        MetaObject.GetMetaByColumnName(sorttempcol).OrderExpression = string.Format("{0}.{1}", 101 + sortlist.Count, sorttempval);
                    }
                }

            }
            //xóa dấu , thừa nếu có trong biểu thức sort
            if (sort.Substring(0, 1) == ",")
            {
                sort = sort.Substring(1, sort.Length - 1);
            }

            var exspression = string.Format("({0})", string.Join("AND", exspressions.ToArray()));
            //build the final expression
            if (exspression != "()")
            {
                query = query.Where(exspression, parameters.ToArray()); //filter agents collection on the expression

            }

            //thiết lập phân trang
            int page = request.Params["paging.page"] == null ? MetaObject.Paging.PageCurrent : int.Parse(request.Params["paging.page"]);
            int pagesize = request.Params["paging.pagesize"] == null ? MetaObject.Paging.PageSize : int.Parse(request.Params["paging.pagesize"]);
            MetaObject.Paging.PageSize = pagesize;

            //int page = request.Params["paging.page"] == null ? -1 : int.Parse(request.Params["paging.page"]);

            MetaObject.Paging.SetPaging(query.Count(), page);

            objectsList = query.OrderBy(sort).ToList();

            return objectsList;
        }

        #region Nested type: Filter

        public class Filter
        {
            public string Datacomparison { get; set; }
            public string DataType { get; set; }
            public string DataValue { get; set; }
            public string Field { get; set; }

            public int Id { get; set; }// định bỏ

            //public static bool checkExistence(int filterIndex, NameValueCollection @params)
            //{
            //    return (@params[string.Format("filter[{0}][field]", filterIndex)] != null);
            //}

            public Filter(Models.SysTableDetailView metacolumn)
            {
                //Id = id;
                Field = metacolumn.ColumnName;
                DataType = Services.ExConvert.Sqltype2Systemtype(metacolumn.DATA_TYPE);
                DataValue = metacolumn.FilterExpression;
                Datacomparison = "=";
            }

            public FilterExpressionResult getExpression()
            {

                string expressionString = null;
                var expressionParams = new List<ObjectParameter>(); //paramerters collection

                char[] delimiterChars = { ';' };
                string[] arrexpression = DataValue == null ? null : DataValue.Split(delimiterChars);


                switch (DataType)
                {
                    case "string":
                        for (int i = 0; i < arrexpression.Length; i++)
                        {
                            switch (i)
                            {
                                case 0:
                                    expressionString = string.Format("(it.{0} like N'{1}%')", Field, arrexpression[i]);
                                    break;
                                case 1:
                                    expressionString = string.Format("(it.{0} not like N'{1}%')", Field, arrexpression[i]);
                                    break;
                                default:
                                    break;
                            }
                        }
                        //expressionString = string.Format("(it.{0} like N'{1}%')", Field, DataValue);
                        break;
                    case "guid":
                        for (int i = 0; i < arrexpression.Length; i++)
                        {
                            switch (i)
                            {
                                case 0:
                                    expressionString = string.Format("(it.{0} = Guid\'{1}\')", Field, arrexpression[i]);
                                    break;
                                case 1:
                                    expressionString = string.Format("(it.{0} != Guid\'{1}\')", Field, arrexpression[i]);
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case "boolean":
                        expressionString = string.Format("(it.{0} = {1})", Field, DataValue);
                        break;
                    case "numeric":
                        if (arrexpression != null)
                        {

                            for (int i = 0; i < 3; i++)
                            {
                                switch (i)
                                {
                                    case 0:
                                        if (!string.IsNullOrWhiteSpace(arrexpression[i]))
                                        {
                                            expressionString = string.Format("(it.{0} = {1})", Field, System.Convert.ToDecimal(arrexpression[i], System.Threading.Thread.CurrentThread.CurrentCulture));
                                        }
                                        break;
                                    case 1:
                                        if (!string.IsNullOrWhiteSpace(arrexpression[i]))
                                        {
                                            if (!string.IsNullOrWhiteSpace(expressionString))
                                            {
                                                expressionString += " and ";
                                            }
                                            expressionString += string.Format("(it.{0} > {1})", Field, System.Convert.ToDecimal(arrexpression[i], System.Threading.Thread.CurrentThread.CurrentCulture));
                                        }
                                        break;
                                    case 2:
                                        if (!string.IsNullOrWhiteSpace(arrexpression[i]))
                                        {
                                            if (!string.IsNullOrWhiteSpace(expressionString))
                                            {
                                                expressionString += " and ";
                                            }
                                            expressionString += string.Format("(it.{0} < {1})", Field, System.Convert.ToDecimal(arrexpression[i], System.Threading.Thread.CurrentThread.CurrentCulture));
                                            //expressionString += string.Format("(it.{0} < {1})", Field, arrexpression[i]);
                                        }
                                        break;
                                    default:
                                        break;
                                }

                            }
                        }
                        //else
                        //{
                        //    expressionString = string.Format("(it.{0} {2} {1})", Field, DataValue, Datacomparison);
                        //}
                        //switch (Datacomparison)
                        //{
                        //    case "gt":
                        //        Datacomparison = ">";
                        //        break;
                        //    case "lt":
                        //        Datacomparison = "<";
                        //        break;
                        //    default:
                        //        Datacomparison = "=";
                        //        break;
                        //}
                        //expressionString = string.Format("(it.{0} {2} {1})", Field, DataValue, Datacomparison);
                        break;
                    case "datetime":

                        if (arrexpression != null)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                switch (i)
                                {
                                    case 0:
                                        if (!string.IsNullOrWhiteSpace(arrexpression[i]))
                                        {

                                            expressionParams.Add(new ObjectParameter("Param" + Field + "eq", DateTime.Parse(arrexpression[i])));
                                            expressionString = string.Format("(it.{0} {2} {1})", Field, "@" + "Param" + Field + "eq", "=");
                                        }
                                        break;
                                    case 1:
                                        if (!string.IsNullOrWhiteSpace(arrexpression[i]))
                                        {
                                            if (!string.IsNullOrWhiteSpace(expressionString))
                                            {
                                                expressionString += " and ";
                                            }
                                            expressionParams.Add(new ObjectParameter("Param" + Field + "gt", DateTime.Parse(arrexpression[i])));
                                            expressionString += string.Format("(it.{0} {2} {1})", Field, "@" + "Param" + Field + "gt", ">");
                                        }
                                        break;
                                    case 2:
                                        if (!string.IsNullOrWhiteSpace(arrexpression[i]))
                                        {
                                            if (!string.IsNullOrWhiteSpace(expressionString))
                                            {
                                                expressionString += " and ";
                                            }
                                            expressionParams.Add(new ObjectParameter("Param" + Field + "lt", DateTime.Parse(arrexpression[i])));
                                            expressionString += string.Format("(it.{0} {2} {1})", Field, "@" + "Param" + Field + "lt", "<");
                                        }
                                        break;
                                    default:
                                        break;
                                }

                            }
                        }
                        //else
                        //{
                        //    expressionString = string.Format("(it.{0} {2} {1})", Field, DataValue, Datacomparison);
                        //}

                        //switch (Datacomparison)
                        //{
                        //    case "gt":
                        //        Datacomparison = ">";
                        //        break;
                        //    case "lt":
                        //        Datacomparison = "<";
                        //        break;
                        //    default:
                        //        Datacomparison = "=";
                        //        break;
                        //}

                        //expressionParams.Add(new ObjectParameter("Param" + Id, DateTime.Parse(DataValue)));
                        //expressionString = string.Format("(it.{0} {2} {1})", Field, "@" + "Param" + Id, Datacomparison);
                        break;
                    case "list":
                        var split = DataValue.Split(new[] { ',' });
                        var r = new string[split.Length];
                        for (var i = 0; i < split.Length; i++)
                        {
                            r[i] = string.Format("(it.{0} = '{1}')", Field, split[i]);
                        }
                        expressionString = string.Format("({0})", string.Join("OR", r));
                        break;
                }
                return expressionString != null
                           ? new FilterExpressionResult { Expression = expressionString, Parameters = expressionParams }
                           : null;
            }

            //Get bieu thuc cho IQUeryAble
            public FilterExpressionResult getExpression2()
            {
                string expressionString = null;
                var expressionParams = new List<ObjectParameter>(); //paramerters collection
                char[] delimiterChars = { ';' };
                string[] arrexpression = DataValue == null ? null : DataValue.Split(delimiterChars);

                switch (DataType)
                {
                    case "string":
                        expressionString = string.Format("(it.{0} != null && it.{0}.Contains(\"{1}\"))", Field, DataValue);
                        break;
                    case "guid":
                        for (int i = 0; i < arrexpression.Length; i++)
                        {
                            switch (i)
                            {
                                case 0:
                                    expressionString = string.Format("(it.{0} = Guid\'{1}\')", Field, arrexpression[i]);
                                    break;
                                case 1:
                                    expressionString = string.Format("(it.{0} != Guid\'{1}\')", Field, arrexpression[i]);
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    case "boolean":
                        expressionString = string.Format("(it.{0} = {1})", Field, DataValue);
                        break;
                    case "numeric":
                        if (arrexpression != null)
                        {

                            for (int i = 0; i < 3; i++)
                            {
                                switch (i)
                                {
                                    case 0:
                                        if (!string.IsNullOrWhiteSpace(arrexpression[i]))
                                        {
                                            expressionString = string.Format("(it.{0} = {1})", Field, System.Convert.ToDecimal(arrexpression[i], System.Threading.Thread.CurrentThread.CurrentCulture));
                                        }
                                        break;
                                    case 1:
                                        if (!string.IsNullOrWhiteSpace(arrexpression[i]))
                                        {
                                            if (!string.IsNullOrWhiteSpace(expressionString))
                                            {
                                                expressionString += " and ";
                                            }
                                            expressionString += string.Format("(it.{0} > {1})", Field, System.Convert.ToDecimal(arrexpression[i], System.Threading.Thread.CurrentThread.CurrentCulture));
                                        }
                                        break;
                                    case 2:
                                        if (!string.IsNullOrWhiteSpace(arrexpression[i]))
                                        {
                                            if (!string.IsNullOrWhiteSpace(expressionString))
                                            {
                                                expressionString += " and ";
                                            }
                                            expressionString += string.Format("(it.{0} < {1})", Field, System.Convert.ToDecimal(arrexpression[i], System.Threading.Thread.CurrentThread.CurrentCulture));
                                            //expressionString += string.Format("(it.{0} < {1})", Field, arrexpression[i]);
                                        }
                                        break;
                                    default:
                                        break;
                                }

                            }
                        }
                        break;
                    case "datetime":

                        if (arrexpression != null)
                        {
                            for (int i = 0; i < 3; i++)
                            {
                                string strdate = "";
                                if (!string.IsNullOrWhiteSpace(arrexpression[i]))
                                {
                                    DateTime d1 = DateTime.Parse(arrexpression[i]);
                                    strdate = string.Format("DateTime({0},{1},{2})", d1.Year, d1.Month, d1.Day);
                                }
                                switch (i)
                                {
                                    case 0:
                                        if (!string.IsNullOrWhiteSpace(arrexpression[i]))
                                        {
                                            expressionString = string.Format("(it.{0} {2} {1})", Field, strdate, "=");
                                        }
                                        break;
                                    case 1:
                                        if (!string.IsNullOrWhiteSpace(arrexpression[i]))
                                        {
                                            if (!string.IsNullOrWhiteSpace(expressionString))
                                            {
                                                expressionString += " and ";
                                            }
                                            expressionString += string.Format("(it.{0} {2} {1})", Field, strdate, ">");
                                        }
                                        break;
                                    case 2:
                                        if (!string.IsNullOrWhiteSpace(arrexpression[i]))
                                        {
                                            if (!string.IsNullOrWhiteSpace(expressionString))
                                            {
                                                expressionString += " and ";
                                            }
                                            expressionString += string.Format("(it.{0} {2} {1})", Field, strdate, "<");
                                            //expressionString = "OrderDate < DateTime(2007, 1, 1)";
                                        }
                                        break;
                                    default:
                                        break;
                                }

                            }
                        }
                        break;
                    case "list":
                        var split = DataValue.Split(new[] { ',' });
                        var r = new string[split.Length];
                        for (var i = 0; i < split.Length; i++)
                        {
                            r[i] = string.Format("(it.{0} = '{1}')", Field, split[i]);
                        }
                        expressionString = string.Format("({0})", string.Join("OR", r));
                        break;
                }
                return expressionString != null
                           ? new FilterExpressionResult { Expression = expressionString, Parameters = expressionParams }
                           : null;
            }

            #region Nested type: FilterExpressionResult

            public class FilterExpressionResult
            {
                public string Expression { get; set; }
                public List<ObjectParameter> Parameters { get; set; }
            }

            #endregion
        }

        #endregion

        //Dành cho CMS
        public static List<T> GetResults<T>(HttpRequestBase request, string MetaName, ObjectQuery<T> query, string stringorder, int pagesize, string keys = "")
        {

            var MetaObject = Services.GlobalMeta.GetMetaObject(MetaName);

            List<T> objectsList;
            var exspressions = new List<string>();
            var parameters = new List<ObjectParameter>();
            string sort = "";
            string sorttempval = "";
            string sorttempcol = "";

            bool issort = false;

            //gán lại giá trị filter
            foreach (var item in MetaObject.MetaTable)
            {

                // thiết lập biểu thức lọc
                string fieldfilter = string.Format("{0}.filter.{1}", MetaName, item.ColumnName);
                string datatype = Services.ExConvert.Sqltype2Systemtype(item.DATA_TYPE);


                if (request.Params[fieldfilter] != null)
                {
                    item.FilterExpression = request.Params[fieldfilter];
                }

                //Nếu kiểu dữ liệu là số hoặc ngày 
                if ("numeric.datetime".Contains(datatype))
                {
                    if (
                        request.Params[string.Format("{0}.filter.{1}.eq", MetaName, item.ColumnName)] != null ||
                        request.Params[string.Format("{0}.filter.{1}.gt", MetaName, item.ColumnName)] != null ||
                        request.Params[string.Format("{0}.filter.{1}.lt", MetaName, item.ColumnName)] != null
                        )
                    {

                        fieldfilter = string.Format("{0}.filter.{1}.eq", MetaName, item.ColumnName);
                        item.FilterExpression = string.Format("{0};", request.Params[fieldfilter] == null ? "" : request.Params[fieldfilter]);

                        fieldfilter = string.Format("{0}.filter.{1}.gt", MetaName, item.ColumnName);
                        item.FilterExpression += string.Format("{0};", request.Params[fieldfilter] == null ? "" : request.Params[fieldfilter]);

                        fieldfilter = string.Format("{0}.filter.{1}.lt", MetaName, item.ColumnName);
                        item.FilterExpression += string.Format("{0};", request.Params[fieldfilter] == null ? "" : request.Params[fieldfilter]);

                    }

                    //fieldfilter = string.Format("{0}.filter.{1}.eq", MetaName, item.ColumnName);
                    //item.FilterExpression = string.Format("{0};", request.Params[fieldfilter] == null ? "" : request.Params[fieldfilter]);

                    //fieldfilter = string.Format("{0}.filter.{1}.gt", MetaName, item.ColumnName);
                    //item.FilterExpression += string.Format("{0};", request.Params[fieldfilter] == null ? "" : request.Params[fieldfilter]);

                    //fieldfilter = string.Format("{0}.filter.{1}.lt", MetaName, item.ColumnName);
                    //item.FilterExpression += string.Format("{0};", request.Params[fieldfilter] == null ? "" : request.Params[fieldfilter]);
                }

                if (!string.IsNullOrEmpty(item.FilterExpression))
                {
                    var expression = new Filter(item).getExpression();
                    if (expression != null)
                    {
                        exspressions.Add(expression.Expression);
                        parameters.AddRange(expression.Parameters);
                    }
                }

                // thiết lập biểu thức sắp xếp
                if (!string.IsNullOrEmpty(item.OrderExpression))
                {
                    sorttempval = item.OrderExpression;
                    sorttempcol = item.ColumnName;
                }

                string fieldsort = string.Format("{0}.sort.{1}", MetaName, item.ColumnName);
                if (request.Params[fieldsort] != null)
                {
                    item.OrderExpression = request.Params[fieldsort];
                    issort = true;
                }
                else // tạm thời chỉ sort 1 trường
                {
                    item.OrderExpression = "";
                }

                if (!string.IsNullOrEmpty(item.OrderExpression))
                {
                    sort = string.Format("it.{0} {1}", item.ColumnName, item.OrderExpression);
                }


            }

            if (!issort && !string.IsNullOrWhiteSpace(sorttempcol))
            {
                MetaObject.GetMetaByColumnName(sorttempcol).OrderExpression = sorttempval;
                sort = string.Format("it.{0} {1}", sorttempcol, sorttempval);
            }

            if (!string.IsNullOrWhiteSpace(stringorder))
            {
                char[] delimiterChars = { ',' };
                string[] arrorder = stringorder.Trim().Split(delimiterChars);
                sort = "";
                for (int i = 0; i < arrorder.Count(); i++)
                {
                    sort += string.Format(",it.{0}", arrorder[i]); // Nếu không có tham số sắp xếp thì lấy ngầm định cột đầu tiên
                    if (arrorder[i].Contains("asc"))
                    {
                        MetaObject.MetaTable[0].OrderExpression = "asc";
                    }
                    else
                    {
                        MetaObject.MetaTable[0].OrderExpression = "desc";
                    }
                }
                //xóa dấu , thừa nếu có trong biểu thức sort
                if (sort.Substring(0, 1) == ",")
                {
                    sort = sort.Substring(1, sort.Length - 1);
                }

            }
            if (!string.IsNullOrWhiteSpace(keys))
            {
                exspressions.Add(keys);
            }
            var exspression = string.Format("({0})", string.Join("AND", exspressions.ToArray()));
            //build the final expression
            if (exspression != "()")
            {
                query = query.Where(exspression, parameters.ToArray()); //filter agents collection on the expression
            }

            //thiết lập phân trang






            //int page = request.Params["paging.page"] == null ? -1 : int.Parse(request.Params["paging.page"]);
            //MetaObject.Paging.PageSize = pagesize;
            int page = request.Params["paging.page"] == null ? MetaObject.Paging.PageCurrent : int.Parse(request.Params["paging.page"]);
            MetaObject.Paging.PageSize = pagesize;

            MetaObject.Paging.SetPaging(query.Count(), page);

            objectsList = query.OrderBy(sort).Skip(MetaObject.Paging.Skip).Take(MetaObject.Paging.PageSize).ToList();

            return objectsList;
        }





    }
}