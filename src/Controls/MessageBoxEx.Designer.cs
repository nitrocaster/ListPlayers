namespace ListPlayers.Controls
{
    partial class MessageBoxEx
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
            this.hpFooter = new System.Windows.Forms.HorizontalPanel();
            this.pbIcon = new System.Windows.Forms.PictureBox();
            this.lMessage = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.hpFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // hpFooter
            // 
            this.hpFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(241)))), ((int)(((byte)(245)))), ((int)(((byte)(251)))));
            this.hpFooter.Controls.Add(this.button1);
            this.hpFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.hpFooter.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hpFooter.Location = new System.Drawing.Point(0, 88);
            this.hpFooter.Name = "hpFooter";
            this.hpFooter.Size = new System.Drawing.Size(314, 47);
            this.hpFooter.TabIndex = 0;
            // 
            // pbIcon
            // 
            this.pbIcon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pbIcon.Location = new System.Drawing.Point(12, 12);
            this.pbIcon.Name = "pbIcon";
            this.pbIcon.Size = new System.Drawing.Size(64, 64);
            this.pbIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbIcon.TabIndex = 1;
            this.pbIcon.TabStop = false;
            // 
            // lMessage
            // 
            this.lMessage.AutoSize = true;
            this.lMessage.Location = new System.Drawing.Point(95, 36);
            this.lMessage.Margin = new System.Windows.Forms.Padding(16, 0, 3, 0);
            this.lMessage.Name = "lMessage";
            this.lMessage.Size = new System.Drawing.Size(50, 13);
            this.lMessage.TabIndex = 2;
            this.lMessage.Text = "Message";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(227, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // MessageBoxEx
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(314, 135);
            this.Controls.Add(this.lMessage);
            this.Controls.Add(this.pbIcon);
            this.Controls.Add(this.hpFooter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MinimumSize = new System.Drawing.Size(320, 163);
            this.Name = "MessageBoxEx";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MessageBoxEx";
            this.hpFooter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.HorizontalPanel hpFooter;
        private System.Windows.Forms.PictureBox pbIcon;
        private System.Windows.Forms.Label lMessage;
        private System.Windows.Forms.Button button1;
    }
}