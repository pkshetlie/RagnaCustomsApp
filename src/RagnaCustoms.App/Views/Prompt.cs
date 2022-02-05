using System.Windows.Forms;

namespace RagnaCustoms.Views
{
    public static class Prompt
    {
        public static string ShowDialog(string text, string caption, string input)
        {
            var prompt = new Form
            {
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Height = 135,
                MaximizeBox = false,
                MinimizeBox = false,
                StartPosition = FormStartPosition.CenterScreen,
                Text = caption,
                Width = 280
            };

            var textLabel = new Label { Left = 12, Top = 12, Width = 240, Text = text };
            var textBox = new TextBox { Left = 12, Top = 32, Width = 240, Text = input };
            var confirmation = new Button
                { Text = "OK", Left = 11, Width = 80, Top = 62, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => prompt.Close();

            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : input;
        }
    }
}