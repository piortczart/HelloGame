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
            this.cbLocalOnly = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(200, 94);
            this.btnPlay.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(122, 58);
            this.btnPlay.TabIndex = 0;
            this.btnPlay.Text = "Go!";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // tbServerName
            // 
            this.tbServerName.Location = new System.Drawing.Point(4, 5);
            this.tbServerName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbServerName.Name = "tbServerName";
            this.tbServerName.Size = new System.Drawing.Size(314, 26);
            this.tbServerName.TabIndex = 1;
            // 
            // tbPlayerName
            // 
            this.tbPlayerName.Location = new System.Drawing.Point(4, 45);
            this.tbPlayerName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tbPlayerName.Name = "tbPlayerName";
            this.tbPlayerName.Size = new System.Drawing.Size(314, 26);
            this.tbPlayerName.TabIndex = 2;
            // 
            // lbLog
            // 
            this.lbLog.AutoSize = true;
            this.lbLog.Location = new System.Drawing.Point(4, 182);
            this.lbLog.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbLog.Name = "lbLog";
            this.lbLog.Size = new System.Drawing.Size(28, 20);
            this.lbLog.TabIndex = 4;
            this.lbLog.Text = "Hi.";
            // 
            // cbCreateLocalServer
            // 
            this.cbCreateServer.AutoSize = true;
            this.cbCreateServer.Location = new System.Drawing.Point(4, 126);
            this.cbCreateServer.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbCreateServer.Name = "cbCreateLocalServer";
            this.cbCreateServer.Size = new System.Drawing.Size(153, 24);
            this.cbCreateServer.TabIndex = 5;
            this.cbCreateServer.Text = "Start local server";
            this.cbCreateServer.UseVisualStyleBackColor = true;
            this.cbCreateServer.CheckedChanged += new System.EventHandler(this.cbIsLocal_CheckedChanged);
            // 
            // cbLocalOnly
            // 
            this.cbLocalOnly.AutoSize = true;
            this.cbLocalOnly.Location = new System.Drawing.Point(4, 92);
            this.cbLocalOnly.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cbLocalOnly.Name = "cbLocalOnly";
            this.cbLocalOnly.Size = new System.Drawing.Size(184, 24);
            this.cbLocalOnly.TabIndex = 6;
            this.cbLocalOnly.Text = "Local only (no server)";
            this.cbLocalOnly.UseVisualStyleBackColor = true;
            // 
            // InitialSetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbLocalOnly);
            this.Controls.Add(this.cbCreateServer);
            this.Controls.Add(this.lbLog);
            this.Controls.Add(this.tbPlayerName);
            this.Controls.Add(this.tbServerName);
            this.Controls.Add(this.btnPlay);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "InitialSetup";
            this.Size = new System.Drawing.Size(328, 218);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.TextBox tbServerName;
        private System.Windows.Forms.TextBox tbPlayerName;
        private System.Windows.Forms.Label lbLog;
        private System.Windows.Forms.CheckBox cbCreateServer;
        private System.Windows.Forms.CheckBox cbLocalOnly;
    }
}
