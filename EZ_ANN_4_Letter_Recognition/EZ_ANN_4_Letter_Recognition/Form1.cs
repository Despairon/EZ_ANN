using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EZ_ANN_4_Letter_Recognition
{
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

            NeuralNetwork ann = new NeuralNetwork(1024, 1024, 1024);
            Teacher teacher = new Teacher(TeachingMethodType.BACK_PROPAGATION);

            TeachingSample s = new TeachingSample(1024, 1024);
            s.loadTeachingSampleFromFile();

            teacher.teach(ann, 0.3, new TeachingSample[1] { s }); //FIXME fixme!!!

            if (ann.isBroken)
                MessageBox.Show("Sorry, ANN is broken :(");

            /* this is for debug purposes */

            Bitmap bitmap = new Bitmap(pbLetterImage.Image);
            double[,] values = new double[bitmap.Width , bitmap.Height];
            for (int x = 0; x < bitmap.Width; x++)
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color bitmapPixelColor = bitmap.GetPixel(x, y);
                    values[x, y] =  (bitmapPixelColor.R == Color.White.R) 
                                 && (bitmapPixelColor.G == Color.White.G) 
                                 && (bitmapPixelColor.B == Color.White.B) ? 0 : 1;
                }
            double[] true_values = new double[values.Length];

            Buffer.BlockCopy(values, 0, true_values, 0, values.Length);

            double[] output = ann.recognize(true_values);

            values = new double[bitmap.Width, bitmap.Height];

            int xx = 0;
            int yy = 0;
            foreach (var val in output)
            {
                values[xx, yy] = val;
                yy++;
                if (yy == 32)
                {
                    xx++;
                    yy = 0;
                }
            }

            Color c;
            for (int x = 0; x < bitmap.Width; x++)
                for (int y = 0; y < bitmap.Height; y++)
                {
                    c = values[x, y] == 1 ? Color.Black : Color.White;
                    bitmap.SetPixel(x, y, c);
                }

            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.ShowDialog();
            try
            {
                bitmap.Save(saveDlg.FileName);
            }
            catch (Exception) { }
            /* end of debug purposes */
        }

        private ImageProcessor imageProcessor;

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}