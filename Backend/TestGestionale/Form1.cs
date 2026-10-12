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



        decimal prezzo = default;
        private string connectionString = @"Server=(localdb)\MSSQLLocalDB;Initial Catalog=DemoDB;Integrated Security=True;";
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txbName.Text) || string.IsNullOrWhiteSpace(txbPrice.Text) || string.IsNullOrWhiteSpace(txbIngredienti.Text))
            {
                MessageBox.Show("Errore, compilare tutti i campi.");
                return;
            }

            if (!decimal.TryParse(txbPrice.Text, out prezzo))
            {
                MessageBox.Show("Errore, inserire un valore valido per il prezzo.");
                return;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "INSERT INTO DemoTB (Nome, Prezzo, Descrizione, Stato) VALUES (@valore1, @valore2, @valore3, @valore4)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@valore1", txbName.Text);
                    cmd.Parameters.AddWithValue("@valore2", prezzo);
                    cmd.Parameters.AddWithValue("@valore3", txbIngredienti.Text);
                    cmd.Parameters.AddWithValue("@valore4", checkBox1.Checked);
                    cmd.ExecuteNonQuery();
                }
            }

            txbID.Clear();
            txbName.Clear();
            txbPrice.Clear();
            txbIngredienti.Clear();
            checkBox1.Checked = false;

            txbName.Focus();
            this.demoTBTableAdapter.Fill(this.demoDBDataSet1.DemoTB);
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

                txbID.Clear();
                txbName.Clear();
                txbPrice.Clear();
                txbIngredienti.Clear();
                txbName.Focus();
            }

            this.demoTBTableAdapter.Fill(this.demoDBDataSet1.DemoTB);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: questa riga di codice carica i dati nella tabella 'demoDBDataSet21.DemoTB'. È possibile spostarla o rimuoverla se necessario.
            this.demoTBTableAdapter1.Fill(this.demoDBDataSet21.DemoTB);
            // TODO: questa riga di codice carica i dati nella tabella 'demoDBDataSet11.Prenotazioni'. È possibile spostarla o rimuoverla se necessario.
            this.prenotazioniTableAdapter.Fill(this.demoDBDataSet11.Prenotazioni);
            // TODO: questa riga di codice carica i dati nella tabella 'demoDBDataSet1.DemoTB'. È possibile spostarla o rimuoverla se necessario.
            this.demoTBTableAdapter.Fill(this.demoDBDataSet1.DemoTB);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                if (row != null)
                {
                    txbID.Text = row.Cells[0]?.Value?.ToString();
                    txbName.Text = row.Cells[1]?.Value?.ToString();
                    txbPrice.Text = row.Cells[2]?.Value?.ToString();
                    txbIngredienti.Text = row.Cells[3]?.Value?.ToString();

                    if (row.Cells[4].Value != DBNull.Value && row.Cells[4].Value != null)
                    {
                        checkBox1.Checked = Convert.ToBoolean(row.Cells[4].Value);
                    }
                    else
                    {
                        checkBox1.Checked = false;
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "UPDATE DemoTB SET Nome = @valore1, Prezzo = @valore2, Descrizione = @valore3, Stato = @valore4 WHERE ID = @valore5";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@valore1", txbName.Text);
                    cmd.Parameters.AddWithValue("@valore2", txbPrice.Text);
                    cmd.Parameters.AddWithValue("@valore3", txbIngredienti.Text);
                    cmd.Parameters.AddWithValue("@valore4", checkBox1.Checked);
                    cmd.Parameters.AddWithValue("@valore5", txbID.Text);
                    cmd.ExecuteNonQuery();
                }

                txbID.Clear();
                txbName.Clear();
                txbPrice.Clear();
                txbIngredienti.Clear();
                checkBox1.Checked = false;

                txbName.Focus();

                this.demoTBTableAdapter.Fill(this.demoDBDataSet1.DemoTB);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox1.Text = "Attivo";
            }
            else
            {
                checkBox1.Text = "Non Attivo";
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            txbOggetto.Text = $"Prenotazione {dataGridView2.CurrentRow.Cells[7].Value}";
            richTextBox1.Text = $"Gentile {dataGridView2.CurrentRow.Cells[1].Value} {dataGridView2.CurrentRow.Cells[2].Value},\n\nLa informiamo che la sua prenotazione per il giorno {dataGridView2.CurrentRow.Cells[3].Value} alle ore {dataGridView2.CurrentRow.Cells[4].Value} è stata confermata.\n\nLa ringraziamo per aver scelto il nostro servizio.\n\nCordiali saluti,\nIl Team di TestGestionale";
            if (!String.IsNullOrEmpty(txbOggetto.Text) && !String.IsNullOrEmpty(richTextBox1.Text))
            {
                button4.Visible = true;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            txbOggetto.Text = $"Prenotazione {dataGridView2.CurrentRow.Cells[7].Value}";
            richTextBox1.Text = $"Gentile {dataGridView2.CurrentRow.Cells[1].Value} {dataGridView2.CurrentRow.Cells[2].Value},\n\nLa informiamo che la sua prenotazione per il giorno {dataGridView2.CurrentRow.Cells[3].Value} alle ore {dataGridView2.CurrentRow.Cells[4].Value} è stata annullata.\n\nCi scusiamo per l'inconveniente e speriamo di poterla servire in futuro.\n\nCordiali saluti,\nIl Team di TestGestionale";
            if (!String.IsNullOrEmpty(txbOggetto.Text) && !String.IsNullOrEmpty(richTextBox1.Text))
            {
                button4.Visible = true;
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView2.Rows[e.RowIndex];

                if (row != null)
                {
                    txbID2.Text = row.Cells[0]?.Value?.ToString();
                    txbDestinatario.Text = row.Cells[2]?.Value?.ToString();

                    button8.Visible = true;
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            txbID2.Clear();
            txbDestinatario.Clear();
            txbOggetto.Clear();
            richTextBox1.Clear();

            button8.Visible = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "TRUNCATE TABLE Prenotazioni";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                txbID2.Clear();
                txbDestinatario.Clear();
                txbOggetto.Clear();
                richTextBox1.Clear();
            }

            this.prenotazioniTableAdapter.Fill(this.demoDBDataSet11.Prenotazioni);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.prenotazioniTableAdapter.Fill(this.demoDBDataSet11.Prenotazioni);
        }
    }
}
