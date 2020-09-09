namespace SDCCVSPackage
{
    partial class SDCCForm
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
            this.FinishButton = new System.Windows.Forms.Button();
            this.SDCCCancelButton = new System.Windows.Forms.Button();
            this.EmptyCheck = new System.Windows.Forms.CheckBox();
            this.SDCCGroup = new System.Windows.Forms.GroupBox();
            this.BuildFormat = new System.Windows.Forms.ComboBox();
            this.BuildFormatLabel = new System.Windows.Forms.Label();
            this.PortTypeLabel = new System.Windows.Forms.Label();
            this.PortType = new System.Windows.Forms.ComboBox();
            this.SDCCGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // FinishButton
            // 
            this.FinishButton.Location = new System.Drawing.Point(180, 119);
            this.FinishButton.Name = "FinishButton";
            this.FinishButton.Size = new System.Drawing.Size(75, 23);
            this.FinishButton.TabIndex = 0;
            this.FinishButton.Text = "Finish";
            this.FinishButton.UseVisualStyleBackColor = true;
            this.FinishButton.Click += new System.EventHandler(this.FinishButton_Click);
            // 
            // SDCCCancelButton
            // 
            this.SDCCCancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.SDCCCancelButton.Location = new System.Drawing.Point(99, 119);
            this.SDCCCancelButton.Name = "SDCCCancelButton";
            this.SDCCCancelButton.Size = new System.Drawing.Size(75, 23);
            this.SDCCCancelButton.TabIndex = 1;
            this.SDCCCancelButton.Text = "Cancel";
            this.SDCCCancelButton.UseVisualStyleBackColor = true;
            this.SDCCCancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // EmptyCheck
            // 
            this.EmptyCheck.AutoSize = true;
            this.EmptyCheck.Location = new System.Drawing.Point(10, 74);
            this.EmptyCheck.Name = "EmptyCheck";
            this.EmptyCheck.Size = new System.Drawing.Size(125, 17);
            this.EmptyCheck.TabIndex = 2;
            this.EmptyCheck.Text = "Create Empty Project";
            this.EmptyCheck.UseVisualStyleBackColor = true;
            // 
            // SDCCGroup
            // 
            this.SDCCGroup.Controls.Add(this.BuildFormat);
            this.SDCCGroup.Controls.Add(this.BuildFormatLabel);
            this.SDCCGroup.Controls.Add(this.PortTypeLabel);
            this.SDCCGroup.Controls.Add(this.PortType);
            this.SDCCGroup.Controls.Add(this.EmptyCheck);
            this.SDCCGroup.Location = new System.Drawing.Point(12, 12);
            this.SDCCGroup.Name = "SDCCGroup";
            this.SDCCGroup.Size = new System.Drawing.Size(243, 101);
            this.SDCCGroup.TabIndex = 3;
            this.SDCCGroup.TabStop = false;
            this.SDCCGroup.Text = "SDCC Configuration Settings";
            // 
            // BuildFormat
            // 
            this.BuildFormat.FormattingEnabled = true;
            this.BuildFormat.Location = new System.Drawing.Point(80, 47);
            this.BuildFormat.Name = "BuildFormat";
            this.BuildFormat.Size = new System.Drawing.Size(156, 21);
            this.BuildFormat.TabIndex = 6;
            this.BuildFormat.Text = "Binary";
            // 
            // BuildFormatLabel
            // 
            this.BuildFormatLabel.AutoSize = true;
            this.BuildFormatLabel.Location = new System.Drawing.Point(6, 50);
            this.BuildFormatLabel.Name = "BuildFormatLabel";
            this.BuildFormatLabel.Size = new System.Drawing.Size(68, 13);
            this.BuildFormatLabel.TabIndex = 5;
            this.BuildFormatLabel.Text = "Build Format:";
            // 
            // PortTypeLabel
            // 
            this.PortTypeLabel.AutoSize = true;
            this.PortTypeLabel.Location = new System.Drawing.Point(7, 22);
            this.PortTypeLabel.Name = "PortTypeLabel";
            this.PortTypeLabel.Size = new System.Drawing.Size(56, 13);
            this.PortTypeLabel.TabIndex = 4;
            this.PortTypeLabel.Text = "Port Type:";
            // 
            // PortType
            // 
            this.PortType.FormattingEnabled = true;
            this.PortType.Location = new System.Drawing.Point(69, 19);
            this.PortType.Name = "PortType";
            this.PortType.Size = new System.Drawing.Size(167, 21);
            this.PortType.TabIndex = 3;
            this.PortType.Text = "gbz80";
            // 
            // SDCCForm
            // 
            this.AcceptButton = this.FinishButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.SDCCCancelButton;
            this.ClientSize = new System.Drawing.Size(267, 149);
            this.Controls.Add(this.SDCCGroup);
            this.Controls.Add(this.SDCCCancelButton);
            this.Controls.Add(this.FinishButton);
            this.Name = "SDCCForm";
            this.Text = "SDCC Project Settings";
            this.SDCCGroup.ResumeLayout(false);
            this.SDCCGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button FinishButton;
        private System.Windows.Forms.Button SDCCCancelButton;
        private System.Windows.Forms.GroupBox SDCCGroup;
        private System.Windows.Forms.Label PortTypeLabel;
        private System.Windows.Forms.Label BuildFormatLabel;
        public System.Windows.Forms.CheckBox EmptyCheck;
        public System.Windows.Forms.ComboBox PortType;
        public System.Windows.Forms.ComboBox BuildFormat;
    }
}