
using System.Windows.Forms;

namespace RagnaCustoms.App.Views
{
    partial class AndroidDeviceSyncForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AndroidDeviceSyncForm));
            this.SyncingProgressBar = new System.Windows.Forms.ProgressBar();
            this.SyncingLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // SyncingProgressBar
            // 
            this.SyncingProgressBar.Location = new System.Drawing.Point(10, 10);
            this.SyncingProgressBar.Name = "SyncingProgressBar";
            this.SyncingProgressBar.Size = new System.Drawing.Size(240, 20);
            this.SyncingProgressBar.TabIndex = 1;
            this.SyncingProgressBar.UseWaitCursor = true;
            this.SyncingProgressBar.Click += new System.EventHandler(this.DownloadingProgressBar_Click);
            // 
            // SyncingLabel
            // 
            this.SyncingLabel.AutoSize = true;
            this.SyncingLabel.Location = new System.Drawing.Point(12, 35);
            this.SyncingLabel.Name = "SyncingLabel";
            this.SyncingLabel.Size = new System.Drawing.Size(0, 13);
            this.SyncingLabel.TabIndex = 2;
            this.SyncingLabel.UseWaitCursor = true;
            // 
            // AndroidDeviceSyncForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(261, 59);
            this.Controls.Add(this.SyncingLabel);
            this.Controls.Add(this.SyncingProgressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "AndroidDeviceSyncForm";
            this.Text = "Oculus syncing";
            this.UseWaitCursor = true;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.ProgressBar SyncingProgressBar;
        public System.Windows.Forms.Label SyncingLabel;
    }
}