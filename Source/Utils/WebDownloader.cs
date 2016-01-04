using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Threading;
using System.Text.RegularExpressions;

namespace Utils
{
    /// <summary>
    /// web信息下载
    /// </summary>
    public class WebDownloader
    {
        /// <summary>
        /// 待数量的url数量
        /// </summary>
        private int handingUrlCount = 0;

        private int requestTimeSpan;
        private int articleCategoryId;
        /// <summary>
        /// 请求队列
        /// </summary>
        private readonly Queue<CrawlerItem> urlQueue = new Queue<CrawlerItem>();

        public WebDownloader(int requestTimeSpan, int articleCategoryId)
        {
            this.articleCategoryId = articleCategoryId;
            this.requestTimeSpan = requestTimeSpan;
            this.handingUrlCount = 0;
        }

        public void SetHandingUrlCount()
        {
            this.handingUrlCount = this.urlQueue.Count;
        }

        public int GetHandingUrlCount()
        {
            return this.handingUrlCount;
        }

        /// <summary>
        /// 发起url请求，并获取返回信息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        /// <param name="strRefer"></param>
        /// <returns></returns>
        //public string GetPageByHttpWebRequest(string url, Encoding encoding, string referer)
        //{

        //    string result = null;

        //    WebResponse response = null;
        //    StreamReader reader = null;

        //    try
        //    {
        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //        request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1; SV1; .NET CLR 2.0.50727)";
        //        request.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, */*";

        //        if (!string.IsNullOrEmpty(referer))
        //        {
        //            Uri u = new Uri(referer);
        //            request.Referer = u.Host;
        //        }
        //        else
        //        {
        //            request.Referer = referer;
        //        }
        //        request.Method = "GET";
        //        response = request.GetResponse();
        //        reader = new StreamReader(response.GetResponseStream(), encoding);
        //        result = reader.ReadToEnd();

        //    }
        //    catch (Exception ex)
        //    {
        //        result = "";
        //    }
        //    finally
        //    {
        //        if (reader != null)
        //            reader.Close();
        //        if (response != null)
        //            response.Close();

        //    }
        //    return result;
        //}

        /// <summary>
        /// 异步下载数据
        /// </summary>
        /// <param name="url"></param>
        /// <param name="encoding"></param>
        void AsyncDownloadString(string requestUrl, string referer, Encoding encoding)
        {
            WebClient webClient = new WebClient();
            webClient.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 6.3; Win64; x64; rv:43.0) Gecko/20100101 Firefox/43.0";
            webClient.Headers["Accept"] = "image / gif, image / x - xbitmap, image / jpeg, image / pjpeg, application / x - shockwave - flash, application / vnd.ms - excel, application / vnd.ms - powerpoint, application / msword, */*";
            webClient.Headers["Referer"] = referer;
            webClient.Encoding = encoding;
            webClient.DownloadStringAsync(new Uri(requestUrl));
            webClient.DownloadStringCompleted += webClient_DownloadStringCompleted;
        }

        /// <summary>
        /// 下载完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
                string strContent = e.Result;

                if (!string.IsNullOrEmpty(strContent))
                {
                    ParseResponseContent.responseContentQueue.Enqueue(strContent);
                }
            }

            int count = Interlocked.Decrement(ref handingUrlCount);
        }

        /// <summary>
        /// 队列新增项
        /// </summary>
        /// <param name="articleCategory">文章分类</param>
        /// <param name="strUrl">请求url</param>
        /// <param name="refer">url的refer</param>
        public void AddUrlQueue(string strUrl, string refer)
        {
            CrawlerItem cI = new CrawlerItem();
            cI.requstUrl = strUrl;
            cI.referer = strUrl;

            urlQueue.Enqueue(cI);
        }

        /// <summary>
        /// 清空队列
        /// </summary>
        public void ClearQueue()
        {
            urlQueue.Clear();
        }

        /// <summary>
        /// 处理队列
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public void ProcessQueue(Encoding encoding)
        {
            CrawlerItem cI = null;
            while (urlQueue.Count > 0)
            {
                cI = urlQueue.Dequeue();

                # region 同步下载
                //cI = urlQueue.Dequeue();
                ////获取请求的内容
                //string strContent = GetPageByHttpWebRequest(cI.requstUrl, encoding, cI.referer);

                //if (!string.IsNullOrEmpty(strContent))
                //{
                //    ParseResponseContent.responseContentQueue.Enqueue(strContent);
                //}

                #endregion

                #region 异步下载

                AsyncDownloadString(cI.requstUrl, cI.referer, encoding);

                #endregion

                //请求一次，进行时间暂停
                Thread.Sleep(this.requestTimeSpan);
            }
        }
    }
}
