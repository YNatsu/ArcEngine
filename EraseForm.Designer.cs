namespace ArcEngine
{
    partial class EraseForm
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
            this.label3 = new System.Windows.Forms.Label();
            this.确定 = new System.Windows.Forms.Button();
            this.取消 = new System.Windows.Forms.Button();
            this.选择 = new System.Windows.Forms.Button();
            this.inputBox = new System.Windows.Forms.ComboBox();
            this.eraseBox = new System.Windows.Forms.ComboBox();
            this.outputBox = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "输入要素";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "擦除要素";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 127);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "输出要素";
            // 
            // 确定
            // 
            this.确定.Location = new System.Drawing.Point(300, 221);
            this.确定.Name = "确定";
            this.确定.Size = new System.Drawing.Size(75, 31);
            this.确定.TabIndex = 3;
            this.确定.Text = "确定";
            this.确定.UseVisualStyleBackColor = true;
            this.确定.Click += new System.EventHandler(this.确定_Click);
            // 
            // 取消
            // 
            this.取消.Location = new System.Drawing.Point(468, 221);
            this.取消.Name = "取消";
            this.取消.Size = new System.Drawing.Size(75, 31);
            this.取消.TabIndex = 4;
            this.取消.Text = "取消";
            this.取消.UseVisualStyleBackColor = true;
            // 
            // 选择
            // 
            this.选择.Location = new System.Drawing.Point(492, 156);
            this.选择.Name = "选择";
            this.选择.Size = new System.Drawing.Size(75, 26);
            this.选择.TabIndex = 5;
            this.选择.Text = "选择";
            this.选择.UseVisualStyleBackColor = true;
            this.选择.Click += new System.EventHandler(this.选择_Click);
            // 
            // inputBox
            // 
            this.inputBox.FormattingEnabled = true;
            this.inputBox.Location = new System.Drawing.Point(22, 31);
            this.inputBox.Name = "inputBox";
            this.inputBox.Size = new System.Drawing.Size(545, 23);
            this.inputBox.TabIndex = 6;
            this.inputBox.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // eraseBox
            // 
            this.eraseBox.FormattingEnabled = true;
            this.eraseBox.Location = new System.Drawing.Point(22, 92);
            this.eraseBox.Name = "eraseBox";
            this.eraseBox.Size = new System.Drawing.Size(545, 23);
            this.eraseBox.TabIndex = 7;
            this.eraseBox.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // outputBox
            // 
            this.outputBox.Location = new System.Drawing.Point(22, 157);
            this.outputBox.Name = "outputBox";
            this.outputBox.Size = new System.Drawing.Size(461, 25);
            this.outputBox.TabIndex = 8;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(22, 258);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(545, 96);
            this.richTextBox1.TabIndex = 9;
            this.richTextBox1.Text = "";
            // 
            // EraseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 375);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.outputBox);
            this.Controls.Add(this.eraseBox);
            this.Controls.Add(this.inputBox);
            this.Controls.Add(this.选择);
            this.Controls.Add(this.取消);
            this.Controls.Add(this.确定);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "EraseForm";
            this.Text = "EraseForm";
            this.Load += new System.EventHandler(this.EraseForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button 确定;
        private System.Windows.Forms.Button 取消;
        private System.Windows.Forms.Button 选择;
        private System.Windows.Forms.ComboBox inputBox;
        private System.Windows.Forms.ComboBox eraseBox;
        private System.Windows.Forms.TextBox outputBox;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}