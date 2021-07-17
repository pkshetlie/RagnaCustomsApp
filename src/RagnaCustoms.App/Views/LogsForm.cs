using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace RagnaCustoms.App.Views
{
    public partial class LogsForm : Form
    {
        public LogsForm()
        {
            InitializeComponent();
        }

        public void Logs_Load(object sender, EventArgs e)
        {
            char[] delimiterChars = { ' ', ';', '\\' };
             
           //check if the file exist
           try
            {

                // Read each line of the file into a string array. Each element
                // of the array is one line of the file.
                string[] lines = System.IO.File.ReadAllLines(@".\ragnacustom.log");

                List<string> listFile = new();
                List<string> listSession = new();
                List<string> listUpload = new();

                //separate the 3 types of lines in 3 distincts arrays
                foreach (string line in lines)
                {
                    if (line.Contains("file"))
                    {
                        listFile.Add(line);
                    }
                    if (line.Contains("session"))
                    {
                        listSession.Add(line);
                    }
                    if (line.Contains("Upload"))
                    {
                        listUpload.Add(line);
                    }
                }

                //check if songs have been uploaded (success or error)
                if (listUpload.Count > 0)
                {
                    //recreate the object to display
                    foreach (string lineUpload in listUpload)
                    {
                        Models.Logs logsToDisplay = new();
                        String[] splitedLineUpload = lineUpload.Split(delimiterChars);
                        string hashUpload = splitedLineUpload[7];
                        string dateUpload = splitedLineUpload[0] + " " + splitedLineUpload[1];
                        logsToDisplay.HashLogs = hashUpload;
                        logsToDisplay.ScoreLogs = splitedLineUpload[10];
                        logsToDisplay.DateLogs = dateUpload;
                        logsToDisplay.StatusLogs = splitedLineUpload[3] + " " + splitedLineUpload[4];


                        foreach (string lineSession in listSession)
                        {
                            if (lineSession.Contains(hashUpload))
                            {
                                String[] splitedLineSession = lineSession.Split(delimiterChars);

                                logsToDisplay.DifficultyLogs = splitedLineSession[10];
                                logsToDisplay.ScoreLogs = splitedLineSession[13];
                            }
                        }

                        foreach (string lineFile in listFile)
                        {
                            if (lineFile.Contains(hashUpload))
                            {
                                String[] splitedLineFile = lineFile.Split(delimiterChars);
                                logsToDisplay.SongNameLogs = splitedLineFile[16];
                            }
                        }
                        logsBindingSource.Insert(0, logsToDisplay);
                        noLogsWarning_Click(false, e);
                    }
                } else
                {
                    //if the file exist but does not contain upload lines
                    noLogsWarning_Click(true, e);
                }   

            } catch
            {
                //in case there is no file to read
                noLogsWarning_Click(true,e);
            }
        }

        private void RefreshLogs_Click(object sender, EventArgs e)
        {
            //clear the dgv for refreshing the logs
            this.LogsDataGridView.Rows.Clear();
            Logs_Load(sender, e);
        }

        private void noLogsWarning_Click(object sender, EventArgs e)
        {
            //display a message to the user if the file does not exist
            //or if no upload line is in the file
            if ((bool)(sender))
            {
                NoLogsWarn.Visible = true;
            } else
            {
                NoLogsWarn.Visible = false;
            }
        }
    }
}
