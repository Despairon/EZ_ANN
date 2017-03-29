using System;
using System.Collections.Generic;

namespace EZ_ANN_4_Letter_Recognition
{
    public class NeuralNetwork
    {
        public NeuralNetwork(int input_layer_neurons_count, int hidden_layer_neurons_count, int output_layer_neurons_count)
        {
            isBroken  = false;

            try
            {

                input_layer  = new InputNeuron[input_layer_neurons_count];
                hidden_layer = new Neuron[hidden_layer_neurons_count];
                output_layer = new OutputNeuron[output_layer_neurons_count];

                for (int i = 0; i < input_layer_neurons_count; i++)
                    input_layer[i] = new InputNeuron(new Range(0, 1), Activation_funcs.ActivationFunctionType.LINEAR);

                for (int i = 0; i < hidden_layer_neurons_count; i++)
                    hidden_layer[i] = new Neuron(new Range(-1, 1), Activation_funcs.ActivationFunctionType.LOGISTIC);

                for (int i = 0; i < output_layer_neurons_count; i++)
                    output_layer[i] = new OutputNeuron(new Range(0, 0.8f), Activation_funcs.ActivationFunctionType.THRESHOLD);

                foreach (var input_neuron in input_layer)
                    input_neuron.connect(null);

                foreach (var hidden_neuron in hidden_layer)
                    foreach (var input_neuron in input_layer)
                        hidden_neuron.connect(input_neuron);

                foreach (var output_neuron in output_layer)
                    foreach (var hidden_neuron in hidden_layer)
                        output_neuron.connect(hidden_neuron);
            }
            catch (Exception)
            {
                isBroken = true;
            }
        }
        private Neuron[]   input_layer;
        private Neuron[]   hidden_layer;
        private Neuron[]   output_layer;

        public bool isBroken  { get; private set; }

        public double[] recognize(double[] input_values)
        {
            try
            {
                if (input_values.Length != input_layer.Length)
                    throw new Exception();

                List<double> outputs = new List<double>();

                foreach (InputNeuron input_neuron in input_layer)
                    input_neuron.feedValue(input_values[Array.IndexOf(input_layer,input_neuron)]);

                foreach (var input_neuron in input_layer)
                    input_neuron.excite();

                foreach (var hidden_neuron in hidden_layer)
                    hidden_neuron.excite();

                foreach (OutputNeuron output_neuron in output_layer)
                    output_neuron.excite();

                foreach (OutputNeuron output_neuron in output_layer)
                    outputs.Add(output_neuron.getOutput());

                return outputs.ToArray();
            }
            catch (Exception)
            {
                isBroken = true;
                return null;
            }
        }

        internal List<Neuron[]> getLayersForTeacher(Teacher teacher)
        {
            if (teacher.isTeaching)
            {
                List<Neuron[]> layers = new List<Neuron[]>();
                layers.Add(input_layer);
                layers.Add(hidden_layer);
                layers.Add(output_layer);
                return layers;
            }
            else
            {
                isBroken = true;
                return null;
            }
        }
    }
}
