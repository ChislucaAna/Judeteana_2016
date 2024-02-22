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

namespace Good_Food
{
    public partial class Creare_cont_client : Form
    {
        public Creare_cont_client()
        {
            InitializeComponent();
            con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\GOOD_FOOD.mdf;Integrated Security=True");
        }

        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader r;
        StreamReader reader;

        public int verifica_preexistenta()
        {
            con.Open();
            int ok = 0;
            cmd = new SqlCommand(String.Format("SELECT * FROM Clienti WHERE email='{0}';", textBox6.Text.ToString()), con);
            r = cmd.ExecuteReader();
            while (r.Read())
                ok = 1;
            con.Close();
            r.Close();

            return ok;
        }

        public void clear_form()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
        }

        public void add_to_db()
        {
            con.Open();
            cmd = new SqlCommand(String.Format("INSERT INTO Clienti VALUES('{0}','{1}','{2}','{3}','{4}',2000);", textBox4.Text.ToString(), textBox1.Text.ToString(), textBox2.Text.ToString(), textBox3.Text.ToString(),textBox6.Text.ToString()), con) ;
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public int verifica_validitatea()
        {
            string email = textBox6.Text;
            if(email.Contains('@') && email.Contains('.') && email.Length>5)
            {
                return 1;
            }
            return 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (verifica_preexistenta() == 1 || verifica_validitatea() == 0)
            {
                MessageBox.Show("Adresa este utilizata deja sau este invalida");
                clear_form();
            }
            else
                add_to_db();
            this.Close();
        }

        private void Creare_cont_client_Load(object sender, EventArgs e)
        {

        }
    }
}
