﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NJUMSCBot.Models
{
    [Serializable]
    public class Department : Item
    {
        public string President { get; set; }
        public string VicePresident { get; set; }

        public override string ToString()
        {
            return $"{Description}\n\n目前部长是{President}，副部长是{VicePresident}。";
        }

    }
}