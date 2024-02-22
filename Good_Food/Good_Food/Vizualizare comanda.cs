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
using System.Windows.Forms;


namespace Good_Food
{
    public partial class Vizualizare_comanda : Form
    {
        public Vizualizare_comanda(int nr,int necesity, int kcal, int pret, string email_adress)
        {
            InitializeComponent();
            con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\GOOD_FOOD.mdf;Integrated Security=True");
            necesar = necesity;
            total_kcal = kcal;
            pret_total = pret;
            email = email_adress;
        }

        Optiuni callable;
        int nr_produse_comanda;
        int necesar,total_kcal, pret_total;
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader r;
        StreamReader reader;
        string email;

        public void update_labels()
        {
            label5.Text = necesar.ToString();
            label6.Text = total_kcal.ToString();
            label7.Text = pret_total.ToString();
        }

        public void elimina(DataGridViewCellEventArgs e)
        {
            dataGridView1.Rows.RemoveAt(e.RowIndex);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex==4)
            {
                elimina(e);
            }
        }

        public void add_to_db()
        {
            con.Open();
            cmd = new SqlCommand(String.Format("INSERT INTO Comenzi VALUES({0},'{1}');",email,DateTime.Now.ToString()), con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            add_to_db();
            MessageBox.Show("Comanda trimisa!");
            this.Close();
        }

        public void completeaza_tabel()
        {
            callable = new Optiuni("uwu");
            for(int cnt=1; cnt<=nr_produse_comanda; cnt++)
                dataGridView1.Rows.Add(callable.comanda[cnt].name, callable.comanda[cnt].kcal, callable.comanda[cnt].pret_bucata, callable.comanda[cnt].bucati);
        }
        private void Vizualizare_comanda_Load(object sender, EventArgs e)
        {
            update_labels();
            completeaza_tabel();
        }
    }
}
