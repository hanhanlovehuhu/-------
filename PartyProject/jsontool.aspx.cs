using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using WebBll;
using WebModel;
using WebUtility;

namespace PartyProject
{
    public partial class jsontool : System.Web.UI.Page
    {
        /// <summary>
        /// 信号量，保证异步http调用结束后再返回页面
        /// </summary>
        private AutoResetEvent reset = new AutoResetEvent(false);

        /// <summary>
        /// 请求环境字典，静态
        /// </summary>
        private static Dictionary<string, string> UrlDic;

        /// <summary>
        /// 请求类型（酒店、旅店探索等),静态
        /// </summary>
        private static List<string> RequestTypeList;

        /// <summary>
        /// 具体请求信息（key,Value)=（请求类别，具体请求类列表）,静态
        /// </summary>
        private static Dictionary<string, TestRequestList> RequestList;

        public static void ClearCache()
        {
            UrlDic = null;
            RequestTypeList = null;
            RequestList = null;
            
        }
        
        /// <summary>
        /// 页面加载事件
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.QueryString["ChangeCoords"] != null && Request.QueryString["ChangeCoords"].ToString() != "")
            {
                string lat = Request.QueryString["lat"].ToString();
                string lng = Request.QueryString["lng"].ToString();
                ChangeCoords.ChangeCoordinate(ref lat, ref lng, 3, 5);
                Response.Write("lat:" + lat + "  lng:" + lng);
                Response.End();
                return;
            }
            
            //提供ajax调用，将字符串转为MD5返回
            if (Request.QueryString["GETMD5"] != null && Request.QueryString["GETMD5"].ToString() != "")
            {
                Response.Write(EncodingUtil.GetMd5(Request.QueryString["GETMD5"].ToString(), "utf-8"));
                Response.End();
                return;
            }
            if (!Page.IsPostBack || UrlDic == null || RequestTypeList == null || RequestList == null)
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(Server.MapPath("~/Config/TestToolConfig.xml"));

                #region 请求环境

                if (UrlDic == null || UrlDic.Count == 0)
                {
                    DropDownList1.Items.Clear();
                    UrlDic = new Dictionary<string, string>();
                    var node = xmlDoc.SelectSingleNode("/TestTool/TestUrls");
                    foreach (XmlElement childNode in node.ChildNodes)
                    {
                        string name = "";
                        string url = "";
                        foreach (XmlAttribute att in childNode.Attributes)
                        {
                            if (att.Name == "name")
                                name = att.Value;
                            else if (att.Name == "url")
                                url = att.Value;
                        }
                        if (name != "" && url != "")
                        {
                            UrlDic.Add(name, url);
                            DropDownList1.Items.Add(name);
                        }

                    }
                    

                }
                else
                {
                    DropDownList1.Items.Clear();
                    foreach (KeyValuePair<string, string> pair in UrlDic)
                    {
                        DropDownList1.Items.Add(pair.Key);
                    }

                }
                lbl_url.Text = UrlDic[DropDownList1.SelectedItem.Text];

                #endregion

                #region 请求类型

                if (RequestTypeList == null)
                {
                    DropDownList2.Items.Clear();
                    RequestTypeList = new List<string>();
                    var requestType = xmlDoc.SelectSingleNode("/TestTool/TestXmls");
                    foreach (XmlElement childNode in requestType.ChildNodes)
                    {
                        var typeName = childNode.Attributes[0].Value;
                        DropDownList2.Items.Add(typeName);
                        RequestTypeList.Add(typeName);
                        SetRequestList(typeName, childNode);
                    }
                }
                else
                {
                    DropDownList2.Items.Clear();
                    foreach (var typeName in RequestTypeList)
                    {
                        DropDownList2.Items.Add(typeName);
                    }
                }
                if (DropDownList2.Items.Count > 0)
                {
                    SetRequest();
                }

