namespace HelloGame
{
    partial class InitialSetupForm
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
            this.initialSetup1 = new InitialSetup();
            this.SuspendLayout();
            // 
            // initialSetup1
            // 
            this.initialSetup1.ClientNetwork = null;
            this.initialSetup1.Location = new System.Drawing.Point(12, 12);
            this.initialSetup1.Name = "initialSetup1";
            this.initialSetup1.Size = new System.Drawing.Size(331, 156);
            this.initialSetup1.TabIndex = 0;
            this.initialSetup1.Load += new System.EventHandler(this.initialSetup1_Load);
            // 
            // InitialSetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(247, 167);
            this.Controls.Add(this.initialSetup1);
            this.Name = "InitialSetupForm";
            this.Text = "InitialSetupForm";
            this.ResumeLayout(false);

        }

        #endregion

        private InitialSetup initialSetup1;
    }
}