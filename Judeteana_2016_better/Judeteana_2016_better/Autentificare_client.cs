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
    public partial class Autentificare_client : Form
    {
        public Autentificare_client()
        {
            InitializeComponent();
            con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\GOOD_FOOD.mdf;Integrated Security=True;MultipleActiveResultSets=True");
        }

        public static string email;
        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader r;
        StreamReader reader;

        private void button1_Click(object sender, EventArgs e)
        {
            email = textBox1.Text;
            con.Open();
            int ok = 0;
            cmd = new SqlCommand(String.Format("SELECT * FROM Clienti WHERE email='{0}' AND parola='{1}';",textBox1.Text,textBox2.Text), con);
            r = cmd.ExecuteReader();
            while(r.Read())
                ok = 1;
            con.Close();
            r.Close();
            if (ok==1)
            {
                this.Hide();
                Optiuni optiuni = new Optiuni();
                optiuni.ShowDialog();
            }
            else
            {
                MessageBox.Show("Eroare autentificare!");
                textBox1.Text = "";
                textBox2.Text = "";
            }
        }
    }
}
