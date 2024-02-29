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
    public partial class Creare_cont_client : Form
    {
        public Creare_cont_client()
        {
            InitializeComponent();
            con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\GOOD_FOOD.mdf;Integrated Security=True;MultipleActiveResultSets=True");
        }

        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader r;
        StreamReader reader;

        public void clear()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox4.Text != textBox5.Text)
            {
                MessageBox.Show("Parolele nu corespund!");
                clear();
            }
            else
            {
                con.Open();
                int ok = 1;
                cmd = new SqlCommand(String.Format("SELECT * FROM Clienti WHERE email='{0}';", textBox6.Text), con);
                r = cmd.ExecuteReader();
                while(r.Read())
                {
                    ok = 0;
                }
                r.Close();
                con.Close();
                if (ok == 0)
                {
                    MessageBox.Show("Exista deja un cont cu aceasta adresa de email");
                    clear();
                }
                else
                {
                    string email = textBox6.Text;
                    string[] bucati1 = email.Split('@');
                    string[] bucati2 = email.Split('.');
                    if (bucati1.Length < 2 || bucati2.Length<2)
                    {
                        MessageBox.Show("Adressa de email invalida");
                        clear();
                    }
                    else
                    {
                        con.Open();
                        cmd = new SqlCommand(String.Format("INSERT INTO CLIENTI VALUES('{0}','{1}','{2}','{3}','{4}',2000)",textBox5.Text,textBox1.Text,textBox2.Text,textBox3.Text,textBox6.Text),con);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        this.Close();
                    }
                }
            }
                
        }
    }
}
