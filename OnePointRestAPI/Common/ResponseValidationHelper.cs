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


using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace OnePointRestAPI.Common
{
    public class ResponseValidationHelper
    {
        /// <summary>
        /// </summary>
        /// <param name="input"></param>
        /// <param name="propertylist"></param>
        /// <returns></returns>
        public static dynamic DynamicResponsefilteronSchemaBasis(dynamic input, dynamic schema)
        {
            JObject finalobj = new JObject();
            if (input != null)
            {
                JObject raw_input = JObject.FromObject(input), raw_schema = JObject.FromObject(schema);
                dynamic input_keys = CommonUtils.GetobjectKeys(raw_input), schema_keys = CommonUtils.GetobjectKeys(raw_schema);
                foreach (var sk in schema_keys)
                {
                    if (raw_input[sk] != null)
                    {
                        finalobj[sk] = ResponseformatterDataTypeBasis(raw_input[sk], raw_schema[sk]);
                    }

                }
               
            }
            return finalobj;
        }

        /// <summary>
        /// </summary>
        /// <param name="input"></param>
        /// <param name="propertylist"></param>
        /// <returns></returns>
        public static dynamic ResponseformatterDataTypeBasis(dynamic input, dynamic schema)
        {
            JObject finalobj = new JObject();
            //if string // if int // if array//if object
            if ((((JToken)input).Type == JTokenType.Array) && (((JToken)schema).Type == JTokenType.Array))
            {
                dynamic ary_keys = new System.Dynamic.ExpandoObject();
                foreach (var ary in schema)
                {
                     ary_keys = CommonUtils.GetobjectKeys(ary);
                }
                JArray arry_finalobj = new JArray();
                foreach (var in_ary in input)
                {
                    dynamic in_ary_keys = CommonUtils.GetobjectKeys(in_ary);
                    foreach (var keyary in ary_keys)
                    {
                        if (in_ary[keyary] != null)
                        {
                            
                            finalobj[keyary] = ResponseformatterDataTypeBasis(in_ary[keyary], schema[0][keyary]);
                        }
                    }
                    arry_finalobj.Add(finalobj);
                }
                return arry_finalobj;

            }
            else if ((((JToken)input).Type == JTokenType.Object) && (((JToken)schema).Type == JTokenType.Object))
            {

                JArray schemaKeys = CommonUtils.GetobjectKeys(schema);              
                foreach (var key in schemaKeys)
                {
                    finalobj[key] = ResponseformatterDataTypeBasis(input[key], schema[key]);
                }
                return finalobj;

            }
            else if ((((JToken)input).Type == JTokenType.String) && (schema == "string"))
            {
                return input.Value;
            }
            else 
            {
                return input.Value;
            }
            //return finalobj;
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
