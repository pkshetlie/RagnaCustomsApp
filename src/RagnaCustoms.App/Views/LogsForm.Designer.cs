
using System.Windows.Forms;

namespace RagnaCustoms.App.Views
{
    partial class LogsForm
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
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogsForm));
            this.LogsDataGridView = new System.Windows.Forms.DataGridView();
            this.dateLogsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.songNameLogsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.difficultyLogsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.scoreLogsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusLogsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.hashLogsDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.logsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.button1 = new System.Windows.Forms.Button();
            this.NoLogsWarn = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.LogsDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.logsBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // LogsDataGridView
            // 
            this.LogsDataGridView.AllowUserToAddRows = false;
            this.LogsDataGridView.AllowUserToDeleteRows = false;
            this.LogsDataGridView.AllowUserToOrderColumns = true;
            this.LogsDataGridView.AutoGenerateColumns = false;
            this.LogsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.LogsDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.LogsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.LogsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dateLogsDataGridViewTextBoxColumn,
            this.songNameLogsDataGridViewTextBoxColumn,
            this.difficultyLogsDataGridViewTextBoxColumn,
            this.scoreLogsDataGridViewTextBoxColumn,
            this.statusLogsDataGridViewTextBoxColumn,
            this.hashLogsDataGridViewTextBoxColumn});
            this.LogsDataGridView.DataSource = this.logsBindingSource;
            this.LogsDataGridView.Location = new System.Drawing.Point(12, 38);
            this.LogsDataGridView.Name = "LogsDataGridView";
            this.LogsDataGridView.ReadOnly = true;
            this.LogsDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.LogsDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.LogsDataGridView.Size = new System.Drawing.Size(744, 406);
            this.LogsDataGridView.TabIndex = 0;
            // 
            // dateLogsDataGridViewTextBoxColumn
            // 
            this.dateLogsDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dateLogsDataGridViewTextBoxColumn.DataPropertyName = "DateLogs";
            this.dateLogsDataGridViewTextBoxColumn.HeaderText = "Date";
            this.dateLogsDataGridViewTextBoxColumn.Name = "dateLogsDataGridViewTextBoxColumn";
            this.dateLogsDataGridViewTextBoxColumn.ReadOnly = true;
            this.dateLogsDataGridViewTextBoxColumn.Width = 55;
            // 
            // songNameLogsDataGridViewTextBoxColumn
            // 
            this.songNameLogsDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.songNameLogsDataGridViewTextBoxColumn.DataPropertyName = "SongNameLogs";
            this.songNameLogsDataGridViewTextBoxColumn.HeaderText = "Song Name";
            this.songNameLogsDataGridViewTextBoxColumn.Name = "songNameLogsDataGridViewTextBoxColumn";
            this.songNameLogsDataGridViewTextBoxColumn.ReadOnly = true;
            this.songNameLogsDataGridViewTextBoxColumn.Width = 88;
            // 
            // difficultyLogsDataGridViewTextBoxColumn
            // 
            this.difficultyLogsDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.difficultyLogsDataGridViewTextBoxColumn.DataPropertyName = "DifficultyLogs";
            this.difficultyLogsDataGridViewTextBoxColumn.HeaderText = "Difficulty";
            this.difficultyLogsDataGridViewTextBoxColumn.Name = "difficultyLogsDataGridViewTextBoxColumn";
            this.difficultyLogsDataGridViewTextBoxColumn.ReadOnly = true;
            this.difficultyLogsDataGridViewTextBoxColumn.Width = 72;
            // 
            // scoreLogsDataGridViewTextBoxColumn
            // 
            this.scoreLogsDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.scoreLogsDataGridViewTextBoxColumn.DataPropertyName = "ScoreLogs";
            this.scoreLogsDataGridViewTextBoxColumn.HeaderText = "Score";
            this.scoreLogsDataGridViewTextBoxColumn.Name = "scoreLogsDataGridViewTextBoxColumn";
            this.scoreLogsDataGridViewTextBoxColumn.ReadOnly = true;
            this.scoreLogsDataGridViewTextBoxColumn.Width = 60;
            // 
            // statusLogsDataGridViewTextBoxColumn
            // 
            this.statusLogsDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.statusLogsDataGridViewTextBoxColumn.DataPropertyName = "StatusLogs";
            this.statusLogsDataGridViewTextBoxColumn.HeaderText = "Status";
            this.statusLogsDataGridViewTextBoxColumn.Name = "statusLogsDataGridViewTextBoxColumn";
            this.statusLogsDataGridViewTextBoxColumn.ReadOnly = true;
            this.statusLogsDataGridViewTextBoxColumn.Width = 62;
            // 
            // hashLogsDataGridViewTextBoxColumn
            // 
            this.hashLogsDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.hashLogsDataGridViewTextBoxColumn.DataPropertyName = "HashLogs";
            this.hashLogsDataGridViewTextBoxColumn.HeaderText = "Hash";
            this.hashLogsDataGridViewTextBoxColumn.Name = "hashLogsDataGridViewTextBoxColumn";
            this.hashLogsDataGridViewTextBoxColumn.ReadOnly = true;
            this.hashLogsDataGridViewTextBoxColumn.Width = 57;
            // 
            // logsBindingSource
            // 
            this.logsBindingSource.DataSource = typeof(RagnaCustoms.Models.Logs);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(768, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.button1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.button1.Location = new System.Drawing.Point(12, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Refresh";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.RefreshLogs_Click);
            // 
            // NoLogsWarn
            // 
            this.NoLogsWarn.AutoSize = true;
            this.NoLogsWarn.BackColor = System.Drawing.Color.Transparent;
            this.NoLogsWarn.ForeColor = System.Drawing.Color.Red;
            this.NoLogsWarn.Location = new System.Drawing.Point(93, 14);
            this.NoLogsWarn.Name = "NoLogsWarn";
            this.NoLogsWarn.Size = new System.Drawing.Size(90, 13);
            this.NoLogsWarn.TabIndex = 4;
            this.NoLogsWarn.Text = "No logs to display";
            this.NoLogsWarn.Visible = false;
            this.NoLogsWarn.Click += new System.EventHandler(this.noLogsWarning_Click);
            // 
            // LogsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(768, 456);
            this.Controls.Add(this.NoLogsWarn);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.LogsDataGridView);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LogsForm";
            this.Text = "Logs";
            this.Load += new System.EventHandler(this.Logs_Load);
            ((System.ComponentModel.ISupportInitialize)(this.LogsDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.logsBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.BindingSource logsBindingSource;
        private System.Windows.Forms.DataGridView LogsDataGridView;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dateLogsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn songNameLogsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn difficultyLogsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn scoreLogsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn statusLogsDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn hashLogsDataGridViewTextBoxColumn;
        private System.Windows.Forms.Label NoLogsWarn;
    }
}