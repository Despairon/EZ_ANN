using System;
using System.Collections.Generic;
using Activation_funcs;
using static EZ_ANN_4_Letter_Recognition.Neuron;

namespace EZ_ANN_4_Letter_Recognition
{
    public enum TeachingMethodType
    {
        BACK_PROPAGATION,
        METHODS_MAX
    }

    public delegate void Teaching(List<Neuron[]> ann_layers, double learning_rate, TeachingSample teaching_sample, Teacher teacher);

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

            private static void back_propagation(List<Neuron[]> ann_layers, double precision, TeachingSample teaching_sample, Teacher teacher)
            {
                Func<double, double, double, double, double> backPropagate = (wi, n, d, outP) => wi - (n * d * outP);

                Func<ActivationFunctionType, Axon, double> derivative = (f_type, axon) =>
                {
                    switch (f_type)
                    {
                        case ActivationFunctionType.HYPERBOLIC_TANGENT:
                            return (1 + axon.value) * (1 - axon.value);
                        case ActivationFunctionType.LOGISTIC:
                            return axon.value * (1 - axon.value);
                        default:
                            return axon.value;
                    }
                };
                
                Neuron[] input_layer = ann_layers.Find(layer => layer is InputNeuron[]);
                Neuron[] hidden_layer = ann_layers.Find(layer => !(layer is InputNeuron[]) && !(layer is OutputNeuron[]));
                Neuron[] output_layer = ann_layers.Find(layer => layer is OutputNeuron[]);

                double[] d_out = new double[output_layer.Length];

                double[] recalculated_weight_for_output_layer = new double[hidden_layer.Length*output_layer.Length];

                for (int it = 0; it < 100; it++)
                {
                
                int ind = 0;

                foreach (var neuron in output_layer)
                {
                    int    index = Array.IndexOf(output_layer, neuron);
                    double Eout  = (-1.0f) * (teaching_sample.desired_outputs[index] - neuron.getAxonForTeacher(teacher).value);
                    double d     = Eout * derivative(neuron.getActivationFuncType(), neuron.getAxonForTeacher(teacher));

                    d_out[index] = d;

                    foreach (var synapse in neuron.getSynapsesForTeacher(teacher))
                    {
                        double recalculatedWeight = backPropagate(synapse.getWeightAsTeacher(teacher), precision, d, synapse.axon.value);
                        recalculated_weight_for_output_layer[ind] = recalculatedWeight;
                        ind++;
                    }
                }

                foreach (var neuron in hidden_layer)
                {
                    double d = 0;

                    foreach (var next_neuron in output_layer)
                    {
                        double d_o  = d_out[Array.IndexOf(output_layer, next_neuron)];
                        double w_ho = next_neuron.getSynapsesForTeacher(teacher).Find(syn => syn.axon == neuron.getAxonForTeacher(teacher)).getWeightAsTeacher(teacher); 
                        d += d_o * w_ho;
                    }

                    d *= derivative(neuron.getActivationFuncType(), neuron.getAxonForTeacher(teacher));

                    foreach (var synapse in neuron.getSynapsesForTeacher(teacher))
                    {
                        double recalculatedWeight = backPropagate(synapse.getWeightAsTeacher(teacher), precision, d, synapse.axon.value);
                        synapse.recalculateWeightAsTeacher(teacher, recalculatedWeight);
                    }
                }

                ind = 0;
                foreach (var neuron in output_layer)
                    foreach (var synapse in neuron.getSynapsesForTeacher(teacher))
                    {
                        synapse.recalculateWeightAsTeacher(teacher, recalculated_weight_for_output_layer[ind]);
                        ind++;
                    }
                }
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
                ann.recognize(sample.input_values);
                doTeaching(layers, precision, sample, this);
                // TODO: we can use outputs from here to get intermediate teaching iteration results
            }

            isTeaching = false;
        }
    }
}
