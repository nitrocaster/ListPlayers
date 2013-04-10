using System.Windows.Forms;

namespace ListPlayers.Dialogs
{
    partial class MainDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainDialog));
            this.lBuild = new System.Windows.Forms.Label();
            this.linkMpn = new System.Windows.Forms.LinkLabelEx();
            this.pnMain = new System.Windows.Forms.Panel();
            this.clDatabaseConstructor = new System.Windows.Forms.CommandLink();
            this.clDatabaseView = new System.Windows.Forms.CommandLink();
            this.horizontalPanel1 = new System.Windows.Forms.HorizontalPanel();
            this.pnMain.SuspendLayout();
            this.horizontalPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lBuild
            // 
            this.lBuild.BackColor = System.Drawing.Color.Transparent;
            resources.ApplyResources(this.lBuild, "lBuild");
            this.lBuild.ForeColor = System.Drawing.SystemColors.GrayText;
            this.lBuild.Name = "lBuild";
            // 
            // linkMpn
            // 
            this.linkMpn.ActiveLinkColor = System.Drawing.Color.Blue;
            resources.ApplyResources(this.linkMpn, "linkMpn");
            this.linkMpn.BackColor = System.Drawing.Color.Transparent;
            this.linkMpn.ForeColor = System.Drawing.SystemColors.GrayText;
            this.linkMpn.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.linkMpn.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.linkMpn.Name = "linkMpn";
            this.linkMpn.TabStop = true;
            // 
            // pnMain
            // 
            this.pnMain.BackColor = System.Drawing.SystemColors.Window;
            this.pnMain.Controls.Add(this.clDatabaseConstructor);
            this.pnMain.Controls.Add(this.clDatabaseView);
            resources.ApplyResources(this.pnMain, "pnMain");
            this.pnMain.Name = "pnMain";
            // 
            // clDatabaseConstructor
            // 
            this.clDatabaseConstructor.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.clDatabaseConstructor, "clDatabaseConstructor");
            this.clDatabaseConstructor.ForeColor = System.Drawing.SystemColors.Window;
            this.clDatabaseConstructor.Name = "clDatabaseConstructor";
            this.clDatabaseConstructor.NoteText = "Формирование новой базы данных или обновление существующей.";
            this.clDatabaseConstructor.UseVisualStyleBackColor = true;
            // 
            // clDatabaseView
            // 
            this.clDatabaseView.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this.clDatabaseView, "clDatabaseView");
            this.clDatabaseView.ForeColor = System.Drawing.SystemColors.Window;
            this.clDatabaseView.Name = "clDatabaseView";
            this.clDatabaseView.NoteText = "Поиск игроков в базе данных по заданным критериям.";
            this.clDatabaseView.UseVisualStyleBackColor = true;
            // 
            // horizontalPanel1
            // 
            this.horizontalPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(251)))));
            this.horizontalPanel1.Controls.Add(this.lBuild);
            this.horizontalPanel1.Controls.Add(this.linkMpn);
            resources.ApplyResources(this.horizontalPanel1, "horizontalPanel1");
            this.horizontalPanel1.Name = "horizontalPanel1";
            // 
            // MainDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.horizontalPanel1);
            this.Controls.Add(this.pnMain);
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = global::ListPlayers.Properties.Resources.IconListPlayers;
            this.MaximizeBox = false;
            this.Name = "MainDialog";
            this.pnMain.ResumeLayout(false);
            this.horizontalPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lBuild;
        private System.Windows.Forms.LinkLabelEx linkMpn;
        private System.Windows.Forms.CommandLink clDatabaseConstructor;
        private System.Windows.Forms.CommandLink clDatabaseView;
        public System.Windows.Forms.Panel pnMain;
        private HorizontalPanel horizontalPanel1;

    }
}