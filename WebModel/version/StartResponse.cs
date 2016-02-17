using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebModel.version
{
    [Serializable]
    public class StartResponse
    {
        public string Version { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string Notice { get; set; }
        public string PicId { get; set; }
        public string PicSrc { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string HoldTime { get; set; }
        public string Interval { get; set; }
    }
}
