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

                Neuron[] input_layer = ann_layers.Find(layer => layer is InputNeuron[]);
                Neuron[] hidden_layer = ann_layers.Find(layer => !(layer is InputNeuron[]) && !(layer is OutputNeuron[]));
                Neuron[] output_layer = ann_layers.Find(layer => layer is OutputNeuron[]);

                for (int i = 0; i < 3000; i++)
                {
                    double[] deltas_output = new double[output_layer.Length];

                    foreach (var neuron in output_layer)
                    {
                        double output = neuron.getAxonForTeacher(teacher).value;
                        double target = teaching_sample.desired_outputs[Array.IndexOf(output_layer, neuron)];
                        deltas_output[Array.IndexOf(output_layer, neuron)] = output * (1 - output) * (target - output);
                    }

                    double[] new_weights_output = new double[output_layer.Length * hidden_layer.Length];
                    int ind = 0;

                    foreach (var neuron in output_layer)
                        foreach (var synapse in neuron.getSynapsesForTeacher(teacher))
                        {
                            double last_weight = synapse.getWeightAsTeacher(teacher);
                            double new_weight = last_weight + (precision * deltas_output[Array.IndexOf(output_layer, neuron)] * synapse.axon.value);
                            new_weights_output[ind] = new_weight;
                            ind++;
                        }

                    double[] deltas_hidden = new double[hidden_layer.Length];

                    foreach (var neuron in hidden_layer)
                    {
                        double output = neuron.getAxonForTeacher(teacher).value;
                        double sum = 0;

                        foreach (var out_neuron in output_layer)
                        {
                            double last_error = out_neuron.getSynapsesForTeacher(teacher).Find(syn => syn.axon == neuron.getAxonForTeacher(teacher)).getWeightAsTeacher(teacher);
                            sum += deltas_output[Array.IndexOf(output_layer, out_neuron)] * last_error;
                        }

                        deltas_hidden[Array.IndexOf(hidden_layer, neuron)] = output * (1 - output) * sum;
                    }

                    foreach (var neuron in hidden_layer)
                        foreach (var synapse in neuron.getSynapsesForTeacher(teacher))
                        {
                            double last_weight = synapse.getWeightAsTeacher(teacher);
                            double new_weight = last_weight + (precision * deltas_hidden[Array.IndexOf(hidden_layer, neuron)] * synapse.axon.value);
                            synapse.recalculateWeightAsTeacher(teacher, new_weight);
                        }

                    ind = 0;

                    foreach (var neuron in output_layer)
                        foreach (var synapse in neuron.getSynapsesForTeacher(teacher))
                        {
                            synapse.recalculateWeightAsTeacher(teacher, new_weights_output[ind]);
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
