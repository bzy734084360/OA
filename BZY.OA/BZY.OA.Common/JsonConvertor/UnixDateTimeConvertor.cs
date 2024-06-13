using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BZY.OA.Common
{
    public class UnixDateTimeConvertor : DateTimeConverterBase
    {

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            long jsTimeStamp = long.Parse(reader.Value.ToString());
            var startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            DateTime dt = startTime.AddSeconds(jsTimeStamp);
            return dt;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long timeStamp = (long)(((DateTime)value) - startTime).TotalSeconds;
            writer.WriteValue(timeStamp);
        }
        
    }
}
