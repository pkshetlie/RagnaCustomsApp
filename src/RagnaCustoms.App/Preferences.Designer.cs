namespace RagnaCustoms.App
{
    partial class Preferences
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Preferences));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.defaultDirButton = new System.Windows.Forms.Button();
            this.DefaultDirTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.closeOnEndCheckbox = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.OrderAlphabetCheckbox = new System.Windows.Forms.CheckBox();
            this.copyRanked = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.RequestFolderText = new System.Windows.Forms.TextBox();
            this.autoStart = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.twitchChannel = new System.Windows.Forms.TextBox();
            this.prefix = new System.Windows.Forms.TextBox();
            this.botMessagePrefixLabel = new System.Windows.Forms.Label();
            this.helptwitchtmi = new System.Windows.Forms.Label();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.twitchOAuth = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.defaultDirButton);
            this.groupBox1.Controls.Add(this.DefaultDirTxt);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 131);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "General";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "ApiKey";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(10, 44);
            this.textBox1.Name = "textBox1";
            this.textBox1.PasswordChar = '*';
            this.textBox1.Size = new System.Drawing.Size(184, 20);
            this.textBox1.TabIndex = 3;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // defaultDirButton
            // 
            this.defaultDirButton.Location = new System.Drawing.Point(146, 97);
            this.defaultDirButton.Name = "defaultDirButton";
            this.defaultDirButton.Size = new System.Drawing.Size(54, 23);
            this.defaultDirButton.TabIndex = 2;
            this.defaultDirButton.Text = "Change";
            this.defaultDirButton.UseVisualStyleBackColor = true;
            this.defaultDirButton.Click += new System.EventHandler(this.defaultDirButton_Click);
            // 
            // DefaultDirTxt
            // 
            this.DefaultDirTxt.Location = new System.Drawing.Point(10, 98);
            this.DefaultDirTxt.Name = "DefaultDirTxt";
            this.DefaultDirTxt.Size = new System.Drawing.Size(184, 20);
            this.DefaultDirTxt.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Base directory:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.closeOnEndCheckbox);
            this.groupBox2.Location = new System.Drawing.Point(12, 149);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(200, 46);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "One click download";
            // 
            // closeOnEndCheckbox
            // 
            this.closeOnEndCheckbox.AutoSize = true;
            this.closeOnEndCheckbox.Location = new System.Drawing.Point(7, 20);
            this.closeOnEndCheckbox.Name = "closeOnEndCheckbox";
            this.closeOnEndCheckbox.Size = new System.Drawing.Size(143, 17);
            this.closeOnEndCheckbox.TabIndex = 0;
            this.closeOnEndCheckbox.Text = "Close on download finish";
            this.closeOnEndCheckbox.UseVisualStyleBackColor = true;
            this.closeOnEndCheckbox.CheckStateChanged += new System.EventHandler(this.closeOnEndCheckbox_CheckStateChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.OrderAlphabetCheckbox);
            this.groupBox3.Controls.Add(this.copyRanked);
            this.groupBox3.Location = new System.Drawing.Point(12, 201);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 79);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Directories";
            // 
            // OrderAlphabetCheckbox
            // 
            this.OrderAlphabetCheckbox.AutoSize = true;
            this.OrderAlphabetCheckbox.Location = new System.Drawing.Point(7, 43);
            this.OrderAlphabetCheckbox.Name = "OrderAlphabetCheckbox";
            this.OrderAlphabetCheckbox.Size = new System.Drawing.Size(173, 17);
            this.OrderAlphabetCheckbox.TabIndex = 2;
            this.OrderAlphabetCheckbox.Text = "Order downloads alphabetically";
            this.OrderAlphabetCheckbox.UseVisualStyleBackColor = true;
            this.OrderAlphabetCheckbox.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // copyRanked
            // 
            this.copyRanked.AutoSize = true;
            this.copyRanked.Location = new System.Drawing.Point(7, 19);
            this.copyRanked.Name = "copyRanked";
            this.copyRanked.Size = new System.Drawing.Size(191, 17);
            this.copyRanked.TabIndex = 1;
            this.copyRanked.Text = "Copy ranked song in specific folder";
            this.copyRanked.UseVisualStyleBackColor = true;
            this.copyRanked.CheckedChanged += new System.EventHandler(this.copyRanked_CheckedChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.RequestFolderText);
            this.groupBox4.Controls.Add(this.autoStart);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.twitchChannel);
            this.groupBox4.Controls.Add(this.prefix);
            this.groupBox4.Controls.Add(this.botMessagePrefixLabel);
            this.groupBox4.Controls.Add(this.helptwitchtmi);
            this.groupBox4.Controls.Add(this.linkLabel2);
            this.groupBox4.Controls.Add(this.twitchOAuth);
            this.groupBox4.Location = new System.Drawing.Point(218, 12);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(222, 268);
            this.groupBox4.TabIndex = 33;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Twitch bot";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 223);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(127, 13);
            this.label5.TabIndex = 39;
            this.label5.Text = "Download directory name";
            // 
            // RequestFolderText
            // 
            this.RequestFolderText.Location = new System.Drawing.Point(10, 239);
            this.RequestFolderText.Name = "RequestFolderText";
            this.RequestFolderText.Size = new System.Drawing.Size(205, 20);
            this.RequestFolderText.TabIndex = 38;
            this.RequestFolderText.TextChanged += new System.EventHandler(this.RequestFolderText_TextChanged);
            // 
            // autoStart
            // 
            this.autoStart.AutoSize = true;
            this.autoStart.Location = new System.Drawing.Point(9, 187);
            this.autoStart.Name = "autoStart";
            this.autoStart.Size = new System.Drawing.Size(80, 17);
            this.autoStart.TabIndex = 37;
            this.autoStart.Text = "Auto-start ?";
            this.autoStart.UseVisualStyleBackColor = true;
            this.autoStart.CheckedChanged += new System.EventHandler(this.autoStart_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 36;
            this.label4.Text = "TMI OAuth";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 35;
            this.label3.Text = "Channel name";
            // 
            // twitchChannel
            // 
            this.twitchChannel.Location = new System.Drawing.Point(10, 114);
            this.twitchChannel.Name = "twitchChannel";
            this.twitchChannel.Size = new System.Drawing.Size(203, 20);
            this.twitchChannel.TabIndex = 32;
            this.twitchChannel.TextChanged += new System.EventHandler(this.twitchChannel_TextChanged);
            // 
            // prefix
            // 
            this.prefix.Location = new System.Drawing.Point(117, 152);
            this.prefix.Name = "prefix";
            this.prefix.Size = new System.Drawing.Size(37, 20);
            this.prefix.TabIndex = 33;
            this.prefix.Text = "! ";
            this.prefix.TextChanged += new System.EventHandler(this.prefix_TextChanged);
            // 
            // botMessagePrefixLabel
            // 
            this.botMessagePrefixLabel.AutoSize = true;
            this.botMessagePrefixLabel.Location = new System.Drawing.Point(9, 155);
            this.botMessagePrefixLabel.Name = "botMessagePrefixLabel";
            this.botMessagePrefixLabel.Size = new System.Drawing.Size(96, 13);
            this.botMessagePrefixLabel.TabIndex = 34;
            this.botMessagePrefixLabel.Text = "Bot message prefix";
            // 
            // helptwitchtmi
            // 
            this.helptwitchtmi.AutoSize = true;
            this.helptwitchtmi.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.helptwitchtmi.Location = new System.Drawing.Point(6, 67);
            this.helptwitchtmi.Name = "helptwitchtmi";
            this.helptwitchtmi.Size = new System.Drawing.Size(71, 13);
            this.helptwitchtmi.TabIndex = 20;
            this.helptwitchtmi.Text = "Get Token At:";
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel2.Location = new System.Drawing.Point(83, 67);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(130, 13);
            this.linkLabel2.TabIndex = 19;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "https://twitchapps.com/tmi/";
            this.linkLabel2.Click += new System.EventHandler(this.linkLabel2_Click);
            // 
            // twitchOAuth
            // 
            this.twitchOAuth.Location = new System.Drawing.Point(9, 44);
            this.twitchOAuth.Name = "twitchOAuth";
            this.twitchOAuth.PasswordChar = '*';
            this.twitchOAuth.Size = new System.Drawing.Size(204, 20);
            this.twitchOAuth.TabIndex = 22;
            this.twitchOAuth.TextChanged += new System.EventHandler(this.twitchOAuth_TextChanged);
            // 
            // Preferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 288);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Preferences";
            this.Text = "Preferences";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button defaultDirButton;
        private System.Windows.Forms.TextBox DefaultDirTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.CheckBox closeOnEndCheckbox;
        private System.Windows.Forms.CheckBox copyRanked;
        private System.Windows.Forms.CheckBox OrderAlphabetCheckbox;
        private System.Windows.Forms.GroupBox groupBox4;
        public System.Windows.Forms.Label helptwitchtmi;
        public System.Windows.Forms.LinkLabel linkLabel2;
        public System.Windows.Forms.TextBox twitchOAuth;
        public System.Windows.Forms.TextBox twitchChannel;
        public System.Windows.Forms.TextBox prefix;
        public System.Windows.Forms.Label botMessagePrefixLabel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.CheckBox autoStart;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox RequestFolderText;
    }
}