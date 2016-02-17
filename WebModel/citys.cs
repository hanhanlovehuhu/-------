using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebModel
{
    public class Citys
    {
        public string  Id { get; set; }
        public string Name { get; set; }
        public string Notice { get; set; }
        public int FirstKm { get; set; }
    }
    [Serializable]
    public class ChengShiModel
    {
        public string Name { get; set; }
        public string Notice { get; set; }
        public int FirstKm { get; set; }
        public IList<PriceItems> PriceItems { get; set; }
    }
    public class PriceItems
    {
        public string Tstart { get; set; }
        public string Tend { get; set; }
        public decimal Price { get; set; }
    }

    public class ChengShiName
    {
        public string name { get; set; }
    }
}
