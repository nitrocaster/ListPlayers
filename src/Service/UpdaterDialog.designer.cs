namespace ListPlayers.Service
{
    public partial class UpdaterDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdaterDialog));
            this.lMain = new System.Windows.Forms.Label();
            this.lVersionNum = new System.Windows.Forms.Label();
            this.tbVersionInfo = new System.Windows.Forms.TextBox();
            this.lVersionInfo = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lMain
            // 
            resources.ApplyResources(this.lMain, "lMain");
            this.lMain.Name = "lMain";
            // 
            // lVersionNum
            // 
            resources.ApplyResources(this.lVersionNum, "lVersionNum");
            this.lVersionNum.Name = "lVersionNum";
            // 
            // tbVersionInfo
            // 
            this.tbVersionInfo.BackColor = System.Drawing.SystemColors.ControlLightLight;
            resources.ApplyResources(this.tbVersionInfo, "tbVersionInfo");
            this.tbVersionInfo.Name = "tbVersionInfo";
            this.tbVersionInfo.ReadOnly = true;
            // 
            // lVersionInfo
            // 
            resources.ApplyResources(this.lVersionInfo, "lVersionInfo");
            this.lVersionInfo.Name = "lVersionInfo";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnDownload
            // 
            resources.ApplyResources(this.btnDownload, "btnDownload");
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // UpdaterDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnDownload);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lVersionInfo);
            this.Controls.Add(this.tbVersionInfo);
            this.Controls.Add(this.lVersionNum);
            this.Controls.Add(this.lMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = global::ListPlayers.Properties.Resources.IconListPlayers;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UpdaterDialog";
            this.ResumeLayout(false);
            this.PerformLayout();

            }

            #endregion

            private System.Windows.Forms.Label lMain;
            private System.Windows.Forms.Label lVersionNum;
            private System.Windows.Forms.TextBox tbVersionInfo;
            private System.Windows.Forms.Label lVersionInfo;
            private System.Windows.Forms.Button btnCancel;
            private System.Windows.Forms.Button btnDownload;
        }
}