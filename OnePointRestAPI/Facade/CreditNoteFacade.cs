#region Header
/*
 ************************************************************************************
 Name: CreditNoteFacade
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
    public class CreditNoteFacade : BaseFacade, ICreditNoteFacade
    {
        public dynamic SearchCreditNote(dynamic value)
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
                (string)CommonUtils.AppConfig.RestEndPoints.CoreBaseEndPoint, "CreditNoteService.ashx?", query));
               
                response = (response.Data != null) ? CommonUtils.RetainPropertiesList(response.Data,
                   CommonUtils.SplitStringToList((string)CommonUtils.ResponseFilterConfig.Post_Search_CreditNoteRequest)) : response;
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
