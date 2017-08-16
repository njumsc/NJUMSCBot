using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NJUMSCBot.Models
{
    [Serializable]
    public class StringConstants
    {
        public string Welcome { get; set; }
        public Dictionary<string, string> Operations { get; set; }

        public string UnknownIntent { get; set; }

        public string Metainfo { get; set; }

        public string Help { get; set; }

        public string Joining { get; set; }

    }
}