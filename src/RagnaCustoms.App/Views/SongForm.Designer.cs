
using System.Windows.Forms;
using RagnaCustoms.App.Properties;

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

            System.Diagnostics.Process.GetCurrentProcess().Kill();

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
            this.Delete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Menu = new System.Windows.Forms.MenuStrip();
            this.FileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compareSongsVersionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preferencesToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.twitchBotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.questToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkAccessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.syncSongsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.languageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.englishToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.frenchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logsScreenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpPageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.SearchResultGridView)).BeginInit();
            this.Menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // SearchTextBox
            // 
            resources.ApplyResources(this.SearchTextBox, "SearchTextBox");
            this.SearchTextBox.Name = "SearchTextBox";
            this.SearchTextBox.TextChanged += new System.EventHandler(this.SearchTextBox_TextChanged);
            // 
            // SearchButton
            // 
            resources.ApplyResources(this.SearchButton, "SearchButton");
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Text = global::RagnaCustoms.App.Properties.Resources.Song_Form_Search;
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
            this.SongDownload,
            this.Delete});
            resources.ApplyResources(this.SearchResultGridView, "SearchResultGridView");
            this.SearchResultGridView.Name = "SearchResultGridView";
            this.SearchResultGridView.ReadOnly = true;
            this.SearchResultGridView.RowHeadersVisible = false;
            this.SearchResultGridView.RowTemplate.Height = 25;
            this.SearchResultGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.SearchResultGridView_CellContentClick);
            this.SearchResultGridView.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.SearchResultGridView_RowsAdded);
            // 
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            resources.ApplyResources(this.Id, "Id");
            this.Id.Name = "Id";
            this.Id.ReadOnly = true;
            // 
            // SongName
            // 
            this.SongName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SongName.DataPropertyName = "Name";
            this.SongName.HeaderText = global::RagnaCustoms.App.Properties.Resources.Song_Form_Name;
            this.SongName.Name = "SongName";
            this.SongName.ReadOnly = true;
            // 
            // SongDifficulties
            // 
            this.SongDifficulties.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SongDifficulties.DataPropertyName = "Difficulties";
            this.SongDifficulties.FillWeight = 50F;
            this.SongDifficulties.HeaderText = global::RagnaCustoms.App.Properties.Resources.Song_Form_Difficulties;
            this.SongDifficulties.Name = "SongDifficulties";
            this.SongDifficulties.ReadOnly = true;
            // 
            // SongAuthor
            // 
            this.SongAuthor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SongAuthor.DataPropertyName = "Author";
            this.SongAuthor.FillWeight = 80F;
            this.SongAuthor.HeaderText = global::RagnaCustoms.App.Properties.Resources.Song_Form_Author;
            this.SongAuthor.Name = "SongAuthor";
            this.SongAuthor.ReadOnly = true;
            // 
            // SongMapper
            // 
            this.SongMapper.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.SongMapper.DataPropertyName = "Mapper";
            this.SongMapper.FillWeight = 80F;
            this.SongMapper.HeaderText = global::RagnaCustoms.App.Properties.Resources.Song_Form_Mapper;
            this.SongMapper.Name = "SongMapper";
            this.SongMapper.ReadOnly = true;
            // 
            // SongDownload
            // 
            this.SongDownload.DataPropertyName = "Download";
            resources.ApplyResources(this.SongDownload, "SongDownload");
            this.SongDownload.Name = "SongDownload";
            this.SongDownload.ReadOnly = true;
            this.SongDownload.Text = "↓";
            this.SongDownload.UseColumnTextForButtonValue = true;
            // 
            // Delete
            // 
            this.Delete.DataPropertyName = "Delete";
            resources.ApplyResources(this.Delete, "Delete");
            this.Delete.Name = "Delete";
            this.Delete.ReadOnly = true;
            this.Delete.Text = "X";
            this.Delete.UseColumnTextForButtonValue = true;
            // 
            // Menu
            // 
            this.Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenuItem,
            this.loginToolStripMenuItem,
            this.preferencesToolStripMenuItem1,
            this.twitchBotToolStripMenuItem,
            this.questToolStripMenuItem,
            this.languageToolStripMenuItem,
            this.HelpMenuItem});
            resources.ApplyResources(this.Menu, "Menu");
            this.Menu.Name = "Menu";
            // 
            // FileMenuItem
            // 
            this.FileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compareSongsVersionToolStripMenuItem,
            this.toolStripMenuItem1,
            this.ExitMenuItem});
            this.FileMenuItem.Name = "FileMenuItem";
            resources.ApplyResources(this.FileMenuItem, "FileMenuItem");
            this.FileMenuItem.Text = global::RagnaCustoms.App.Properties.Resources.Song_Form_File;
            // 
            // compareSongsVersionToolStripMenuItem
            // 
            this.compareSongsVersionToolStripMenuItem.Name = "compareSongsVersionToolStripMenuItem";
            resources.ApplyResources(this.compareSongsVersionToolStripMenuItem, "compareSongsVersionToolStripMenuItem");
            this.compareSongsVersionToolStripMenuItem.Text = global::RagnaCustoms.App.Properties.Resources.Song_Form_CompareSongversion;
            this.compareSongsVersionToolStripMenuItem.Click += new System.EventHandler(this.compareSongsVersionToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // ExitMenuItem
            // 
            this.ExitMenuItem.Name = "ExitMenuItem";
            resources.ApplyResources(this.ExitMenuItem, "ExitMenuItem");
            this.ExitMenuItem.Text = global::RagnaCustoms.App.Properties.Resources.Song_Form_Exit;
            this.ExitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // preferencesToolStripMenuItem1
            // 
            this.preferencesToolStripMenuItem1.Name = "preferencesToolStripMenuItem1";
            resources.ApplyResources(this.preferencesToolStripMenuItem1, "preferencesToolStripMenuItem1");
            this.preferencesToolStripMenuItem1.Click += new System.EventHandler(this.preferencesToolStripMenuItem1_Click);
            // 
            // twitchBotToolStripMenuItem
            // 
            this.twitchBotToolStripMenuItem.Name = "twitchBotToolStripMenuItem";
            resources.ApplyResources(this.twitchBotToolStripMenuItem, "twitchBotToolStripMenuItem");
            this.twitchBotToolStripMenuItem.Click += new System.EventHandler(this.twitchBotToolStripMenuItem_Click);
            // 
            // questToolStripMenuItem
            // 
            this.questToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkAccessToolStripMenuItem,
            this.syncSongsToolStripMenuItem});
            this.questToolStripMenuItem.Name = "questToolStripMenuItem";
            resources.ApplyResources(this.questToolStripMenuItem, "questToolStripMenuItem");
            // 
            // checkAccessToolStripMenuItem
            // 
            this.checkAccessToolStripMenuItem.Name = "checkAccessToolStripMenuItem";
            resources.ApplyResources(this.checkAccessToolStripMenuItem, "checkAccessToolStripMenuItem");
            this.checkAccessToolStripMenuItem.Text = global::RagnaCustoms.App.Properties.Resources.Song_Form_CheckAccess;
            this.checkAccessToolStripMenuItem.Click += new System.EventHandler(this.checkAccessToolStripMenuItem_Click);
            // 
            // syncSongsToolStripMenuItem
            // 
            this.syncSongsToolStripMenuItem.Name = "syncSongsToolStripMenuItem";
            resources.ApplyResources(this.syncSongsToolStripMenuItem, "syncSongsToolStripMenuItem");
            this.syncSongsToolStripMenuItem.Text = global::RagnaCustoms.App.Properties.Resources.Song_Form_SyncSong;
            this.syncSongsToolStripMenuItem.Click += new System.EventHandler(this.syncSongsToolStripMenuItem_Click);
            // 
            // languageToolStripMenuItem
            // 
            this.languageToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.englishToolStripMenuItem,
            this.frenchToolStripMenuItem});
            this.languageToolStripMenuItem.Name = "languageToolStripMenuItem";
            resources.ApplyResources(this.languageToolStripMenuItem, "languageToolStripMenuItem");
            this.languageToolStripMenuItem.Text = global::RagnaCustoms.App.Properties.Resources.Song_Form_Language;
            // 
            // englishToolStripMenuItem
            // 
            this.englishToolStripMenuItem.CheckOnClick = true;
            this.englishToolStripMenuItem.Name = "englishToolStripMenuItem";
            resources.ApplyResources(this.englishToolStripMenuItem, "englishToolStripMenuItem");
            this.englishToolStripMenuItem.Click += new System.EventHandler(this.englishToolStripMenuItem_Click);
            // 
            // frenchToolStripMenuItem
            // 
            this.frenchToolStripMenuItem.CheckOnClick = true;
            this.frenchToolStripMenuItem.Name = "frenchToolStripMenuItem";
            resources.ApplyResources(this.frenchToolStripMenuItem, "frenchToolStripMenuItem");
            this.frenchToolStripMenuItem.Click += new System.EventHandler(this.frenchToolStripMenuItem_Click);
            // 
            // HelpMenuItem
            // 
            this.HelpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.logFileToolStripMenuItem,
            this.logsScreenToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.helpPageToolStripMenuItem});
            this.HelpMenuItem.Name = "HelpMenuItem";
            resources.ApplyResources(this.HelpMenuItem, "HelpMenuItem");
            this.HelpMenuItem.Text = global::RagnaCustoms.App.Properties.Resources.Song_Form_Help;
            // 
            // logFileToolStripMenuItem
            // 
            this.logFileToolStripMenuItem.Name = "logFileToolStripMenuItem";
            resources.ApplyResources(this.logFileToolStripMenuItem, "logFileToolStripMenuItem");
            this.logFileToolStripMenuItem.Text = global::RagnaCustoms.App.Properties.Resources.Song_Form_LogFile;
            this.logFileToolStripMenuItem.Click += new System.EventHandler(this.logFileToolStripMenuItem_Click);
            // 
            // logsScreenToolStripMenuItem
            // 
            this.logsScreenToolStripMenuItem.Name = "logsScreenToolStripMenuItem";
            resources.ApplyResources(this.logsScreenToolStripMenuItem, "logsScreenToolStripMenuItem");
            this.logsScreenToolStripMenuItem.Text = global::RagnaCustoms.App.Properties.Resources.Song_Form_LogScreen;
            this.logsScreenToolStripMenuItem.Click += new System.EventHandler(this.logScreenToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            resources.ApplyResources(this.aboutToolStripMenuItem, "aboutToolStripMenuItem");
            this.aboutToolStripMenuItem.Text = global::RagnaCustoms.App.Properties.Resources.Song_Form_About;
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // loginToolStripMenuItem
            // 
            this.loginToolStripMenuItem.Name = "loginToolStripMenuItem";
            resources.ApplyResources(this.loginToolStripMenuItem, "loginToolStripMenuItem");
            this.loginToolStripMenuItem.Click += new System.EventHandler(this.loginToolStripMenuItem_Click);
            // 
            // helpPageToolStripMenuItem
            // 
            this.helpPageToolStripMenuItem.Name = "helpPageToolStripMenuItem";
            resources.ApplyResources(this.helpPageToolStripMenuItem, "helpPageToolStripMenuItem");
            this.helpPageToolStripMenuItem.Click += new System.EventHandler(this.helpPageToolStripMenuItem_Click);
            // 
            // SongForm
            // 
            this.AcceptButton = this.SearchButton;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.SearchResultGridView);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.SearchTextBox);
            this.Controls.Add(this.Menu);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.Menu;
            this.MaximizeBox = false;
            this.Name = "SongForm";
            this.Load += new System.EventHandler(this.SongForm_Load);
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
        private System.Windows.Forms.ToolStripMenuItem HelpMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem questToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkAccessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem syncSongsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compareSongsVersionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logsScreenToolStripMenuItem;
        private ToolStripMenuItem languageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem englishToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem frenchToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem1;
        private DataGridViewTextBoxColumn Id;
        private DataGridViewTextBoxColumn SongName;
        private DataGridViewTextBoxColumn SongDifficulties;
        private DataGridViewTextBoxColumn SongAuthor;
        private DataGridViewTextBoxColumn SongMapper;
        private DataGridViewButtonColumn SongDownload;
        private DataGridViewButtonColumn Delete;
        private ToolStripMenuItem preferencesToolStripMenuItem1;
        private ToolStripMenuItem twitchBotToolStripMenuItem;
        private ToolStripMenuItem loginToolStripMenuItem;
        private ToolStripMenuItem helpPageToolStripMenuItem;
    }
}