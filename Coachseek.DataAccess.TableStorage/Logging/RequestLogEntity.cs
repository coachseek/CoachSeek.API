using System;
using Coachseek.Logging.Contracts;
using Microsoft.WindowsAzure.Storage.Table;

namespace Coachseek.DataAccess.TableStorage.Logging
{
    public class RequestLogEntity : TableEntity
    {
        public RequestLogEntity(RequestLogMessage requestLogMessage)
        {
            PartitionKey = requestLogMessage.StartTime.ToString("yyyy-MM-dd hh");
            RowKey = requestLogMessage.StartTime.ToString("mm:ss.fff");;

            HttpMethod = requestLogMessage.HttpMethod;
            Url = requestLogMessage.Url;
            StartTime = requestLogMessage.StartTime;
            FinishTime = requestLogMessage.FinishTime;
            Duration = requestLogMessage.Duration;
            StatusCode = requestLogMessage.StatusCode;
            BusinessDomain = requestLogMessage.BusinessDomain;
            UserLogin = requestLogMessage.UserLogin;
            Data = requestLogMessage.Data;
        }

        public string HttpMethod { get; set; }
        public string Url { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime FinishTime { get; set; }
        public double Duration { get; set; }
        public int StatusCode { get; set; }
        public string BusinessDomain { get; set; }
        public string UserLogin { get; set; }
        public string Data { get; set; }
    }
}
