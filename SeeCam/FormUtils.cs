using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeeCam
{
    public static class FormUtils
    {
        public static Rectangle GetClientBounds(Form form)
        {
            var tl = form.PointToScreen(new Point(0, 0));
            var br = form.PointToScreen(new Point(form.ClientSize));
            return new Rectangle(tl.X, tl.Y, br.X - tl.X, br.Y - tl.Y);
        }

        public static Rectangle Add(this Rectangle rect, RectDiff diff)
        {
            return new Rectangle
            {
                X = rect.X + diff.Left,
                Y = rect.Y + diff.Top,
                Width = rect.Width + diff.Right - diff.Left,
                Height = rect.Height + diff.Bottom - diff.Top
            };
        }

        public static RectDiff Sub(this Rectangle rect1, Rectangle rect2)
        {
            return new RectDiff
            {
                Top = rect1.Top - rect2.Top,
                Bottom = rect1.Bottom - rect2.Bottom,
                Left = rect1.Left - rect2.Left,
                Right = rect1.Right - rect2.Right
            };
        }
    }

    public struct RectDiff
    {
        public int Top { get; set; }
        public int Bottom { get; set; }
        public int Left { get; set; }
        public int Right { get; set; }
    }
}
