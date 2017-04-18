using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZ_ANN_4_Letter_Recognition.ANN
{
    public class ANN_Manager
    {
        public ANN_Manager()
        {

        }

        public NeuralNetwork createANN(int input_neurons_count, int hidden_neurons_count, int output_neurons_count)
        {
            return new NeuralNetwork(input_neurons_count, hidden_neurons_count, output_neurons_count);
        }

        public Teacher createTeacherForANN(NeuralNetwork ANN, TeachingMethodType teaching_method)
        {
            return ANN.isBroken ? null : new Teacher(teaching_method);
        }


    }
}
