
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
            this.botMessagePrefixLabel = new System.Windows.Forms.Label();
            this.prefix = new System.Windows.Forms.TextBox();
            this.songRequests = new System.Windows.Forms.DataGridView();
            this.Song = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Author = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Viewer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.refreshApps = new System.Windows.Forms.Button();
            this.ragnarockApp = new System.Windows.Forms.Label();
            this.bot_enabled = new System.Windows.Forms.CheckBox();
            this.twitchChannel = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.twitchOAuth = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.helptwitchtmi = new System.Windows.Forms.Label();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.versionLabel = new System.Windows.Forms.Label();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.songRequests)).BeginInit();
            this.SuspendLayout();
            // 
            // botMessagePrefixLabel
            // 
            this.botMessagePrefixLabel.AutoSize = true;
            this.botMessagePrefixLabel.Location = new System.Drawing.Point(515, 124);
            this.botMessagePrefixLabel.Name = "botMessagePrefixLabel";
            this.botMessagePrefixLabel.Size = new System.Drawing.Size(96, 13);
            this.botMessagePrefixLabel.TabIndex = 31;
            this.botMessagePrefixLabel.Text = "Bot message prefix";
            // 
            // prefix
            // 
            this.prefix.Location = new System.Drawing.Point(617, 120);
            this.prefix.Name = "prefix";
            this.prefix.Size = new System.Drawing.Size(37, 20);
            this.prefix.TabIndex = 30;
            this.prefix.Text = "! ";
            // 
            // songRequests
            // 
            this.songRequests.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.songRequests.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Song,
            this.Author,
            this.Viewer,
            this.Id});
            this.songRequests.Location = new System.Drawing.Point(12, 146);
            this.songRequests.Name = "songRequests";
            this.songRequests.Size = new System.Drawing.Size(645, 373);
            this.songRequests.TabIndex = 29;
            // 
            // Song
            // 
            this.Song.HeaderText = "Song";
            this.Song.Name = "Song";
            this.Song.ReadOnly = true;
            this.Song.Width = 250;
            // 
            // Author
            // 
            this.Author.HeaderText = "Author";
            this.Author.Name = "Author";
            this.Author.ReadOnly = true;
            this.Author.Width = 150;
            // 
            // Viewer
            // 
            this.Viewer.HeaderText = "Viewer";
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
            // refreshApps
            // 
            this.refreshApps.Location = new System.Drawing.Point(56, 92);
            this.refreshApps.Name = "refreshApps";
            this.refreshApps.Size = new System.Drawing.Size(75, 76);
            this.refreshApps.TabIndex = 27;
            this.refreshApps.Text = "button1";
            this.refreshApps.UseVisualStyleBackColor = true;
            // 
            // ragnarockApp
            // 
            this.ragnarockApp.AutoSize = true;
            this.ragnarockApp.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ragnarockApp.Location = new System.Drawing.Point(15, 100);
            this.ragnarockApp.Name = "ragnarockApp";
            this.ragnarockApp.Size = new System.Drawing.Size(35, 13);
            this.ragnarockApp.TabIndex = 26;
            this.ragnarockApp.Text = "label4";
            // 
            // bot_enabled
            // 
            this.bot_enabled.AutoSize = true;
            this.bot_enabled.Location = new System.Drawing.Point(15, 76);
            this.bot_enabled.Name = "bot_enabled";
            this.bot_enabled.Size = new System.Drawing.Size(74, 17);
            this.bot_enabled.TabIndex = 25;
            this.bot_enabled.Text = "Enabled ?";
            this.bot_enabled.UseVisualStyleBackColor = true;
            // 
            // twitchChannel
            // 
            this.twitchChannel.Location = new System.Drawing.Point(166, 46);
            this.twitchChannel.Name = "twitchChannel";
            this.twitchChannel.Size = new System.Drawing.Size(201, 20);
            this.twitchChannel.TabIndex = 24;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 13);
            this.label3.TabIndex = 23;
            this.label3.Text = "Your twitch channel";
            // 
            // twitchOAuth
            // 
            this.twitchOAuth.Location = new System.Drawing.Point(166, 6);
            this.twitchOAuth.Name = "twitchOAuth";
            this.twitchOAuth.PasswordChar = '*';
            this.twitchOAuth.Size = new System.Drawing.Size(201, 20);
            this.twitchOAuth.TabIndex = 22;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(145, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Twitch Chat OAuth password";
            // 
            // helptwitchtmi
            // 
            this.helptwitchtmi.AutoSize = true;
            this.helptwitchtmi.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.helptwitchtmi.Location = new System.Drawing.Point(15, 29);
            this.helptwitchtmi.Name = "helptwitchtmi";
            this.helptwitchtmi.Size = new System.Drawing.Size(207, 13);
            this.helptwitchtmi.TabIndex = 20;
            this.helptwitchtmi.Text = "Récupérez votre clé Chat OAuth Twitch sur";
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel2.Location = new System.Drawing.Point(225, 29);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(130, 13);
            this.linkLabel2.TabIndex = 19;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "https://twitchapps.com/tmi/";
            // 
            // versionLabel
            // 
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(20, 528);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(35, 13);
            this.versionLabel.TabIndex = 32;
            this.versionLabel.Text = "label5";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.LinkColor = System.Drawing.Color.DodgerBlue;
            this.linkLabel1.Location = new System.Drawing.Point(230, 528);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(165, 13);
            this.linkLabel1.TabIndex = 28;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Application by twitch.tv/Rhokapa";
            // 
            // TwitchBotForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 552);
            this.Controls.Add(this.botMessagePrefixLabel);
            this.Controls.Add(this.prefix);
            this.Controls.Add(this.songRequests);
            this.Controls.Add(this.refreshApps);
            this.Controls.Add(this.ragnarockApp);
            this.Controls.Add(this.bot_enabled);
            this.Controls.Add(this.twitchChannel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.twitchOAuth);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.helptwitchtmi);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.linkLabel1);
            this.Name = "TwitchBotForm";
            this.Text = "TwitchBotForm";
            ((System.ComponentModel.ISupportInitialize)(this.songRequests)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label botMessagePrefixLabel;
        private System.Windows.Forms.TextBox prefix;
        private System.Windows.Forms.DataGridView songRequests;
        private System.Windows.Forms.DataGridViewTextBoxColumn Song;
        private System.Windows.Forms.DataGridViewTextBoxColumn Author;
        private System.Windows.Forms.DataGridViewTextBoxColumn Viewer;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.Button refreshApps;
        private System.Windows.Forms.Label ragnarockApp;
        private System.Windows.Forms.CheckBox bot_enabled;
        private System.Windows.Forms.TextBox twitchChannel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox twitchOAuth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label helptwitchtmi;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}