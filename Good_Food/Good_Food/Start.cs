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
    public partial class Start : Form
    {
        public Start()
        {
            InitializeComponent();
            con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\GOOD_FOOD.mdf;Integrated Security=True");
        }
    

        SqlConnection con;
        SqlCommand cmd;
        SqlDataReader r;
        StreamReader reader;

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Creare_cont_client callable = new Creare_cont_client();
            callable.ShowDialog();
            this.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Autentificare_client callable = new Autentificare_client();
            callable.ShowDialog();
        }

        public void create_database()
        {
            con.Open();
            reader = new StreamReader("meniu.txt");
            string line;
            reader.ReadLine();
            while((line=reader.ReadLine())!=null)
            {
                string[] bucati = line.Split(';');
                if (bucati.Length == 7)
                {
                    cmd = new SqlCommand(String.Format("INSERT INTO Meniu VALUES({0},'{1}','{2}',{3},{4},{5});", bucati[0], bucati[1], bucati[2], bucati[3], bucati[4], bucati[5]), con);
                    cmd.ExecuteNonQuery();
                }
            }
            con.Close();
        }

        private void Start_Load(object sender, EventArgs e)
        {
            create_database();
        }
    }
}
