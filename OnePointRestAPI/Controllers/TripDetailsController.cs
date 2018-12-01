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
using OnePointRestAPI.Common.Logger;
using OnePointRestAPI.Facade;
using OnePointRestAPI.ValidationModels;

namespace OnePointRestAPI.Controllers
{
    [Route("api")]
    public class TripDetailsController : BaseAPIController
    {

        private ITripDetailsFacade _TripDetailsFacade;
        private ITripDetailsFacade TripDetailsFacade
        {
            get
            {
                return _TripDetailsFacade ?? (_TripDetailsFacade = new TripDetailsFacade());
            }
        }

        /// <summary>
        /// Retrives the TripDetails based on the  MFRef Number
        /// </summary>
        /// <param name="value"></param>       
        /// <returns>somethng to write for</returns>
        /// <response code="201" examples=''> Session Generated</response>
        /// <response code="400">Bad request</response>
        /// <response code="500">Internal Server Error</response>
        /// <response code="501">501 Server Error</response> 
        /// <remarks>
        /// Retrives the TripDetails based on the  MFRef Number
        /// - MFRef 
        ///  </remarks>
        [HttpGet]
        [Route("TripDetails/{MFRef}")]
        public dynamic SearchTripDetails(SearchTripDetails value)
        {
            dynamic response = new ExpandoObject();
            try
            {
              
                if (value.MFRef != null)
                {
                    var mfrefvalidate = ValidateMFRef(value.MFRef);
                    if (!(bool)mfrefvalidate.Success)
                    {
                        response.Data = null;
                        response.Success = false;
                        response.Message = mfrefvalidate.Message;
                        return response;
                    }
                }

                response = TripDetailsFacade.SearchTripDetails(value.MFRef);
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
