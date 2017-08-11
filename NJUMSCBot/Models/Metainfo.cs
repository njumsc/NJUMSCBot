using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NJUMSCBot.Models
{
    [Serializable]
    public class Metainfo
    {
        static Metainfo()
        {
            Data.Data.Read<Metainfo>(nameof(Metainfo));
        }

        [JsonProperty]
        public static string UpdateTime { get; set; }
        [JsonProperty]
        public static string Github { get; set; }

        public new static string ToString()
        {
            return $"last updated on {UpdateTime}. Join our development on {Github}!";
        }
    }
}