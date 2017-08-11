using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using NJUMSCBot.Models;

namespace NJUMSCBot.Data
{
    public static class Data
    {

        static Data()
        {
            Constants = Read<StringConstants>(nameof(StringConstants));
            DepartmentInfo = Read<Info<Department>>("DepartmentInfo");
            BenefitInfo = Read<Info<Item>>("BenefitInfo");
            ActivityInfo = Read<Info<Item>>("ClubActivityInfo");
            CompetitionInfo = Read<Info<Item>>("CompetitionInfo");
            ClubIntro = Read<ClubIntroduction>("ClubIntroduction");
        }

        public static StringConstants Constants { get; private set; }
        public static Info<Department> DepartmentInfo { get; private set; }
        public static Info<Item> BenefitInfo { get; private set; }
        public static Info<Item> ActivityInfo { get; private set; }
        public static Info<Item> CompetitionInfo { get; private set; }
        public static ClubIntroduction ClubIntro { get; private set; }



        /// <summary>
        /// Fetch data from json files
        /// </summary>
        /// <typeparam name="TReturn">Type of return value</typeparam>
        /// <returns>deserialized data in json files of type TReturn </returns>
        private static TReturn Read<TReturn>(string fileName)
        {
            string content = Properties.Resources.ResourceManager.GetString(fileName);
            return JsonConvert.DeserializeObject<TReturn>(content);

        }
        
        public static void ForEach<T>(this IEnumerable<T> array, Action<T> action)
        {
            foreach(T a in array)
            {
                action(a);
            }
        }
    }
}