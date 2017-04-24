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

            img = pBox.Image.GetThumbnailImage(pBox.Width, pBox.Height, new Image.GetThumbnailImageAbort(abortImage), System.IntPtr.Zero);

            pBox.Image = img;

            graphics = Graphics.FromImage(img);

            pen          = new Pen(Color.Black, 10);
            pen.StartCap = LineCap.Round;
            pen.EndCap   = LineCap.Round;

            isDrawing = false;

            bitmap = new Bitmap(img.GetThumbnailImage(16,16, new Image.GetThumbnailImageAbort(abortImage), System.IntPtr.Zero));
            
            pBox.DrawToBitmap(bitmap, new Rectangle(0,0,bitmap.Width,bitmap.Height));

            pBox.MouseDown   += new MouseEventHandler(startDrawing);
            pBox.MouseUp     += new MouseEventHandler(stopDrawing);
            pBox.MouseMove   += new MouseEventHandler(onDraw);
        }
        private PictureBox pBox;
        private Graphics   graphics;
        private Image      img;
        private Pen        pen;
        private Point      lastPoint;
        private bool       isDrawing;
        public  Bitmap     bitmap { get; private set; }

        private bool abortImage()
        {
            return false;
        }

        private void onDraw(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        pen.Color = Color.Black;
                        break;
                    case MouseButtons.Right:
                        pen.Color = Color.White;
                        break;
                    default: break;
                }
                graphics.DrawLine(pen, lastPoint, e.Location);
                lastPoint = e.Location;
                render();
            }
        }

        private void render()
        {
            pBox.Image = img;
            bitmap = new Bitmap(img.GetThumbnailImage(16, 16, new Image.GetThumbnailImageAbort(abortImage), System.IntPtr.Zero));
            pBox.Invalidate();
        }

        private void startDrawing(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                lastPoint.X = e.X;
                lastPoint.Y = e.Y;
                isDrawing = true;
            }
        }

        private void stopDrawing(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
                isDrawing = false;
        }

        public void clear()
        {
            graphics.Clear(Color.White);
            render();
        }

    }

}
