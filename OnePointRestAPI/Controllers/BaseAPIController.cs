using Microsoft.AspNetCore.Mvc;
using OnePointRestAPI.Common;
using OnePointRestAPI.Common.Logger;
using System;
using System.Dynamic;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace OnePointRestAPI.Controllers
{
    
    public class BaseAPIController : ControllerBase
    {
        //Initiallising variable to load logger class instance
		public static readonly ILogger LogManager = UtilsFactory.Logger;

        public BaseAPIController()
        {
            //LogManager.Log("BaseApiController", LogType.Info);
        }
        [HttpGet]
        internal dynamic ValidateMFRef(string mfref)
        {
            dynamic response = new ExpandoObject();
            try
            {

                dynamic SessionData = CommonUtils.JsonDeSerialize(HttpContext.Request?.Headers["SessionData"]);
                var mfrefvalidate = Common.CommonUtils.RestClient_GET(string.Join("",
                (string)CommonUtils.AppConfig.RestEndPoints.CoreBaseEndPoint, "ValidateMFRef.ashx?MFRef=", mfref, "&ClientId=", SessionData.UserInfo.clientId));
                 response.Success = mfrefvalidate.Data==null?false:mfrefvalidate.Data.Success;
                 response.Message = mfrefvalidate.Data == null ? mfrefvalidate.Message : mfrefvalidate.Data.Error;
               
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                LogManager.Log(ex, LogType.Error);
            }
            return response;
        }
    }
}
