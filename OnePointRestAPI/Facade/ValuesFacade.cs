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
using RestSharp;
using RestSharp.Portable.HttpClient;

namespace OnePointRestAPI.Facade
{
    public class ValuesFacade : BaseFacade, IValuesFacade
    {
        
        public dynamic Get()
        { 
            //if(RedisCacheHelper.GetDatabase() != null)
            //{

           
            //dynamic test = RedisCacheHelper.Get("testdatatodos");
            //if (test==null)
            //{
            //    dynamic data = Common.CommonUtils.RestClient_GET("https://jsonplaceholder.typicode.com/todos");
            //    RedisCacheHelper.Set("testdatatodos", data);
            //    return data;
            //}
            //else
            //{
            //    return test;
            //}
            //}
            //else
            //{
            //    return "redis db failure";
            //}
            return Common.CommonUtils.RestClient_GET("https://jsonplaceholder.typicode.com/todos");

        }

        public dynamic Post(dynamic value)
        {
            try
            {
                //MCN001947 //bharath@123
                dynamic response = new ExpandoObject();
                string postData = "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' " +
                    "xmlns:mys='Mystifly.OnePoint' xmlns:mys1='http://schemas.datacontract.org/2004/07/Mystifly.OnePoint'> <soapenv:Header/>" +
                    " <soapenv:Body> <mys:CreateSession> <mys:rq> <mys1:AccountNumber>"+ value.AccountNumber + "</mys1:AccountNumber>" +
                    " <mys1:Password>" + value.Password + "</mys1:Password> <mys1:Target>Production</mys1:Target> " +
                    "<mys1:UserName>"+ value.UserName + "</mys1:UserName> </mys:rq> </mys:CreateSession> </soapenv:Body> </soapenv:Envelope>";

                dynamic headers= new Dictionary<string,string>();
                headers["SOAPAction"]="Mystifly.OnePoint/OnePoint/CreateSession";
                var resp = Common.CommonUtils.xmlClient_PostSoapData("http://192.168.0.249/APIV2/OnePoint.svc", postData,"Post", headers);
                return resp;
                //return xDoc["s:Envelope"]["s:Body"]["CreateSessionResponse"]["CreateSessionResult"]["a:SessionId"].LastChild.Value;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
