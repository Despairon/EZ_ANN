using System;
using System.Collections.Generic;
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
    }
}
