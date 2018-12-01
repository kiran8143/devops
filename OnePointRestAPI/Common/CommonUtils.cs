#region Header
/*
 ************************************************************************************
 Name: Common Utils
 Description: This is for executing all the common operations
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
using System.Linq;
using Newtonsoft.Json;
using RestSharp.Portable.HttpClient;
using RestSharp.Portable;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Dynamic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using System.Text.RegularExpressions;
using System.IO;
using System.Net;
using System.Text;
using System.Xml;
using OnePointRestAPI.Common.Logger;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System.IO.Compression;

namespace OnePointRestAPI.Common
{
    public static class CommonUtils
    {
        
        #region Reading Configuration and Response Message Files
        public static dynamic AppConfig
        {
            get
            {
                #region For console Environment
                //Reading File
                //var appConfig = new ConfigurationBuilder()
                //  .SetBasePath(Directory.GetCurrentDirectory())
                //  .AddJsonFile(@"AppConfig/config.json").Build();
                ////Creating Dictionary
                //dynamic appConfigDict = new Dictionary<string, string>();
                ////Pushing data into dictionary
                //appConfig.GetChildren().ToList().ForEach(x => appConfigDict.Add(x.Key, CommonUtils.CnvStr(x.Value)));
                ////Serialising the dictionary to make it normal object after Deserialization
                //string serializedConfigJson = JsonConvert.SerializeObject(appConfigDict);
                //return JsonConvert.DeserializeObject(serializedConfigJson);
                #endregion

                #region For IIS Server Environment
                return JsonConvert.DeserializeObject(System.IO.File.ReadAllText(Path.Combine("AppConfig", "config.json")));
                #endregion
            }
        }

        public static dynamic ResponseFilterConfig
        {
            get
            {
                #region For IIS Server Environment
                return JsonConvert.DeserializeObject(System.IO.File.ReadAllText(Path.Combine("AppConfig", "response_filter_config.json")));
                #endregion
            }
        }

        public static dynamic Message
        {
            get
            {
                #region For  Environment
                //Reading File
                var responseMessage = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile(@"AppConfig/response_messages_en.json").Build();
                //Creating Dictionary
                Dictionary<string, string> responseMessageDict = new Dictionary<string, string>();
                //Pushing data into dictionary
                responseMessage.GetChildren().ToList().ForEach(x => responseMessageDict.Add(x.Key, CommonUtils.CnvStr(x.Value)));
                //Serialising the dictionary to make it normal object after Deserialization
                string serializedMessageJson = JsonConvert.SerializeObject(responseMessageDict);
                return JsonConvert.DeserializeObject(serializedMessageJson);
                #endregion

                #region For IIS Server Environment
                //return JsonConvert.DeserializeObject(System.IO.File.ReadAllText(@"AppConfig\response_messages_en.json"));    
                #endregion
            }
        }

       

        #endregion Reading Configuration and Message Files

        /// <summary>
        /// This generic method is used to check if property exist in dynamic object
        /// </summary>
        /// <param name="dynamicObject">any dynamic object to compare</param>
        /// <param name="propertyToCheck">name of the property To Check if exists</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool CheckIfPropertyExistInDynamicObject(dynamic dynamicObject, string propertyToCheck)
        {
            if (dynamicObject.GetType().FullName.Equals("Newtonsoft.Json.Linq.JObject"))
            {
                return ((IDictionary<string, Newtonsoft.Json.Linq.JToken>)dynamicObject).ContainsKey(propertyToCheck);
            }
            else
            {
                return ((IDictionary<string, Object>)dynamicObject).ContainsKey(propertyToCheck);
            }
        }

        /// <summary>
        /// This generic method is used to check if key exist in dictionary
        /// </summary>
        /// <param name="dynamicObject">any dynamic object to compare</param>
        /// <param name="propertyToCheck">name of the property To Check if exists</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static bool CheckIfKeyExistInDictionary(dynamic dictionary, string keyToCheck)
        {
            var keyExists = false;
            foreach (var item in dictionary.Keys)
            {
                if(item == keyToCheck)
                {
                    keyExists = true;
                    break;
                }
            }
            return keyExists;
        }

        /// <summary>
		/// </summary>
		/// <param name="input"></param>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		public static bool RemoveProperties(dynamic input, string propertyName)
        {
            var dict = (IDictionary<string, object>)input;
            dict.Remove(propertyName);
            return dict.Remove(propertyName);
        }
        /// <summary>
		/// </summary>
		/// <param name="input"></param>
		/// <param name="propertylist"></param>
		/// <returns></returns>
		public static dynamic RemovePropertiesList(dynamic input, List<string> propertyName)
        {
            JObject raw_value = JObject.FromObject(input);
            foreach (string prop in propertyName)
            {
                JToken value;
                raw_value.TryGetValue(prop, out value);
                if (value!=null)
                {
                    raw_value.Remove(prop);
                }
                             
            }
            return raw_value;
        }

        /// <summary>
		/// </summary>
		/// <param name="input"></param>
		/// <param name="propertylist"></param>
		/// <returns></returns>
		public static dynamic RetainPropertiesList(dynamic input, List<string> propertyName)
        {
            JObject finalobj = new JObject();
            if (input != null)
            {
                JObject raw_value = JObject.FromObject(input);
                foreach (string prop in propertyName)
                {
                    JToken value;
                    raw_value.TryGetValue(prop, out value);
                    if (value != null)
                    {
                        finalobj.Add(prop, value);
                    }

                }
            }
            return finalobj;
        }

        public static string GetRandomAlphaNumeric(int padLength)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(Enumerable.Repeat(chars, padLength)
                        .Select(s => s[random.Next(s.Length)])
                        .ToArray());
            return result;
        }

        /// <summary>
        /// Random unique strings like the ones being generated by MSDN library
        /// </summary>
        /// <remarks>http://stackoverflow.com/questions/730268/unique-random-string-generation/730418#730418</remarks>
        /// <returns></returns>
        public static string UniqueRandomString()
        {
            Guid g = Guid.NewGuid();
            string GuidString = Convert.ToBase64String(g.ToByteArray());
            GuidString = GuidString.Replace("=", "");
            GuidString = GuidString.Replace("+", "");
            return GuidString;
        }
        #region "Datatype conversions from string"

        public static Decimal StrToDecimal(string value)
        {
            return StrToDecimal(value, 0);
        }
        public static Decimal StrToDecimal(string value, Decimal defaultData)
        {
            Decimal result;
            if (!Decimal.TryParse(value, out result))
            {
                result = defaultData;
            }
            return result;
        }
        public static Double StrToDouble(string value)
        {
            return StrToDouble(value, 0);
        }
        public static Double StrToDouble(string value, Double defaultData)
        {
            Double result;
            if (!Double.TryParse(value, out result))
            {
                result = defaultData;
            }
            return result;
        }
        public static Boolean StrToBoolean(string value)
        {
            return StrToBoolean(value, false);
        }
        public static Boolean StrToBoolean(string value, Boolean defaultData)
        {
            Boolean result;
            if (!Boolean.TryParse(value, out result))
            {

                int intdata;
                if (Int32.TryParse(value, out intdata))
                {

                    result = (intdata != 0);
                }
                else
                {
                    result = defaultData;
                }
            }
            return result;
        }
        public static int StrToInt(string value)
        {
            return StrToInt(value, 0);
        }
        public static int StrToInt(string value, int defaultData)
        {
            int result;
            if (!Int32.TryParse(value, out result))
            {
                result = defaultData;
            }
            return result;
        }
        public static long StrToLong(string value)
        {
            try
            {
                long result;
                if (!long.TryParse(value, out result))
                {
                    result = 0;
                }
                return result;
            }
            catch (Exception ex)
            {
               
                return 0;
            }
        }
        /// <summary>
        /// Taken from http://haacked.com/archive/2007/08/21/i-knew-how-to-validate-an-email-address-until-i.aspx
        /// </summary>
        /// <returns></returns>
        private static Regex CreateValidEmailRegex()
        {
            string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
        }
        public static bool IsEmailValid(string emailAddress)
        {
            Regex ValidEmailRegex = CreateValidEmailRegex();
            bool isValid = ValidEmailRegex.IsMatch(emailAddress);

            return isValid;
        }

        public static short StrToShort(string value)
        {
            try
            {
                short result;
                if (!short.TryParse(value, out result))
                {
                    result = 0;
                }
                return result;
            }
            catch (Exception ex)
            {
               
                return 0;
            }
        }

        public static string CnvStr(object target)
        {
            if (target == null)
            {
                return string.Empty;
            }
            //if (target == DBNull.Value)
            //{
            //    return string.Empty;
            //}
            try
            {
                string ret = target.ToString();
                return ConvProperString(ret);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
		/// Replaces single quote with two single quotes and also preserves Leading and Trailing spaces [indentations]
		/// </summary>
		/// <param name="input">string</param>
		/// <returns>string</returns>
		public static string ConvProperString(string input)
        {
            string wk = string.Empty;
            wk = Regex.Replace(input, @"^(?<1>\r\n)*(?<3>.*?)\s*$", @"$3", RegexOptions.Multiline);
            return wk;
        }

        public static string GetAsDBString(string input)
        {
            string result;
            result = string.IsNullOrEmpty(input) ? "" : input.Replace("'", "''");
            return "'" + result + "'";
        }

        /// <summary>
        /// This generic method to split the string by comma
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string[] SplitString(string value)
        {
            var returnStr = new string[] { };
            if (!string.IsNullOrEmpty(value))
            {
                returnStr = value.Split(',');
            }
            return returnStr;
        }



        /// <summary>
        /// This generic method to split the string by comma
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static List<string> SplitStringToList(string value)
        {
            var returnStr = new string[] { };
            if (!string.IsNullOrEmpty(value))
            {
                returnStr = value.Split(',');
            }
           
            List<string> Items = new List<string>();

            foreach (string arrItem in returnStr)
            {
                Items.Add(arrItem);
            }
            return Items;
        }


    #endregion "Datatype conversions from string"

    #region "JSON Serialization & De-serialization using NEWTONSOFT"

    /// <summary>
    /// Method called to serialize a dynamic object
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public static dynamic JsonSerialize(dynamic request)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(request);
        }

        /// <summary>
        /// Method called to serialize a dynamic object with datetime in a format
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static dynamic JsonSerializeWithDateFormatter(dynamic request)
        {
            return JsonConvert.SerializeObject(request, new IsoDateTimeConverter());
        }

        /// <summary>
        /// Method called to De-serialize a dynamic object
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static dynamic JsonDeSerialize(dynamic request)
        {
            return JsonConvert.DeserializeObject(request);
        }

        /// <summary>
        /// Method called to De-serialize a dynamic object
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static dynamic stringToJObject(string request)
        {
            return JObject.Parse(request);
        }

        #endregion "JSON Serialization & De-serialization using NEWTONSOFT"

      

        #region Making HTTP calls

        /// <summary>
        /// Makes HTTP GET call to apiUrl
        /// </summary>
        /// <param name="apiUrl"></param>
        /// <returns>response string</returns>
        public static dynamic RestClient_GET(string url)
        {
            RestClient client = new RestClient();
            dynamic request = new RestRequest();
            dynamic response = new ExpandoObject();
            try
            {
                System.Uri uri = new System.Uri(url);
                client.BaseUrl = uri;
                request.Method = Method.GET;
               // request.AddHeader("Content-Type", "application/json; charset=utf-8");
              //  request.AddHeader("Accept", "application/json");
                response.Data = Common.CommonUtils.JsonDeSerialize(client.Execute(request).Result.Content);
                response.Success = response.Data!=null?true:false;
            }
            catch (Exception ex)
            {
                response.Data = null;
                response.Success = false;
                response.Message = ex.Message;
                UtilsFactory.Logger.Log(ex, LogType.Error);
            }
            return response;
        }

        /// <summary>
        /// Send REST request.
        /// </summary>
        /// <param name="key"></param>		
        /// <returns>Response</returns>
        public static dynamic RestClient_PostData(string serviceUrl, dynamic data,string Method,dynamic headers)
        {
            dynamic resp = new ExpandoObject();

            try
            {
                // Create a request using a URL that can receive a post. 
                WebRequest request = WebRequest.Create(serviceUrl);
                // Set the Method property of the request to POST.
                request.Method = Method;
                // Create POST data and convert it to a byte array.
                string postData = data;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                var eoAsDict = ((IDictionary<String, String>)headers);
                foreach (var kvp in eoAsDict)
                {
                    request.Headers.Add(kvp.Key, kvp.Value);
                }
                request.ContentType = "application/json";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                Console.WriteLine(responseFromServer);
                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();
               
                resp.Data = JsonDeSerialize(responseFromServer);
                resp.Success = resp.Data != null ? true : false;
            }
            catch (Exception ex)
            {
                resp.Data = null;
                resp.Success = false;
                resp.Message = ex.Message;
                UtilsFactory.Logger.Log(ex, LogType.Error);

            }

            return resp;
        }

        /// <summary>
        /// Send REST request.
        /// </summary>
        /// <param name="key"></param>		
        /// <returns>Response</returns>
        public static dynamic RestClient_PostData_gzip(string Url, dynamic PostData, string Method, dynamic headers)
        {
            dynamic resp = new ExpandoObject();

            try
            {
                HttpWebRequest Http = (HttpWebRequest)WebRequest.Create(Url);

               
                    Http.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip,deflate");

                if (!string.IsNullOrEmpty(PostData))
                {
                    Http.Method = Method;
                    byte[] lbPostBuffer = Encoding.Default.GetBytes(PostData);

                    Http.ContentLength = lbPostBuffer.Length;

                    Stream PostStream = Http.GetRequestStream();
                    PostStream.Write(lbPostBuffer, 0, lbPostBuffer.Length);
                    PostStream.Close();
                }

                HttpWebResponse WebResponse = (HttpWebResponse)Http.GetResponse();

                Stream responseStream = responseStream = WebResponse.GetResponseStream();
                if (WebResponse.ContentEncoding.ToLower().Contains("gzip"))
                    responseStream = new GZipStream(responseStream, CompressionMode.Decompress);
                else if (WebResponse.ContentEncoding.ToLower().Contains("deflate"))
                    responseStream = new DeflateStream(responseStream, CompressionMode.Decompress);

                StreamReader Reader = new StreamReader(responseStream, Encoding.Default);

                string responseFromServer = Reader.ReadToEnd();

                WebResponse.Close();
                responseStream.Close();

                resp.Data = responseFromServer;
                resp.Success = resp.Data != null ? true : false;
            }
            catch (Exception ex)
            {
                resp.Data = null;
                resp.Success = false;
                resp.Message = ex.Message;
                UtilsFactory.Logger.Log(ex, LogType.Error);

            }

            return resp;
        }


        /// <summary>
        /// Send REST request.
        /// </summary>
        /// <param name="key"></param>		
        /// <returns>Response</returns>
        public static dynamic xmlClient_PostSoapData(string serviceUrl, string data, string Method,dynamic headers)
        {
            dynamic resp = new ExpandoObject();

            try
            {
                // Create a request using a URL that can receive a post. 
                WebRequest request = WebRequest.Create(serviceUrl);
                // Set the Method property of the request to POST.
                request.Method = Method;
                // Create POST data and convert it to a byte array.
                string postData = data;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.
                var eoAsDict = ((IDictionary<String, String>)headers);
                foreach (var kvp in eoAsDict)
                {                   
                    request.Headers.Add(kvp.Key,kvp.Value);
                }
                request.ContentType = "text/xml";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse response = request.GetResponse();
                // Display the status.
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                Console.WriteLine(responseFromServer);
                // Clean up the streams.
                reader.Close();
                dataStream.Close();
                response.Close();
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(responseFromServer);
                resp.Data = xDoc;
                resp.Success = resp.Data != null ? true : false;
            }
            catch (Exception ex)
            {
                resp.Data = null;
                resp.Success = false;
                resp.Message = ex.Message;
                UtilsFactory.Logger.Log(ex, LogType.Error);
            }

            return resp;
        }


        #endregion Making HTTP calls
        #region "Encryption and Decryption"
        /// <summary>
        /// Decrypt a crypted string.
        /// </summary>
        public static dynamic Decrypt(string cryptedString)
        {
           
            return cryptedString;
        }

        /// <summary>
        /// Encrypt a plain text.
        /// </summary>
        public static dynamic Encrypt(string originalString)
        {
            
            return originalString;
        }

        #endregion "Encryption and Decryption"
        public static Boolean UrlToExclude(string path)
        {
            String[] urls = CommonUtils.SplitString((string)CommonUtils.AppConfig.RoutesToExcludeFromAuthentication);

            foreach (var url in urls)
            {
                if (path.ToLower().Contains(url.Replace("\n", "").Trim().ToLower()))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsList(object o)
        {
            if (o == null) return false;
            return o is IList &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }

        public static bool IsDictionary(object o)
        {
            if (o == null) return false;
            return o is IDictionary &&
                   o.GetType().IsGenericType &&
                   o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>));
        }

        public static IEnumerable<String> GetErrors(this ModelStateDictionary modelState)
        {

            return modelState.Values.SelectMany(v => v.Errors)
                                    .Select(v => v.ErrorMessage + " " + v.Exception).ToList();

        }
        public static List<String> GetobjectKeys(JObject input)
        {

            return input.Properties().Select(p => p.Name).ToList();

        }

        /// <summary>
        /// Copy source stream to destination stream.
        /// </summary>
        /// <param name="src">Source stream.</param>
        /// <param name="dest">Destination stream.</param>
        public static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }

        /// <summary>
        /// Compresses string to byte array using GZipStream.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static byte[] Zip(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    //msi.CopyTo(gs);
                    CopyTo(msi, gs);
                }

                return mso.ToArray();
            }
        }

        /// <summary>
        /// Decompresses byte array to string.
        /// </summary>
        /// <param name="bytes">Byre array.</param>
        /// <returns>string.</returns>
        public static string Unzip(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    //gs.CopyTo(mso);
                    CopyTo(gs, mso);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }

        public static HttpContext CurrentContext => new HttpContextAccessor().HttpContext;
        public static string UserHostAddress => CurrentContext.Features.Get<IHttpConnectionFeature>().RemoteIpAddress.ToString();
        public static string UserAgent => CurrentContext.Request.Headers["User-Agent"].ToString();
        public static dynamic requestBody => CurrentContext.Items["RawRequestBody"];

        public static byte[] strToToHexByte(string hexString)
        {
            // byte[] array = Encoding.ASCII.GetBytes(hexString);
            byte[] array = Convert.FromBase64String(hexString);
            return array;
        }



    }
}
