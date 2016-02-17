using System;
using System.Collections.Generic;

namespace WebModel
{

    [Serializable]
    public class UpVerResponse
    {
        public string version { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string Notice { get; set; }
    }
}