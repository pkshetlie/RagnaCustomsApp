
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
            this.Menu = new System.Windows.Forms.MenuStrip();
            this.FileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SendScoreAutomaticallyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ApiKeyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.HelpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SongName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SongDifficulties = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SongAuthor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SongMapper = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SongDownload = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.SearchResultGridView)).BeginInit();
            this.Menu.SuspendLayout();
            this.SuspendLayout();
            // 
            // SearchTextBox
            // 
            this.SearchTextBox.Location = new System.Drawing.Point(12, 27);
            this.SearchTextBox.Name = "SearchTextBox";
            this.SearchTextBox.PlaceholderText = "Search for a song";
            this.SearchTextBox.Size = new System.Drawing.Size(519, 23);
            this.SearchTextBox.TabIndex = 1;
            // 
            // SearchButton
            // 
            this.SearchButton.Location = new System.Drawing.Point(537, 27);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(75, 23);
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
            this.SearchResultGridView.Location = new System.Drawing.Point(12, 56);
            this.SearchResultGridView.Name = "SearchResultGridView";
            this.SearchResultGridView.ReadOnly = true;
            this.SearchResultGridView.RowHeadersVisible = false;
            this.SearchResultGridView.RowTemplate.Height = 25;
            this.SearchResultGridView.Size = new System.Drawing.Size(600, 373);
            this.SearchResultGridView.TabIndex = 3;
            this.SearchResultGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.SearchResultGridView_CellContentClick);
            // 
            // Menu
            // 
            this.Menu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileMenuItem,
            this.ToolsMenuItem,
            this.HelpMenuItem});
            this.Menu.Location = new System.Drawing.Point(0, 0);
            this.Menu.Name = "Menu";
            this.Menu.Size = new System.Drawing.Size(624, 24);
            this.Menu.TabIndex = 0;
            this.Menu.Text = "menuStrip1";
            // 
            // FileMenuItem
            // 
            this.FileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExitMenuItem});
            this.FileMenuItem.Name = "FileMenuItem";
            this.FileMenuItem.Size = new System.Drawing.Size(37, 20);
            this.FileMenuItem.Text = "&File";
            // 
            // ExitMenuItem
            // 
            this.ExitMenuItem.Name = "ExitMenuItem";
            this.ExitMenuItem.Size = new System.Drawing.Size(93, 22);
            this.ExitMenuItem.Text = "E&xit";
            this.ExitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // ToolsMenuItem
            // 
            this.ToolsMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SendScoreAutomaticallyMenuItem,
            this.ApiKeyMenuItem});
            this.ToolsMenuItem.Name = "ToolsMenuItem";
            this.ToolsMenuItem.Size = new System.Drawing.Size(46, 20);
            this.ToolsMenuItem.Text = "&Tools";
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
            // 
            // ApiKeyMenuItem
            // 
            this.ApiKeyMenuItem.Name = "ApiKeyMenuItem";
            this.ApiKeyMenuItem.Size = new System.Drawing.Size(206, 22);
            this.ApiKeyMenuItem.Text = "&Configure API key...";
            this.ApiKeyMenuItem.Click += new System.EventHandler(this.ApiKeyMenuItem_Click);
            // 
            // HelpMenuItem
            // 
            this.HelpMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AboutMenuItem});
            this.HelpMenuItem.Name = "HelpMenuItem";
            this.HelpMenuItem.Size = new System.Drawing.Size(44, 20);
            this.HelpMenuItem.Text = "&Help";
            // 
            // AboutMenuItem
            // 
            this.AboutMenuItem.Name = "AboutMenuItem";
            this.AboutMenuItem.Size = new System.Drawing.Size(116, 22);
            this.AboutMenuItem.Text = "&About...";
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
            // SongForm
            // 
            this.AcceptButton = this.SearchButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 441);
            this.Controls.Add(this.SearchResultGridView);
            this.Controls.Add(this.SearchButton);
            this.Controls.Add(this.SearchTextBox);
            this.Controls.Add(this.Menu);
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
        private System.Windows.Forms.MenuStrip Menu;
        private System.Windows.Forms.ToolStripMenuItem FileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SendScoreAutomaticallyMenuItem;
        private System.Windows.Forms.ToolStripMenuItem HelpMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AboutMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ApiKeyMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn SongName;
        private System.Windows.Forms.DataGridViewTextBoxColumn SongDifficulties;
        private System.Windows.Forms.DataGridViewTextBoxColumn SongAuthor;
        private System.Windows.Forms.DataGridViewTextBoxColumn SongMapper;
        private System.Windows.Forms.DataGridViewButtonColumn SongDownload;
    }
}