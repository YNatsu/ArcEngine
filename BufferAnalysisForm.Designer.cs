namespace ArcEngine
{
    partial class BufferAnalysisForm
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
            this.inputComBox = new System.Windows.Forms.ComboBox();
            this.outputPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.linearUnitButton = new System.Windows.Forms.RadioButton();
            this.propertyButton = new System.Windows.Forms.RadioButton();
            this.bufferDistanceBox = new System.Windows.Forms.TextBox();
            this.unitsComBox = new System.Windows.Forms.ComboBox();
            this.propertyBox = new System.Windows.Forms.ComboBox();
            this.确定 = new System.Windows.Forms.Button();
            this.取消 = new System.Windows.Forms.Button();
            this.selectPath = new System.Windows.Forms.Button();
            this.txtMessages = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "输入要素";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "输出要素";
            // 
            // inputComBox
            // 
            this.inputComBox.FormattingEnabled = true;
            this.inputComBox.Location = new System.Drawing.Point(13, 48);
            this.inputComBox.Name = "inputComBox";
            this.inputComBox.Size = new System.Drawing.Size(610, 23);
            this.inputComBox.TabIndex = 2;
            // 
            // outputPath
            // 
            this.outputPath.Location = new System.Drawing.Point(13, 104);
            this.outputPath.Name = "outputPath";
            this.outputPath.Size = new System.Drawing.Size(541, 25);
            this.outputPath.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 146);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "距离";
            // 
            // linearUnitButton
            // 
            this.linearUnitButton.AutoSize = true;
            this.linearUnitButton.Location = new System.Drawing.Point(36, 180);
            this.linearUnitButton.Name = "linearUnitButton";
            this.linearUnitButton.Size = new System.Drawing.Size(88, 19);
            this.linearUnitButton.TabIndex = 5;
            this.linearUnitButton.TabStop = true;
            this.linearUnitButton.Text = "线性单位";
            this.linearUnitButton.UseVisualStyleBackColor = true;
            // 
            // propertyButton
            // 
            this.propertyButton.AutoSize = true;
            this.propertyButton.Location = new System.Drawing.Point(36, 245);
            this.propertyButton.Name = "propertyButton";
            this.propertyButton.Size = new System.Drawing.Size(58, 19);
            this.propertyButton.TabIndex = 6;
            this.propertyButton.TabStop = true;
            this.propertyButton.Text = "字段";
            this.propertyButton.UseVisualStyleBackColor = true;
            // 
            // bufferDistanceBox
            // 
            this.bufferDistanceBox.Location = new System.Drawing.Point(36, 214);
            this.bufferDistanceBox.Name = "bufferDistanceBox";
            this.bufferDistanceBox.Size = new System.Drawing.Size(434, 25);
            this.bufferDistanceBox.TabIndex = 7;
            // 
            // unitsComBox
            // 
            this.unitsComBox.FormattingEnabled = true;
            this.unitsComBox.Items.AddRange(new object[] {
            "米",
            "千米",
            "英里"});
            this.unitsComBox.Location = new System.Drawing.Point(476, 214);
            this.unitsComBox.Name = "unitsComBox";
            this.unitsComBox.Size = new System.Drawing.Size(147, 23);
            this.unitsComBox.TabIndex = 8;
            // 
            // propertyBox
            // 
            this.propertyBox.FormattingEnabled = true;
            this.propertyBox.Items.AddRange(new object[] {
            "米",
            "千米"});
            this.propertyBox.Location = new System.Drawing.Point(36, 279);
            this.propertyBox.Name = "propertyBox";
            this.propertyBox.Size = new System.Drawing.Size(587, 23);
            this.propertyBox.TabIndex = 9;
            // 
            // 确定
            // 
            this.确定.Location = new System.Drawing.Point(378, 340);
            this.确定.Name = "确定";
            this.确定.Size = new System.Drawing.Size(75, 23);
            this.确定.TabIndex = 10;
            this.确定.Text = "确定";
            this.确定.UseVisualStyleBackColor = true;
            this.确定.Click += new System.EventHandler(this.确定_Click);
            // 
            // 取消
            // 
            this.取消.Location = new System.Drawing.Point(517, 340);
            this.取消.Name = "取消";
            this.取消.Size = new System.Drawing.Size(75, 23);
            this.取消.TabIndex = 11;
            this.取消.Text = "取消";
            this.取消.UseVisualStyleBackColor = true;
            this.取消.Click += new System.EventHandler(this.取消_Click);
            // 
            // selectPath
            // 
            this.selectPath.Location = new System.Drawing.Point(560, 104);
            this.selectPath.Name = "selectPath";
            this.selectPath.Size = new System.Drawing.Size(75, 23);
            this.selectPath.TabIndex = 12;
            this.selectPath.Text = "选择";
            this.selectPath.UseVisualStyleBackColor = true;
            this.selectPath.Click += new System.EventHandler(this.btnOutputLayer_Click);
            // 
            // txtMessages
            // 
            this.txtMessages.Location = new System.Drawing.Point(13, 389);
            this.txtMessages.Name = "txtMessages";
            this.txtMessages.Size = new System.Drawing.Size(622, 96);
            this.txtMessages.TabIndex = 13;
            this.txtMessages.Text = "";
            // 
            // BufferAnalysisForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 511);
            this.Controls.Add(this.txtMessages);
            this.Controls.Add(this.selectPath);
            this.Controls.Add(this.取消);
            this.Controls.Add(this.确定);
            this.Controls.Add(this.propertyBox);
            this.Controls.Add(this.unitsComBox);
            this.Controls.Add(this.bufferDistanceBox);
            this.Controls.Add(this.propertyButton);
            this.Controls.Add(this.linearUnitButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.outputPath);
            this.Controls.Add(this.inputComBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "BufferAnalysisForm";
            this.Text = "BufferAnalysisForm";
            this.Load += new System.EventHandler(this.BufferAnalysisForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox inputComBox;
        private System.Windows.Forms.TextBox outputPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton linearUnitButton;
        private System.Windows.Forms.RadioButton propertyButton;
        private System.Windows.Forms.TextBox bufferDistanceBox;
        private System.Windows.Forms.ComboBox unitsComBox;
        private System.Windows.Forms.ComboBox propertyBox;
        private System.Windows.Forms.Button 确定;
        private System.Windows.Forms.Button 取消;
        private System.Windows.Forms.Button selectPath;
        private System.Windows.Forms.RichTextBox txtMessages;
    }
}