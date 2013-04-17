namespace PrintDemo
{
    partial class PrintForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pageSteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pagePreviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pagePrintToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(365, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pageSteToolStripMenuItem,
            this.pagePreviewToolStripMenuItem,
            this.pagePrintToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(39, 21);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // pageSteToolStripMenuItem
            // 
            this.pageSteToolStripMenuItem.Name = "pageSteToolStripMenuItem";
            this.pageSteToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.pageSteToolStripMenuItem.Text = "Page Setting";
            this.pageSteToolStripMenuItem.Click += new System.EventHandler(this.pageSteToolStripMenuItem_Click);
            // 
            // pagePreviewToolStripMenuItem
            // 
            this.pagePreviewToolStripMenuItem.Name = "pagePreviewToolStripMenuItem";
            this.pagePreviewToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.pagePreviewToolStripMenuItem.Text = "Page Preview";
            this.pagePreviewToolStripMenuItem.Click += new System.EventHandler(this.pagePreviewToolStripMenuItem_Click);
            // 
            // pagePrintToolStripMenuItem
            // 
            this.pagePrintToolStripMenuItem.Name = "pagePrintToolStripMenuItem";
            this.pagePrintToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.pagePrintToolStripMenuItem.Text = "Page Print";
            this.pagePrintToolStripMenuItem.Click += new System.EventHandler(this.pagePrintToolStripMenuItem_Click);
            // 
            // PrintForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(365, 292);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "PrintForm";
            this.Text = "打印实例";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.PrintForm_Paint);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pageSteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pagePreviewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pagePrintToolStripMenuItem;
    }
}

