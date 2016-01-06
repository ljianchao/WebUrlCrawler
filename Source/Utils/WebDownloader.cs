using log4net;
using System;
using System.Net;
using System.Text;
using System.Threading;

namespace Utils
{
    /// <summary>
    /// web信息下载
    /// </summary>
    public class WebDownloader
    {
        /// <summary>
        /// 创建日志对象
        /// </summary>
        private readonly ILog logInfo = LogManager.GetLogger(typeof(WebDownloader));

        /// <summary>
        /// 待数量的url数量
        /// </summary>
        private int handingUrlCount = 0;

        private int requestTimeSpan;
        private int articleCategoryId;


        public WebDownloader(int requestTimeSpan, int articleCategoryId)
        {
            this.articleCategoryId = articleCategoryId;
            this.requestTimeSpan = requestTimeSpan;
        }

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
                    HandleData.responseContentQueue.Enqueue(strContent);
                }
            }

            Interlocked.Increment(ref handingUrlCount);
            logInfo.InfoFormat("[downloader]已下载url数量：{0}", handingUrlCount);
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
            cI.referer = refer;

            HandleData.handingUrlQueue.Enqueue(cI);
            //增加数量
            //int count = Interlocked.Increment(ref handingUrlCount);
        }

        /// <summary>
        /// 清空队列
        /// </summary>
        public void ClearQueue()
        {
            HandleData.handingUrlQueue.Clear();
            //this.handingUrlCount = 0;
        }

        /// <summary>
        /// 处理队列
        /// </summary>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public void ProcessQueue(Encoding encoding)
        {
            DateTime startTime = DateTime.Now;
            logInfo.InfoFormat("[downloader]开始处理请求时间：{0}，总共需要处理请求的数量为：{1}",
                startTime, HandleData.handingUrlQueue.Count);
            this.handingUrlCount = 0;

            CrawlerItem cI = null;
            while (HandleData.handingUrlQueue.Count > 0)
            {
                
                cI = HandleData.handingUrlQueue.Dequeue();

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

            DateTime endTime = DateTime.Now;
            logInfo.InfoFormat("[downloader]结束处理请求时间：{0}, 总共耗时：{1}",
                endTime, DateUtils.DateDiffForMillisecond(startTime, endTime));
        }
    }
}
