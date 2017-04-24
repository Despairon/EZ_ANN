using System;
using System.Collections.Generic;

namespace EZ_ANN_4_Letter_Recognition
{
    public enum TeachingMethodType
    {
        BACK_PROPAGATION,
    }

    public delegate void Teaching(List<Neuron[]> ann_layers, double learning_rate, TeachingSample teaching_sample, Teacher teacher);

    [Serializable]
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

        [Serializable]
        private class Methods
        {
            public Methods()
            {
                methods[(int)TeachingMethodType.BACK_PROPAGATION] = new Teaching(back_propagation);
            }
            private static Teaching[] methods = new Teaching[Enum.GetValues(typeof(TeachingMethodType)).Length];

            private static void back_propagation(List<Neuron[]> ann_layers, double precision, TeachingSample teaching_sample, Teacher teacher)
            {

                Neuron[] input_layer = ann_layers.Find(layer => layer is InputNeuron[]);
                Neuron[] hidden_layer = ann_layers.Find(layer => !(layer is InputNeuron[]) && !(layer is OutputNeuron[]));
                Neuron[] output_layer = ann_layers.Find(layer => layer is OutputNeuron[]);

                double[] deltas_output = new double[output_layer.Length];
                double[] deltas_hidden = new double[hidden_layer.Length];

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

            public Teaching this[TeachingMethodType type]
            {
                get
                {
                    return methods[(int)type];
                }
            }
        }

        public string teach(NeuralNetwork ann, double precision, TeachingSample[] teaching_samples, int iterations)
        {
            string teachResult = "";

            DateTime startTime = DateTime.Now;

            isTeaching = true;

            List<Neuron[]> layers = ann.getLayersForTeacher(this);

            try
            {
                for (int i = 0; i < iterations; i++)
                {
                    foreach (var sample in teaching_samples)
                    {
                        ann.recognize(sample.input_values);
                        doTeaching(layers, precision, sample, this);
                        // TODO: we can use outputs from here to get intermediate teaching iteration results
                    }
                }
            }
            catch (Exception)
            {
                return "error";
            }

            isTeaching = false;

            DateTime endTime = DateTime.Now;

            TimeSpan diffTime = endTime - startTime;

            int totalSeconds = diffTime.Seconds + (diffTime.Minutes * 60) + (diffTime.Hours * 3600);
            int averageSecondsOnIteration = totalSeconds / iterations;
            int iteration_hours = averageSecondsOnIteration / 3600;
            int iteration_minutes = averageSecondsOnIteration / 60;
            int iteration_seconds = averageSecondsOnIteration;

            teachResult = "Total teaching time: "    + string.Format("{0:00}:{1:00}:{2:00}\n", diffTime.Hours, diffTime.Minutes, diffTime.Seconds)
                        + "Average iteration time: " + string.Format("{0:00}:{1:00}:{2:00}", iteration_hours, iteration_minutes, iteration_seconds);

            return teachResult;
        }

        public TeachingMethodType getTeachingMethod()
        {
            foreach (TeachingMethodType method in Enum.GetValues(typeof(TeachingMethodType)))
            {
                if (methods[method] == doTeaching)
                    return method;
            }

            return 0;
        }
    }
}
