using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BZY.OA.Common
{
    /// <summary>
    /// 转换Json格式帮助类
    /// </summary>
    public static class JsonHelper
    {
        public static object ToJson(this string json)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                // 设置为驼峰命名
                //ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return JsonConvert.DeserializeObject(json, serializerSettings);
        }

        public static string ToJson(this object obj)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                // 设置为驼峰命名
                //ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            return JsonConvert.SerializeObject(obj, serializerSettings);
        }

        /// <summary>
        /// 生成JSON字符串
        /// </summary>
        /// <param name="obj">生成json的对象</param>
        /// <param name="formatJson">是否格式化</param>
        /// <param name="dateFormat">日期的格式（默认为：yyyy-MM-dd，也可以设置其他的如：yyyy-MM-dd HH:mm:ss）</param>
        /// <returns></returns>
        public static string ToJson(this object obj, bool formatJson = false, string dateFormat = "yyyy-MM-dd")
        {
            var serializerSettings = new JsonSerializerSettings
            {
                // 设置为驼峰命名
                //ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Converters = new List<JsonConverter>
                {
                    new IsoDateTimeConverter
                    {
                        DateTimeFormat = dateFormat
                    }
                }
            };

            return formatJson ? JsonConvert.SerializeObject(obj, Formatting.Indented, serializerSettings)
                : JsonConvert.SerializeObject(obj, serializerSettings);
        }

        public static string ToApiJson(this object obj, bool formatJson = false)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                // 设置为驼峰命名
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                //使用返回时间为
                DateTimeZoneHandling = DateTimeZoneHandling.Local,
                Converters = new List<JsonConverter>
                {
                    new UnixDateTimeConvertor()
                }
            };

            return formatJson ? JsonConvert.SerializeObject(obj, Formatting.Indented, serializerSettings)
                : JsonConvert.SerializeObject(obj, serializerSettings);
        }

        public static DataTable JsonToDataTable(this string strJson)
        {
            #region
            DataTable tb = null;
            //获取数据  
            Regex rg = new Regex(@"(?<={)[^}]+(?=})");
            MatchCollection mc = rg.Matches(strJson);
            for (int i = 0; i < mc.Count; i++)
            {
                string strRow = mc[i].Value;
                string[] strRows = strRow.Split(',');
                //创建表  
                if (tb == null)
                {
                    tb = new DataTable();
                    tb.TableName = "Table";
                    foreach (string str in strRows)
                    {
                        DataColumn dc = new DataColumn();
                        string[] strCell = str.Split(':');
                        dc.DataType = typeof(String);
                        dc.ColumnName = strCell[0].ToString().Replace("\"", "").Trim();
                        tb.Columns.Add(dc);
                    }
                    tb.AcceptChanges();
                }
                //增加内容  
                DataRow dr = tb.NewRow();
                for (int r = 0; r < strRows.Length; r++)
                {
                    object strText = strRows[r].Split(':')[1].Trim().Replace("，", ",").Replace("：", ":").Replace("/", "").Replace("\"", "").Trim();
                    if (strText.ToString().Length >= 5)
                    {
                        if (strText.ToString().Substring(0, 5) == "Date(")//判断是否JSON日期格式
                        {
                            strText = JsonToDateTime(strText.ToString()).ToString("yyyy-MM-dd HH:mm:ss");
                        }
                    }
                    dr[r] = strText;
                }
                tb.Rows.Add(dr);
                tb.AcceptChanges();
            }
            return tb;
            #endregion
        }

        public static List<T> JsonToList<T>(this string json)
        {
            return JsonConvert.DeserializeObject<List<T>>(json);
        }

        public static T JsonToEntity<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        public static JObject ToJObject(this string json)
        {
            return json == null ? JObject.Parse("{}") : JObject.Parse(json.Replace("&nbsp;", ""));
        }

        public static T ToObject<T>(this string json)
        {
            return json == null ? default(T) : JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// json 的日期格式与.Net DateTime类型的转换
        /// </summary>
        /// <param name="jsonDate">Date(1242357713797+0800)</param>
        /// <returns></returns>
        public static DateTime JsonToDateTime(string jsonDate)
        {
            string value = jsonDate.Substring(5, jsonDate.Length - 6) + "+0800";
            DateTimeKind kind = DateTimeKind.Utc;
            int index = value.IndexOf('+', 1);
            if (index == -1)
                index = value.IndexOf('-', 1);
            if (index != -1)
            {
                kind = DateTimeKind.Local;
                value = value.Substring(0, index);
            }
            long javaScriptTicks = long.Parse(value, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture);
            long InitialJavaScriptDateTicks = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks;
            DateTime utcDateTime = new DateTime((javaScriptTicks * 10000) + InitialJavaScriptDateTicks, DateTimeKind.Utc);
            DateTime dateTime;
            switch (kind)
            {
                case DateTimeKind.Unspecified:
                    dateTime = DateTime.SpecifyKind(utcDateTime.ToLocalTime(), DateTimeKind.Unspecified);
                    break;
                case DateTimeKind.Local:
                    dateTime = utcDateTime.ToLocalTime();
                    break;
                default:
                    dateTime = utcDateTime;
                    break;
            }
            return dateTime;
        }
    }
}
