using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace EZ_ANN_4_Letter_Recognition
{
    public class ANN_Manager
    {
        public ANN_Manager()
        {
            ann_table = new List<ANN_table_item>();
            selected_ann = null; 
        }
        private List<ANN_table_item> ann_table;
        private ANN_table_item selected_ann;
        private const string ANN_FILENAME = "neural_networks.bin";

        [Serializable]
        public class ANN_table_item
        {
            public ANN_table_item(string name, NeuralNetwork ann, Teacher teacher)
            {
                this.name    = name;
                this.ann     = ann;
                this.teacher = teacher;
            }

            public string        name;
            public NeuralNetwork ann;
            public Teacher       teacher;
        }

        public bool createANN(string name, int input_neurons_count, int hidden_neurons_count, int output_neurons_count, TeachingMethodType teachingMethod)
        {
            NeuralNetwork ann     = new NeuralNetwork(input_neurons_count, hidden_neurons_count, output_neurons_count);
            Teacher       teacher = new Teacher(teachingMethod);

            if (ann_table.Find(nn => nn.name == name) != null || ann.getANNInfo().isBroken)
                return false;
            else
            {
                ann_table.Add(new ANN_table_item(name, ann, teacher));
                return true;
            }
        }

        public bool deleteANN(string name)
        {
            ANN_table_item ann = ann_table.Find(nn => nn.name == name);

            if (ann == null)
                return false;
            else
            {
                ann_table.Remove(ann);
                return true;
            }
        }

        public string teachANN(double precision, TeachingSample[] samples, int iterations)
        {
            if (selected_ann == null)
                return "error";
            else
                return selected_ann.teacher.teach(selected_ann.ann, precision, samples, iterations);
        }

        public double[] useANN(double[] values)
        {
            if (selected_ann == null)
                return null;
            else
                return selected_ann.ann.recognize(values);
        }

        public bool saveTable()
        {
            if (ann_table.Count > 0)
            {
                FileStream fs = new FileStream(ANN_FILENAME, FileMode.OpenOrCreate);

                BinaryFormatter bf = new BinaryFormatter();

                bf.Serialize(fs, ann_table);

                fs.Close();

                return true;
            }
            else
                return false;
        }

        public bool loadTable()
        {
            if (File.Exists(ANN_FILENAME))
            {
                FileStream fs = new FileStream(ANN_FILENAME, FileMode.Open);

                BinaryFormatter bf = new BinaryFormatter();

                ann_table = (List<ANN_table_item>)bf.Deserialize(fs);

                fs.Close();

                return true;
            }
            else
                return false;           
        }

        public bool selectANN(string name)
        {
            try
            {
                selected_ann = ann_table.Find(ann => ann.name == name);
                return true;
            }
            catch (Exception) { return false; }
        }

        public string getSelectedANNName()
        {
            try
            {
                return selected_ann.name;
            }
                catch (Exception) { return "None"; }
        }

        public List<string[]> getTableInfo()
        {
            List<string[]> tableInfo = new List<string[]>();

            foreach (var element in ann_table)
            {
                string name = element.name;
                string input_neurons_count  = element.ann.getANNInfo().input_neurons.ToString();
                string hidden_neurons_count = element.ann.getANNInfo().hidden_neurons.ToString();
                string output_neurons_count = element.ann.getANNInfo().output_neurons.ToString();
                string method = element.teacher.getTeachingMethod().ToString();

                tableInfo.Add(new string[] { name, input_neurons_count , hidden_neurons_count , output_neurons_count , method });
            }

            return tableInfo;
        }
    }
}
