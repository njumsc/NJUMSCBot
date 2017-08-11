using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NJUMSCBot.Models
{
    [Serializable]
    public class Competition : Item
    {
        static Competition()
        {
            Competitions = Data.Data.Read<Competition[]>(nameof(Competition));
        }

        public static Competition[] Competitions { get; private set; }

    }
}