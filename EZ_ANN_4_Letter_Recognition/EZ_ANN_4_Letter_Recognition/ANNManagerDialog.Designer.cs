namespace EZ_ANN_4_Letter_Recognition
{
    partial class ANNManagerDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.ANN_table = new System.Windows.Forms.DataGridView();
            this.ANN_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ANN_input_neurons_count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ANN_hidden_neurons_count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ANN_output_neurons_count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ANN_teaching_method = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.ANN_table)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(630, 407);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(602, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(130, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Select ANN";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // ANN_table
            // 
            this.ANN_table.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ANN_table.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ANN_name,
            this.ANN_input_neurons_count,
            this.ANN_hidden_neurons_count,
            this.ANN_output_neurons_count,
            this.ANN_teaching_method});
            this.ANN_table.Location = new System.Drawing.Point(12, 12);
            this.ANN_table.MultiSelect = false;
            this.ANN_table.Name = "ANN_table";
            this.ANN_table.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.ANN_table.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ANN_table.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.ANN_table.Size = new System.Drawing.Size(584, 418);
            this.ANN_table.TabIndex = 2;
            this.ANN_table.Tag = "";
            // 
            // ANN_name
            // 
            this.ANN_name.HeaderText = "Name";
            this.ANN_name.Name = "ANN_name";
            this.ANN_name.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ANN_name.Width = 150;
            // 
            // ANN_input_neurons_count
            // 
            this.ANN_input_neurons_count.HeaderText = "input neurons";
            this.ANN_input_neurons_count.Name = "ANN_input_neurons_count";
            this.ANN_input_neurons_count.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ANN_input_neurons_count.Width = 80;
            // 
            // ANN_hidden_neurons_count
            // 
            this.ANN_hidden_neurons_count.HeaderText = "hidden neurons";
            this.ANN_hidden_neurons_count.Name = "ANN_hidden_neurons_count";
            this.ANN_hidden_neurons_count.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ANN_hidden_neurons_count.Width = 80;
            // 
            // ANN_output_neurons_count
            // 
            this.ANN_output_neurons_count.HeaderText = "output neurons";
            this.ANN_output_neurons_count.Name = "ANN_output_neurons_count";
            this.ANN_output_neurons_count.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ANN_output_neurons_count.Width = 80;
            // 
            // ANN_teaching_method
            // 
            this.ANN_teaching_method.HeaderText = "teaching method";
            this.ANN_teaching_method.Name = "ANN_teaching_method";
            this.ANN_teaching_method.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ANN_teaching_method.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ANN_teaching_method.Width = 150;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(602, 96);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(130, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "Update Table";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(602, 138);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(130, 23);
            this.button4.TabIndex = 4;
            this.button4.Text = "Load Table";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(602, 186);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(130, 23);
            this.button5.TabIndex = 5;
            this.button5.Text = "Save Table";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(602, 52);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(130, 23);
            this.button6.TabIndex = 6;
            this.button6.Text = "Delete ANN";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // ANNManagerDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(751, 442);
            this.ControlBox = false;
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.ANN_table);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ANNManagerDialog";
            this.Text = "ANN Manager";
            this.Load += new System.EventHandler(this.ANNManagerDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ANN_table)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView ANN_table;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.DataGridViewTextBoxColumn ANN_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn ANN_input_neurons_count;
        private System.Windows.Forms.DataGridViewTextBoxColumn ANN_hidden_neurons_count;
        private System.Windows.Forms.DataGridViewTextBoxColumn ANN_output_neurons_count;
        private System.Windows.Forms.DataGridViewComboBoxColumn ANN_teaching_method;
        private System.Windows.Forms.Button button6;
    }
}