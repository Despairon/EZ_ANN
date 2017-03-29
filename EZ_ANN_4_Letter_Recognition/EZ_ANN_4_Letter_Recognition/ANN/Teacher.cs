using System;
using System.Collections.Generic;

namespace EZ_ANN_4_Letter_Recognition
{
    public enum TeachingMethodType
    {
        BACK_PROPAGATION,
        METHODS_MAX
    }

    public delegate void Teaching(List<Neuron[]> ann_layers, double[] ann_outs, double precision, TeachingSample teaching_sample);

    public class Teacher
    {
        public Teacher(TeachingMethodType teachingMethodType)
        {
            doTeaching = methods[teachingMethodType];
            isTeaching = false;
        }
        private static Methods methods = new Methods();
        private Teaching doTeaching;
        public  bool isTeaching { get; private set; }

        private class Methods
        {
            public Methods()
            {
                methods[(int)TeachingMethodType.BACK_PROPAGATION] = new Teaching(back_propagation);
            }
            private static Teaching[] methods = new Teaching[(int)TeachingMethodType.METHODS_MAX];

            private static void back_propagation(List<Neuron[]> ann_layers, double[] ann_outs, double precision, TeachingSample teaching_sample)
            {
                Func<double, double, double, double, double> backPropagate = (wi, n, s, outP) => wi + (n * s * outP);

                Neuron[] input_layer = ann_layers.Find(layer => layer is InputNeuron[]);
                Neuron[] hidden_layer = ann_layers.Find(layer => !(layer is InputNeuron[]) && !(layer is OutputNeuron[]));
                Neuron[] output_layer = ann_layers.Find(layer => layer is OutputNeuron[]);


            }

            public Teaching this[TeachingMethodType type]
            {
                get
                {
                    return methods[(int)type];
                }
            }
        }

        public void teach(NeuralNetwork ann, double precision, TeachingSample[] teaching_samples)
        {
            isTeaching = true;

            List<Neuron[]> layers = ann.getLayersForTeacher(this);

            foreach (var sample in teaching_samples)
            {
                double[] outputs = ann.recognize(sample.input_values);
                doTeaching(layers, outputs, precision, sample);
                // TODO: we can use outputs from here to get intermediate teaching iteration results
            }

            isTeaching = false;
        }
    }
}
