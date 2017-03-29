using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EZ_ANN_4_Letter_Recognition
{
    public enum TeachingMethodType
    {
        BACK_PROPAGATION,
        METHODS_MAX
    }

    public delegate void Teaching(NeuralNetwork ann);

    public class Teacher
    {
        public Teacher(TeachingMethodType teachingMethodType)
        {
            teachingMethod = methods[teachingMethodType];
        }
        private static Methods methods = new Methods();
        private Teaching teachingMethod;

        private class Methods
        {
            public Methods()
            {
                methods[(int)TeachingMethodType.BACK_PROPAGATION] = new Teaching(back_propagation);
            }
            private static Teaching[] methods = new Teaching[(int)TeachingMethodType.METHODS_MAX];
            
            private static void back_propagation(NeuralNetwork ann)
            {
                // TODO: implement back propagation here somehow!!!
            }

            public Teaching this[TeachingMethodType type]
            {
                get
                {
                    return methods[(int)type];
                }
            }
        }

        public void teach(NeuralNetwork ann, double precision, double[] teaching_sample)
        {
            double[] outputs = ann.recognize(teaching_sample);
            while (Array.Exists(outputs, num => num - teaching_sample[Array.IndexOf(outputs, num)] >= precision))
            {
                teachingMethod(ann);
                outputs = ann.recognize(teaching_sample);    
                // TODO: we can use outputs from here to get intermediate teaching iteration results      
            }
        }
    }
}
