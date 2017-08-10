using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Resources;
using Microsoft.Bot.Builder.Resource;
using Newtonsoft.Json;

namespace NJUMSCBot.Models
{
    [Serializable]
    public class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ReadMoreUrl { get; set; }

        /// <summary>
        /// Fetch data from json files
        /// </summary>
        /// <typeparam name="TItem">Type of model of the item</typeparam>
        /// <typeparam name="TReturn">Type of return value</typeparam>
        /// <returns>deserialized data in json files of type TReturn </returns>
        public static TReturn Read<TItem,TReturn>() where TItem : Item
        {
            string content = Properties.Resources.ResourceManager.GetString(typeof(TItem).Name);
            var s = JsonConvert.DeserializeObject<TReturn>(content);
            return s;

        }

    }
}