namespace ArcEngine
{
    partial class NetWorkForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.featureDataset = new System.Windows.Forms.ComboBox();
            this.networkDataset = new System.Windows.Forms.ComboBox();
            this.确定 = new System.Windows.Forms.Button();
            this.取消 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(0, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "要素数据集";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(0, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "网络数据集";
            // 
            // featureDataset
            // 
            this.featureDataset.FormattingEnabled = true;
            this.featureDataset.Location = new System.Drawing.Point(21, 40);
            this.featureDataset.Name = "featureDataset";
            this.featureDataset.Size = new System.Drawing.Size(439, 23);
            this.featureDataset.TabIndex = 2;
            this.featureDataset.SelectedIndexChanged += new System.EventHandler(this.featureDataset_SelectedIndexChanged);
            // 
            // networkDataset
            // 
            this.networkDataset.FormattingEnabled = true;
            this.networkDataset.Location = new System.Drawing.Point(21, 123);
            this.networkDataset.Name = "networkDataset";
            this.networkDataset.Size = new System.Drawing.Size(439, 23);
            this.networkDataset.TabIndex = 3;
            // 
            // 确定
            // 
            this.确定.Location = new System.Drawing.Point(212, 184);
            this.确定.Name = "确定";
            this.确定.Size = new System.Drawing.Size(75, 32);
            this.确定.TabIndex = 4;
            this.确定.Text = "确定";
            this.确定.UseVisualStyleBackColor = true;
            this.确定.Click += new System.EventHandler(this.确定_Click);
            // 
            // 取消
            // 
            this.取消.Location = new System.Drawing.Point(333, 184);
            this.取消.Name = "取消";
            this.取消.Size = new System.Drawing.Size(75, 32);
            this.取消.TabIndex = 5;
            this.取消.Text = "取消";
            this.取消.UseVisualStyleBackColor = true;
            this.取消.Click += new System.EventHandler(this.取消_Click);
            // 
            // NetWorkForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 307);
            this.Controls.Add(this.取消);
            this.Controls.Add(this.确定);
            this.Controls.Add(this.networkDataset);
            this.Controls.Add(this.featureDataset);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "NetWorkForm";
            this.Text = "NetWorkForm";
            this.Load += new System.EventHandler(this.NetWorkForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox featureDataset;
        private System.Windows.Forms.ComboBox networkDataset;
        private System.Windows.Forms.Button 确定;
        private System.Windows.Forms.Button 取消;
    }
}