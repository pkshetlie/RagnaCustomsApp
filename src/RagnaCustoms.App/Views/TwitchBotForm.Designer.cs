
using System.Windows.Forms;
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
            this.botMessagePrefixLabel = new System.Windows.Forms.Label();
            this.prefix = new System.Windows.Forms.TextBox();
            this.songRequests = new System.Windows.Forms.DataGridView();
            this.Song = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Author = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Viewer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bot_enabled = new System.Windows.Forms.CheckBox();
            this.twitchChannel = new System.Windows.Forms.TextBox();
            this.twitchOAuth = new System.Windows.Forms.TextBox();
            this.helptwitchtmi = new System.Windows.Forms.Label();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.songRequests)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // botMessagePrefixLabel
            // 
            this.botMessagePrefixLabel.AutoSize = true;
            this.botMessagePrefixLabel.Location = new System.Drawing.Point(6, 48);
            this.botMessagePrefixLabel.Name = "botMessagePrefixLabel";
            this.botMessagePrefixLabel.Size = new System.Drawing.Size(96, 13);
            this.botMessagePrefixLabel.TabIndex = 31;
            this.botMessagePrefixLabel.Text = "Bot message prefix";
            this.botMessagePrefixLabel.Click += new System.EventHandler(this.botMessagePrefixLabel_Click);
            // 
            // prefix
            // 
            this.prefix.Location = new System.Drawing.Point(110, 44);
            this.prefix.Name = "prefix";
            this.prefix.Size = new System.Drawing.Size(37, 20);
            this.prefix.TabIndex = 30;
            this.prefix.Text = "! ";
            this.prefix.TextChanged += new System.EventHandler(this.prefix_TextChanged);
            // 
            // songRequests
            // 
            this.songRequests.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.songRequests.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Song,
            this.Author,
            this.Viewer,
            this.Id});
            this.songRequests.Location = new System.Drawing.Point(12, 83);
            this.songRequests.Name = "songRequests";
            this.songRequests.Size = new System.Drawing.Size(645, 420);
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
            // bot_enabled
            // 
            this.bot_enabled.AutoSize = true;
            this.bot_enabled.Location = new System.Drawing.Point(7, 46);
            this.bot_enabled.Name = "bot_enabled";
            this.bot_enabled.Size = new System.Drawing.Size(74, 17);
            this.bot_enabled.TabIndex = 25;
            this.bot_enabled.Text = "Enabled ?";
            this.bot_enabled.UseVisualStyleBackColor = true;
            this.bot_enabled.CheckedChanged += new System.EventHandler(this.bot_enabled_CheckedChanged_1);
            // 
            // twitchChannel
            // 
            this.twitchChannel.Location = new System.Drawing.Point(9, 19);
            this.twitchChannel.Name = "twitchChannel";
            this.twitchChannel.Size = new System.Drawing.Size(138, 20);
            this.twitchChannel.TabIndex = 24;
            this.twitchChannel.TextChanged += new System.EventHandler(this.twitchChannel_TextChanged);
            // 
            // twitchOAuth
            // 
            this.twitchOAuth.Location = new System.Drawing.Point(9, 19);
            this.twitchOAuth.Name = "twitchOAuth";
            this.twitchOAuth.PasswordChar = '*';
            this.twitchOAuth.Size = new System.Drawing.Size(185, 20);
            this.twitchOAuth.TabIndex = 22;
            this.twitchOAuth.TextChanged += new System.EventHandler(this.twitchOAuth_TextChanged_1);
            // 
            // helptwitchtmi
            // 
            this.helptwitchtmi.AutoSize = true;
            this.helptwitchtmi.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.helptwitchtmi.Location = new System.Drawing.Point(6, 39);
            this.helptwitchtmi.Name = "helptwitchtmi";
            this.helptwitchtmi.Size = new System.Drawing.Size(70, 13);
            this.helptwitchtmi.TabIndex = 20;
            this.helptwitchtmi.Text = "Get Token at ";
            this.helptwitchtmi.Click += new System.EventHandler(this.helptwitchtmi_Click);
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel2.Location = new System.Drawing.Point(6, 51);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(130, 13);
            this.linkLabel2.TabIndex = 19;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "https://twitchapps.com/tmi/";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked_1);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.helptwitchtmi);
            this.groupBox1.Controls.Add(this.linkLabel2);
            this.groupBox1.Controls.Add(this.twitchOAuth);
            this.groupBox1.Location = new System.Drawing.Point(12, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 69);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "TMI OAuth password";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.twitchChannel);
            this.groupBox2.Controls.Add(this.prefix);
            this.groupBox2.Controls.Add(this.botMessagePrefixLabel);
            this.groupBox2.Location = new System.Drawing.Point(218, 13);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(153, 69);
            this.groupBox2.TabIndex = 33;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Twitch channel";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(7, 21);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(80, 17);
            this.checkBox1.TabIndex = 34;
            this.checkBox1.Text = "Auto-start ?";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBox1);
            this.groupBox3.Controls.Add(this.bot_enabled);
            this.groupBox3.Location = new System.Drawing.Point(377, 13);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(109, 69);
            this.groupBox3.TabIndex = 35;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Config";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.button2);
            this.groupBox4.Controls.Add(this.button1);
            this.groupBox4.Location = new System.Drawing.Point(492, 13);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(165, 69);
            this.groupBox4.TabIndex = 36;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Commands";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(7, 17);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(152, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Manage custom commands";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(7, 43);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(152, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Manage base commands";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // TwitchBotForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(669, 530);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.songRequests);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "TwitchBotForm";
            this.Text = "TwitchBotForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TwitchBotForm_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.songRequests)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Label botMessagePrefixLabel;
        public System.Windows.Forms.TextBox prefix;
        public System.Windows.Forms.DataGridView songRequests;
        public System.Windows.Forms.DataGridViewTextBoxColumn Song;
        public System.Windows.Forms.DataGridViewTextBoxColumn Author;
        public System.Windows.Forms.DataGridViewTextBoxColumn Viewer;
        public System.Windows.Forms.DataGridViewTextBoxColumn Id;
        public System.Windows.Forms.CheckBox bot_enabled;
        public System.Windows.Forms.TextBox twitchChannel;
        public System.Windows.Forms.TextBox twitchOAuth;
        public System.Windows.Forms.Label helptwitchtmi;
        public System.Windows.Forms.LinkLabel linkLabel2;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        public CheckBox checkBox1;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private Button button2;
        private Button button1;
    }
}