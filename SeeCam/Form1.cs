using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using AForge.Video;

namespace SeeCam
{
    public partial class Form1 : Form
    {
        private RectDiff diffOnStrip;
        private CameraManager cameras;

        private void UpdateCamera(int index)
        {
            var name = cameras.SelectCamera(index);
            if (name != null)
            {
                Text = "SeeCam : " + name;
            }
        }

        private void UpdateImage(Bitmap img)
        {
            var hvSrc = (double)img.Width / (double)img.Height;
            var hvDst = (double)ClientSize.Width / (double)ClientSize.Height;
            var newWidth = 0;
            var newHeight = 0;
            var newX = 0;
            var newY = 0;
            if (hvDst > hvSrc)
            {
                // destination is wider, so resize with height
                newHeight = ClientSize.Height;
                newWidth = (int)((double)newHeight * hvSrc);
                newY = 0;
                newX = (ClientSize.Width - newWidth) / 2;
            }
            else
            {
                // source is wider, so resize with width
                newWidth = ClientSize.Width;
                newHeight = (int)((double)newWidth / hvSrc);
                newX = 0;
                newY = (ClientSize.Height - newHeight) / 2;
            }

            var frame = new Bitmap(ClientSize.Width, ClientSize.Height);
            var g = Graphics.FromImage(frame);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(img, newX, newY, newWidth, newHeight);
            g.Dispose();
            pictureBox1.Image = frame;
        }

        private void OnNewFrame(object sender, NewFrameEventArgs e)
        {
            if (InvokeRequired)
            {
                var action = new Action<Bitmap>(UpdateImage);
                try
                {
                    Invoke(action, e.Frame);
                }
                catch(ObjectDisposedException)
                {
                    // nop
                }
                return;
            }
            UpdateImage(e.Frame);
        }

        private void OnCameraClick(object sender, EventArgs e)
        {
            var menu = sender as ToolStripMenuItem;
            if (menu == null) return;
            if (menu.Tag == null) return;
            if (menu.Checked) return;
            var index = (int)menu.Tag;
            UpdateCamera(index);
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void contextMenuShowTitleBar_Click(object sender, EventArgs e)
        {
            if (contextMenuShowTitleBar.Checked)
            {
                // show titlebar, border...
                var clientBorder = FormUtils.GetClientBounds(this);
                FormBorderStyle = FormBorderStyle.Sizable;
                Bounds = clientBorder.Add(diffOnStrip);
            }
            else
            {
                // hide titlebar, border...
                var clientBorder = FormUtils.GetClientBounds(this);
                diffOnStrip = Bounds.Sub(clientBorder);
                FormBorderStyle = FormBorderStyle.None;
                Bounds = clientBorder;
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            var count = contextMenuStrip1.Items.Count;
            for (var i = count - 1; i > 1; i--)
            {
                var menu = contextMenuStrip1.Items[i];
                menu.Click -= OnCameraClick;
                contextMenuStrip1.Items.RemoveAt(i);
            }

            foreach (var camInfo in cameras.EnumerateCameras())
            {
                var menu = new ToolStripMenuItem(camInfo.Name);
                menu.Click += OnCameraClick;
                menu.Tag = camInfo.Index;
                menu.Checked = camInfo.IsUsing;
                contextMenuStrip1.Items.Add(menu);
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            cameras = new CameraManager();
            cameras.NewFrame += OnNewFrame;
            var count = cameras.CountCameras();
            if (count < 1) return;
            UpdateCamera(count - 1);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            cameras.StopCamera();
            cameras.NewFrame -= OnNewFrame;
        }
    }
}
