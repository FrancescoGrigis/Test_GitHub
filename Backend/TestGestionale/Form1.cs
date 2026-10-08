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

                string query = "INSERT INTO TestTable (Test) VALUES (@valore1)"; // Use parameterized query to prevent SQL injection

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@valore1", textBox1.Text);
                    cmd.ExecuteNonQuery();
                }
                textBox1.Clear();
                textBox1.Focus();
            }
        }
    }
}
