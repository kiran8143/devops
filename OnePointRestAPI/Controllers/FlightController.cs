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
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using OnePointRestAPI.Common;
using OnePointRestAPI.Common.Logger;
using OnePointRestAPI.Facade;
using OnePointRestAPI.ValidationModels;

namespace OnePointRestAPI.Controllers
{
    [Route("api")]
    public class FlightController : BaseAPIController
    {

        private IFlightFacade _FlightFacade;
        private IFlightFacade FlightFacade
        {
            get
            {
                return _FlightFacade ?? (_FlightFacade = new FlightFacade());
            }
        }

        // POST api/Search/Flight
        /// <summary>
        /// Filter Flight Data
        /// </summary>
        /// <param name="value"></param>       
        /// <returns>somethng to write for</returns>
        /// <response code="201" examples=''> Done</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="501">501 Server Error</response> 
        /// <remarks>
        ///  Filter's Flight data based on the UserInputs
        ///  </remarks>
        [HttpPost]
        [Route("Search/Flight")]
        public dynamic SearchFlight([FromBody] Flight value)
        {

            dynamic response = new ExpandoObject();
            try
            {
                JObject raw_value = JObject.FromObject(value);
                dynamic SessionData = CommonUtils.JsonDeSerialize(HttpContext.Request?.Headers["SessionData"]);
                raw_value.Add("clientId", SessionData.UserInfo.clientId);
                raw_value.Add("memberId", SessionData.UserInfo.memberId);
                response = FlightFacade.SearchFlight(raw_value);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                LogManager.Log(ex, LogType.Error);
            }
            return response;
        }
        // POST api/Revalidate/Flight
        /// <summary>
        /// Revalidate Flight Data
        /// </summary>
        /// <param name="value"></param>       
        /// <returns>somethng to write for</returns>
        /// <response code="201" examples=''> Done</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="501">501 Server Error</response> 
        /// <remarks>
        ///  Revalidate's Flight data based on the UserInputs
        ///  </remarks>
        [HttpPost]
        [Route("Revalidate/Flight")]
        public dynamic RevalidateFlight([FromBody] RevalidateFlight value)
        {

            dynamic response = new ExpandoObject();
            try
            {
                JObject raw_value = JObject.FromObject(value);
                dynamic SessionData = CommonUtils.JsonDeSerialize(HttpContext.Request?.Headers["SessionData"]);
                raw_value.Add("ClientId", SessionData.UserInfo.clientId);
                raw_value.Add("MemberId", SessionData.UserInfo.memberId);
                response = FlightFacade.RevalidateFlight(raw_value);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                LogManager.Log(ex, LogType.Error);
            }
            return response;
        }
        // POST api/Revalidate/Flight
        /// <summary>
        /// To Book a Flight
        /// </summary>
        /// <param name="value"></param>       
        /// <returns>somethng to write for</returns>
        /// <response code="201" examples=''> Done</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="501">501 Server Error</response> 
        /// <remarks>
        ///  To Book a Flight based on the UserInputs
        ///  </remarks>
        [HttpPost]
        [Route("Book/Flight")]
        public dynamic BookFlight([FromBody] BookFlight value)
        {

            dynamic response = new ExpandoObject();
            try
            {
                JObject raw_value = JObject.FromObject(value);
                dynamic SessionData = CommonUtils.JsonDeSerialize(HttpContext.Request?.Headers["SessionData"]);
                raw_value.Add("ClientId", SessionData.UserInfo.clientId);
                raw_value.Add("MemberId", SessionData.UserInfo.memberId);
                response = FlightFacade.BookFlight(raw_value);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                LogManager.Log(ex, LogType.Error);
            }
            return response;
        }
        // POST api/OrderTicket
        /// <summary>
        /// Order Flight Ticket
        /// </summary>
        /// <param name="value"></param>       
        /// <returns>somethng to write for</returns>
        /// <response code="201" examples=''> Done</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="501">501 Server Error</response> 
        /// <remarks>
        ///  Order Flight Ticket based on the UserInputs
        ///  </remarks>
        [HttpPost]
        [Route("OrderTicket")]
        public dynamic OrderTicket([FromBody] OrderTicket value)
        {

            dynamic response = new ExpandoObject();
            try
            {
                JObject raw_value = JObject.FromObject(value);
                dynamic SessionData = CommonUtils.JsonDeSerialize(HttpContext.Request?.Headers["SessionData"]);
                raw_value.Add("ClientId", SessionData.UserInfo.clientId);
                raw_value.Add("MemberId", SessionData.UserInfo.memberId);
                response = FlightFacade.OrderTicket(raw_value);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                LogManager.Log(ex, LogType.Error);
            }
            return response;
        }
        // POST api/FlightFareRules
        /// <summary>
        /// Retrives Flight Fare Rules
        /// </summary>
        /// <param name="value"></param>       
        /// <returns>somethng to write for</returns>
        /// <response code="201" examples=''> Done</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="501">501 Server Error</response> 
        /// <remarks>
        ///  Retrives Flight Fare Rules based on the UserInputs
        ///  </remarks>
        [HttpPost]
        [Route("FlightFareRules")]
        public dynamic FlightFareRules([FromBody] FlightFareRules value)
        {

            dynamic response = new ExpandoObject();
            try
            {
                JObject raw_value = JObject.FromObject(value);
                dynamic SessionData = CommonUtils.JsonDeSerialize(HttpContext.Request?.Headers["SessionData"]);
                raw_value.Add("ClientId", SessionData.UserInfo.clientId);
                raw_value.Add("MemberId", SessionData.UserInfo.memberId);
                response = FlightFacade.FlightFareRules(raw_value);
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
