using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;

namespace expedition
{
    public class Postgres
    {
        private NpgsqlConnection _conn;
        private NpgsqlCommand _cmd;
        private NpgsqlDataReader _dr;
        private DataTable _tabell;
        
        public Postgres()
        {
            _conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["db_expedition"].ConnectionString);
            _conn.Open();
            _tabell = new DataTable();
        }

        private void sqlNonQuery(string sql)
        {
            try
            {
                _cmd = new NpgsqlCommand(sql, _conn);
                _cmd.ExecuteNonQuery();
            }
            catch (NpgsqlException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }

        private DataTable sqlFråga(string sql)
        {
            try
            {
                _cmd = new NpgsqlCommand(sql, _conn);
                _dr = _cmd.ExecuteReader();
                _tabell.Load(_dr);
                return _tabell;
            }
            catch (NpgsqlException ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                DataColumn c1 = new DataColumn("Error");
                DataColumn c2 = new DataColumn("ErrorMessage");

                c1.DataType = System.Type.GetType("System.Boolean");
                c2.DataType = System.Type.GetType("System.String");

                _tabell.Columns.Add(c1);
                _tabell.Columns.Add(c2);

                //skapa rad
                DataRow rad = _tabell.NewRow();
                rad[c1] = true;
                rad[c2] = ex.Message;
                _tabell.Rows.Add(rad);


                return _tabell;
            }
            //finally
            //{
            //    _conn.Close();
            //}
        }



        /// <summary>
        /// Hämtar alla fjälltoppar och returnerar dem som en lista.
        /// </summary>
        /// <returns>Lista med fjälltoppar</returns>
        public List<Fjälltopp> HämtaFjälltopp()
        {
            string sql = "select id as \"ft.id\", höjd as \"ft.höjd\", namn as \"ft.namn\" from fjälltopp ft";
            _tabell = sqlFråga(sql);

            List<Fjälltopp> toppar = new List<Fjälltopp>();

            if (_tabell.Columns[0].ColumnName.Equals("Error"))
            {
                Fjälltopp ft = new Fjälltopp();
                ft.Error = true;
                ft.ErrorMessage = _tabell.Rows[0][1].ToString();

                toppar.Add(ft);
            }
            //Fjälltopp topp;
            

            //while (_dr.Read())
            //{
            //    topp = new Fjälltopp()
            //    {
            //        namn = _dr["ft.namn"].ToString(),
            //        höjd = (int)_dr["ft.höjd"],
            //        id = (int)_dr["ft.id"]
            //    };
            //    toppar.Add(topp);
            //}
            return toppar;
        }
        public void TestaTransaktion()
        {
            string sql;
            sqlNonQuery("begin");
            sql = "select 1 from person where id=2 for update";
            sqlNonQuery(sql);
            sql = "update person set  förnamn='Erik' where id=2";
            sqlNonQuery(sql);
            sql = "select id as \"ft.id\", höjd as \"ft.höjd\", namn as \"ft.namn\" from fjälltopp ft";
            _tabell = sqlFråga(sql);
            //sqlNonQuery("commit");
            //_conn.Close();

        }
    }
}
