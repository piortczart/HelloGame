namespace HelloGame.Client
{
    partial class InitialSetup
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnPlay = new System.Windows.Forms.Button();
            this.tbServerName = new System.Windows.Forms.TextBox();
            this.tbPlayerName = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.lbLog = new System.Windows.Forms.Label();
            this.cbIsLocal = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(133, 61);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(81, 38);
            this.btnPlay.TabIndex = 0;
            this.btnPlay.Text = "Go!";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // tbServerName
            // 
            this.tbServerName.Location = new System.Drawing.Point(3, 3);
            this.tbServerName.Name = "tbServerName";
            this.tbServerName.Size = new System.Drawing.Size(211, 20);
            this.tbServerName.TabIndex = 1;
            // 
            // tbPlayerName
            // 
            this.tbPlayerName.Location = new System.Drawing.Point(3, 29);
            this.tbPlayerName.Name = "tbPlayerName";
            this.tbPlayerName.Size = new System.Drawing.Size(211, 20);
            this.tbPlayerName.TabIndex = 2;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(3, 55);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 3;
            // 
            // lbLog
            // 
            this.lbLog.AutoSize = true;
            this.lbLog.Location = new System.Drawing.Point(3, 118);
            this.lbLog.Name = "lbLog";
            this.lbLog.Size = new System.Drawing.Size(20, 13);
            this.lbLog.TabIndex = 4;
            this.lbLog.Text = "Hi.";
            // 
            // cbIsLocal
            // 
            this.cbIsLocal.AutoSize = true;
            this.cbIsLocal.Location = new System.Drawing.Point(3, 82);
            this.cbIsLocal.Name = "cbIsLocal";
            this.cbIsLocal.Size = new System.Drawing.Size(105, 17);
            this.cbIsLocal.TabIndex = 5;
            this.cbIsLocal.Text = "Start local server";
            this.cbIsLocal.UseVisualStyleBackColor = true;
            this.cbIsLocal.CheckedChanged += new System.EventHandler(this.cbIsLocal_CheckedChanged);
            // 
            // InitialSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbIsLocal);
            this.Controls.Add(this.lbLog);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.tbPlayerName);
            this.Controls.Add(this.tbServerName);
            this.Controls.Add(this.btnPlay);
            this.Name = "InitialSetup";
            this.Size = new System.Drawing.Size(219, 142);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.TextBox tbServerName;
        private System.Windows.Forms.TextBox tbPlayerName;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label lbLog;
        private System.Windows.Forms.CheckBox cbIsLocal;
    }
}
