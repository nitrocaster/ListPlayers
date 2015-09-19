namespace ListPlayers.PcdbExport
{
    partial class TextExporterDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextExporterDialog));
            this.pbProgress = new System.Windows.Forms.ProgressBar();
            this.lStatus = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.hpFooter = new System.Windows.Forms.HorizontalPanel();
            this.tbCounter = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // pbProgress
            // 
            resources.ApplyResources(this.pbProgress, "pbProgress");
            this.pbProgress.Name = "pbProgress";
            // 
            // lStatus
            // 
            resources.ApplyResources(this.lStatus, "lStatus");
            this.lStatus.Name = "lStatus";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // hpFooter
            // 
            this.hpFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(251)))));
            resources.ApplyResources(this.hpFooter, "hpFooter");
            this.hpFooter.Name = "hpFooter";
            // 
            // tbCounter
            // 
            this.tbCounter.BackColor = System.Drawing.SystemColors.Window;
            this.tbCounter.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.tbCounter, "tbCounter");
            this.tbCounter.Name = "tbCounter";
            // 
            // TextExporterDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.pbProgress);
            this.Controls.Add(this.lStatus);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.hpFooter);
            this.Controls.Add(this.tbCounter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = global::ListPlayers.Properties.Resources.IconListPlayers;
            this.MaximizeBox = false;
            this.Name = "TextExporterDialog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TextExporterDialog_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar pbProgress;
        private System.Windows.Forms.Label lStatus;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.HorizontalPanel hpFooter;
        private System.Windows.Forms.TextBox tbCounter;
    }
}