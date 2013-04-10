namespace ListPlayers.Dialogs
{
    partial class DatabaseUpdaterDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatabaseUpdaterDialog));
            this.pbProgress = new System.Windows.Forms.ProgressBar();
            this.btnCancel = new System.Windows.Forms.Button();
            this.hpFooter = new System.Windows.Forms.HorizontalPanel();
            this.lCurrentFile = new System.Windows.Forms.Label();
            this.tbCounter = new System.Windows.Forms.TextBox();
            this.tbHashesFound = new System.Windows.Forms.TextBox();
            this.lHashesFound = new System.Windows.Forms.Label();
            this.gbFound = new System.Windows.Forms.GroupBox();
            this.tbGsidsFound = new System.Windows.Forms.TextBox();
            this.tbIpsFound = new System.Windows.Forms.TextBox();
            this.tbNamesFound = new System.Windows.Forms.TextBox();
            this.lGsidsFound = new System.Windows.Forms.Label();
            this.lIpsFound = new System.Windows.Forms.Label();
            this.lNamesFound = new System.Windows.Forms.Label();
            this.gbAppended = new System.Windows.Forms.GroupBox();
            this.tbGsidsAppended = new System.Windows.Forms.TextBox();
            this.tbIpsAppended = new System.Windows.Forms.TextBox();
            this.tbNamesAppended = new System.Windows.Forms.TextBox();
            this.lGsidsAppended = new System.Windows.Forms.Label();
            this.lIpsAppended = new System.Windows.Forms.Label();
            this.lNamesAppended = new System.Windows.Forms.Label();
            this.tbHashesAppended = new System.Windows.Forms.TextBox();
            this.lHashesAppended = new System.Windows.Forms.Label();
            this.pbFileProgress = new System.Windows.Forms.ProgressBar();
            this.gbFound.SuspendLayout();
            this.gbAppended.SuspendLayout();
            this.SuspendLayout();
            // 
            // pbProgress
            // 
            resources.ApplyResources(this.pbProgress, "pbProgress");
            this.pbProgress.Name = "pbProgress";
            // 
            // btnCancel
            // 
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
            // lCurrentFile
            // 
            resources.ApplyResources(this.lCurrentFile, "lCurrentFile");
            this.lCurrentFile.Name = "lCurrentFile";
            // 
            // tbCounter
            // 
            this.tbCounter.BackColor = System.Drawing.SystemColors.Window;
            this.tbCounter.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.tbCounter, "tbCounter");
            this.tbCounter.Name = "tbCounter";
            // 
            // tbHashesFound
            // 
            this.tbHashesFound.BackColor = System.Drawing.SystemColors.Window;
            this.tbHashesFound.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.tbHashesFound, "tbHashesFound");
            this.tbHashesFound.Name = "tbHashesFound";
            // 
            // lHashesFound
            // 
            resources.ApplyResources(this.lHashesFound, "lHashesFound");
            this.lHashesFound.Name = "lHashesFound";
            // 
            // gbFound
            // 
            this.gbFound.Controls.Add(this.tbGsidsFound);
            this.gbFound.Controls.Add(this.tbIpsFound);
            this.gbFound.Controls.Add(this.tbNamesFound);
            this.gbFound.Controls.Add(this.lGsidsFound);
            this.gbFound.Controls.Add(this.lIpsFound);
            this.gbFound.Controls.Add(this.lNamesFound);
            this.gbFound.Controls.Add(this.tbHashesFound);
            this.gbFound.Controls.Add(this.lHashesFound);
            resources.ApplyResources(this.gbFound, "gbFound");
            this.gbFound.Name = "gbFound";
            this.gbFound.TabStop = false;
            // 
            // tbGsidsFound
            // 
            this.tbGsidsFound.BackColor = System.Drawing.SystemColors.Window;
            this.tbGsidsFound.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.tbGsidsFound, "tbGsidsFound");
            this.tbGsidsFound.Name = "tbGsidsFound";
            // 
            // tbIpsFound
            // 
            this.tbIpsFound.BackColor = System.Drawing.SystemColors.Window;
            this.tbIpsFound.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.tbIpsFound, "tbIpsFound");
            this.tbIpsFound.Name = "tbIpsFound";
            // 
            // tbNamesFound
            // 
            this.tbNamesFound.BackColor = System.Drawing.SystemColors.Window;
            this.tbNamesFound.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.tbNamesFound, "tbNamesFound");
            this.tbNamesFound.Name = "tbNamesFound";
            // 
            // lGsidsFound
            // 
            resources.ApplyResources(this.lGsidsFound, "lGsidsFound");
            this.lGsidsFound.Name = "lGsidsFound";
            // 
            // lIpsFound
            // 
            resources.ApplyResources(this.lIpsFound, "lIpsFound");
            this.lIpsFound.Name = "lIpsFound";
            // 
            // lNamesFound
            // 
            resources.ApplyResources(this.lNamesFound, "lNamesFound");
            this.lNamesFound.Name = "lNamesFound";
            // 
            // gbAppended
            // 
            this.gbAppended.Controls.Add(this.tbGsidsAppended);
            this.gbAppended.Controls.Add(this.tbIpsAppended);
            this.gbAppended.Controls.Add(this.tbNamesAppended);
            this.gbAppended.Controls.Add(this.lGsidsAppended);
            this.gbAppended.Controls.Add(this.lIpsAppended);
            this.gbAppended.Controls.Add(this.lNamesAppended);
            this.gbAppended.Controls.Add(this.tbHashesAppended);
            this.gbAppended.Controls.Add(this.lHashesAppended);
            resources.ApplyResources(this.gbAppended, "gbAppended");
            this.gbAppended.Name = "gbAppended";
            this.gbAppended.TabStop = false;
            // 
            // tbGsidsAppended
            // 
            this.tbGsidsAppended.BackColor = System.Drawing.SystemColors.Window;
            this.tbGsidsAppended.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.tbGsidsAppended, "tbGsidsAppended");
            this.tbGsidsAppended.Name = "tbGsidsAppended";
            // 
            // tbIpsAppended
            // 
            this.tbIpsAppended.BackColor = System.Drawing.SystemColors.Window;
            this.tbIpsAppended.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.tbIpsAppended, "tbIpsAppended");
            this.tbIpsAppended.Name = "tbIpsAppended";
            // 
            // tbNamesAppended
            // 
            this.tbNamesAppended.BackColor = System.Drawing.SystemColors.Window;
            this.tbNamesAppended.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.tbNamesAppended, "tbNamesAppended");
            this.tbNamesAppended.Name = "tbNamesAppended";
            // 
            // lGsidsAppended
            // 
            resources.ApplyResources(this.lGsidsAppended, "lGsidsAppended");
            this.lGsidsAppended.Name = "lGsidsAppended";
            // 
            // lIpsAppended
            // 
            resources.ApplyResources(this.lIpsAppended, "lIpsAppended");
            this.lIpsAppended.Name = "lIpsAppended";
            // 
            // lNamesAppended
            // 
            resources.ApplyResources(this.lNamesAppended, "lNamesAppended");
            this.lNamesAppended.Name = "lNamesAppended";
            // 
            // tbHashesAppended
            // 
            this.tbHashesAppended.BackColor = System.Drawing.SystemColors.Window;
            this.tbHashesAppended.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.tbHashesAppended, "tbHashesAppended");
            this.tbHashesAppended.Name = "tbHashesAppended";
            // 
            // lHashesAppended
            // 
            resources.ApplyResources(this.lHashesAppended, "lHashesAppended");
            this.lHashesAppended.Name = "lHashesAppended";
            // 
            // pbFileProgress
            // 
            resources.ApplyResources(this.pbFileProgress, "pbFileProgress");
            this.pbFileProgress.Name = "pbFileProgress";
            // 
            // DatabaseUpdaterDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.pbFileProgress);
            this.Controls.Add(this.gbAppended);
            this.Controls.Add(this.gbFound);
            this.Controls.Add(this.tbCounter);
            this.Controls.Add(this.lCurrentFile);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.pbProgress);
            this.Controls.Add(this.hpFooter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = global::ListPlayers.Properties.Resources.IconListPlayers;
            this.MaximizeBox = false;
            this.Name = "DatabaseUpdaterDialog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DatabaseUpdateDialog_FormClosing);
            this.gbFound.ResumeLayout(false);
            this.gbFound.PerformLayout();
            this.gbAppended.ResumeLayout(false);
            this.gbAppended.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar pbProgress;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.HorizontalPanel hpFooter;
        private System.Windows.Forms.Label lCurrentFile;
        private System.Windows.Forms.TextBox tbCounter;
        public System.Windows.Forms.TextBox tbHashesFound;
        private System.Windows.Forms.Label lHashesFound;
        private System.Windows.Forms.GroupBox gbFound;
        private System.Windows.Forms.Label lNamesFound;
        private System.Windows.Forms.Label lGsidsFound;
        private System.Windows.Forms.Label lIpsFound;
        public System.Windows.Forms.TextBox tbGsidsFound;
        public System.Windows.Forms.TextBox tbIpsFound;
        public System.Windows.Forms.TextBox tbNamesFound;
        private System.Windows.Forms.GroupBox gbAppended;
        public  System.Windows.Forms.TextBox tbGsidsAppended;
        public  System.Windows.Forms.TextBox tbIpsAppended;
        public  System.Windows.Forms.TextBox tbNamesAppended;
        private System.Windows.Forms.Label lGsidsAppended;
        private System.Windows.Forms.Label lIpsAppended;
        private System.Windows.Forms.Label lNamesAppended;
        public  System.Windows.Forms.TextBox tbHashesAppended;
        private System.Windows.Forms.Label lHashesAppended;
        private System.Windows.Forms.ProgressBar pbFileProgress;
    }
}