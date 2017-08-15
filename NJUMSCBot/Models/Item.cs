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
    }
}