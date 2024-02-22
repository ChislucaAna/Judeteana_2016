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
    public partial class Autentificare_client : Form
    {
        public Autentificare_client()
        {
            InitializeComponent();
            con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\GOOD_FOOD.mdf;Integrated Security=True");
        }

        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader r;
        StreamReader reader;

        public int verifica_existenta()
        {
            con.Open();
            int ok = 0;
            cmd = new SqlCommand(String.Format("SELECT * FROM Clienti WHERE (email='{0}' AND parola='{1}');", textBox1.Text,textBox2.Text), con);
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
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if(verifica_existenta()==1)
            {
                this.Hide();
                Optiuni callable = new Optiuni(textBox1.Text);
                callable.ShowDialog();
            }
            else
            {
                MessageBox.Show("Eroare autentificare!");
                clear_form();
            }
        }

        private void Autentificare_client_Load(object sender, EventArgs e)
        {

        }
    }
}
