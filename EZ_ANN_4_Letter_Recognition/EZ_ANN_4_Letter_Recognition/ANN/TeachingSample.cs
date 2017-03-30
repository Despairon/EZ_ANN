using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public bool generateTeachingSampleFromBitmap(Bitmap bitmap)
        {
            return true; // TODO: implement
        }

        public bool loadTeachingSampleFromFile()
        {

            // TODO: REMAKE!!! THIS IS KOSTYL' (hardcode)
            for (int i=0; i < input_values.Length; i++)
            {
                Random rnd = new Random(DateTime.Now.Millisecond);
                input_values[i] = rnd.NextDouble();
                desired_outputs[i] = input_values[i];
            }
            return true; 
        }
    }
}
