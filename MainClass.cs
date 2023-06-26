
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows;
using System.Windows.Forms;

namespace RM
{
    internal class MainClass
    {

        public static readonly string con_string = "Data Source=LAPTOP-AHHL9HD4;Initial Catalog=RM;User ID=ro;Password=ro123;";




        public static SqlConnection con = new SqlConnection(con_string);

        public static bool IsValidUser(string user, string pass)
        {
            bool IsValid = false;
            string query = @"SELECT * FROM users WHERE username = '" + user + "' AND upass = '" + pass + "'";
            using (SqlConnection con = new SqlConnection(con_string))
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                SqlCommand cmd = new SqlCommand(query, con);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {  
                    IsValid = true;
                    user = dt.Rows[0]["uName"].ToString();
                }
            }
            return IsValid;
        }
        public static string user;
        public static string USER
        {
            get { return user; }
            private set { user = value; }

        }
        public static DataTable ExecuteQuery(string query, Hashtable ht)
        {
            
            SqlCommand cmd = new SqlCommand(query, con);
            DataTable dt = new DataTable();
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
                reader.Close();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                con.Close();
            }
            return dt;
        }
        //method for crud operations
        public static int SQL (String query,Hashtable ht)
        {
            int res = 0;
            try
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType= CommandType.Text;
                foreach(DictionaryEntry item in ht)
                {
                    cmd.Parameters.AddWithValue(item.Key.ToString(), item.Value);
                }
                if (con.State==ConnectionState.Closed)
                {
                    con.Open();
                }
                res= cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }

            }
            catch(Exception ex)
            {
               System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
            return res;
        }
        //for loading data from database
        public static void LoadData(String query, DataGridView gv, ListBox lb)
        {
            // serial no in grid view
            gv.CellFormatting += new DataGridViewCellFormattingEventHandler(gv_CellFormatting);

            try
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                SqlDataAdapter da= new SqlDataAdapter(cmd);
                DataTable dt= new DataTable(); 
                da.Fill(dt);


                for (int i = 0; i < lb.Items.Count; i++)
                {
                    String colNam1 = ((DataGridViewColumn)lb.Items[i]).Name;
                    gv.Columns[colNam1].DataPropertyName = dt.Columns[i].ToString();

                }
                gv.DataSource = dt;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
                con.Close();
            }
        }
        private static void gv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            Guna.UI2.WinForms.Guna2DataGridView gv = (Guna.UI2.WinForms.Guna2DataGridView)sender;
            int count = 0;
            foreach (DataGridViewRow row in gv.Rows)
            {
                count++;
                row.Cells[0].Value = count;
            }
        }
        public static void BlurBackground(Form Model)
        {
            Form Background = new Form();
            using (Model)
            {
                Background.StartPosition = FormStartPosition.Manual;
                Background.FormBorderStyle= FormBorderStyle.None;
                Background.Opacity = 0.5d;
                Background.BackColor = Color.Black;
                Background.Size = frmMain.Instance.Size;
                Background.Location = frmMain.Instance.Location;
                Background.ShowInTaskbar = false;
                Background.Show();
                Model.Owner = Background;
                Model.ShowDialog(Background);
                Background.Dispose();

            }
        }
        public static void CBFill (string query,ComboBox cb)
        {
            SqlCommand cmd= new SqlCommand(query,con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cb.DisplayMember = "Name";
            cb.ValueMember = "id";
            cb.DataSource = dt;
            cb.SelectedIndex = -1;

        }
    }

}
