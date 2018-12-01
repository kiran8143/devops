#region Header
/*
 ************************************************************************************
 Name: InvoiceController
 Description: This returns rest data to client 
 Created On:  28-sep-2018
 Created By:  Uday Kiran
 Last Modified On: 
 Last Modified By: 
 Last Modified Reason: 
 ************************************************************************************
 */
#endregion

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using OnePointRestAPI.Common;
using OnePointRestAPI.Common.Logger;
using OnePointRestAPI.Facade;
using OnePointRestAPI.ValidationModels;

namespace OnePointRestAPI.Controllers
{
    [Route("api")]
    public class InvoiceController : BaseAPIController
    {

        private IInvoiceFacade _InvoiceFacade;
        private IInvoiceFacade InvoiceFacade
        {
            get
            {
                return _InvoiceFacade ?? (_InvoiceFacade = new InvoiceFacade());
            }
        }
        // POST api/Search/Invoice
        /// <summary>
        /// Filter Invoice Data
        /// </summary>
        /// <param name="value"></param>       
        /// <returns>somethng to write for</returns>
        /// <response code="201" examples=''> Done</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="501">501 Server Error</response> 
        /// <remarks>
        ///  Filter's Invoice based on the UserInputs
        ///  - Page 
        ///  </remarks>
        [HttpPost]
        [Route("Search/Invoice")]
        public dynamic SearchInvoice([FromBody]Invoice value)
        {
         
            dynamic response = new ExpandoObject();
            try
            {
                JObject raw_value = JObject.FromObject(value);
                dynamic SessionData = CommonUtils.JsonDeSerialize(HttpContext.Request?.Headers["SessionData"]);
                raw_value.Add("ClientId", SessionData.UserInfo.clientId);
                response = InvoiceFacade.SearchInvoice(raw_value);
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
