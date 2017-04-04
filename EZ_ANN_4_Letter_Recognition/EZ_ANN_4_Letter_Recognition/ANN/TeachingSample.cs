using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace EZ_ANN_4_Letter_Recognition
{
    public class TeachingSample
    {
        public TeachingSample(int input_size, int output_size)
        {
            input_values = new double[input_size];
            desired_outputs = new double[output_size];
        }
        public double[] input_values { get; private set; }
        public double[] desired_outputs { get; private set; }

        public bool generateTeachingSampleFromImage(string file)
        {

            Image image = Image.FromFile(file);

            Bitmap bitmap = new Bitmap(image);

            if (bitmap.Height * bitmap.Width != input_values.Length)
                return false;

            double[,] values = new double[bitmap.Width, bitmap.Height];

            for (int x = 0; x < bitmap.Width; x++)
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color bitmapPixelColor = bitmap.GetPixel(x, y);
                    values[x, y] =  (bitmapPixelColor.R == Color.White.R)
                                 && (bitmapPixelColor.G == Color.White.G)
                                 && (bitmapPixelColor.B == Color.White.B) ? 0 : 1;
                }

            try
            {
                Buffer.BlockCopy(values, 0, input_values, 0, values.Length);
                Buffer.BlockCopy(values, 0, desired_outputs, 0, values.Length);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public static TeachingSample[] generateTeachingSamplesFromFile(string file)
        {

            List<TeachingSample> samples = new List<TeachingSample>();

            try
            {
                StreamReader reader = File.OpenText(file);

                string current_str;

                while (!reader.EndOfStream)
                {
                    current_str = reader.ReadLine();
                    string[] values = current_str.Split(':');

                    string input_str  = values[0].Trim();
                    string output_str = values[1].Trim();

                    string[] input_strs  = input_str.Split(',');
                    string[] output_strs = output_str.Split(',');

                    TeachingSample sample = new TeachingSample(input_strs.Length, output_strs.Length);

                    foreach (var input in input_strs)
                        sample.input_values[Array.IndexOf(input_strs, input)] = Convert.ToDouble(input);
                    foreach (var output in output_strs)
                        sample.desired_outputs[Array.IndexOf(output_strs, output)] = Convert.ToDouble(output);

                    samples.Add(sample);
                }
            }
            catch (Exception)
            {
                return null;
            }
            return samples.ToArray();
        }

    }
}
