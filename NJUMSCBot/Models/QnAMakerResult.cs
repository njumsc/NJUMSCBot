using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NJUMSCBot.Models
{
    [Serializable]
    public class QnAPair
    {
        public string Answer { get; set; }
        public string[] Questions { get; set; }
        public double Score { get; set; }
    }

    [Serializable]
    public class QnAResponse
    {
        public QnAPair[] Answers { get; set; }
    }
}