using ListPlayers.Properties;

namespace ListPlayers.Dialogs
{
    partial class DatabaseConstructorDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatabaseConstructorDialog));
            this.btnForward = new System.Windows.Forms.Button();
            this.hpFooter = new System.Windows.Forms.HorizontalPanel();
            this.btnBrowseSrc = new System.Windows.Forms.Button();
            this.btnBrowseDest = new System.Windows.Forms.Button();
            this.lSrc = new System.Windows.Forms.Label();
            this.lDest = new System.Windows.Forms.Label();
            this.lCreateEmptyDb = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.linkCreateEmptyDbCS = new System.Windows.Forms.LinkLabel();
            this.linkCreateEmptyDbCOP = new System.Windows.Forms.LinkLabel();
            this.cbSrc = new System.Windows.Forms.ComboBox();
            this.cbDest = new System.Windows.Forms.ComboBox();
            this.cbSubFolders = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnForward
            // 
            this.btnForward.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(251)))));
            resources.ApplyResources(this.btnForward, "btnForward");
            this.btnForward.Name = "btnForward";
            this.btnForward.UseVisualStyleBackColor = true;
            this.btnForward.Click += new System.EventHandler(this.btnForward_Click);
            // 
            // hpFooter
            // 
            this.hpFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(251)))));
            resources.ApplyResources(this.hpFooter, "hpFooter");
            this.hpFooter.Name = "hpFooter";
            // 
            // btnBrowseSrc
            // 
            resources.ApplyResources(this.btnBrowseSrc, "btnBrowseSrc");
            this.btnBrowseSrc.Name = "btnBrowseSrc";
            this.btnBrowseSrc.UseVisualStyleBackColor = true;
            this.btnBrowseSrc.Click += new System.EventHandler(this.btnBrowseSrc_Click);
            // 
            // btnBrowseDest
            // 
            resources.ApplyResources(this.btnBrowseDest, "btnBrowseDest");
            this.btnBrowseDest.Name = "btnBrowseDest";
            this.btnBrowseDest.UseVisualStyleBackColor = true;
            this.btnBrowseDest.Click += new System.EventHandler(this.btnBrowseDest_Click);
            // 
            // lSrc
            // 
            resources.ApplyResources(this.lSrc, "lSrc");
            this.lSrc.Name = "lSrc";
            // 
            // lDest
            // 
            resources.ApplyResources(this.lDest, "lDest");
            this.lDest.Name = "lDest";
            // 
            // lCreateEmptyDb
            // 
            resources.ApplyResources(this.lCreateEmptyDb, "lCreateEmptyDb");
            this.lCreateEmptyDb.Name = "lCreateEmptyDb";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(251)))));
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // linkCreateEmptyDbCS
            // 
            this.linkCreateEmptyDbCS.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(28)))), ((int)(((byte)(116)))));
            resources.ApplyResources(this.linkCreateEmptyDbCS, "linkCreateEmptyDbCS");
            this.linkCreateEmptyDbCS.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(28)))), ((int)(((byte)(116)))));
            this.linkCreateEmptyDbCS.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(28)))), ((int)(((byte)(116)))));
            this.linkCreateEmptyDbCS.Name = "linkCreateEmptyDbCS";
            this.linkCreateEmptyDbCS.TabStop = true;
            this.linkCreateEmptyDbCS.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(28)))), ((int)(((byte)(116)))));
            this.linkCreateEmptyDbCS.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkCreateEmptyDbCS_LinkClicked);
            // 
            // linkCreateEmptyDbCOP
            // 
            this.linkCreateEmptyDbCOP.ActiveLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(28)))), ((int)(((byte)(116)))));
            resources.ApplyResources(this.linkCreateEmptyDbCOP, "linkCreateEmptyDbCOP");
            this.linkCreateEmptyDbCOP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(28)))), ((int)(((byte)(116)))));
            this.linkCreateEmptyDbCOP.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(28)))), ((int)(((byte)(116)))));
            this.linkCreateEmptyDbCOP.Name = "linkCreateEmptyDbCOP";
            this.linkCreateEmptyDbCOP.TabStop = true;
            this.linkCreateEmptyDbCOP.VisitedLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(28)))), ((int)(((byte)(116)))));
            this.linkCreateEmptyDbCOP.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkCreateEmptyDbCOP_LinkClicked);
            // 
            // cbSrc
            // 
            resources.ApplyResources(this.cbSrc, "cbSrc");
            this.cbSrc.FormattingEnabled = true;
            this.cbSrc.Name = "cbSrc";
            // 
            // cbDest
            // 
            resources.ApplyResources(this.cbDest, "cbDest");
            this.cbDest.FormattingEnabled = true;
            this.cbDest.Name = "cbDest";
            // 
            // cbSubFolders
            // 
            resources.ApplyResources(this.cbSubFolders, "cbSubFolders");
            this.cbSubFolders.Checked = true;
            this.cbSubFolders.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSubFolders.Name = "cbSubFolders";
            this.cbSubFolders.UseVisualStyleBackColor = true;
            // 
            // DatabaseConstructorDialog
            // 
            this.AcceptButton = this.btnForward;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.cbSubFolders);
            this.Controls.Add(this.cbDest);
            this.Controls.Add(this.cbSrc);
            this.Controls.Add(this.linkCreateEmptyDbCOP);
            this.Controls.Add(this.linkCreateEmptyDbCS);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lCreateEmptyDb);
            this.Controls.Add(this.lDest);
            this.Controls.Add(this.lSrc);
            this.Controls.Add(this.btnBrowseDest);
            this.Controls.Add(this.btnBrowseSrc);
            this.Controls.Add(this.btnForward);
            this.Controls.Add(this.hpFooter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = global::ListPlayers.Properties.Resources.IconListPlayers;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DatabaseConstructorDialog";
            this.ShowIcon = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DatabaseConstructorDialog_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnForward;
        private System.Windows.Forms.HorizontalPanel hpFooter;
        private System.Windows.Forms.Button btnBrowseSrc;
        private System.Windows.Forms.Button btnBrowseDest;
        private System.Windows.Forms.Label lSrc;
        private System.Windows.Forms.Label lDest;
        private System.Windows.Forms.Label lCreateEmptyDb;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.LinkLabel linkCreateEmptyDbCS;
        private System.Windows.Forms.LinkLabel linkCreateEmptyDbCOP;
        private System.Windows.Forms.ComboBox cbSrc;
        private System.Windows.Forms.ComboBox cbDest;
        private System.Windows.Forms.CheckBox cbSubFolders;
    }
}