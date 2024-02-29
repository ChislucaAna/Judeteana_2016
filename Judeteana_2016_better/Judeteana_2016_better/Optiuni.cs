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
    public partial class Optiuni : Form
    {
        public Optiuni()
        {
            InitializeComponent();
            con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\GOOD_FOOD.mdf;Integrated Security=True;MultipleActiveResultSets=True");
        }

        SqlConnection con;
        SqlCommand cmd,cmd1,cmd2;
        SqlDataReader r,r1,r2;
        StreamReader reader;
        public static int necesar=2000;
        int bucati,calorii,pret;
        public static int total_kcal, pret_total;
        int comanda_creeata = 0;
        public static string id_comanda;
        int nr_comanda = 1;
        int nr_subcomanda = 1;
        public static string id_client;
        public static int buget;

        public class items
        {
            public string name;
            public int kcal;
            public int pret;
            public int cantitate;

            public items(string nume, int cal, int price, int quantity)
            {
                name = nume;
                kcal = cal;
                pret = price;
                cantitate = quantity;
            }
        }

        public static items[] comanda = new items[100];


        public int gaseste_necesar(int s)
        {
            if (s < 250)
                return 1800;
            if (s >= 250 && s <= 275)
                return 2200;
            else
                return 2800;
        }

        public void find_id()
        {
            con.Open();
            cmd = new SqlCommand(String.Format("SELECT * FROM Clienti WHERE email='{0}';", Autentificare_client.email), con);
            r = cmd.ExecuteReader();
            while (r.Read())
                id_client = r[0].ToString();
            con.Close();
            r.Close();
        }

        public void actualizare()
        {
            con.Open();
            cmd = new SqlCommand(String.Format("UPDATE Clienti SET kcal_zilnice={0} WHERE email='{1}'",necesar.ToString(), Autentificare_client.email), con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public void update_textbox()
        {
            textBox4.Text = necesar.ToString();
            textBox5.Text = necesar.ToString();
            textBox8.Text = necesar.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int s = Convert.ToInt32(textBox1.Text) + Convert.ToInt32(textBox2.Text) + Convert.ToInt32(textBox3.Text);
            necesar = gaseste_necesar(s);
            update_textbox();
            actualizare();
        }

        private void Optiuni_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'gOOD_FOODDataSet.Meniu' table. You can move, or remove it, as needed.
            this.meniuTableAdapter.Fill(this.gOOD_FOODDataSet.Meniu);
            // TODO: This line of code loads data into the 'gOOD_FOODDataSet.Clienti' table. You can move, or remove it, as needed.
            this.clientiTableAdapter.Fill(this.gOOD_FOODDataSet.Clienti);
            find_id();
            update_textbox();

        }

        public void modifica_pret(DataGridViewCellEventArgs e)
        {
            pret_total += bucati * pret;
            label7.Text = pret_total.ToString();
        }

        public void creeaza_comanda()
        {
            comanda_creeata = 1;
            id_comanda = nr_comanda.ToString();
            con.Open();
            cmd = new SqlCommand(String.Format("INSERT INTO Comanda VALUES('{0}',{1},'{2}');",id_comanda,id_client,DateTime.Now.ToString()), con);
            cmd.ExecuteNonQuery();
            con.Close();
            nr_comanda++;
        }

        public void adauga_in_clasa(DataGridViewCellEventArgs e)
        {
            items aux = new items(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString(), calorii, pret, bucati);
            comanda[nr_subcomanda] = aux;
        }

        public void creeaza_subcomanda(DataGridViewCellEventArgs e)
        {
            string id_produs = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() ;
            con.Open();
            cmd = new SqlCommand(String.Format("INSERT INTO Subcomenzi VALUES('{0}',{1},{2});",id_comanda,id_produs,bucati), con);
            cmd.ExecuteNonQuery();
            con.Close();
            nr_subcomanda++;
            adauga_in_clasa(e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Vizualizare_comanda vizualizare_Comanda = new Vizualizare_comanda();
            vizualizare_Comanda.ShowDialog();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            buget = Convert.ToInt32(textBox9.Text);

            con.Open();
            cmd = new SqlCommand("SELECT * FROM Meniu WHERE felul=1;", con);
            r = cmd.ExecuteReader();
            while(r.Read())
            {
                cmd1 = new SqlCommand("SELECT * FROM Meniu WHERE felul=2;", con);
                r1 = cmd1.ExecuteReader();
                while (r1.Read())
                {
                    cmd2 = new SqlCommand("SELECT * FROM Meniu WHERE felul=3;", con);
                    r2 = cmd2.ExecuteReader();

                    while(r2.Read())
                    {
                        int fonduri_ramase = buget - Convert.ToInt32(r2[3].ToString())- Convert.ToInt32(r1[3].ToString())- Convert.ToInt32(r[3].ToString());
                        int calorii_ramase = necesar- Convert.ToInt32(r2[4].ToString()) - Convert.ToInt32(r1[4].ToString()) - Convert.ToInt32(r[4].ToString());

                        if(fonduri_ramase>=0 && calorii_ramase>=0)
                        {
                            dataGridView2.Rows.Add(r[1].ToString(), r1[1].ToString(), r2[1].ToString(), necesar, buget);
                        }
                    }
                }
                //add_to_db();
            }

            con.Close();
        }

        public void modifica_kcal(DataGridViewCellEventArgs e)
        {
            total_kcal += bucati * calorii;
            label6.Text = total_kcal.ToString();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex==5)
            {
                MessageBox.Show("Comanda trimisa");
                this.Close();
            }
        }

        public void adauga_in_chart(DataGridViewCellEventArgs e)
        {
            chart1.Series["Series1"].Points.Clear();
            foreach (items aux in comanda)
            {
                if(aux!=null)
                    chart1.Series["Series1"].Points.AddXY(aux.name, aux.kcal);
            }
        }

        public void adauga(DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows[e.RowIndex].Cells[6].Value != null)
                bucati = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[6].Value);
            else
                bucati = 1;
            calorii = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[4].Value);
            pret = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[3].Value);

            if (bucati < 0)
                MessageBox.Show("Cantitate negativa");
            else
            {
                if(comanda_creeata==0)
                    creeaza_comanda();
                modifica_kcal(e);
                modifica_pret(e);
                creeaza_subcomanda(e);
                adauga_in_chart(e);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex==7)
            {
                adauga(e);
            }
        }
    }
}
