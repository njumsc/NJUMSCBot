using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace NJUMSCBot.Models
{
    [Serializable]
    public class Benefit : Item
    {
        static Benefit()
        {
            Benefits = Data.Data.Read<Benefit[]>(nameof(Benefit));
        }

        public static Benefit[] Benefits { get; private set; }
    }
}