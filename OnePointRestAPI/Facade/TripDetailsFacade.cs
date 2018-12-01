#region Header
/*
 ************************************************************************************
 Name: ValuesFacade
 Description: Facade/logic layer for all the Values operations 
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
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using OnePointRestAPI.Common;
using OnePointRestAPI.Common.Logger;
using RestSharp;
using RestSharp.Portable.HttpClient;

namespace OnePointRestAPI.Facade
{
    public class TripDetailsFacade : BaseFacade, ITripDetailsFacade
    {
       

        public dynamic SearchTripDetails(string value)
        {
           
            dynamic response = new ExpandoObject();
            try
            {
                
                response = Common.CommonUtils.RestClient_GET(string.Join("",
                (string)CommonUtils.AppConfig.RestEndPoints.CoreBaseEndPoint, "TripDetailsService.ashx?MFRef=", value));

                response = (response.Data != null) ? CommonUtils.RetainPropertiesList(response.Data,
                  CommonUtils.SplitStringToList((string)CommonUtils.ResponseFilterConfig.Post_Search_TripDetailsRequest)) : response;
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
