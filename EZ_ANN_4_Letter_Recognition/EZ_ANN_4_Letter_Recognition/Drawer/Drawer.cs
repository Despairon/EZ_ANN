using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace EZ_ANN_4_Letter_Recognition
{

    public class Drawer
    {
        public Drawer(PictureBox pBox)
        {
            this.pBox = pBox;

            graphics = Graphics.FromImage(pBox.Image);

            pen = new Pen(Color.Black, 5);
            pen.StartCap = LineCap.Round;
            pen.EndCap = LineCap.Round;

            isDrawing = false;

            bitmap = new Bitmap(pBox.Size.Width, pBox.Size.Height, graphics);

            pBox.MouseDown   += new MouseEventHandler(startDrawing);
            pBox.MouseUp     += new MouseEventHandler(stopDrawing);
            pBox.MouseMove   += new MouseEventHandler(onDraw);
        }
        private PictureBox pBox;
        private Graphics graphics;
        private Pen pen;
        private Point lastPoint;
        private bool isDrawing;
        public Bitmap bitmap { get; private set; }

        private void onDraw(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                graphics.DrawLine(pen, lastPoint, e.Location);
                lastPoint = e.Location;
                render();
            }
        }

        private void render()
        {
            Rectangle rect = new Rectangle(0, 0, pBox.Width, pBox.Height);
            pBox.DrawToBitmap(bitmap, rect);
            pBox.Invalidate();
        }

        private void startDrawing(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                lastPoint.X = e.X;
                lastPoint.Y = e.Y;
                isDrawing = true;
            }
        }

        private void stopDrawing(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                isDrawing = false;
        }

        public void clear()
        {
            graphics.Clear(Color.White);
            render();
        }

    }

}
