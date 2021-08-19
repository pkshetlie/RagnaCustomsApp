
using System.Windows.Forms;

namespace RagnaCustoms.Views
{
    partial class SongForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SongForm));
            this.SearchTextBox = new System.Windows.Forms.TextBox();
            this.SearchButton = new System.Windows.Forms.Button();
            this.SearchResultGridView = new System.Windows.Forms.DataGridView();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SongName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SongDifficulties = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SongAuthor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SongMapper = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SongDownload = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Menu = new System.Windows.Forms.MenuStrip();
            this.FileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compareSongsVersionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scoreSystemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ApiKeyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SendScoreAutomaticallyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoCloseDownloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.streamingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configureApiKeyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.overlayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gotoOverlayUrlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.twitchBotToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.questToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkAccessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.syncSongsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logsScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.languageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.englishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.frenchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.SearchResultGridView)).BeginInit();
            this.Menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // SearchTextBox
            // 
            this.SearchTextBox.Location = new System.Drawing.Point(10, 23);
            this.SearchTextBox.Name = "SearchTextBox";
            this.SearchTextBox.Size = new System.Drawing.Size(445, 20);
            this.SearchTextBox.TabIndex = 1;
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(460, 23);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(64, 20);
            this.SearchButton.TabIndex = 2;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // SearchResultGridView
            // 
            this.SearchResultGridView.AllowUserToAddRows = false;
            this.SearchResultGridView.AllowUserToDeleteRows = false;
            this.SearchResultGridView.AllowUserToResizeColumns = false;
            this.SearchResultGridView.AllowUserToResizeRows = false;
            this.SearchResultGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SearchResultGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Id,
            this.SongName,
            this.SongDifficulties,
            this.SongAuthor,
            this.SongMapper,
            this.SongDownload});
            this.SearchResultGridView.Location = new System.Drawing.Point(10, 49);
            this.SearchResultGridView.Name = "SearchResultGridView";
            this.SearchResultGridView.ReadOnly = true;
            this.SearchResultGridView.RowHeadersVisible = false;
            this.SearchResultGridView.RowTemplate.Height = 25;
            this.SearchResultGridView.Size = new System.Drawing.Size(514, 323);
            this.SearchResultGridView.TabIndex = 3;
            this.SearchResultGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.SearchResultGridView_CellContentClick);
            // 
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            this.Id.Visible = false;
            // 
            // SongName
            // 
            this.SongName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SongName.DataPropertyName = "Name";
            this.SongName.HeaderText = "Name";
            this.SongName.Name = "SongName";
            this.SongName.ReadOnly = true;
            // 
            // SongDifficulties
            // 
            this.SongDifficulties.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SongDifficulties.DataPropertyName = "Difficulties";
            this.SongDifficulties.FillWeight = 50F;
            this.SongDifficulties.HeaderText = "Difficulties";
            this.SongDifficulties.Name = "SongDifficulties";
            this.SongDifficulties.ReadOnly = true;
            // 
            // SongAuthor
            // 
            this.SongAuthor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SongAuthor.DataPropertyName = "Author";
            this.SongAuthor.FillWeight = 80F;
            this.SongAuthor.HeaderText = "Author";
            this.SongAuthor.Name = "SongAuthor";
            this.SongAuthor.ReadOnly = true;
            // 
            // SongMapper
            // 
            this.SongMapper.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SongMapper.DataPropertyName = "Mapper";
            this.SongMapper.FillWeight = 80F;
            this.SongMapper.HeaderText = "Mapper";
            this.SongMapper.Name = "SongMapper";
            this.SongMapper.ReadOnly = true;
            // 
            // SongDownload
            // 
            this.SongDownload.HeaderText = "";
            this.SongDownload.Name = "SongDownload";
            this.SongDownload.ReadOnly = true;
            this.SongDownload.Text = "↓";
            this.SongDownload.UseColumnTextForButtonValue = true;
            this.SongDownload.Width = 30;
            // 
            // Menu
            // 
            this.Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenuItem,
            this.ToolsMenuItem,
            this.questToolStripMenuItem,
            this.HelpMenuItem,
            this.languageToolStripMenuItem});
            this.Menu.Location = new System.Drawing.Point(0, 0);
            this.Menu.Name = "Menu";
            this.Menu.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.Menu.Size = new System.Drawing.Size(535, 24);
            this.Menu.TabIndex = 0;
            this.Menu.Text = "menuStrip1";
            // 
            // FileMenuItem
            // 
            this.FileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compareSongsVersionToolStripMenuItem,
            this.ExitMenuItem});
            this.FileMenuItem.Name = "FileMenuItem";
            this.FileMenuItem.Size = new System.Drawing.Size(37, 20);
            this.FileMenuItem.Text = "&File";
            // 
            // compareSongsVersionToolStripMenuItem
            // 
            this.compareSongsVersionToolStripMenuItem.Name = "compareSongsVersionToolStripMenuItem";
            this.compareSongsVersionToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.compareSongsVersionToolStripMenuItem.Text = "Compare songs version";
            this.compareSongsVersionToolStripMenuItem.Click += new System.EventHandler(this.compareSongsVersionToolStripMenuItem_Click);
            // 
            // ExitMenuItem
            // 
            this.ExitMenuItem.Name = "ExitMenuItem";
            this.ExitMenuItem.Size = new System.Drawing.Size(198, 22);
            this.ExitMenuItem.Text = "E&xit";
            this.ExitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // ToolsMenuItem
            // 
            this.ToolsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scoreSystemToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.streamingToolStripMenuItem});
            this.ToolsMenuItem.Name = "ToolsMenuItem";
            this.ToolsMenuItem.Size = new System.Drawing.Size(46, 20);
            this.ToolsMenuItem.Text = "&Tools";
            // 
            // scoreSystemToolStripMenuItem
            // 
            this.scoreSystemToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ApiKeyMenuItem,
            this.SendScoreAutomaticallyMenuItem});
            this.scoreSystemToolStripMenuItem.Name = "scoreSystemToolStripMenuItem";
            this.scoreSystemToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.scoreSystemToolStripMenuItem.Text = "Score system";
            // 
            // ApiKeyMenuItem
            // 
            this.ApiKeyMenuItem.Name = "ApiKeyMenuItem";
            this.ApiKeyMenuItem.Size = new System.Drawing.Size(206, 22);
            this.ApiKeyMenuItem.Text = "&Configure API key...";
            this.ApiKeyMenuItem.Click += new System.EventHandler(this.ApiKeyMenuItem_Click);
            // 
            // SendScoreAutomaticallyMenuItem
            // 
            this.SendScoreAutomaticallyMenuItem.Checked = true;
            this.SendScoreAutomaticallyMenuItem.CheckOnClick = true;
            this.SendScoreAutomaticallyMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SendScoreAutomaticallyMenuItem.Name = "SendScoreAutomaticallyMenuItem";
            this.SendScoreAutomaticallyMenuItem.Size = new System.Drawing.Size(206, 22);
            this.SendScoreAutomaticallyMenuItem.Text = "&Send score automatically";
            this.SendScoreAutomaticallyMenuItem.CheckedChanged += new System.EventHandler(this.SendScoreMenuItem_CheckedChanged);
            this.SendScoreAutomaticallyMenuItem.Click += new System.EventHandler(this.SendScoreAutomaticallyMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.autoCloseDownloadToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // autoCloseDownloadToolStripMenuItem
            // 
            this.autoCloseDownloadToolStripMenuItem.CheckOnClick = true;
            this.autoCloseDownloadToolStripMenuItem.Name = "autoCloseDownloadToolStripMenuItem";
            this.autoCloseDownloadToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.autoCloseDownloadToolStripMenuItem.Text = "Auto close download";
            this.autoCloseDownloadToolStripMenuItem.CheckedChanged += new System.EventHandler(this.autoCloseDownloadToolStripMenuItem_CheckedChanged);
            // 
            // streamingToolStripMenuItem
            // 
            this.streamingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configureApiKeyToolStripMenuItem,
            this.overlayToolStripMenuItem,
            this.gotoOverlayUrlToolStripMenuItem,
            this.twitchBotToolStripMenuItem1});
            this.streamingToolStripMenuItem.Name = "streamingToolStripMenuItem";
            this.streamingToolStripMenuItem.Size = new System.Drawing.Size(143, 22);
            this.streamingToolStripMenuItem.Text = "Streaming";
            // 
            // configureApiKeyToolStripMenuItem
            // 
            this.configureApiKeyToolStripMenuItem.Name = "configureApiKeyToolStripMenuItem";
            this.configureApiKeyToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.configureApiKeyToolStripMenuItem.Text = "Configure API key ...";
            this.configureApiKeyToolStripMenuItem.Click += new System.EventHandler(this.configureApiKeyToolStripMenuItem_Click);
            // 
            // overlayToolStripMenuItem
            // 
            this.overlayToolStripMenuItem.CheckOnClick = true;
            this.overlayToolStripMenuItem.Name = "overlayToolStripMenuItem";
            this.overlayToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.overlayToolStripMenuItem.Text = "Enable overlay";
            this.overlayToolStripMenuItem.CheckedChanged += new System.EventHandler(this.overlayToolStripMenuItem_CheckedChanged);
            // 
            // gotoOverlayUrlToolStripMenuItem
            // 
            this.gotoOverlayUrlToolStripMenuItem.Name = "gotoOverlayUrlToolStripMenuItem";
            this.gotoOverlayUrlToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.gotoOverlayUrlToolStripMenuItem.Text = "Get overlay url";
            this.gotoOverlayUrlToolStripMenuItem.Click += new System.EventHandler(this.gotoOverlayUrlToolStripMenuItem_Click);
            // 
            // twitchBotToolStripMenuItem1
            // 
            this.twitchBotToolStripMenuItem1.Name = "twitchBotToolStripMenuItem1";
            this.twitchBotToolStripMenuItem1.Size = new System.Drawing.Size(181, 22);
            this.twitchBotToolStripMenuItem1.Text = "TwitchBot";
            this.twitchBotToolStripMenuItem1.Click += new System.EventHandler(this.twitchBotToolStripMenuItem1_Click);
            // 
            // questToolStripMenuItem
            // 
            this.questToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkAccessToolStripMenuItem,
            this.syncSongsToolStripMenuItem});
            this.questToolStripMenuItem.Name = "questToolStripMenuItem";
            this.questToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.questToolStripMenuItem.Text = "Quest";
            // 
            // checkAccessToolStripMenuItem
            // 
            this.checkAccessToolStripMenuItem.Name = "checkAccessToolStripMenuItem";
            this.checkAccessToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.checkAccessToolStripMenuItem.Text = "Check access";
            this.checkAccessToolStripMenuItem.Click += new System.EventHandler(this.checkAccessToolStripMenuItem_Click);
            // 
            // syncSongsToolStripMenuItem
            // 
            this.syncSongsToolStripMenuItem.Name = "syncSongsToolStripMenuItem";
            this.syncSongsToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.syncSongsToolStripMenuItem.Text = "Sync songs";
            this.syncSongsToolStripMenuItem.Click += new System.EventHandler(this.syncSongsToolStripMenuItem_Click);
            // 
            // HelpMenuItem
            // 
            this.HelpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logFileToolStripMenuItem,
            this.logsScreenToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.HelpMenuItem.Name = "HelpMenuItem";
            this.HelpMenuItem.Size = new System.Drawing.Size(44, 20);
            this.HelpMenuItem.Text = "&Help";
            // 
            // logFileToolStripMenuItem
            // 
            this.logFileToolStripMenuItem.Name = "logFileToolStripMenuItem";
            this.logFileToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.logFileToolStripMenuItem.Text = "Log file";
            this.logFileToolStripMenuItem.Click += new System.EventHandler(this.logFileToolStripMenuItem_Click);
            // 
            // logsScreenToolStripMenuItem
            // 
            this.logsScreenToolStripMenuItem.Name = "logsScreenToolStripMenuItem";
            this.logsScreenToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.logsScreenToolStripMenuItem.Text = "Logs screen";
            this.logsScreenToolStripMenuItem.Visible = false;
            this.logsScreenToolStripMenuItem.Click += new System.EventHandler(this.logScreenToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // languageToolStripMenuItem
            // 
            this.languageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.englishToolStripMenuItem,
            this.frenchToolStripMenuItem});
            this.languageToolStripMenuItem.Name = "languageToolStripMenuItem";
            this.languageToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.languageToolStripMenuItem.Text = "Language";
            // 
            // englishToolStripMenuItem
            // 
            this.englishToolStripMenuItem.CheckOnClick = true;
            this.englishToolStripMenuItem.Name = "englishToolStripMenuItem";
            this.englishToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.englishToolStripMenuItem.Text = "English";
            this.englishToolStripMenuItem.Click += new System.EventHandler(this.englishToolStripMenuItem_Click);
            // 
            // frenchToolStripMenuItem
            // 
            this.frenchToolStripMenuItem.CheckOnClick = true;
            this.frenchToolStripMenuItem.Name = "frenchToolStripMenuItem";
            this.frenchToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.frenchToolStripMenuItem.Text = "French";
            // 
            // SongForm
            // 
            this.AcceptButton = this.SearchButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(535, 382);
            this.Controls.Add(this.SearchResultGridView);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.SearchTextBox);
            this.Controls.Add(this.Menu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.Menu;
            this.MaximizeBox = false;
            this.Name = "SongForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RagnaCustoms";
            ((System.ComponentModel.ISupportInitialize)(this.SearchResultGridView)).EndInit();
            this.Menu.ResumeLayout(false);
            this.Menu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox SearchTextBox;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.DataGridView SearchResultGridView;
        private new System.Windows.Forms.MenuStrip Menu;
        private System.Windows.Forms.ToolStripMenuItem FileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HelpMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scoreSystemToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoCloseDownloadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SendScoreAutomaticallyMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ApiKeyMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem questToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkAccessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem syncSongsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compareSongsVersionToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn SongName;
        private System.Windows.Forms.DataGridViewTextBoxColumn SongDifficulties;
        private System.Windows.Forms.DataGridViewTextBoxColumn SongAuthor;
        private System.Windows.Forms.DataGridViewTextBoxColumn SongMapper;
        private System.Windows.Forms.DataGridViewButtonColumn SongDownload;
        private System.Windows.Forms.ToolStripMenuItem logsScreenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem streamingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem twitchBotToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem overlayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gotoOverlayUrlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configureApiKeyToolStripMenuItem;
        private ToolStripMenuItem languageToolStripMenuItem;
        private ToolStripMenuItem englishToolStripMenuItem;
        private ToolStripMenuItem frenchToolStripMenuItem;
    }
}