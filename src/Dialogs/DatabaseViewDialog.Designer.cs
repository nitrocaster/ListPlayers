namespace ListPlayers.Dialogs
{
    partial class DatabaseViewDialog
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatabaseViewDialog));
            this.lGSID = new System.Windows.Forms.Label();
            this.lIP = new System.Windows.Forms.Label();
            this.lName = new System.Windows.Forms.Label();
            this.lHex = new System.Windows.Forms.Label();
            this.tbSearchHash = new System.Windows.Forms.TextBox();
            this.tbSearchGsid = new System.Windows.Forms.TextBox();
            this.tbSearchIp = new System.Windows.Forms.TextBox();
            this.tbSearchName = new System.Windows.Forms.TextBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tbDbPath = new System.Windows.Forms.ComboBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.lDbPath = new System.Windows.Forms.Label();
            this.tbDetails = new System.Windows.Forms.TextBox();
            this.chkNamePattern = new System.Windows.Forms.CheckBox();
            this.chkIpPattern = new System.Windows.Forms.CheckBox();
            this.chkAllRelatedData = new System.Windows.Forms.CheckBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.dtpTimestamp = new System.Windows.Forms.DateTimePicker();
            this.pnlSearchProgress = new System.Windows.Forms.Panel();
            this.lSearchProgress = new System.Windows.Forms.Label();
            this.pbSearch = new System.Windows.Forms.ProgressBar();
            this.cmResults = new System.Windows.Forms.ContextMenu();
            this.cmiExpandBranch = new System.Windows.Forms.MenuItem();
            this.cmiCollapseBranch = new System.Windows.Forms.MenuItem();
            this.cmiDivider = new System.Windows.Forms.MenuItem();
            this.cmiExpandAll = new System.Windows.Forms.MenuItem();
            this.cmiCollapseAll = new System.Windows.Forms.MenuItem();
            this.cmiDivider2 = new System.Windows.Forms.MenuItem();
            this.cmiCopy = new System.Windows.Forms.MenuItem();
            this.tvResult = new System.Windows.Forms.TreeViewEx();
            this.pnNameInfo = new System.Windows.Forms.Panel();
            this.ttNameFilter = new System.Windows.Forms.ToolTip(this.components);
            this.pnlSearchProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // lGSID
            // 
            resources.ApplyResources(this.lGSID, "lGSID");
            this.lGSID.Name = "lGSID";
            // 
            // lIP
            // 
            resources.ApplyResources(this.lIP, "lIP");
            this.lIP.Name = "lIP";
            // 
            // lName
            // 
            resources.ApplyResources(this.lName, "lName");
            this.lName.Name = "lName";
            // 
            // lHex
            // 
            resources.ApplyResources(this.lHex, "lHex");
            this.lHex.Name = "lHex";
            // 
            // tbSearchHash
            // 
            resources.ApplyResources(this.tbSearchHash, "tbSearchHash");
            this.tbSearchHash.Name = "tbSearchHash";
            // 
            // tbSearchGsid
            // 
            resources.ApplyResources(this.tbSearchGsid, "tbSearchGsid");
            this.tbSearchGsid.Name = "tbSearchGsid";
            // 
            // tbSearchIp
            // 
            resources.ApplyResources(this.tbSearchIp, "tbSearchIp");
            this.tbSearchIp.Name = "tbSearchIp";
            // 
            // tbSearchName
            // 
            resources.ApplyResources(this.tbSearchName, "tbSearchName");
            this.tbSearchName.Name = "tbSearchName";
            // 
            // btnExport
            // 
            resources.ApplyResources(this.btnExport, "btnExport");
            this.btnExport.Name = "btnExport";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnRefresh
            // 
            resources.ApplyResources(this.btnRefresh, "btnRefresh");
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tbDbPath
            // 
            this.tbDbPath.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.tbDbPath, "tbDbPath");
            this.tbDbPath.Name = "tbDbPath";
            this.tbDbPath.SelectedValueChanged += new System.EventHandler(this.tbDbPath_SelectedValueChanged);
            // 
            // btnBrowse
            // 
            resources.ApplyResources(this.btnBrowse, "btnBrowse");
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // lDbPath
            // 
            resources.ApplyResources(this.lDbPath, "lDbPath");
            this.lDbPath.Name = "lDbPath";
            // 
            // tbDetails
            // 
            this.tbDetails.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.tbDetails, "tbDetails");
            this.tbDetails.Name = "tbDetails";
            this.tbDetails.ReadOnly = true;
            this.tbDetails.TextChanged += new System.EventHandler(this.tbDetails_TextChanged);
            // 
            // chkNamePattern
            // 
            resources.ApplyResources(this.chkNamePattern, "chkNamePattern");
            this.chkNamePattern.Name = "chkNamePattern";
            this.chkNamePattern.UseVisualStyleBackColor = true;
            // 
            // chkIpPattern
            // 
            resources.ApplyResources(this.chkIpPattern, "chkIpPattern");
            this.chkIpPattern.Name = "chkIpPattern";
            this.chkIpPattern.UseVisualStyleBackColor = true;
            // 
            // chkAllRelatedData
            // 
            resources.ApplyResources(this.chkAllRelatedData, "chkAllRelatedData");
            this.chkAllRelatedData.Name = "chkAllRelatedData";
            this.chkAllRelatedData.Tag = "Show all data related with found players";
            this.chkAllRelatedData.UseVisualStyleBackColor = true;
            // 
            // btnApply
            // 
            resources.ApplyResources(this.btnApply, "btnApply");
            this.btnApply.Name = "btnApply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // dtpTimestamp
            // 
            resources.ApplyResources(this.dtpTimestamp, "dtpTimestamp");
            this.dtpTimestamp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpTimestamp.MinDate = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            this.dtpTimestamp.Name = "dtpTimestamp";
            this.dtpTimestamp.Value = new System.DateTime(2011, 11, 27, 12, 8, 10, 0);
            this.dtpTimestamp.ValueChanged += new System.EventHandler(this.dtTimestamp_ValueChanged);
            // 
            // pnlSearchProgress
            // 
            this.pnlSearchProgress.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSearchProgress.Controls.Add(this.lSearchProgress);
            this.pnlSearchProgress.Controls.Add(this.pbSearch);
            resources.ApplyResources(this.pnlSearchProgress, "pnlSearchProgress");
            this.pnlSearchProgress.Name = "pnlSearchProgress";
            // 
            // lSearchProgress
            // 
            resources.ApplyResources(this.lSearchProgress, "lSearchProgress");
            this.lSearchProgress.BackColor = System.Drawing.Color.Transparent;
            this.lSearchProgress.Name = "lSearchProgress";
            // 
            // pbSearch
            // 
            resources.ApplyResources(this.pbSearch, "pbSearch");
            this.pbSearch.Name = "pbSearch";
            this.pbSearch.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            // 
            // cmResults
            // 
            this.cmResults.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.cmiExpandBranch,
            this.cmiCollapseBranch,
            this.cmiDivider,
            this.cmiExpandAll,
            this.cmiCollapseAll,
            this.cmiDivider2,
            this.cmiCopy});
            this.cmResults.Popup += new System.EventHandler(this.cmResults_Popup);
            // 
            // cmiExpandBranch
            // 
            this.cmiExpandBranch.Index = 0;
            resources.ApplyResources(this.cmiExpandBranch, "cmiExpandBranch");
            this.cmiExpandBranch.Click += new System.EventHandler(this.cmiExpandBranch_Click);
            // 
            // cmiCollapseBranch
            // 
            this.cmiCollapseBranch.Index = 1;
            resources.ApplyResources(this.cmiCollapseBranch, "cmiCollapseBranch");
            this.cmiCollapseBranch.Click += new System.EventHandler(this.cmiCollapseBranch_Click);
            // 
            // cmiDivider
            // 
            this.cmiDivider.Index = 2;
            resources.ApplyResources(this.cmiDivider, "cmiDivider");
            // 
            // cmiExpandAll
            // 
            this.cmiExpandAll.Index = 3;
            resources.ApplyResources(this.cmiExpandAll, "cmiExpandAll");
            this.cmiExpandAll.Click += new System.EventHandler(this.cmiExpandAll_Click);
            // 
            // cmiCollapseAll
            // 
            this.cmiCollapseAll.Index = 4;
            resources.ApplyResources(this.cmiCollapseAll, "cmiCollapseAll");
            this.cmiCollapseAll.Click += new System.EventHandler(this.cmiCollapseAll_Click);
            // 
            // cmiDivider2
            // 
            this.cmiDivider2.Index = 5;
            resources.ApplyResources(this.cmiDivider2, "cmiDivider2");
            // 
            // cmiCopy
            // 
            this.cmiCopy.Index = 6;
            resources.ApplyResources(this.cmiCopy, "cmiCopy");
            this.cmiCopy.Click += new System.EventHandler(this.cmiCopy_Click);
            // 
            // tvResult
            // 
            this.tvResult.ContextMenu = this.cmResults;
            resources.ApplyResources(this.tvResult, "tvResult");
            this.tvResult.FullRowSelect = true;
            this.tvResult.HideSelection = false;
            this.tvResult.LineColor = System.Drawing.Color.Silver;
            this.tvResult.Name = "tvResult";
            this.tvResult.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvResult_AfterSelect);
            this.tvResult.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tvResult_KeyUp);
            this.tvResult.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tvResult_MouseDown);
            // 
            // pnNameInfo
            // 
            this.pnNameInfo.BackgroundImage = global::ListPlayers.Properties.Resources.IconInfoSmall;
            resources.ApplyResources(this.pnNameInfo, "pnNameInfo");
            this.pnNameInfo.Name = "pnNameInfo";
            // 
            // DatabaseViewDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnNameInfo);
            this.Controls.Add(this.pnlSearchProgress);
            this.Controls.Add(this.dtpTimestamp);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.chkAllRelatedData);
            this.Controls.Add(this.chkIpPattern);
            this.Controls.Add(this.chkNamePattern);
            this.Controls.Add(this.tbDetails);
            this.Controls.Add(this.tvResult);
            this.Controls.Add(this.tbSearchHash);
            this.Controls.Add(this.tbSearchGsid);
            this.Controls.Add(this.tbSearchIp);
            this.Controls.Add(this.tbSearchName);
            this.Controls.Add(this.lDbPath);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.tbDbPath);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.lGSID);
            this.Controls.Add(this.lIP);
            this.Controls.Add(this.lName);
            this.Controls.Add(this.lHex);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = global::ListPlayers.Properties.Resources.IconListPlayers;
            this.MaximizeBox = false;
            this.Name = "DatabaseViewDialog";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DatabaseViewDialog_FormClosing);
            this.pnlSearchProgress.ResumeLayout(false);
            this.pnlSearchProgress.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lGSID;
        private System.Windows.Forms.Label lIP;
        private System.Windows.Forms.Label lName;
        private System.Windows.Forms.Label lHex;
        private System.Windows.Forms.TextBox tbSearchHash;
        private System.Windows.Forms.TextBox tbSearchGsid;
        private System.Windows.Forms.TextBox tbSearchIp;
        private System.Windows.Forms.TextBox tbSearchName;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ComboBox tbDbPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label lDbPath;
        private System.Windows.Forms.Label lSearchProgress;
        private System.Windows.Forms.TreeViewEx tvResult;
        private System.Windows.Forms.TextBox tbDetails;
        private System.Windows.Forms.CheckBox chkNamePattern;
        private System.Windows.Forms.CheckBox chkIpPattern;
        private System.Windows.Forms.CheckBox chkAllRelatedData;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.DateTimePicker dtpTimestamp;
        private System.Windows.Forms.Panel pnlSearchProgress;
        private System.Windows.Forms.ProgressBar pbSearch;
        private System.Windows.Forms.ContextMenu cmResults;
        private System.Windows.Forms.MenuItem cmiExpandAll;
        private System.Windows.Forms.MenuItem cmiExpandBranch;
        private System.Windows.Forms.MenuItem cmiCollapseBranch;
        private System.Windows.Forms.MenuItem cmiDivider;
        private System.Windows.Forms.MenuItem cmiCollapseAll;
        private System.Windows.Forms.MenuItem cmiDivider2;
        private System.Windows.Forms.MenuItem cmiCopy;
        private System.Windows.Forms.Panel pnNameInfo;
        private System.Windows.Forms.ToolTip ttNameFilter;

    }
}