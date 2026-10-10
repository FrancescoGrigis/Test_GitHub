using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace TestGestionale
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private string connectionString = @"Server=(localdb)\MSSQLLocalDB;Initial Catalog=DemoDB;Integrated Security=True;";
        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "INSERT INTO DemoTB (Nome, Prezzo, Descrizione, isActive) VALUES (@valore1, @valore2, @valore3, @valore4)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@valore1", textBox1.Text);
                    cmd.Parameters.AddWithValue("@valore2", textBox2.Text);
                    cmd.Parameters.AddWithValue("@valore3", richTextBox1.Text);
                    cmd.Parameters.AddWithValue("@valore4", checkBox1.Checked);
                    cmd.ExecuteNonQuery();
                }

                if (checkBox1.Checked)
                {
                    checkBox1.Text = "Attivo";
                }
                else
                {
                    checkBox1.Text = "Non Attivo";
                }

                textBox1.Clear();
                textBox2.Clear();
                richTextBox1.Clear();
                checkBox1.Checked = false;

                textBox1.Focus();
            }
            this.demoTBTableAdapter.Fill(this.demoDBDataSet.DemoTB);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "TRUNCATE TABLE DemoTB";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.ExecuteNonQuery();
                }
                textBox1.Focus();
            }
            this.demoTBTableAdapter.Fill(this.demoDBDataSet.DemoTB);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.demoTBTableAdapter.Fill(this.demoDBDataSet.DemoTB);

        }
    }
}
