namespace MWinFormsCore
{
    partial class BrowserUserControl
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
            this.browserSplitContainer = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.browserSplitContainer)).BeginInit();
            this.browserSplitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // browserSplitContainer
            // 
            this.browserSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.browserSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.browserSplitContainer.Name = "browserSplitContainer";
            this.browserSplitContainer.Panel2Collapsed = true;
            this.browserSplitContainer.Size = new System.Drawing.Size(704, 594);
            this.browserSplitContainer.SplitterDistance = 430;
            this.browserSplitContainer.TabIndex = 0;
            // 
            // BrowserUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.browserSplitContainer);
            this.Name = "BrowserUserControl";
            this.Size = new System.Drawing.Size(704, 594);
            ((System.ComponentModel.ISupportInitialize)(this.browserSplitContainer)).EndInit();
            this.browserSplitContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private SplitContainer browserSplitContainer;
    }
}
