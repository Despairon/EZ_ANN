using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Activation_funcs;

namespace EZ_ANN_4_Letter_Recognition
{
    public class Neuron
    {
        public Neuron(Range sensitivity, ActivationFunctionType f_type)
        {
            this.sensitivity = sensitivity;

            activationFunc = new ActivationFunction(f_type);

            synapses = new List<Synapse>();

            axon = new Axon();
        }
        protected readonly Range              sensitivity;
        protected readonly ActivationFunction activationFunc;
        protected readonly List<Synapse>      synapses;
        protected readonly Axon               axon;

        private double summator()
        {
            double linearCombination = 0;

            foreach (var synapse in synapses)
                linearCombination += synapse.getLinearCombination();

            return linearCombination;
        }

        public void excite()
        {
            axon.value = activationFunc.calculate(sensitivity,summator());
        }

        public virtual void connect(Neuron neuron)
        {
            synapses.Add(new Synapse(neuron.axon));
        }

        protected class Synapse
        {
            public Synapse(Axon axon)
            {
                Random random = new Random();

                weight = random.NextDouble();

                this.axon = axon;
            }

            public Synapse()
            {
                Random random = new Random();

                weight = random.NextDouble();

                axon = new Axon();

                axon.value = 0;
            }

            private double weight;
            public Axon axon;

            public double getLinearCombination()
            {
                return weight * axon.value;
            }
        }

        protected class Axon
        {
            public Axon()
            {
                value = 0;
            }
            public double value;
        }
    }

    public class InputNeuron : Neuron
    {
        public InputNeuron(Range sensitivity, ActivationFunctionType f_type) : base (sensitivity, f_type)
        {

        }

        public void feedValue(double value)
        {
            if (synapses.Count > 0)
                synapses[0].axon.value = value;
        }

        public override void connect(Neuron neuron)
        {
            if (neuron == null)
                synapses.Add(new Synapse());
        }
    }

    public class OutputNeuron : Neuron
    {
        public OutputNeuron(Range sensitivity, ActivationFunctionType f_type) : base (sensitivity, f_type)
        {

        }

        public double getOutput()
        {
            return axon.value;
        }
    }
}
