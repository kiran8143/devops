#region Header
/*
 ************************************************************************************
 Name: FlightFacade
 Description: Facade/logic layer for all the User operations 
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
    public class FlightFacade : BaseFacade, IFlightFacade
    {
        public dynamic SearchFlight(dynamic value)
        {

            dynamic response = new ExpandoObject();
            try
            {
                response = Common.CommonUtils.RestClient_PostData_gzip(
                    string.Join("", (string)CommonUtils.AppConfig.RestEndPoints.SearchEngineBaseEndPoint, "Flights/Search"),
                    CommonUtils.JsonSerialize(value), "Post", new Dictionary<string, string>());
                if (response.Data != null) {
                  dynamic data = CommonUtils.JsonDeSerialize(CommonUtils.Unzip(CommonUtils.strToToHexByte(response.Data)));
                    //response = (data.Success==true) ? ResponseValidationHelper.DynamicResponsefilteronSchemaBasis(data.PricedItineraries,CommonUtils.ResponseFilterConfig.Post_Search_FlightJson) : response;
                    response = new ExpandoObject();
                    if (data.Success == true)
                    {                       
                        response.Data = data.PricedItineraries;
                        response.Success = true;
                    }
                    else
                    {
                        response.Data = data.Errors;
                        response.Success = false;
                        response.Message = "Error in retriving Data";
                    }
                
                }
                else
                {
                    response.Data = response;
                    response.Success = false;
                }

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                LogManager.Log(ex, LogType.Error);
            }
            return response;
        }
        public dynamic RevalidateFlight(dynamic value)
        {

            dynamic response = new ExpandoObject();
            try
            {
                response = Common.CommonUtils.RestClient_PostData_gzip(
                    string.Join("", (string)CommonUtils.AppConfig.RestEndPoints.SearchEngineBaseEndPoint, "Flights/Revalidate"),
                    CommonUtils.JsonSerialize(value), "Post", new Dictionary<string, string>());
                if (response.Data != null)
                {
                    dynamic data = CommonUtils.JsonDeSerialize(CommonUtils.Unzip(CommonUtils.strToToHexByte(response.Data)));
                    response = new ExpandoObject();
                    if (data.Success == true)
                    {
                        response.Data = data.PricedItineraries;
                        response.Success = true;
                    }
                    else
                    {
                        response.Data = null;
                        response.Success = false;
                        response.Message = data.Errors[0].Message;
                    }
                }
                }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                LogManager.Log(ex, LogType.Error);
            }
            return response;
        }


        public dynamic BookFlight(dynamic value)
        {

            dynamic response = new ExpandoObject();
            try
            {
                response = Common.CommonUtils.RestClient_PostData_gzip(
                    string.Join("", (string)CommonUtils.AppConfig.RestEndPoints.SearchEngineBaseEndPoint, "Flights/Book"),
                    CommonUtils.JsonSerialize(value), "Post", new Dictionary<string, string>());
                if (response.Data != null)
                {
                    dynamic data = CommonUtils.JsonDeSerialize(CommonUtils.Unzip(CommonUtils.strToToHexByte(response.Data)));
                    response = new ExpandoObject();
                    if (data.Success == true)
                    {
                        response.Data = data;
                        response.Success = true;
                    }
                    else
                    {
                        response.Data = null;
                        response.Success = false;
                        response.Message = data.Errors[0].Message;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                LogManager.Log(ex, LogType.Error);
            }
            return response;
        }

        public dynamic OrderTicket(dynamic value)
        {

            dynamic response = new ExpandoObject();
            try
            {
                response = Common.CommonUtils.RestClient_PostData_gzip(
                    string.Join("", (string)CommonUtils.AppConfig.RestEndPoints.SearchEngineBaseEndPoint, "Flights/OrderTicket"),
                    CommonUtils.JsonSerialize(value), "Post", new Dictionary<string, string>());
                if (response.Data != null)
                {
                    dynamic data = CommonUtils.JsonDeSerialize(CommonUtils.Unzip(CommonUtils.strToToHexByte(response.Data)));
                    response = new ExpandoObject();
                    if (data.Success == true)
                    {
                        response.Data = data;
                        response.Success = true;
                    }
                    else
                    {
                        response.Data = null;
                        response.Success = false;
                        response.Message = data.Errors[0].Message;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                LogManager.Log(ex, LogType.Error);
            }
            return response;
        }

        public dynamic FlightFareRules(dynamic value)
        {

            dynamic response = new ExpandoObject();
            try
            {
                response = Common.CommonUtils.RestClient_PostData_gzip(
                    string.Join("", (string)CommonUtils.AppConfig.RestEndPoints.SearchEngineBaseEndPoint, "Flights/FareRules"),
                    CommonUtils.JsonSerialize(value), "Post", new Dictionary<string, string>());
                if (response.Data != null)
                {
                    dynamic data = CommonUtils.JsonDeSerialize(CommonUtils.Unzip(CommonUtils.strToToHexByte(response.Data)));
                    response = new ExpandoObject();
                    if (data.Success == true)
                    {
                        response.Data = data;
                        response.Success = true;
                    }
                    else
                    {
                        response.Data = null;
                        response.Success = false;
                        response.Message = data.Errors[0].Message;
                    }
                }
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
