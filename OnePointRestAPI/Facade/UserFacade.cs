#region Header
/*
 ************************************************************************************
 Name: UserFacade
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
    public class UserFacade : BaseFacade, IUserFacade
    {
        
        public dynamic ValidateSession(string sessionID)
        {
            dynamic response = new ExpandoObject();
            try
            {
                response= Common.CommonUtils.RestClient_GET(string.Join("",
               (string)CommonUtils.AppConfig.RestEndPoints.CoreBaseEndPoint, "ValidateSession.ashx?SessionId=", sessionID));
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
                LogManager.Log(ex, LogType.Error);
            }
            return response;

        }

        public dynamic GenerateSession(dynamic value)
        {
            dynamic response = new ExpandoObject();
            try
            {
                //MCN001947 //bharath@123
                    string postData = "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' " +
                    "xmlns:mys='Mystifly.OnePoint' xmlns:mys1='http://schemas.datacontract.org/2004/07/Mystifly.OnePoint'> <soapenv:Header/>" +
                    " <soapenv:Body> <mys:CreateSession> <mys:rq> <mys1:AccountNumber>" + value.AccountNumber + "</mys1:AccountNumber>" +
                    " <mys1:Password>" + value.Password + "</mys1:Password> <mys1:Target>Production</mys1:Target> " +
                    "<mys1:UserName>" + value.UserName + "</mys1:UserName> </mys:rq> </mys:CreateSession> </soapenv:Body> </soapenv:Envelope>";

                dynamic headers = new Dictionary<string, string>();
                headers["SOAPAction"] = "Mystifly.OnePoint/OnePoint/CreateSession";
                dynamic resp = Common.CommonUtils.xmlClient_PostSoapData(string.Join("",CommonUtils.AppConfig.RestEndPoints.SoapBaseEndPoint,"OnePoint.svc"),
                    postData, "Post", headers);              
                if (resp.Data == null) return resp;

                if (resp.Data["s:Envelope"]["s:Body"]["CreateSessionResponse"]["CreateSessionResult"]["a:SessionStatus"].LastChild.Value=="true")
                {
                    response.SessionId = resp.Data["s:Envelope"]["s:Body"]["CreateSessionResponse"]["CreateSessionResult"]["a:SessionId"].LastChild.Value;

                }
                else
                {
                    response.Data = null;
                    response.Message = "Invalid login credentials supplied";
                    response.Success = false;                   
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
                LogManager.Log(ex, LogType.Error);
            }
            return response;
        }

    }
}
