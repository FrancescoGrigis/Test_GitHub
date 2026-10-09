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

        private string connectionString = @"Server=(localdb)\MSSQLLocalDB;Initial Catalog=TestDB;Integrated Security=True;";
        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "INSERT INTO PizzaMenu (NomePizza, Prezzo, Descrizione, Tag) VALUES (@valore1, @valore2, @valore3, @valore4)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@valore1", textBox1.Text);
                    cmd.Parameters.AddWithValue("@valore2", textBox2.Text);
                    cmd.Parameters.AddWithValue("@valore3", textBox3.Text);
                    cmd.Parameters.AddWithValue("@valore4", textBox4.Text);
                    cmd.ExecuteNonQuery();
                }

                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                textBox4.Clear();

                textBox1.Focus();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "TRUNCATE TABLE PizzaMenu";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
