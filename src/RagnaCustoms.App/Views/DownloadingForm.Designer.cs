
namespace RagnaCustoms.Views
{
    partial class DownloadingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DownloadingForm));
            this.DownloadingProgressBar = new System.Windows.Forms.ProgressBar();
            this.DownloadingLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // DownloadingProgressBar
            // 
            this.DownloadingProgressBar.Location = new System.Drawing.Point(10, 10);
            this.DownloadingProgressBar.Name = "DownloadingProgressBar";
            this.DownloadingProgressBar.Size = new System.Drawing.Size(240, 20);
            this.DownloadingProgressBar.TabIndex = 0;
            // 
            // DownloadingLabel
            // 
            this.DownloadingLabel.AutoSize = true;
            this.DownloadingLabel.Location = new System.Drawing.Point(11, 36);
            this.DownloadingLabel.Name = "DownloadingLabel";
            this.DownloadingLabel.Size = new System.Drawing.Size(207, 13);
            this.DownloadingLabel.TabIndex = 1;
            this.DownloadingLabel.Text = "Downloading, sit back and relax, we got it.";
            this.DownloadingLabel.UseWaitCursor = true;
            // 
            // DownloadingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(261, 59);
            this.Controls.Add(this.DownloadingLabel);
            this.Controls.Add(this.DownloadingProgressBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "DownloadingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RagnaCustoms";
            this.Load += new System.EventHandler(this.DownloadingForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar DownloadingProgressBar;
        private System.Windows.Forms.Label DownloadingLabel;
    }
}