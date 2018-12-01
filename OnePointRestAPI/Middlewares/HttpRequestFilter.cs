#region Header
/*
 ************************************************************************************
 Name: HttpRequestFilter  MiddleWare
 Description: MiddleWare to capture the request information validation of headers will 
              be taken care here 
 Created On:  28-sep-2018
 Created By:  Uday Kiran
 Last Modified On: 
 Last Modified By: 
 Last Modified Reason: 
 ************************************************************************************
 */
#endregion

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OnePointRestAPI.Common;
using OnePointRestAPI.Common.Logger;
using OnePointRestAPI.Facade;
using System;
using System.Dynamic;
using System.Net;
using System.Threading.Tasks;

namespace OnePointRestAPI.Middlewares
{
    /// <summary>
    /// This interceptor is used to manipulate the request object
    /// </summary>
    public class HttpRequestFilter 
    {
        public static readonly ILogger LogManager = UtilsFactory.Logger;
        private readonly RequestDelegate _next;

        public HttpRequestFilter(RequestDelegate next)
        {
            _next = next;           
        }
        private IUserFacade _UserFacade;
        private IUserFacade UserFacade
        {
            get
            {
                return _UserFacade ?? (_UserFacade = new UserFacade());
            }
        }
        public async Task Invoke(HttpContext context)
        {

            var _req = context.Request;



            if ((bool)CommonUtils.AppConfig.EnableAutherization)
            {
                if (!CommonUtils.UrlToExclude(_req.Path))
                {
                    context=Autherize(context);                  
                }
            }


            await _next(context);

            
        }

        private dynamic Autherize(HttpContext context)
        {
            dynamic resp = new ExpandoObject(), response = new ExpandoObject();
            resp.IsValid = false; 
            try
            {
                string sessionID = context.Request?.Headers["Authorization"];                
                if ((sessionID == null)||(!sessionID.Contains("Bearer ")))
                {
                    resp.Message = "Missing session info in Header/Please validate format 'Bearer<Space><SessionId>'";
                    resp.StatusCode = 401;
                    LogManager.Log(resp.Message, LogType.Error);
                }
                else
                {
                    sessionID = sessionID.Split(" ")[1];
                    if (RedisCacheHelper.GetDatabase() != null)
                    {
                        dynamic Rc = RedisCacheHelper.Get(sessionID);
                        if (Rc == null)
                        {
                            response = UserFacade.ValidateSession(sessionID);
                            if ((!response.Success)
                                || (response.Data.GetType() != typeof(Newtonsoft.Json.Linq.JObject))
                                || (!(bool)response.Data.isValid))
                            {
                                resp.Message = "Invalid Session";
                                resp.StatusCode = 401;
                                LogManager.Log("Invalid Session sessionID "+ sessionID, LogType.Error);

                            }
                            else
                            {
                                RedisCacheHelper.Set(sessionID, response.Data);
                                resp.IsValid = true;
                                resp.UserInfo = response.Data;
                            }
                        }
                        else
                        {
                            resp.IsValid = true;
                            resp.UserInfo = Rc;
                        }
                    }
                    else
                    {
                        resp.Message = "Internal Server Error";
                        resp.StatusCode = 500;
                        LogManager.Log("Redis Connection Failure ", LogType.Error);
                    }

                }
              
            }
            catch (Exception ex)
            {
                resp.IsValid = false;
                resp.Message = ex.Message;
                resp.StatusCode = 500;
                LogManager.Log(ex, LogType.Error);
            }

            context.Request.Headers.Add("SessionData", (string)CommonUtils.JsonSerialize(resp));
            return context;
        }

      
    }
}