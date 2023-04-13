﻿using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
// <auto-generated />
//
// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using ethosIQ_AVOXI_Shared.API;
//
//    var getCdRsResponse = GetCdRsResponse.FromJson(jsonString);

namespace ethosIQ_AVOXI_Shared.API
{
    

    public partial class GetCDRsResponse
    {
        [JsonProperty("data")]
        public List<CallData> Data { get; set; }
    }

    public partial class CallData
    {
        [JsonProperty("avoxi_call_id")]
        public string AvoxiCallId { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("from_country")]
        public string FromCountry { get; set; }

        [JsonProperty("to_country")]
        public string ToCountry { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("direction")]
        public string Direction { get; set; }

        [JsonProperty("date_start")]
        public string DateStart { get; set; }

        [JsonProperty("date_answered")]
        public string DateAnswered { get; set; }

        [JsonProperty("date_end")]
        public string DateEnd { get; set; }

        [JsonProperty("duration")]
        public Duration Duration { get; set; }

        [JsonProperty("forwarded_to")]
        public List<string> ForwardedTo { get; set; }

        [JsonProperty("sorted_dispositions")]
        public List<object> SortedDispositions { get; set; }
    }

    public partial class Duration
    {
        [JsonProperty("seconds")]
        [JsonConverter(typeof(ParseStringConverter))]
        public long Seconds { get; set; }

        [JsonProperty("formatted")]
        public string Formatted { get; set; }
    }

    public partial class GetCDRsResponse
    {
        public static GetCDRsResponse FromJson(string json) => JsonConvert.DeserializeObject<GetCDRsResponse>(json, ethosIQ_AVOXI_Shared.API.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this GetCDRsResponse self) => JsonConvert.SerializeObject(self, ethosIQ_AVOXI_Shared.API.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class ParseStringConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (Int64.TryParse(value, out long l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }
}
