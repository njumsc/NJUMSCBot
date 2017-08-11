using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NJUMSCBot.Models
{
    [Serializable]
    public class ClubIntroduction
    {
        public string Introduction { get; set; }

        public string Website { get; set; }

        public override string ToString()
        {
            return $"{Introduction}\n我们的网址是{Website}";
        }
    }
}