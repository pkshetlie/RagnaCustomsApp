using System.Windows.Forms;

using System;
using System.Drawing;
using System.Globalization;
using System.Runtime.InteropServices;
using RagnaCustoms.App.Properties;

namespace RagnaCustoms.Views
{
    public static class Prompt
    {
        private static readonly Color WindowColor = Color.FromArgb(10, 22, 32);
        private static readonly Color SurfaceColor = Color.FromArgb(18, 33, 46);
        private static readonly Color InputColor = Color.FromArgb(25, 43, 57);
        private static readonly Color TextColor = Color.FromArgb(238, 246, 250);
        private static readonly Color MutedTextColor = Color.FromArgb(145, 176, 194);
        private static readonly Color AccentColor = Color.FromArgb(47, 171, 218);
        private static readonly Color AccentTextColor = Color.FromArgb(5, 22, 32);

        private const int WmNclButtonDown = 0x00A1;
        private const int HtCaption = 2;

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        public static string ShowDialog(string text, string caption, string input)
        {
            var prompt = new Form
            {
                FormBorderStyle = FormBorderStyle.None,
                Height = 190,
                MaximizeBox = false,
                MinimizeBox = false,
                StartPosition = FormStartPosition.CenterScreen,
                Text = caption,
                Width = 420,
                BackColor = WindowColor
            };

            var titleBar = new Panel { Dock = DockStyle.Top, Height = 46, BackColor = WindowColor, Padding = new Padding(18, 0, 10, 0) };
            var brand = new Panel
            {
                Dock = DockStyle.Left,
                Width = 58,
                BackColor = Color.Transparent
            };
            var logo = new PictureBox
            {
                Size = new Size(36, 28),
                Location = new Point(7, 9),
                Image = Resources.logocompressed,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };
            brand.Controls.Add(logo);
            var title = new Label
            {
                AutoSize = false,
                Dock = DockStyle.Fill,
                Text = caption,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = TextColor,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Padding = new Padding(8, 0, 0, 0)
            };
            var closeButton = new Button
            {
                Dock = DockStyle.Right,
                Width = 42,
                Text = "×",
                FlatStyle = FlatStyle.Flat,
                BackColor = WindowColor,
                ForeColor = MutedTextColor,
                Font = new Font("Segoe UI", 15f),
                TabStop = false,
                Cursor = Cursors.Hand
            };
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(151, 53, 64);
            closeButton.Click += (sender, e) => prompt.Close();
            titleBar.Controls.Add(title);
            titleBar.Controls.Add(closeButton);
            titleBar.Controls.Add(brand);
            titleBar.MouseDown += DragWindow;
            brand.MouseDown += DragWindow;
            logo.MouseDown += DragWindow;
            title.MouseDown += DragWindow;

            var content = new Panel { Dock = DockStyle.Fill, BackColor = SurfaceColor, Padding = new Padding(28, 24, 28, 20) };
            var textLabel = new Label
            {
                AutoSize = true,
                Left = 28,
                Top = 24,
                Text = text,
                ForeColor = TextColor,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 9f, FontStyle.Regular)
            };
            var textBox = new TextBox
            {
                Left = 28,
                Top = 52,
                Width = 364,
                Height = 28,
                Text = input,
                BackColor = InputColor,
                ForeColor = TextColor,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10f)
            };
            var confirmation = new Button
            {
                Text = Resources.ResourceManager.GetString("Prompt.Form.Ok", CultureInfo.CurrentUICulture) ?? "OK",
                Left = 28,
                Width = 100,
                Height = 34,
                Top = 94,
                DialogResult = DialogResult.OK,
                FlatStyle = FlatStyle.Flat,
                BackColor = AccentColor,
                ForeColor = AccentTextColor,
                Font = new Font("Segoe UI", 8.5f, FontStyle.Bold),
                UseVisualStyleBackColor = false,
                Cursor = Cursors.Hand
            };
            confirmation.FlatAppearance.BorderSize = 0;
            confirmation.Click += (sender, e) => prompt.Close();

            content.Controls.Add(textBox);
            content.Controls.Add(confirmation);
            content.Controls.Add(textLabel);
            prompt.Controls.Add(content);
            prompt.Controls.Add(titleBar);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : input;
        }

        private static void DragWindow(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            var form = (sender as Control)?.FindForm();
            if (form == null) return;
            ReleaseCapture();
            SendMessage(form.Handle, WmNclButtonDown, new IntPtr(HtCaption), IntPtr.Zero);
        }
    }
}
