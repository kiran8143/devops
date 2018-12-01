#region Header
/*
 ************************************************************************************
 Name: HelperUtils
 Description: This are the common helper operations
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
using System.ComponentModel;

namespace OnePointRestAPI.Common
{
    public class HelperUtils
    {        
        public static dynamic AttachCommonFields(dynamic input, string action)
        {
            //Map the input dynamic object to dictionary
            var dict = (IDictionary<string, object>)input;
            var UserCode = input.UserCode;
            //Remove the MethodType property which is not required in the data Saving
            //dict.Remove("MethodType");
            switch (action)
            {
                case "POST":                  
                    dict.Add("CreatedBy", UserCode);
                    dict.Add("DateCreated", DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc));
                    dict.Add("ModifiedBy", UserCode);
                    dict.Add("DateModified", DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc));
                    dict.Add("UpdateCount", 1);
                  
                    break;

                default:
                   
                    dict.Add("ModifiedBy", 1);
                    dict.Add("DateModified", DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc));
                    dict["UpdateCount"] = CommonUtils.StrToInt(input.UpdateCount) + 1;
                    break;
            }
            return dict;
        }

        private static Dictionary<String, Object> Dyn2Dict(dynamic dynObj)
        {
            var dictionary = new Dictionary<string, object>();
            foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(dynObj))
            {
                object obj = propertyDescriptor.GetValue(dynObj);
                dictionary.Add(propertyDescriptor.Name, obj);
            }
            return dictionary;
        }
    }
}
