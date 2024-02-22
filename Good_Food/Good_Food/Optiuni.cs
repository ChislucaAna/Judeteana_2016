using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace Good_Food
{
    

    public partial class Optiuni : Form
    {
        public Optiuni(string email_adress)
        {
            InitializeComponent();
            con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\GOOD_FOOD.mdf;Integrated Security=True");
            email = email_adress;
        }

        int s, necesar;
        string email;
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader r;
        StreamReader reader;
        int total_kcal, pret_total;
        int[] id_produse_comanda = new int[100];
        int nr_produse_comanda;

        public class item
        {
            public  int id;
            public  int kcal;
            public  int pret_bucata;
            public  int bucati;
            public  string name;

            public item( int identificator,  int calorii,  int price_per_piece,  int pieces)
            {
                id = identificator;
                kcal = calorii;
                pret_bucata = price_per_piece;
                bucati = pieces;
            }
        }

        public item[] comanda = new item[100];


        public void update_db()
        {
            con.Open();
            cmd = new SqlCommand(String.Format("UPDATE Clienti SET kcal_zilnice={0} WHERE email='{0}';", necesar.ToString(),email), con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void completeaza_tabel()
        {
            con.Open();
            cmd = new SqlCommand("SELECT * FROM Meniu;", con);
            r = cmd.ExecuteReader();
            while(r.Read())
            {
                dataGridView1.Rows.Add(r[0].ToString(), r[1].ToString(), r[2].ToString(), r[3].ToString(), r[4].ToString(), r[5].ToString(),'1');
            }
            con.Close();
        }

        private void Optiuni_Load(object sender, EventArgs e)
        {
            completeaza_tabel();
        }

        public void update_labels()
        {
            label6.Text = total_kcal.ToString();
            label7.Text = pret_total.ToString();
        }

        public void update_values(DataGridViewCellEventArgs e)
        {
            int rowindex = e.RowIndex;

            int calorii_bucata = Convert.ToInt32(dataGridView1.Rows[rowindex].Cells[4].Value.ToString());
            int bucati = Convert.ToInt32(dataGridView1.Rows[rowindex].Cells[6].Value.ToString());
            int pret_bucata = Convert.ToInt32(dataGridView1.Rows[rowindex].Cells[3].Value.ToString());

            int calorii = calorii_bucata * bucati;
            int pret = pret_bucata * bucati;

            pret_total += pret;
            total_kcal += calorii;
        }

        public void find_name_by_id()
        {
            con.Open();
            cmd = new SqlCommand(String.Format("SELECT * FROM Meniu WHERE id_produs={0}", comanda[nr_produse_comanda].id.ToString()), con);
            r = cmd.ExecuteReader();
            while (r.Read())
            {
                comanda[nr_produse_comanda].name = r[1].ToString();
            }
            r.Close();
            con.Close();
        }

        public void add_to_comanda(DataGridViewCellEventArgs e)
        {
            int rowindex = e.RowIndex;
            int calorii_bucata = Convert.ToInt32(dataGridView1.Rows[rowindex].Cells[4].Value.ToString());
            int bucati = Convert.ToInt32(dataGridView1.Rows[rowindex].Cells[6].Value.ToString());
            int pret_bucata = Convert.ToInt32(dataGridView1.Rows[rowindex].Cells[3].Value.ToString());
            int index = Convert.ToInt32(dataGridView1.Rows[rowindex].Cells[0].Value.ToString());

            nr_produse_comanda++;
            item aux = new item(index, calorii_bucata, pret_bucata, bucati);
            comanda[nr_produse_comanda] = aux;
            find_name_by_id();
        }

        public void adauga(DataGridViewCellEventArgs e)
        {
            int rowindex = e.RowIndex;
            if(Convert.ToInt32(dataGridView1.Rows[rowindex].Cells[6].Value.ToString())<0)
            {
                MessageBox.Show("Cantitate negativa!");
            }
            else
            {
                add_to_comanda(e);
                update_values(e);
                update_labels();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Vizualizare_comanda callable = new Vizualizare_comanda(nr_produse_comanda,necesar,total_kcal,pret_total,email);
            callable.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex==7)
            {
                adauga(e);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            s = Convert.ToInt32(textBox1.Text) + Convert.ToInt32(textBox2.Text) + Convert.ToInt32(textBox3.Text);
            if (s < 250)
                necesar = 1800;
            if (s >= 250 && s <= 275)
                necesar = 2200;
            if (s > 275)
                necesar = 2500;
            textBox4.Text = necesar.ToString();
            textBox5.Text = necesar.ToString();
            update_db();
        }
    }
}
