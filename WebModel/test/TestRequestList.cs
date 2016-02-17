using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebModel
{
    public class TestRequestList
    {
        public List<TestReqeustItem> RequestItemList { get; set; }
        public TestRequestList()
        {
            RequestItemList = new List<TestReqeustItem>();
        }
    }
}
