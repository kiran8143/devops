#region Header
/*
 ************************************************************************************
 Name: ValuesController
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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using OnePointRestAPI.Common;
using OnePointRestAPI.Common.Logger;
using OnePointRestAPI.Facade;
using OnePointRestAPI.ValidationModels;

namespace OnePointRestAPI.Controllers
{
    [Route("api")]
    public class PostTicketingRequestController : BaseAPIController
    {
        private IPostTicketingRequestFacade _PostTicketingRequestFacade;      
        private IPostTicketingRequestFacade PostTicketingRequestFacade
        {
            get
            {
                return _PostTicketingRequestFacade ?? (_PostTicketingRequestFacade = new PostTicketingRequestFacade());
            }           
        }
        // POST api/Search/PostTicketingRequest
        /// <summary>
        /// Filter PostTicketingRequest Data
        /// </summary>
        /// <param name="value"></param>       
        /// <returns>somethng to write for</returns>
        /// <response code="201" examples=''> Done</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="501">501 Server Error</response> 
        /// <remarks>
        ///  Filter's PostTicketingRequest based on the UserInputs   
        ///  - PTRId 
        ///  - Page 
        ///  - MFRef
        ///  </remarks>
        [HttpPost]
        [Route("Search/PostTicketingRequest")]
        public dynamic SearchPostTicketingRequest([FromBody] SearchPostTicketingRequest value)
        {
            dynamic response = new ExpandoObject();
            try
            {
                if (value.MFRef != null) {
                var mfrefvalidate = ValidateMFRef(value.MFRef);
                    if (!(bool)mfrefvalidate.Success)
                    {
                        response.Data = null;
                        response.Success = false;
                        response.Message = mfrefvalidate.Message;
                        return response;
                    }
                }
                JObject raw_value = JObject.FromObject(value);
                dynamic SessionData = CommonUtils.JsonDeSerialize(HttpContext.Request?.Headers["SessionData"]);
                raw_value.Add("ClientId", SessionData.UserInfo.clientId);
                response = PostTicketingRequestFacade.SearchPostTicketingRequest(raw_value);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                LogManager.Log(ex, LogType.Error);
            }
            
            return response;
        }

        // POST api/PostTicketingRequest
        /// <summary>
        /// Create PostTicketingRequest
        /// </summary>
        /// <param name="value"></param>       
        /// <returns>somethng to write for</returns>
        /// <response code="201" examples=''> Done</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="501">501 Server Error</response> 
        /// <remarks>
        ///  Create PostTicketingRequest based on the UserInputs   
        ///  - firstName 
        ///  - lastName  
        ///  - title    
        ///  - passengerType
        ///  - ptrType
        ///  </remarks>
        [HttpPost]
        [Route("PostTicketingRequest")]
        public dynamic PostTicketingRequest([FromBody] PostTicketingRequest value)
        {
            dynamic response = new ExpandoObject();
            try
            {
                if (value.mFRef != null)
                {
                    var mfrefvalidate = ValidateMFRef(value.mFRef);
                    if (!(bool)mfrefvalidate.Success)
                    {
                        response.Data = null;
                        response.Success = false;
                        response.Message = mfrefvalidate.Message;
                        return response;
                    }
                }
                JObject raw_value = JObject.FromObject(value);
                dynamic SessionData = CommonUtils.JsonDeSerialize(HttpContext.Request?.Headers["SessionData"]);
                raw_value.Add("clientId", SessionData.UserInfo.clientId);
                raw_value.Add("memberId", SessionData.UserInfo.memberId);
                response = PostTicketingRequestFacade.PostTicketingRequest((dynamic)raw_value);

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                LogManager.Log(ex, LogType.Error);
            }
            return response;
        }


        // POST api/MarkAsRead
        /// <summary>
        /// Mark As Read
        /// </summary>
        /// <param name="value"></param>       
        /// <returns>somethng to write for</returns>
        /// <response code="201" examples=''> Done</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="501">501 Server Error</response> 
        /// <remarks>
        ///   MarkAsRead based on the UserInputs   
        ///  - id
        ///  - RequestType
        ///  </remarks>
        [HttpPost]
        [Route("MarkAsRead")]
        public dynamic MarkAsRead([FromBody] MarkAsRead value)
        {
            dynamic response = new ExpandoObject();
            try
            {
                JObject raw_value = JObject.FromObject(value);
                dynamic SessionData = CommonUtils.JsonDeSerialize(HttpContext.Request?.Headers["SessionData"]);
                raw_value.Add("ClientId", SessionData.UserInfo.clientId);
                response = PostTicketingRequestFacade.MarkAsRead((dynamic)raw_value);
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
