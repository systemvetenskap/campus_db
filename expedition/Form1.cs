using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace expedition
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public BindingList<person> personlista = new BindingList<person>();
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
            NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;User Id=webuser;Password=test123;Database=expedition");
                
                string sql = "select * from person";
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                NpgsqlDataReader dr = cmd.ExecuteReader();
               // NpgsqlDataReader dr = cmd.ExecuteReader();
                person p;
                while (dr.Read())
                {
                    p = new person()
                    {
                        förnamn = dr["förnamn"].ToString(),
                        efternamn = dr["efternamn"].ToString(),
                        id = (int)dr["id"]
                    };
                    personlista.Add(p);
                   // MessageBox.Show(dr["förnamn"].ToString());
                    listBox1.DisplayMember = "FullständigtNamn";
                    listBox1.DataSource = personlista;
                }
            }
            catch (NpgsqlException ex)
            {
                if (ex.Code.Equals("28P01"))
                {
                    MessageBox.Show("Nu blev lösenordet fel");
                }
                else
                {
                    MessageBox.Show(ex.Code);
                }
            }
            //finally
            //{
            //    conn.Close();
            //}

        }

        private void button2_Click(object sender, EventArgs e)
        {
            NpgsqlConnection conn = new NpgsqlConnection("Server=localhost;Port=5432;User Id=webuser;Password=test123;Database=expedition");
            string lösenord = textBox1.Text;
            string sql = "select * from person where passwd = :pass";
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("pass", lösenord);
            NpgsqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                MessageBox.Show(dr["förnamn"].ToString());
            }
            conn.Close();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Postgres db = new Postgres();
            List<Fjälltopp> toppar = new List<Fjälltopp>();
            toppar = db.HämtaFjälltopp();
            listBox1.DataSource = toppar;


        }
    }
}
