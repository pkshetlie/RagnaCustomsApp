
namespace RagnaCustoms
{
    partial class Updater
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Updater));
            this.labelAvailable = new System.Windows.Forms.Label();
            this.yesButton = new System.Windows.Forms.Button();
            this.noButton = new System.Windows.Forms.Button();
            this.manualButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.newVersion = new System.Windows.Forms.Label();
            this.currentVersion = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelAvailable
            // 
            this.labelAvailable.AutoSize = true;
            this.labelAvailable.Location = new System.Drawing.Point(13, 13);
            this.labelAvailable.Name = "labelAvailable";
            this.labelAvailable.Size = new System.Drawing.Size(149, 26);
            this.labelAvailable.TabIndex = 0;
            this.labelAvailable.Text = "An update is available,\r\ndo you want to install it now ? ";
            // 
            // yesButton
            // 
            this.yesButton.Location = new System.Drawing.Point(15, 115);
            this.yesButton.Name = "yesButton";
            this.yesButton.Size = new System.Drawing.Size(75, 23);
            this.yesButton.TabIndex = 1;
            this.yesButton.Text = "Do it !";
            this.yesButton.UseVisualStyleBackColor = true;
            this.yesButton.Visible = false;
            this.yesButton.Click += new System.EventHandler(this.yesButton_Click);
            // 
            // noButton
            // 
            this.noButton.Location = new System.Drawing.Point(105, 115);
            this.noButton.Name = "noButton";
            this.noButton.Size = new System.Drawing.Size(75, 23);
            this.noButton.TabIndex = 2;
            this.noButton.Text = "No";
            this.noButton.UseVisualStyleBackColor = true;
            this.noButton.Visible = false;
            this.noButton.Click += new System.EventHandler(this.button2_Click);
            // 
            // manualButton
            // 
            this.manualButton.Location = new System.Drawing.Point(15, 150);
            this.manualButton.Name = "manualButton";
            this.manualButton.Size = new System.Drawing.Size(178, 23);
            this.manualButton.TabIndex = 3;
            this.manualButton.Text = "Download new version";
            this.manualButton.UseVisualStyleBackColor = true;
            this.manualButton.Click += new System.EventHandler(this.button3_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Your version : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "The new one :";
            // 
            // newVersion
            // 
            this.newVersion.AutoSize = true;
            this.newVersion.Location = new System.Drawing.Point(86, 64);
            this.newVersion.Name = "newVersion";
            this.newVersion.Size = new System.Drawing.Size(76, 13);
            this.newVersion.TabIndex = 6;
            this.newVersion.Text = "The new one :";
            // 
            // currentVersion
            // 
            this.currentVersion.AutoSize = true;
            this.currentVersion.Location = new System.Drawing.Point(86, 51);
            this.currentVersion.Name = "currentVersion";
            this.currentVersion.Size = new System.Drawing.Size(76, 13);
            this.currentVersion.TabIndex = 7;
            this.currentVersion.Text = "The new one :";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(13, 77);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(115, 13);
            this.linkLabel1.TabIndex = 8;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "What does it change ?";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(16, 99);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(163, 10);
            this.progressBar1.TabIndex = 9;
            this.progressBar1.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(63, 121);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Don\'t care";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Updater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(205, 179);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.currentVersion);
            this.Controls.Add(this.newVersion);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.manualButton);
            this.Controls.Add(this.noButton);
            this.Controls.Add(this.yesButton);
            this.Controls.Add(this.labelAvailable);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Updater";
            this.Text = "New version !";
            this.Load += new System.EventHandler(this.Updater_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelAvailable;
        private System.Windows.Forms.Button yesButton;
        private System.Windows.Forms.Button noButton;
        private System.Windows.Forms.Button manualButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label newVersion;
        private System.Windows.Forms.Label currentVersion;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button button1;
    }
}