using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace EZ_ANN_4_Letter_Recognition
{
    public partial class ANNManagerDialog : Form
    {
        public ANNManagerDialog(ANN_Manager ann_manager)
        {
            InitializeComponent();

            this.ann_manager = ann_manager;

            foreach (TeachingMethodType method in Enum.GetValues(typeof(TeachingMethodType)))
                (ANN_table.Columns[4] as DataGridViewComboBoxColumn).Items.Add(method.ToString());

            (ANN_table.Columns[4] as DataGridViewComboBoxColumn).Items.Add("");

            updateGrid();
        }
        private ANN_Manager ann_manager;

        private void button1_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            try
            {
                updateTable();

                if (ANN_table.SelectedCells.Count > 0)
                {
                    var cell      = ANN_table.SelectedCells[0];
                    var row       = cell.OwningRow;
                    var name_cell = row.Cells[0];

                    string ann_name = name_cell.Value.ToString();

                    ann_manager.selectANN(ann_name);
                }
                else throw new Exception();

                Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Please, select cell containing correct info and try again");
            }
        }

        private void updateTable()
        {
            updateGrid();

            try
            {
                foreach (DataGridViewRow row in ANN_table.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        string name = row.Cells[0].Value.ToString();
                        int input_neurons = Convert.ToInt32(row.Cells[1].Value);
                        int hidden_neurons = Convert.ToInt32(row.Cells[2].Value);
                        int output_neurons = Convert.ToInt32(row.Cells[3].Value);
                        TeachingMethodType teachingMethod = 0;

                        string tableName = row.Cells[4].Value.ToString();
                        if (tableName != "" && tableName != null)
                        {
                            string existingName = Array.Find(Enum.GetNames(typeof(TeachingMethodType)), nm => nm == tableName);
                            teachingMethod = (TeachingMethodType)Enum.GetValues(typeof(TeachingMethodType)).GetValue(Array.IndexOf(Enum.GetNames(typeof(TeachingMethodType)), existingName));
                        }
                        else
                            return;

                        if (!ann_manager.createANN(name, input_neurons, hidden_neurons, output_neurons, teachingMethod))
                        {
                            ann_manager.deleteANN(name);
                            ann_manager.createANN(name, input_neurons, hidden_neurons, output_neurons, teachingMethod);
                        }
                    }
                }
            }
            catch (Exception) { }
        }

        private void updateGrid()
        {
            List<string[]> tableInfo = ann_manager.getTableInfo();

            ANN_table.Rows.Clear();
            foreach (var row in tableInfo)
                ANN_table.Rows.Add(row);
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            updateTable();
        }

        private void button4_Click(object sender, System.EventArgs e)
        {
            ann_manager.loadTable();

            updateGrid();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            updateTable();

            ann_manager.saveTable();
        }

        private void ANNManagerDialog_Load(object sender, EventArgs e)
        {
            ann_manager.loadTable();

            List<string[]> tableInfo = ann_manager.getTableInfo();

            ANN_table.Rows.Clear();
            foreach (var row in tableInfo)
                ANN_table.Rows.Add(row);

            updateGrid();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (ANN_table.SelectedCells.Count > 0)
                {
                    var cell = ANN_table.SelectedCells[0];
                    var row = cell.OwningRow;
                    var name_cell = row.Cells[0];

                    string ann_name = name_cell.Value.ToString();

                    ann_manager.deleteANN(ann_name);

                    ANN_table.Rows.Remove(row);
                }
                else throw new Exception();
            }
            catch (Exception)
            {
                MessageBox.Show("Please, select cell containing correct info and try again");
            }
        }
    }
}
