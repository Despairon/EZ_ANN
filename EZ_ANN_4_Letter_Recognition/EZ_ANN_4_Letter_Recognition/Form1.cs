using System;
using System.Drawing;
using System.Text;
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

            //OpenFileDialog d = new OpenFileDialog();
            //d.ShowDialog();

            //TeachingSample[] samples = TeachingSample.generateTeachingSamplesFromFile(d.FileName);

            //if (samples == null)
            //{
            //    MessageBox.Show("Error reading sample from file...");
            //    return;
            //}

            //MessageBox.Show("please wait, neural network is creating...");

            //NeuralNetwork ann = new NeuralNetwork(2, 5, 1);
            //Teacher teacher = new Teacher(TeachingMethodType.BACK_PROPAGATION);

            //teacher.teach(ann, 1, samples, 3000);

            //string[] result = new string[4];

            //double[] outputs = ann.recognize(new double[] { 0, 0 } );

            //result[0] = "0 | 0 = 1 on " + string.Format("{0:#.###}",outputs[0]*100) + "%\n";

            //outputs = ann.recognize(new double[] { 0, 1 });

            //result[1] = "0 | 1 = 1 on " + string.Format("{0:#.###}", outputs[0]*100) + "%\n";

            //outputs = ann.recognize(new double[] { 1, 0 });

            //result[2] = "1 | 0 = 1 on " + string.Format("{0:#.###}", outputs[0] * 100) + "%\n";

            //outputs = ann.recognize(new double[] { 1, 1 });

            //result[3] = "1 | 1 = 1 on " + string.Format("{0:#.###}", outputs[0] * 100) + "%\n";

            //MessageBox.Show("ANN thinks of: \n" + result[0] + result[1] + result[2] + result[3]);

            NeuralNetwork ann = new NeuralNetwork(1024, 1024, 1024);

            if (ann.isBroken)
            {
                MessageBox.Show("Sorry, ANN is broken :(");
                return;
            }

            Teacher teacher = new Teacher(TeachingMethodType.BACK_PROPAGATION);

            TeachingSample[] samples = null;

            MessageBox.Show("Ann created. Please, choose images to generate teaching samples");

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Multiselect = true;
            dlg.ShowDialog();
            if (dlg.FileNames.Length > 0)
            {
                samples = new TeachingSample[dlg.FileNames.Length];
                for (int i = 0; i < samples.Length; i++)
                {
                    samples[i] = new TeachingSample(1024, 1024);
                    if (!samples[i].generateTeachingSampleFromImage(dlg.FileNames[i]))
                    {
                        MessageBox.Show("one of the files selected is bad");
                        return;
                    }
                }

            }
            else
            {
                MessageBox.Show("No files for teaching sample selected!");
                return;
            }

            if (samples != null)
                teacher.teach(ann, 1, samples, 1);
            else
                return;

            /* this is for debug purposes */

            Bitmap bitmap = new Bitmap(pbLetterImage.Image);
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

        private void recognizeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}