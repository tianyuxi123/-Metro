namespace MetroGraphApp
{
    partial class FrmMain
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
            MetroGraphApp.MetroPath metroPath1 = new MetroGraphApp.MetroPath();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.metroGraphView1 = new MetroGraphApp.MetroGraphView();
            this.SuspendLayout();
            // 
            // metroGraphView1
            // 
            this.metroGraphView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.metroGraphView1.BackColor = System.Drawing.Color.White;
            this.metroGraphView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.metroGraphView1.CurPath = metroPath1;
            this.metroGraphView1.EndNode = null;
            this.metroGraphView1.Location = new System.Drawing.Point(12, 12);
            this.metroGraphView1.Name = "metroGraphView1";
            this.metroGraphView1.Size = new System.Drawing.Size(706, 395);
            this.metroGraphView1.StartNode = null;
            this.metroGraphView1.TabIndex = 0;
            this.metroGraphView1.ZoomScale = 1F;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 421);
            this.Controls.Add(this.metroGraphView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmMain";
            this.Text = "上海地铁交通线路图";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private MetroGraphView metroGraphView1;
    }
}

