#region Header
/*
 ************************************************************************************
 Name: UserController
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
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OnePointRestAPI.Common.Logger;
using OnePointRestAPI.Facade;
using OnePointRestAPI.ValidationModels;

namespace OnePointRestAPI.Controllers
{
    [EnableCors("OnePointRestAPIPolicy")]
    [Route("api")]
    [ApiController]
    public class UserController : BaseAPIController
    {

        private IUserFacade _UserFacade;
        private IUserFacade UserFacade
        {
            get
            {
                return _UserFacade ?? (_UserFacade = new UserFacade());
            }
        }

        ///// <summary>
        ///// Validate session variables of the User
        ///// </summary>
        ///// <param name="sessionID"> Unique session token of LoggedIn User</param>       
        ///// <returns>somethng to write for</returns>
        ///// <response code="201" examples=''><paramref name="Session"/> Generated</response>
        ///// <response code="400">Bad request</response>
        ///// <response code="500">Internal Server Error</response>
        ///// <response code="501">501 Server Error</response> 
        ///// <remarks>
        ///// Validate session variables passed by the User  and returns the respective user information     
        ///// </remarks>

        //[HttpGet]
        //[Route("ValidateSession/{sessionID}")]
        //public dynamic ValidateSession(string sessionID)
        //{
        //    dynamic response = new ExpandoObject();
        //    try
        //    {
        //        response=UserFacade.ValidateSession(sessionID);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Success = false;
        //        response.Message = ex.Message;
        //        LogManager.Log(ex, LogType.Error);
        //    }
        //    return response;
        //}
        // POST api/CreateSession
        /// <summary>
        /// Create session based on the UserInfo
        /// </summary>
        /// <param name="value"></param>       
        /// <returns>somethng to write for</returns>
        /// <response code="201" examples=''> Session Generated</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="501">501 Server Error</response> 
        /// <remarks>
        ///  Create session based on the UserInputs
        ///  - AccountNumber 
        ///  - Password
        ///  - UserName 
        ///  </remarks>
        [HttpPost]
        [Route("CreateSession")]
        public dynamic GenerateSession([FromBody] GenerateSession value)
        {
            dynamic response = new ExpandoObject();
            try
            {
                response = UserFacade.GenerateSession((dynamic)value);
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
