namespace GetWebHref
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnGet = new System.Windows.Forms.Button();
            this.txtDomainUrl = new System.Windows.Forms.TextBox();
            this.rtbContent = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtRelativeUrl = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEndPageIndex = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtStartPageIndex = new System.Windows.Forms.TextBox();
            this.cobArticleCategory = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnGet
            // 
            this.btnGet.Location = new System.Drawing.Point(527, 15);
            this.btnGet.Name = "btnGet";
            this.btnGet.Size = new System.Drawing.Size(97, 23);
            this.btnGet.TabIndex = 0;
            this.btnGet.Text = "获取";
            this.btnGet.UseVisualStyleBackColor = true;
            this.btnGet.Click += new System.EventHandler(this.btnGet_Click);
            // 
            // txtDomainUrl
            // 
            this.txtDomainUrl.Location = new System.Drawing.Point(83, 49);
            this.txtDomainUrl.Name = "txtDomainUrl";
            this.txtDomainUrl.Size = new System.Drawing.Size(155, 21);
            this.txtDomainUrl.TabIndex = 1;
            // 
            // rtbContent
            // 
            this.rtbContent.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.rtbContent.Location = new System.Drawing.Point(0, 116);
            this.rtbContent.Name = "rtbContent";
            this.rtbContent.Size = new System.Drawing.Size(651, 380);
            this.rtbContent.TabIndex = 2;
            this.rtbContent.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "域名：";
            // 
            // txtRelativeUrl
            // 
            this.txtRelativeUrl.Location = new System.Drawing.Point(375, 49);
            this.txtRelativeUrl.Name = "txtRelativeUrl";
            this.txtRelativeUrl.Size = new System.Drawing.Size(249, 21);
            this.txtRelativeUrl.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(271, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "页面相对地址：";
            // 
            // txtEndPageIndex
            // 
            this.txtEndPageIndex.Location = new System.Drawing.Point(375, 81);
            this.txtEndPageIndex.Name = "txtEndPageIndex";
            this.txtEndPageIndex.Size = new System.Drawing.Size(155, 21);
            this.txtEndPageIndex.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(295, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "结束页数：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "开始页数：";
            // 
            // txtStartPageIndex
            // 
            this.txtStartPageIndex.Location = new System.Drawing.Point(83, 81);
            this.txtStartPageIndex.Name = "txtStartPageIndex";
            this.txtStartPageIndex.Size = new System.Drawing.Size(155, 21);
            this.txtStartPageIndex.TabIndex = 1;
            // 
            // cobArticleCategory
            // 
            this.cobArticleCategory.FormattingEnabled = true;
            this.cobArticleCategory.Location = new System.Drawing.Point(83, 17);
            this.cobArticleCategory.Name = "cobArticleCategory";
            this.cobArticleCategory.Size = new System.Drawing.Size(155, 20);
            this.cobArticleCategory.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 5;
            this.label5.Text = "文章分类：";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 496);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cobArticleCategory);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rtbContent);
            this.Controls.Add(this.txtStartPageIndex);
            this.Controls.Add(this.txtEndPageIndex);
            this.Controls.Add(this.txtRelativeUrl);
            this.Controls.Add(this.txtDomainUrl);
            this.Controls.Add(this.btnGet);
            this.Name = "Form1";
            this.Text = "文章采集";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGet;
        private System.Windows.Forms.TextBox txtDomainUrl;
        private System.Windows.Forms.RichTextBox rtbContent;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRelativeUrl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtEndPageIndex;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtStartPageIndex;
        private System.Windows.Forms.ComboBox cobArticleCategory;
        private System.Windows.Forms.Label label5;
    }
}

