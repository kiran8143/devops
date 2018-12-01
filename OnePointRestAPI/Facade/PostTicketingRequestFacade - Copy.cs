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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using OnePointRestAPI.Common;
using OnePointRestAPI.Common.Logger;
using RestSharp;
using RestSharp.Portable.HttpClient;

namespace OnePointRestAPI.Facade
{
    public class PostTicketingRequestFacade : BaseFacade, IPostTicketingRequestFacade
    {
        public dynamic PostTicketingRequest(dynamic value)
        {
           
            dynamic response = new ExpandoObject();
            try
            {  
                response = Common.CommonUtils.RestClient_PostData(
                    string.Join("", (string)CommonUtils.AppConfig.RestEndPoints.CoreBaseEndPoint, "PostTicketingRequest.ashx"),
                    CommonUtils.JsonSerialize(value), "Post",new Dictionary<string, string>());
                response = (response.Data != null) ? ResponseValidationHelper.DynamicResponsefilteronSchemaBasis(response.Data,
               CommonUtils.ResponseFilterConfig.Post_Add_PostTicketingJson) : response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                LogManager.Log(ex, LogType.Error);
            }
            return response;
        }


        public dynamic SearchPostTicketingRequest(dynamic value)
        {
           
            dynamic response = new ExpandoObject();
            try
            {
                string query = "";
                foreach (var key in value)
                {
                    dynamic attrval = value[key.Name];

                    if (attrval != null)
                    {
                        if (key.Name == "Page")
                        {
                            query = query + "&Index=" + attrval;
                        }
                        else
                        {
                            query = query + "&" + key.Name + "=" + attrval;
                        }                      
                    }
                }
                response = Common.CommonUtils.RestClient_GET(string.Join("",
                (string)CommonUtils.AppConfig.RestEndPoints.CoreBaseEndPoint, "PostTicketingRequest.ashx?",query));

                //response = (response.Data != null) ? CommonUtils.RetainPropertiesList(response.Data,
                // CommonUtils.SplitStringToList((string)CommonUtils.ResponseFilterConfig.Post_Search_PostTicketingRequest)) : response;

                response = (response.Data != null) ? ResponseValidationHelper.DynamicResponsefilteronSchemaBasis(response.Data,
                 CommonUtils.ResponseFilterConfig.Post_Search_PostTicketingJson) : response;
                

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                LogManager.Log(ex, LogType.Error);
            }
            return response;
        }

        public dynamic MarkAsRead(dynamic value)
        {

            dynamic response = new ExpandoObject();
            try
            {
                response = Common.CommonUtils.RestClient_PostData(
                    string.Join("", (string)CommonUtils.AppConfig.RestEndPoints.CoreBaseEndPoint, "MarkAsRead.ashx"),
                    CommonUtils.JsonSerialize(value), "Post", new Dictionary<string, string>());

                response = (response.Data != null) ? CommonUtils.RetainPropertiesList(response.Data,
               CommonUtils.SplitStringToList((string)CommonUtils.ResponseFilterConfig.Post_Add_MarkAsReadResponse)) : response;



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
