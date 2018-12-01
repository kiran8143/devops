
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace OnePointRestAPI.Common.Logger
{
    public class LogAdapter : ILogger
    {
        private string _LogType;
        private dynamic _Loggging = new ExpandoObject();        

        public void Log(string message, LogType logType)
        {
            _Loggging.LogType = CommonUtils.CnvStr(logType);
            _Loggging.Message = message;
             Task.Run(() => LogsWriter(logType, _Loggging));
        }

        public void Log(Type caller, string message, LogType logType)
        {
            _Loggging.LogType = CommonUtils.CnvStr(logType);
            _Loggging.Message = message;
            _Loggging.Caller = CommonUtils.CnvStr(caller.Namespace);
             Task.Run(() => LogsWriter(logType, _Loggging));
        }

        public void Log(string message, Exception exception, LogType logType)
        {
            _Loggging.LogType = CommonUtils.CnvStr(logType);
            _Loggging.Message = message;
            _Loggging.Exception = exception.Message;
            _Loggging.StackTrace = exception.StackTrace;
            _Loggging.Source = exception.Source;
             Task.Run(() => LogsWriter(logType, _Loggging));
        }

        public void Log(Type caller, string message, Exception exception, LogType logType)
        {
            _Loggging.LogType = CommonUtils.CnvStr(logType);
            _Loggging.Message = message;
            _Loggging.Caller = CommonUtils.CnvStr(caller.Namespace);
            _Loggging.Exception = exception.Message;
            _Loggging.StackTrace = exception.StackTrace;
            _Loggging.Source = exception.Source;
             Task.Run(() => LogsWriter(logType, _Loggging));
        }

        public void Log(Exception exception, LogType logType)
        {
            _Loggging.LogType = CommonUtils.CnvStr(logType);
            _Loggging.Exception = exception.Message;
            _Loggging.StackTrace = exception.StackTrace;
            _Loggging.Source = exception.Source;
            Task.Run(() =>LogsWriter(logType, _Loggging));
            
        }


     
        /// <summary>
        /// Function to Write  Logs
        /// </summary>
        /// <param name="logType , log"></param>
        /// <returns>response Task<dynamic></returns>
        private dynamic LogsWriter(LogType logType, dynamic log)
        {
            try
            {
                //Checking whether logs are enabled or not
                bool IsLogsEnabled = CommonUtils.StrToBoolean(CommonUtils.CnvStr(CommonUtils.AppConfig.Logs_Enabled));
                if (IsLogsEnabled)
                {
                    //Exception Logs
                    if (logType.Equals(LogType.Error))
                    {
                        _LogType = CommonUtils.CnvStr(CommonUtils.AppConfig.Logs_Exception_Name);
                    }
                    //Information Logs
                    else
                    {
                        _LogType = CommonUtils.CnvStr(CommonUtils.AppConfig.Logs_Information_Name);
                    }

                  //  var dta = CommonUtils.JsonSerialize(CommonUtils.CurrentContext.Request);
                    //Calling function to write logs 
                   Console.WriteLine(CommonUtils.JsonSerialize(log));

                  //Common.CommonUtils.RestClient_PostData((string)CommonUtils.AppConfig.LogProfilerUrl,
                  // CommonUtils.JsonSerialize(logreqFormater(log)), "Post", new Dictionary<string, string>());
                }
            }
            catch (Exception ex)
            {
                //Do nothing
            }
            return "";
        }

        #region Write Logs
        /// <summary>
        /// Function to Write  Logs
        /// </summary>
        /// <param name="logType , log"></param>
        /// <returns>response Task<dynamic></returns>
        private dynamic logreqFormater(dynamic data)
        {

            dynamic response = new ExpandoObject();
            try
            {
                response = CommonUtils.AppConfig.LogFormat;
                response.Id = Guid.NewGuid().ToString();
                response.SearchId = Guid.NewGuid().ToString();
                response.Docs[0].Content = CommonUtils.CurrentContext.Items["RawRequestBody"];

            }
            catch (Exception ex)
            {
                //Do nothing
            }
            return response;
        }

        #endregion Write Logs 
    }
}
