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
        public string UpdateTime { get; set; }
        public string Github { get; set; }

        public override string ToString()
        {
            return $"last updated on {UpdateTime}. Join our development on {Github}!";
        }
    }
}