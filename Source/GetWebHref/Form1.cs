using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utils;

namespace GetWebHref
{
    public partial class Form1 : Form
    {
        private JC.LogicBussiness.Facade.IArticleProxy articleProxy =
            new JC.LogicBussiness.Imp.ArticleImpl();

        /// <summary>
        /// 请求线程
        /// </summary>
        private BackgroundWorker worker = new BackgroundWorker();

        /// <summary>
        /// 解析线程
        /// </summary>
        private BackgroundWorker parseWorker = new BackgroundWorker();

        private WebDownloader downloader = null;

        private ResponseContentParser parseResponseContent = null;

        public Form1()
        {
            InitializeComponent();
            
            try
            {
                this.worker.DoWork += worker_DoWork;

                this.parseWorker.DoWork += parseWorker_DoWork;
                this.parseWorker.RunWorkerCompleted += parseWorker_RunWorkerCompleted;

                // 文章分类列表绑定
                BindingSource bindingSource = new BindingSource();
                bindingSource.DataSource = articleProxy.GetAllArticleCategories();
                this.cobArticleCategory.DataSource = bindingSource;
                this.cobArticleCategory.DisplayMember = "Value";
                this.cobArticleCategory.ValueMember = "Key";
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }          

        //开始下载
        private void btnGet_Click(object sender, EventArgs e)
        {
            this.btnGet.Enabled = false;
            this.rtbContent.Text = "";

            //初始化url
            GatherInitUrls();
            //初始化解析器
            this.parseResponseContent = new ResponseContentParser(
                Convert.ToInt32(this.cobArticleCategory.SelectedValue), HandleData.handingUrlQueue.Count, new TestParser());

            this.worker.RunWorkerAsync();
            this.parseWorker.RunWorkerAsync();
        }

        /// <summary>
        /// 初始化url信息
        /// </summary>
        private void GatherInitUrls()
        {
            //域名
            string strPagePre = this.txtDomainUrl.Text.Trim();
            if (strPagePre.EndsWith("/")) 
            {
                strPagePre = strPagePre.Remove(strPagePre.Length - 1);
            }

            //相对路径，分页参数需用占位符替换
            string strPagePost = this.txtRelativeUrl.Text.Trim();
            if (!strPagePost.StartsWith("/"))
            {
                strPagePost = "/" + strPagePost;
            }

            //拼接成完整的请求url
            string strPage = string.Concat(strPagePre, strPagePost);

            //请求开始的页码
            int startPageIndex = 1;
            //请求结束的页码
            int endPageIndex = 1;

            if (!string.IsNullOrEmpty(this.txtStartPageIndex.Text.Trim()))
            {
                int.TryParse(this.txtStartPageIndex.Text.Trim(),out startPageIndex);
            }

            if (!string.IsNullOrEmpty(this.txtEndPageIndex.Text.Trim()))
            {
                int.TryParse(this.txtEndPageIndex.Text.Trim(),out endPageIndex);
            }

            int articleCategoryId = Convert.ToInt32(this.cobArticleCategory.SelectedValue);
            //初始化下载器
            downloader = new WebDownloader(AppHelper.RequestTimeSpan, articleCategoryId);

            string preUrl = "";
            for (int i = startPageIndex; i <= endPageIndex; i++)
            {
                //添加到下载队列
                string strUrl = string.Format(strPage, i);
                if (string.IsNullOrEmpty(preUrl))
                {                    
                    downloader.AddUrlQueue(strUrl, strUrl);
                }
                else
                {
                    downloader.AddUrlQueue(strUrl, preUrl);
                    preUrl = strUrl;
                }
            }
        }

        /// <summary>
        /// 请求数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            //进行数据请求
            downloader.ProcessQueue(Encoding.UTF8);
        }

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void parseWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            parseResponseContent.ParseContent();
        }

        /// <summary>
        /// 解析完成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void parseWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("下载完成");
            this.btnGet.Enabled = true;
        }
    }
}
