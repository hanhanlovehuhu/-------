using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebUtility
{
    [Serializable]
    public class PageArgs
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex;
        /// <summary>
        /// 页面大小
        /// </summary>
        public int PageSize;
        /// <summary>
        /// 记录总条数
        /// </summary>
        public int RecordCount;
        /// <summary>
        /// 页面总数
        /// </summary>
        public int PageCount;
    }
}
