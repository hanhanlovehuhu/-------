using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aidaijia.API.Entity
{
    public class FavorableInfoEntity
    {
        /// <summary>
        /// 优惠码
        /// </summary>
        public string CouponCode { set; get; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { set; get; }
        /// <summary>
        /// 图片路径
        /// </summary>
        public string ImgPath { set; get; }
        /// <summary>
        /// 图片链接
        /// </summary>
        public string UrlPath { set; get; }
        /// <summary>
        /// 图片说明
        /// </summary>
        public string ImgAlt { set; get; }
        /// <summary>
        /// 图片背影颜色
        /// </summary>
        public string ImgBackColor { set; get; }
    }
}
