using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NJUMSCBot.Models
{
    [Serializable]
    public class ClubActivity : Item
    {
        static ClubActivity()
        {
            Activities = Data.Data.Read<ClubActivity[]>(nameof(ClubActivity));
        }

        public static ClubActivity[] Activities { get; private set; }
    }
}