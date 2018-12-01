#region Header
/*
 ************************************************************************************
 Name: ResponseFilter  MiddleWare
 Description: MiddleWare to alter the response information
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
using System.Collections.Generic;
using System.Dynamic;

namespace OnePointRestAPI.Middlewares
{
    /// <summary>
    /// This interceptor is used to manipulate the response object
    /// </summary>
    public class ResponseFilter : ResultFilterAttribute
    {
        public dynamic EncryptedResponse { get; private set; }

        //Initiallising variable to load logger class instance
        public static readonly ILogger LogManager = UtilsFactory.Logger;
        // temperory  --need to revisit the logic
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            dynamic response = new ExpandoObject();
            try
            {
                if (CommonUtils.AppConfig.App_Environment != CommonUtils.AppConfig.Environment_Name_Dev)
                {
                    var settings = new JsonSerializerSettings{ ContractResolver = new DefaultContractResolver()};


                    // RESPONSE WILL ALWAYS BE A OBJECT ONLY ERRORS WILL BE THROWN AS STRING RESULT
                    if ((((ObjectResult)context.Result).Value is string) && !CommonUtils.CheckIfPropertyExistInDynamicObject(((ObjectResult)context.Result).Value, "Data"))
                    {
                        response.Data = ((ObjectResult)context.Result).Value;
                        response.Success = false;

                    }
                    else if(CommonUtils.CheckIfPropertyExistInDynamicObject(((ObjectResult)context.Result).Value, "Data"))
                    {
                        response = ((ObjectResult)context.Result).Value;
                    }
                    else if (CommonUtils.CheckIfPropertyExistInDynamicObject(((ObjectResult)context.Result).Value, "Success")&& !CommonUtils.CheckIfPropertyExistInDynamicObject(((ObjectResult)context.Result).Value, "Data"))
                    {
                        response.Data = null;
                    }
                    else
                    {
                        response.Data = ((ObjectResult)context.Result).Value;
                        response.Success = true;                                                                
                    }
                   
                    if (!CommonUtils.IsList(response.Data)&&
                        response.Data!=null&& (response.Success!= true)
                        && CommonUtils.CheckIfPropertyExistInDynamicObject(response.Data, "Error"))
                    {
                        response.Success = false;
                        response.Message = response.Data.Error;
                        response.Data = null;
                       
                    }
                   

                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
                LogManager.Log(ex, LogType.Error);
            }

            ((ObjectResult)context.Result).Value = response;
            base.OnResultExecuting(context);
        }
    }
}