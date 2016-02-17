using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebModel
{
    public class Pinglun
    {
        public int uid { get; set; }
        public string name { get; set; }
        public string Info { get; set; }
        public int plstate { get; set; }
        public DateTime addtime { get; set; }
    }


    public class SjComment
    {
        public string Ucode { get; set; }
        public int Evaluate { get; set; }
        public string CustomerName { get; set; }
        public string Cellphone
        { get; set; }
        public string Comment { get; set; }
        public DateTime Create_time { get; set; }
    }
}
