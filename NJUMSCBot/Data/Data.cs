using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace NJUMSCBot.Data
{
    public static class Data
    {

        /// <summary>
        /// Fetch data from json files
        /// </summary>
        /// <typeparam name="TItem">Type of model of the item</typeparam>
        /// <typeparam name="TReturn">Type of return value</typeparam>
        /// <returns>deserialized data in json files of type TReturn </returns>
        public static TReturn Read<TReturn>(string fileName)
        {
            string content = Properties.Resources.ResourceManager.GetString(fileName);
            var s = JsonConvert.DeserializeObject<TReturn>(content);
            return s;

        }
        
        public static void ForEach<T>(this IEnumerable<T> array, Action<T> action)
        {
            foreach(T a in array)
            {
                action(a);
            }
        }
    }
}