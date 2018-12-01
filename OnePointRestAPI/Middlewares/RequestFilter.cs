#region Header
/*
 ************************************************************************************
 Name: RequestFilter  MiddleWare
 Description: MiddleWare to capture the request information
 Created On:  28-sep-2018
 Created By:  Uday Kiran
 Last Modified On: 
 Last Modified By: 
 Last Modified Reason: 
 ************************************************************************************
 */
#endregion

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using OnePointRestAPI.Common;
using OnePointRestAPI.Common.Logger;
using System;
using System.Dynamic;

namespace OnePointRestAPI.Middlewares
{
    /// <summary>
    /// This interceptor is used to manipulate the request object
    /// </summary>
    public class RequestFilter : ActionFilterAttribute
    {
        //Initiallising variable to load logger class instance
        public static readonly ILogger LogManager = UtilsFactory.Logger;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            
            dynamic response = new ExpandoObject();
            try
            {
             //  step1: verifiying the token 

                if ((bool)CommonUtils.AppConfig.EnableAutherization&&
                    (!CommonUtils.UrlToExclude((context.HttpContext.Request).Path)))
                {
                    dynamic SessionData = CommonUtils.JsonDeSerialize(context.HttpContext.Request?.Headers["SessionData"]);

                    if (!(bool)SessionData.IsValid)
                    {
                            response.Success = false;
                            response.Data = null;
                            response.Message = SessionData.Message;
                            context.HttpContext.Response.StatusCode = SessionData.StatusCode;
                            context.Result = new ObjectResult(response);
                        return;
                    }
                }

                //  step2: request model validation  check
                // validation of models are verified and response is returned incase of invalid
                if (!context.ModelState.IsValid)
                {
                    response.Success = false;
                    response.Data = CommonUtils.GetErrors(context.ModelState);
                    response.Message = "Validation Error";
                    context.HttpContext.Response.StatusCode = 400;
                    // custom enum validation message --need to revisit the logic
                    if (response.Data[0].Contains("'. Path '"))
                    {
                        response.Data[0] = response.Data[0].Split("'. Path '")[1].Split("',")[0] + " is not valid";
                    }
                    context.Result = new ObjectResult(response);
                    base.OnActionExecuting(context);
                    return;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;               
                response.Data = null;               
                context.HttpContext.Response.StatusCode = 500;
                context.Result = new ObjectResult(response);
                LogManager.Log(ex, LogType.Error);

            }         
            base.OnActionExecuting(context);
        }
    }
}