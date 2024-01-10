namespace MWebBrowserForm
{
    partial class CustomWinformWebBrowser
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.CefWebBrowser = new CefSharp.WinForms.ChromiumWebBrowser();
            this.SuspendLayout();
            // 
            // CefWebBrowser
            // 
            this.CefWebBrowser.ActivateBrowserOnCreation = false;
            this.CefWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CefWebBrowser.Location = new System.Drawing.Point(0, 0);
            this.CefWebBrowser.Name = "CefWebBrowser";
            this.CefWebBrowser.Size = new System.Drawing.Size(150, 150);
            this.CefWebBrowser.TabIndex = 0;
            // 
            // CustomWinformWebBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CefWebBrowser);
            this.Name = "CustomWinformWebBrowser";
            this.ResumeLayout(false);

        }

        #endregion

        public CefSharp.WinForms.ChromiumWebBrowser CefWebBrowser;
    }
}
