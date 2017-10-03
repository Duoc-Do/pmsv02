using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace WebApp.Areas.Accounting.Services
{
    public static class ExConvert
    {
        //public static string String2Unicode<T>(this T value)
        //{
        //    return HttpUtility.HtmlDecode(value).ToString();
        //    ////các tham số key, langue
        //    //string resourceName = value.ToString();


        //    //char[] chars = HttpUtility.HtmlEncode(resourceName).ToCharArray();
        //    //StringBuilder result = new StringBuilder(resourceName.Length + (int)(resourceName.Length * 0.1));

        //    //foreach (char c in chars)
        //    //{
        //    //    int _value =System.Convert.ToInt32(c);
        //    //    if (_value > 127)
        //    //        result.AppendFormat("&#{0};", _value);
        //    //    else
        //    //        result.Append(c);
        //    //}

        //    //return result.ToString();

        //}

        public static string Price2String(object value)
        {

            return Data2String(value, "System.Decimal", "c", "CIUPCC");
        }

        public static string Quantity2String(object value)
        {
            return Data2String(value, "System.Decimal", "n", "CICC");
        }

        public static string Data2String(object value, string datatype)
        {
            return Data2String(value, datatype, "", "CICC");
        }

        public static object String2Data(string value, string datatype)
        {
            switch (datatype)
            {
                case "boolean":
                case "System.Boolean":
                    return Convert.ToBoolean(value);
                case "System.Nullable`1[System.DateTime]":
                case "System.DateTime":
                case "datetime":
                    return Convert.ToDateTime(value);
                case "numeric":
                case "System.Nullable`1[System.Decimal]":
                case "System.Decimal":
                    return Convert.ToDecimal(value);
                case "System.Nullable`1[System.Int32]":
                case "System.Int32":
                case "int":
                    return Convert.ToInt32(value);
                default: // nếu là string
                    return value;
            }
        }


        public static string Data2String(object value, string datatype, string formatvalue)
        {
            return Data2String(value, datatype, formatvalue, "CICC");
        }
        public static string Data2String(object value, string datatype, string formatvalue, string cultureinfo)
        {
            //return "";
            if (string.IsNullOrWhiteSpace(cultureinfo)) { cultureinfo = "CICC"; }
            System.Globalization.CultureInfo _cultureinfo = (System.Globalization.CultureInfo)Services.GlobalVariant.GetCultureInfo()[cultureinfo];

            if (string.IsNullOrWhiteSpace(formatvalue))
            {
                switch (datatype)
                {
                    case "System.Boolean":
                        if (value.ToString() != "")
                        {
                            if ((bool)value)
                            {
                                return "true";
                            }
                        }
                        return "false";
                    case "boolean":
                        if (value != null)
                        {

                            if (value.ToString() != "")
                            {
                                if ((bool)value)
                                {
                                    return "true";
                                }
                            }
                        }
                        return "false";

                    case "System.Nullable`1[System.DateTime]":
                    case "System.DateTime":
                    case "datetime":
                        return string.Format(_cultureinfo, "{0:d}", value);
                    case "numeric":
                    case "System.Nullable`1[System.Decimal]":
                    case "System.Decimal":
                        return string.Format(_cultureinfo, "{0:n}", value);

                    case "System.Nullable`1[System.Int32]":
                    case "System.Int32":
                        return string.Format(_cultureinfo, "{0:n0}", value);
                    default: // nếu là string
                        return string.Format("{0}", value);
                }
            }
            else
            {
                string _formatstring = string.Format("{0}{1}{2}", "{0:", formatvalue, "}");
                return string.Format(_cultureinfo, _formatstring, value).Trim();
            }

        }
        public static int SetDecimalDigitsEx(string formatvalue)
        {
            int DecimalDigits = 0;
            if ((formatvalue.Contains("n") || formatvalue.Contains("f")) && formatvalue.Length == 2)
            {
                return int.Parse(formatvalue.Substring(1));
            }

            return DecimalDigits;
        }
        public static int SetDecimalDigits(string formatvalue, string cultureinfo)
        {
            int DecimalDigits = 0;
            if (string.IsNullOrWhiteSpace(cultureinfo)) { cultureinfo = "CICC"; }
            System.Globalization.CultureInfo _cultureinfo = (System.Globalization.CultureInfo)Services.GlobalVariant.GetCultureInfo()[cultureinfo];

            if (!string.IsNullOrWhiteSpace(formatvalue))
            {
                switch (formatvalue)
                {
                    case "n":
                        DecimalDigits = _cultureinfo.NumberFormat.NumberDecimalDigits; 
                        break;
                    case "f":
                        DecimalDigits = _cultureinfo.NumberFormat.NumberDecimalDigits;
                        break;
                    case "c":
                        DecimalDigits = _cultureinfo.NumberFormat.CurrencyDecimalDigits;
                        break;
                    default:
                        return SetDecimalDigitsEx(formatvalue);
                }

            }
            else
            {
                DecimalDigits = _cultureinfo.NumberFormat.NumberDecimalDigits;
            }

            return DecimalDigits;

            //n
            //f0
            //p0
            //f
            //c
            //n2
            //n0
            //n4
            //c0
        }

        //Loại bỏ dấu
        public static string RemoveDiacritics(string stIn)
        {
            string stFormD = stIn.Normalize(System.Text.NormalizationForm.FormD);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            for (int ich = 0; ich < stFormD.Length; ich++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[ich]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[ich]);
                }
            }

            return (sb.ToString().Normalize(System.Text.NormalizationForm.FormC));
        }

        public static string UrlNonDiacritics(string stIn)
        {
            string url = RemoveDiacritics(stIn.Trim());
            url = System.Text.RegularExpressions.Regex.Replace(url, " ", "-");
            url = System.Text.RegularExpressions.Regex.Replace(url, @"Đ", "D");
            url = System.Text.RegularExpressions.Regex.Replace(url, @"đ", "d");
            return System.Text.RegularExpressions.Regex.Replace(url, @"[^a-zA-Z0-9\s-]", "").ToLower();

        }

        #region Sql
        public static string Sqltype2Systemtype(string datatype)
        {
            //image
            //int
            //decimal
            //smallint
            //datetime
            //uniqueidentifier
            //varchar
            //NULL
            //nchar
            //smalldatetime
            //char
            //bit
            //ntext
            //nvarchar

            switch (datatype)
            {
                case "nvarchar":
                    return "string";
                case "varchar":
                    return "string";
                case "char":
                    return "string";
                case "nchar":
                    return "string";
                case "ntext":
                    return "string";
                case "image":
                    return "string";
                case "int":
                    return "int";
                case "decimal":
                    return "numeric";
                case "smallint":
                    return "int";
                case "datetime":
                    return "datetime";

                case "uniqueidentifier":
                    return "guid";
                case "smalldatetime":
                    return "datetime";
                case "bit":
                    return "boolean";

                default:
                    return "string";
            }

        }

        public static SqlDbType Convert2SqlDbType(string ptype)
        {

            switch (ptype)
            {
                case "image": return SqlDbType.Image;
                case "int": return SqlDbType.Int;
                case "decimal": return SqlDbType.Decimal;
                case "varbinary": return SqlDbType.VarBinary;
                case "text": return SqlDbType.Text;
                case "smallint": return SqlDbType.SmallInt;
                case "datetime": return SqlDbType.DateTime;
                case "varchar": return SqlDbType.VarChar;
                case "nchar": return SqlDbType.NChar;
                case "smalldatetime": return SqlDbType.SmallDateTime;
                case "char": return SqlDbType.Char;
                case "bigint": return SqlDbType.BigInt;
                case "bit": return SqlDbType.Bit;
                case "ntext": return SqlDbType.NText;
                case "nvarchar": return SqlDbType.NVarChar;
                default:
                    return SqlDbType.NVarChar;
            }
        }

        public static SqlParameter ParseSqlParam(dynamic data, Models.SysTableDetailView columnitem)
        {
            return ParseSqlParam(data, columnitem, columnitem.ColumnName, ParameterDirection.Input);
        }

        public static SqlParameter ParseSqlParam(dynamic data, Models.SysTableDetailView columnitem, ParameterDirection direction)
        {
            return ParseSqlParam(data, columnitem, columnitem.ColumnName, direction);
        }

        public static SqlParameter ParseSqlParam(dynamic data, Models.SysTableDetailView columnitem, string paraName)
        {
            return ParseSqlParam(data, columnitem, paraName, ParameterDirection.Input);
        }

        public static SqlParameter ParseSqlParam(dynamic data, Models.SysTableDetailView columnitem, string paraName, ParameterDirection direction)
        {

            var sqlparameter = new SqlParameter();
            sqlparameter.Direction = direction;
            sqlparameter.ParameterName = string.Format("@{0}", paraName);

            var value = data.GetType().GetProperty(columnitem.ColumnName).GetValue(data, null) ?? String.Empty;
            if (string.IsNullOrEmpty(value.ToString()))
            {
                sqlparameter.SqlDbType = Convert2SqlDbType(columnitem.DATA_TYPE);
                sqlparameter.Value = DBNull.Value;
            }
            else
            {
                sqlparameter.Value = value;
            }

            return sqlparameter;

        }

        public static List<SqlParameter> Data2SqlParam(dynamic data, Models.MetaObject metaobject)
        {
            return Data2SqlParam(data, metaobject, "",null);
        }
        public static List<SqlParameter> Data2SqlParam(dynamic data, Models.MetaObject metaobject, string outputparaname)
        {
            return Data2SqlParam(data, metaobject, outputparaname, null);
        }
        public static List<SqlParameter> Data2SqlParam(dynamic data, Models.MetaObject metaobject,SqlParameter parameter)
        {
            return Data2SqlParam(data, metaobject, "", parameter);
        }
        public static List<SqlParameter> Data2SqlParam(dynamic data, Models.MetaObject metaobject, string outputparaname, SqlParameter parameter)
        {
            List<SqlParameter> parameterlist = new List<SqlParameter>();
            outputparaname = string.Format("@{0}", outputparaname);

            foreach (var item in metaobject.MetaTable)
            {
                SqlParameter para = ExConvert.ParseSqlParam(data, item);
                if (outputparaname==para.ParameterName) 
                {
                    //tạm thời sử dụng 1 output
                    para.Direction = ParameterDirection.Output;
                }
                parameterlist.Add(para);
            }
            if (parameter!=null)
            {
                parameterlist.Add(parameter);                
            }
            return parameterlist;
        }

        public static object GetValueSqlParam(this SqlParameter[] parameters, string fieldname)
        {
            fieldname = string.Format("@{0}", fieldname);
            var presult = parameters.Where(m => m.ParameterName == fieldname).First();
            if (presult != null)
            {
                return presult.Value;
            }
            return null;
        }

        #endregion



        // chuyển khoảng thời gian

        /// <summary>
        /// Relative formatting of DateTime (e.g. 2 hours ago, a month ago)
        /// </summary>
        /// <param name="source">Source (UTC format)</param>
        /// <returns>Formatted date and time string</returns>
        public static string RelativeFormat(DateTime source)
        {
            return RelativeFormat(source, string.Empty);
        }

        /// <summary>
        /// Relative formatting of DateTime (e.g. 2 hours ago, a month ago)
        /// </summary>
        /// <param name="source">Source (UTC format)</param>
        /// <param name="defaultFormat">Default format string (in case relative formatting is not applied)</param>
        /// <returns>Formatted date and time string</returns>
        public static string RelativeFormat(DateTime source, string defaultFormat)
        {
            return RelativeFormat(source, false, defaultFormat);
        }

        /// <summary>
        /// Relative formatting of DateTime (e.g. 2 hours ago, a month ago)
        /// </summary>
        /// <param name="source">Source (UTC format)</param>
        /// <param name="convertToUserTime">A value indicating whether we should convet DateTime instance to user local time (in case relative formatting is not applied)</param>
        /// <param name="defaultFormat">Default format string (in case relative formatting is not applied)</param>
        /// <returns>Formatted date and time string</returns>
        public static string RelativeFormat(DateTime source,
            bool convertToUserTime, string defaultFormat)
        {
            string result = "";

            var ts = new TimeSpan(DateTime.Now.Ticks - source.Ticks);
            double delta = ts.TotalSeconds;

            if (delta > 0)
            {
                if (delta < 60) // 60 (seconds)
                {
                    //result = ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";
                    result = ts.Seconds == 1 ? "một giây trước" : ts.Seconds + " giây trước";
                }
                else if (delta < 120) //2 (minutes) * 60 (seconds)
                {
                    //result = "a minute ago";
                    result = "một phút trước";
                }
                else if (delta < 2700) // 45 (minutes) * 60 (seconds)
                {
                    //result = ts.Minutes + " minutes ago";
                    result = ts.Minutes + " phút trước";
                }
                else if (delta < 5400) // 90 (minutes) * 60 (seconds)
                {
                    //result = "an hour ago";
                    result = "một giờ trước";
                }
                else if (delta < 86400) // 24 (hours) * 60 (minutes) * 60 (seconds)
                {
                    int hours = ts.Hours;
                    if (hours == 1)
                        hours = 2;
                    //result = hours + " hours ago";
                    result = hours + " giờ trước";
                }
                else if (delta < 172800) // 48 (hours) * 60 (minutes) * 60 (seconds)
                {
                    //result = "yesterday";
                    result = "hôm qua";
                }
                else if (delta < 2592000) // 30 (days) * 24 (hours) * 60 (minutes) * 60 (seconds)
                {
                    //result = ts.Days + " days ago";
                    result = ts.Days + " ngày trước";
                }
                else if (delta < 31104000) // 12 (months) * 30 (days) * 24 (hours) * 60 (minutes) * 60 (seconds)
                {
                    int months = System.Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                    //result = months <= 1 ? "one month ago" : months + " months ago";
                    result = months <= 1 ? "một tháng trước" : months + " tháng trước";
                }
                else
                {
                    int years = System.Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                    //result = years <= 1 ? "one year ago" : years + " years ago";
                    result = years <= 1 ? "một năm trước" : years + " năm trước";
                }
            }
            else
            {
                DateTime tmp1 = source;
                //if (convertToUserTime)
                //{
                //    tmp1 = EngineContext.Current.Resolve<IDateTimeHelper>().ConvertToUserTime(tmp1, DateTimeKind.Utc);
                //}
                //default formatting
                if (!String.IsNullOrEmpty(defaultFormat))
                {
                    result = tmp1.ToString(defaultFormat);
                }
                else
                {
                    result = tmp1.ToString();
                }
            }
            return result;
        }
    }

    public static class Extension
    {

        public static string Currency(this decimal value)
        {
            return ExConvert.Data2String(value, "Numeric", "c", "CICC");
        }

        public static string Quantity(this decimal value)
        {
            return ExConvert.Quantity2String(value);
        }

        public static string Price(this decimal value)
        {
            return ExConvert.Price2String(value);
        }

        public static string ToLocalDateString(this DateTime date)
        {
            return date.ToString("dd/MM/yyyy");
        }

        public static string ToLocalDateTimeString(this DateTime date)
        {
            return date.ToString("dd/MM/yyyy HH:mm:ss");
        }

        public static string RelativeFormat(this DateTime value)
        {
            return ExConvert.RelativeFormat(value);
        }

        public static DateTime EndOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
        }

        public static DateTime StartOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }

        public static DateTime EndOfMonth(this DateTime date)
        {
            var date2 = StartOfMonth(date.AddMonths(1)).AddDays(-1);
            return new DateTime(date2.Year, date2.Month, date2.Day, 23, 59, 59, 999);
        }
        public static bool IsEndOfMonth(this DateTime date)
        {
            var date2 = StartOfMonth(date.AddMonths(1)).AddDays(-1);
            return date2.Day == date.Day;
        }
        public static DateTime StartOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1, 0, 0, 0, 0);
        }

    }
}