using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NJUMSCBot.Models
{
    [Serializable]
    public class Operations
    {
        public string ClubIntroduction { get; set; } 
        public string Activities { get; set; }
        public string Competitions { get;  set; }
        public string Departments { get; set; }
        public string Benefits { get; set; }
        public string Help { get; set; }
        public string Joining { get; set; }
    }
}