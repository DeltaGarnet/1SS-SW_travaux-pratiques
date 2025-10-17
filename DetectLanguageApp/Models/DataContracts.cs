using System.Collections.Generic;
using Newtonsoft.Json;

namespace DetectLanguageApp.Models
{
    public class Language
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
    public class Detection
    {
        [JsonProperty("language")]
        public string LanguageCode { get; set; }

        [JsonProperty("isReliable")]
        public bool IsReliable { get; set; }

        [JsonProperty("confidence")]
        public double Confidence { get; set; }
        [JsonIgnore]
        public string LanguageName { get; set; }
    }
    public class DetectionResponse
    {
        [JsonProperty("data")]
        public DetectionData Data { get; set; }
    }

    public class DetectionData
    {
        [JsonProperty("detections")]
        public List<Detection> Detections { get; set; }
    }
    public class AccountStatus
    {
        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("requests")]
        public int Requests { get; set; }

        [JsonProperty("bytes")]
        public long Bytes { get; set; }

        [JsonProperty("plan")]
        public string Plan { get; set; }

        [JsonProperty("plan_expires")]
        public string PlanExpires { get; set; }

        [JsonProperty("daily_requests_limit")]
        public int DailyRequestsLimit { get; set; }

        [JsonProperty("daily_bytes_limit")]
        public long DailyBytesLimit { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
    public class AccountStatusResponse
    {
        [JsonProperty("data")]
        public AccountStatus Data { get; set; }
    }
    public class LanguageListResponse
    {
        [JsonProperty("data")]
        public LanguageListData Data { get; set; }
    }

    public class LanguageListData
    {
        [JsonProperty("languages")]
        public List<Language> Languages { get; set; }
    }
}