                #endregion
            }
        }

        /// <summary>
        /// 确认发送事件
        /// </summary>
        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string requestXML = tbx_Request.Text.Trim();
                //if (requestXML.IndexOf("|") == 0)
                //{
                //    throw new Exception("服务请求失败");
                //}
                string returnString = JsonBusinessFactory.Execute(requestXML); ;
                tbx_Response.Text = returnString;
            }
            catch (Exception ex)
            {
                string returnString = "EX|" + ex.Message;
                tbx_Response.Text = returnString;
            }
        }

        /// <summary>
        /// 获得MD5
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btn_MD5_Click(object sender, EventArgs e)
        {
            tb_MD5.Text = EncodingUtil.GetMd5(tb_MD5.Text,"utf-8");
        }

        /// <summary>
        /// 发送请求回调方法
        /// </summary>
        /// <param name="result"></param>
        private void RequestCallback(IAsyncResult result)
        {
            byte[] bSendingFile = Encoding.UTF8.GetBytes(tbx_Request.Text);
            HttpWebRequest request = (HttpWebRequest)(result.AsyncState);
            Stream requestStream = request.EndGetRequestStream(result);
            requestStream.Write(bSendingFile, 0, bSendingFile.Length);
            requestStream.Flush();
            requestStream.Close();
            request.BeginGetResponse(WebResponseCallback, request);
        }


        /// <summary>
        /// Http请求结束后调用的回调方法
        /// </summary>
        /// <param name="result"></param>
        private void WebResponseCallback(IAsyncResult result)
        {

            HttpWebRequest request = (HttpWebRequest)(result.AsyncState);
            WebResponse response = request.EndGetResponse(result) as HttpWebResponse;
            if (response != null)
            {
                Stream responseStream = response.GetResponseStream();
                using (StreamReader streamReader = new StreamReader(responseStream))
                {
                    tbx_Response.Text = streamReader.ReadToEnd();
                }

            }
            reset.Set();

        }

        /// <summary>
        /// 测试环境选择事件
        /// </summary>
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (UrlDic != null)
            {
                if (UrlDic.ContainsKey(DropDownList1.SelectedItem.Text))
                {
                    lbl_url.Text = UrlDic[DropDownList1.SelectedItem.Text];
                }
            }

        }


        /// <summary>
        /// 设置ReqeustList
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="node"></param>
        private void SetRequestList(string typeName, XmlElement node)
        {
            if (RequestList == null)
                RequestList = new Dictionary<string, TestRequestList>();
            if (RequestList.Keys.Contains(typeName))
                RequestList.Remove(typeName);
            var list = new TestRequestList();
            foreach (XmlElement childNode in node.ChildNodes)
            {
                list.RequestItemList.Add(new TestReqeustItem()
                                             {
                                                 Id = childNode.Attributes[0].Value,
                                                 RequestName = childNode.Attributes[1].Value,
                                                 XmlFilePath = childNode.Attributes[2].Value
                                             });
            }
            RequestList.Add(typeName, list);
        }

        /// <summary>
        /// 设置请求下拉框
        /// </summary>
        private void SetRequest()
        {
            var typeName = DropDownList2.SelectedItem.Text;
            if (RequestList.ContainsKey(typeName))
            {
                var ob = RequestList[typeName];
                if (ob.RequestItemList.Count > 0)
                {
                    DropDownList3.Items.Clear();
                    foreach (var item in ob.RequestItemList)
                    {
                        DropDownList3.Items.Add(item.RequestName);
                    }
                    lbl_RequestId.Text = ob.RequestItemList[0].Id;
                    var xmlDoc1 = new XmlDocument();
                    xmlDoc1.Load(Server.MapPath(ob.RequestItemList[DropDownList3.SelectedIndex].XmlFilePath));

                    tbx_Request.Text = FormatXml(xmlDoc1);
                }


            }

        }

        /// <summary>
        /// xml格式化
        /// </summary>
        /// <param name="xd">xml文档</param>
        /// <returns>格式化字符串</returns>
        private string FormatXml(XmlDocument xd)
        {

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            XmlTextWriter xtw = null;
            try
            {
                xtw = new XmlTextWriter(sw);

                xtw.Formatting = Formatting.Indented;
                xtw.Indentation = 1;
                xtw.IndentChar = ' ';

                xd.WriteTo(xtw);
            }
            finally
            {
                if (xtw != null)
                    xtw.Close();
            }
            return sb.ToString();
        }

        /// <summary>
        /// 请求类别选择事件
        /// </summary>
        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbx_Response.Text = "";
            SetRequest();
        }

        /// <summary>
        /// 具体请求选择事件
        /// </summary>
        protected void DropDownList3_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbx_Response.Text = "";
            var typeName = DropDownList2.SelectedItem.Text;
            if (RequestList.ContainsKey(typeName))
            {
                var ob = RequestList[typeName];
                if (ob.RequestItemList.Count > 0)
                {

                    lbl_RequestId.Text = ob.RequestItemList[DropDownList3.SelectedIndex].Id;
                    var xmlDoc1 = new XmlDocument();
                    xmlDoc1.Load(Server.MapPath(ob.RequestItemList[DropDownList3.SelectedIndex].XmlFilePath));
                    tbx_Request.Text = FormatXml(xmlDoc1);
                }


            }
        }

    }
}