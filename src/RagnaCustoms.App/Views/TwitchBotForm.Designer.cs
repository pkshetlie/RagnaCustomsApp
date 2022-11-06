
using System.Windows.Forms;
using RagnaCustoms.App.Properties;
using RagnaCustoms.Services;

namespace RagnaCustoms.App.Views
{
    partial class TwitchBotForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TwitchBotForm));
            this.songRequests = new System.Windows.Forms.DataGridView();
            this.Song = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Author = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Viewer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EnableButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.songRequests)).BeginInit();
            this.SuspendLayout();
            // 
            // songRequests
            // 
            this.songRequests.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.songRequests.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Song,
            this.Author,
            this.Viewer,
            this.Id});
            this.songRequests.Location = new System.Drawing.Point(12, 35);
            this.songRequests.Name = "songRequests";
            this.songRequests.Size = new System.Drawing.Size(645, 468);
            this.songRequests.TabIndex = 29;
            // 
            // Song
            // 
            this.Song.HeaderText = global::RagnaCustoms.App.Properties.Resources.TwitchBot_Form_Song;
            this.Song.Name = "Song";
            this.Song.ReadOnly = true;
            this.Song.Width = 250;
            // 
            // Author
            // 
            this.Author.HeaderText = global::RagnaCustoms.App.Properties.Resources.TwitchBot_Form_Author;
            this.Author.Name = "Author";
            this.Author.ReadOnly = true;
            this.Author.Width = 150;
            // 
            // Viewer
            // 
            this.Viewer.HeaderText = global::RagnaCustoms.App.Properties.Resources.TwitchBot_Form_Viewer;
            this.Viewer.Name = "Viewer";
            this.Viewer.ReadOnly = true;
            this.Viewer.Width = 150;
            // 
            // Id
            // 
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            // 
            // EnableButton
            // 
            this.EnableButton.Location = new System.Drawing.Point(12, 6);
            this.EnableButton.Name = "EnableButton";
            this.EnableButton.Size = new System.Drawing.Size(75, 23);
            this.EnableButton.TabIndex = 30;
            this.EnableButton.Text = "Start";
            this.EnableButton.UseVisualStyleBackColor = true;
            this.EnableButton.Click += new System.EventHandler(this.EnableButton_Click);
            // 
            // TwitchBotForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 530);
            this.Controls.Add(this.EnableButton);
            this.Controls.Add(this.songRequests);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "TwitchBotForm";
            this.Text = "Twitch bot requests";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TwitchBotForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.songRequests)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.DataGridView songRequests;
        public System.Windows.Forms.DataGridViewTextBoxColumn Song;
        public System.Windows.Forms.DataGridViewTextBoxColumn Author;
        public System.Windows.Forms.DataGridViewTextBoxColumn Viewer;
        public System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private Button EnableButton;
    }
}