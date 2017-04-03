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
                    values[x, y] = (bitmapPixelColor.R == Color.White.R)
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

        public bool generateTeachingSampleFromFile(string file)
        {
            try
            {
                StreamReader reader = File.OpenText(file);

                string current_str;

                int ind = 0;
                while (!reader.EndOfStream)
                {
                    current_str = reader.ReadLine();
                    current_str = current_str.Replace(',','.');
                    string[] values = current_str.Split(':');

                    if (values.Length > 1)
                    {
                        string input_values_str = values[0];
                        string desired_output_str = values[1];

                        input_values[ind] = Convert.ToDouble(input_values_str);
                        desired_outputs[ind] = Convert.ToDouble(desired_output_str);
                    }
                    else
                    {
                        string desired_output_str = values[1];
                        desired_outputs[ind] = Convert.ToDouble(desired_output_str);
                    }
                    ind++;
                }
                if (ind != input_values.Length)
                    throw new Exception();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

    }
}
