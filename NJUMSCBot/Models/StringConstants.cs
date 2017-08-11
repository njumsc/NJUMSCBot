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
        static StringConstants()
        {
            Data.Data.Read<StringConstants>(nameof(StringConstants));
        }

        [JsonProperty]
        public static string Welcome { get; private set; }
        [JsonProperty]
        public static string UnknownIntent { get; private set; }
        [JsonProperty]
        public static string Metainfo { get; private set; }
        [JsonProperty]
        public static string Help { get; private set; }
        [JsonProperty]
        public static string DepartmentNotExist { get; private set; }
        [JsonProperty]
        public static string HelpPrompt { get; private set; }
        [JsonProperty]
        public static string CompetitionNotExist { get; private set; }
        [JsonProperty]
        public static string ActivityNotExist { get; private set; }
        [JsonProperty]
        public static string BenefitNotExist { get; private set; }
        [JsonProperty]
        public static string DepartmentIntroduction { get; private set; }

    }
}