using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PublisherConsoleApp
{
    public static class HelperSerialize
    {
        public static byte[] Serialize<T>(this T obj)
        {
            if (obj is null)
            {
                return null;
            }
            var json = JsonConvert.SerializeObject(obj);
            return Encoding.UTF8.GetBytes(json);
        }
    }
}
