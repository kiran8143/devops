using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace OnePointRestAPI.Common
{
    public static class RedisCacheHelper
    {
        public static dynamic Get(string cacheKey)
        {
            return Deserialize(GetDatabase().StringGet(cacheKey));
        }

       

        public static void Set(string cacheKey, object cacheValue)
        {
            GetDatabase().StringSet(cacheKey, Serialize(cacheValue), new TimeSpan(0, (int)CommonUtils.AppConfig.ConnectionStrings.TTL_In_min, 0));
        }

        private static dynamic Serialize(object obj)
        {
            if (obj == null)
            {
                return null;
            }
            return CommonUtils.JsonSerialize(obj);
            //BinaryFormatter objBinaryFormatter = new BinaryFormatter();
            //using (MemoryStream objMemoryStream = new MemoryStream())
            //{
            //    objBinaryFormatter.Serialize(objMemoryStream, obj);
            //    byte[] objDataAsByte = objMemoryStream.ToArray();
            //    return objDataAsByte;
            //}           
        }

        private static dynamic Deserialize(string str)
        {
            if (str == null)
            {
                return null;
            }
            return CommonUtils.JsonDeSerialize(str);
            //using (MemoryStream objMemoryStream = new MemoryStream(bytes))
            //{
            //    T result = (T)objBinaryFormatter.Deserialize(objMemoryStream);
            //    return result;
            //}
        }

        public static StackExchange.Redis.IDatabase GetDatabase()
        {
            StackExchange.Redis.IDatabase databaseReturn = null;
            string connectionString =(string)CommonUtils.AppConfig.ConnectionStrings.RedisConnection;
            var connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);
            if (connectionMultiplexer.IsConnected)
                databaseReturn = connectionMultiplexer.GetDatabase();

            return databaseReturn;
        }
    }
}
