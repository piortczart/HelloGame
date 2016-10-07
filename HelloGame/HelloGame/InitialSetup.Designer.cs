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
            this.lbLog = new System.Windows.Forms.Label();
            this.cbCreateServer = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbClan = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(135, 86);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(81, 38);
            this.btnPlay.TabIndex = 0;
            this.btnPlay.Text = "Go!";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // tbServerName
            // 
            this.tbServerName.Location = new System.Drawing.Point(62, 3);
            this.tbServerName.Name = "tbServerName";
            this.tbServerName.Size = new System.Drawing.Size(151, 20);
            this.tbServerName.TabIndex = 1;
            // 
            // tbPlayerName
            // 
            this.tbPlayerName.Location = new System.Drawing.Point(62, 29);
            this.tbPlayerName.Name = "tbPlayerName";
            this.tbPlayerName.Size = new System.Drawing.Size(151, 20);
            this.tbPlayerName.TabIndex = 2;
            // 
            // lbLog
            // 
            this.lbLog.AutoSize = true;
            this.lbLog.Location = new System.Drawing.Point(91, 135);
            this.lbLog.Name = "lbLog";
            this.lbLog.Size = new System.Drawing.Size(20, 13);
            this.lbLog.TabIndex = 4;
            this.lbLog.Text = "Hi.";
            // 
            // cbCreateServer
            // 
            this.cbCreateServer.AutoSize = true;
            this.cbCreateServer.Location = new System.Drawing.Point(0, 108);
            this.cbCreateServer.Name = "cbCreateServer";
            this.cbCreateServer.Size = new System.Drawing.Size(105, 17);
            this.cbCreateServer.TabIndex = 5;
            this.cbCreateServer.Text = "Start local server";
            this.cbCreateServer.UseVisualStyleBackColor = true;
            this.cbCreateServer.CheckedChanged += new System.EventHandler(this.cbIsLocal_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 5);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "server:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 31);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "name:";
            // 
            // cbClan
            // 
            this.cbClan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbClan.FormattingEnabled = true;
            this.cbClan.Location = new System.Drawing.Point(62, 54);
            this.cbClan.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cbClan.Name = "cbClan";
            this.cbClan.Size = new System.Drawing.Size(151, 21);
            this.cbClan.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 56);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "clan:";
            // 
            // InitialSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbClan);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbCreateServer);
            this.Controls.Add(this.lbLog);
            this.Controls.Add(this.tbPlayerName);
            this.Controls.Add(this.tbServerName);
            this.Controls.Add(this.btnPlay);
            this.Name = "InitialSetup";
            this.Size = new System.Drawing.Size(219, 155);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.TextBox tbServerName;
        private System.Windows.Forms.TextBox tbPlayerName;
        private System.Windows.Forms.Label lbLog;
        private System.Windows.Forms.CheckBox cbCreateServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbClan;
        private System.Windows.Forms.Label label3;
    }
}
