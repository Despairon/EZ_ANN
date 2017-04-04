using System;
using System.Collections.Generic;
using Activation_funcs;
using System.Threading.Tasks;
using System.Timers;

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

        public Axon getAxonForTeacher(Teacher teacher)
        {
            return teacher.isTeaching ? axon : null;
        }

        public List<Synapse> getSynapsesForTeacher(Teacher teacher)
        {
            return teacher.isTeaching ? synapses : null;
        }

        public ActivationFunctionType getActivationFuncType()
        {
            return activationFunc.type;
        }

        public class Synapse
        {
            public Synapse (Axon axon)
            {
                if (!RandomWeightGenerator.isInit)
                    RandomWeightGenerator.init();

                RandomWeightGenerator.start();

                while (RandomWeightGenerator.generationInProgress) { }

                generateRandomWeight();

                this.axon = axon;
            }

            public Synapse()
            {
                Random random = new Random();

                weight = 1;

                axon = new Axon();

                axon.value = 0;
            }

            private double weight;
            public Axon axon { get; }

            public double getLinearCombination()
            {
                return weight * axon.value;
            }

            public void recalculateWeightAsTeacher(Teacher teacher, double newWeight)
            {
                weight = teacher.isTeaching ? newWeight : weight;
            }

            public double getWeightAsTeacher(Teacher teacher)
            {
                return teacher.isTeaching ? weight : -1;
            }
            
            private void generateRandomWeight()
            {
                Random random = new Random();

                weight = random.NextDouble();
            }

            private static class RandomWeightGenerator
            {
                public static void init()
                {
                    delay = new Timer(2);
                    delay.Enabled = false;
                    delay.Elapsed += tick;
                    generationInProgress = false;
                    isInit = true;
                }
                private static Timer delay;
                public  static bool  generationInProgress { get; private set; }
                public static bool   isInit { get; private set; } = false;

                public static void start()
                {
                    delay.Start();
                    generationInProgress = true;
                }

                private static void tick(object sender, ElapsedEventArgs args)
                {
                    delay.Stop();
                    generationInProgress = false;
                }
            }
        }

        public class Axon
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
