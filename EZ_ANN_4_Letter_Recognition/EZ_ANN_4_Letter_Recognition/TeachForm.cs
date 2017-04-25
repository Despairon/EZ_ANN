using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace EZ_ANN_4_Letter_Recognition
{
    public partial class TeachForm : Form
    {
        public TeachForm(ANN_Manager ann_manager)
        {
            InitializeComponent();

            this.ann_manager = ann_manager;
        }
        private ANN_Manager ann_manager;

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bGo_Click(object sender, EventArgs e)
        {
            try
            {
                TeachingSample[] samples = null;

                if (rbText.Checked)
                {
                    MessageBox.Show("Please, choose text file to generate teaching samples");

                    OpenFileDialog dlg = new OpenFileDialog();
                    dlg.ShowDialog();

                    if (dlg.FileName != null && dlg.FileName != "")
                    {
                        samples = TeachingSample.generateTeachingSamplesFromFile(dlg.FileName);

                        if (samples == null)
                            throw new Exception();

                    }
                }
                else if (rbImage.Checked)
                {
                    MessageBox.Show("Please, choose images to generate teaching samples");

                    OpenFileDialog dlg = new OpenFileDialog();
                    dlg.Multiselect = true;
                    dlg.ShowDialog();
                    if (dlg.FileNames.Length > 0)
                    {
                        samples = new TeachingSample[dlg.FileNames.Length];
                        for (int i = 0; i < samples.Length; i++)
                        {
                            List<string[]> anns_info = ann_manager.getTableInfo();

                            int input_length  = 0;
                            int output_length = 0;
                            foreach (var info in anns_info)
                            {
                                if (info[0] == ann_manager.getSelectedANNName())
                                {
                                    input_length = Convert.ToInt32(info[1]);
                                    output_length = Convert.ToInt32(info[3]);
                                    break;
                                }
                            }

                            samples[i] = new TeachingSample(input_length, output_length);
                            if (!samples[i].generateTeachingSampleFromImage(dlg.FileNames[i]))
                            {
                                MessageBox.Show("one of the files selected is bad");
                                return;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("No files for teaching sample selected!");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("An odd situation occured. This is strange as fuck (also as div by 0), and program cant proceed...");
                    throw new Exception();
                }

                if (samples != null)
                {
                    double precision  = Convert.ToDouble(textBox2.Text);
                    int    iterations = Convert.ToInt32(textBox1.Text);

                    Close();

                    string teachingResults = ann_manager.teachANN(precision, samples, iterations);

                    MessageBox.Show("Teaching results: " + teachingResults);
                }
                else
                    throw new Exception();

            }
            catch (Exception) { MessageBox.Show("An error occured. Probably, you entered wrong values, or choose wrong file(s)"); };
            
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar >= '0' && e.KeyChar <= '9' || e.KeyChar == '\b')
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= '0' && e.KeyChar <= '9') || (e.KeyChar == '.' && !textBox2.Text.Contains(".") && textBox2.Text != "") || e.KeyChar == '\b')
                e.Handled = false;
            else
                e.Handled = true;
        }
    }
}
