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
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace OnePointRestAPI.Middlewares
{
    /// <summary>
    /// This interceptor is used to log the request object
    /// </summary>
    public class ReqRespLogMiddleware
    {
        private readonly RequestDelegate _next;

        public ReqRespLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            //Workaround - copy original Stream
            var initalBody = context.Request.Body;

            using (var bodyReader = new StreamReader(context.Request.Body))
            {
                string body = await bodyReader.ReadToEndAsync();
                //Do something with body
                //Replace write only request body with read/write memorystream so you can read from it later
                context.Items["RawRequestBody"] = Common.CommonUtils.JsonDeSerialize(body);
                context.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(body));

                //handle other middlewares
                await _next.Invoke(context);

                //Workaround - return back to original Stream
                context.Request.Body = initalBody;
            }
        }       

    }
}