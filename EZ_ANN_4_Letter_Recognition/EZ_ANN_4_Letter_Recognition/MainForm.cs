using System;
using System.Drawing;
using System.Windows.Forms;

namespace EZ_ANN_4_Letter_Recognition
{
    [Serializable]
    public struct Range
    {
        public Range(double min, double max)
        {
            this.min = min;
            this.max = max;
        }

        public double min;
        public double max;
    }

    public partial class mainForm : Form
    {
        public mainForm()
        {
            InitializeComponent();
            imageProcessor = new ImageProcessor(loadToolStripMenuItem,
                                                saveToolStripMenuItem,
                                                clearToolStripMenuItem,
                                                pbLetterImage);
            ann_manager = new ANN_Manager();
        }
        private ImageProcessor imageProcessor;
        private ANN_Manager    ann_manager;

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void recognizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ann_manager.getSelectedANNName() != "None")
            {
                /* this is for debug purposes */

                Bitmap bitmap = new Bitmap(pbLetterImage.Image.GetThumbnailImage(16, 16, null, System.IntPtr.Zero));
                double[,] values = new double[bitmap.Width, bitmap.Height];
                for (int x = 0; x < bitmap.Width; x++)
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        Color bitmapPixelColor = bitmap.GetPixel(x, y);
                        values[x, y] = (bitmapPixelColor.R == Color.White.R)
                                     && (bitmapPixelColor.G == Color.White.G)
                                     && (bitmapPixelColor.B == Color.White.B) ? 0 : 1;
                    }
                double[] true_values = new double[values.Length];

                Buffer.BlockCopy(values, 0, true_values, 0, values.Length);

                double[] output = ann_manager.useANN(true_values);

                values = new double[bitmap.Width, bitmap.Height];

                int xx = 0;
                int yy = 0;
                foreach (var val in output)
                {
                    values[xx, yy] = val;
                    yy++;
                    if (yy == bitmap.Width)
                    {
                        xx++;
                        yy = 0;
                    }
                }
                Color c;
                for (int x = 0; x < bitmap.Width; x++)
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        if (values[x, y] >= 0.5 && values[x, y] <= 0.6)
                            c = Color.LightGray;
                        else if (values[x, y] >= 0.6 && values[x, y] <= 0.7)
                            c = Color.SlateGray;
                        else if (values[x, y] >= 0.7 && values[x, y] <= 0.8)
                            c = Color.DarkSlateGray;
                        else if (values[x, y] >= 0.8 && values[x, y] <= 0.9)
                            c = Color.Gray;
                        else if (values[x, y] >= 0.9 && values[x, y] < 1)
                            c = Color.DarkGray;
                        else if (values[x, y] == 1)
                            c = Color.Black;
                        else
                            c = Color.White;
                        bitmap.SetPixel(x, y, c);
                    }

                MessageBox.Show("select where to save recreated image");

                SaveFileDialog saveDlg = new SaveFileDialog();
                saveDlg.ShowDialog();
                try
                {
                    bitmap.Save(saveDlg.FileName);
                }
                catch (Exception) { }
                /* end of debug purposes */
            }
            else
                MessageBox.Show("Select ANN from ANN manager first!");
        }

        private void teachToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ann_manager.getSelectedANNName() != "None")
            {
                new TeachForm(ann_manager).ShowDialog();
            }
            else
                MessageBox.Show("Select ANN from ANN manager first!");
        }

        private void managerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ANNManagerDialog(ann_manager).ShowDialog();

            toolStripANNName.Text = ann_manager.getSelectedANNName();
        }
    }
}