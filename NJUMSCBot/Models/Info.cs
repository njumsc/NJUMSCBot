using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NJUMSCBot.Models
{
    [Serializable]
    public class Info<TItem>
    {
        public string Introduction { get; set; }
        public string NotExist { get; set; }
        public TItem[] Items { get; set; }
    }
}