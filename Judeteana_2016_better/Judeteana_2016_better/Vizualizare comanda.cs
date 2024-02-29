using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Threading;

namespace Judeteana_2016_better
{
    public partial class Vizualizare_comanda : Form
    {
        public Vizualizare_comanda()
        {
            InitializeComponent();
            con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\GOOD_FOOD.mdf;Integrated Security=True;MultipleActiveResultSets=True");
        }

        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader r;
        StreamReader reader;

        private void Vizualizare_comanda_Load(object sender, EventArgs e)
        {
            foreach(Optiuni.items aux in Optiuni.comanda)
            {
                if(aux!=null)
                {
                    dataGridView1.Rows.Add(aux.name,aux.kcal.ToString(),aux.pret.ToString(),aux.cantitate.ToString());
                }
            }
        }

        public void update_labels()
        {
            textBox5.Text = Optiuni.necesar.ToString();
            textBox6.Text = Optiuni.total_kcal.ToString();
            textBox7.Text = Optiuni.pret_total.ToString();
        }

        public void elimina(DataGridViewCellEventArgs e)
        {
            Optiuni.pret_total -= Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[2].Value);
            Optiuni.total_kcal -= Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[1].Value);
            update_labels();
            dataGridView1.Rows.RemoveAt(e.RowIndex);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex==4)
            {
                elimina(e);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Comanda trimisa");
            this.Close();
        }
    }
}
