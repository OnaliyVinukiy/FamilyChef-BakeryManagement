using Guna.UI2.WinForms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace RM.View
{
    public partial class frmCounterView : Form
    {
        public frmCounterView()
        {
            InitializeComponent();
        }

        private void frmCounterView_Load(object sender, EventArgs e)
        {
            GetOrders();
        }

        private void GetOrders()
        {
            // Clear the container control
            flowLayoutPanel1.Controls.Clear();

            string query1 = @"Select * from tblMain  where status ='Paid'";
            SqlCommand cmd1 = new SqlCommand(query1, MainClass.con);
            DataTable dt1 = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            da.Fill(dt1);

            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                // Create a new instance of FlowLayoutPanel for each row
                FlowLayoutPanel p1 = new FlowLayoutPanel();
                p1.AutoSize = true;
                p1.Width = 230;
                p1.Height = 200;
                p1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
                p1.BorderStyle = BorderStyle.FixedSingle;
                p1.Margin = new Padding(10, 10, 10, 10);

                FlowLayoutPanel p2 = new FlowLayoutPanel();
                p2 = new FlowLayoutPanel();
                p2.BackColor = Color.FromArgb(0, 128, 128);
                p2.AutoSize = true;
                p2.Width = 230;
                p2.Height = 125;
                p2.Margin = new Padding(0, 0, 0, 0);

                Label lb2 = new Label();
                lb2.ForeColor = Color.Black;
                lb2.Margin = new Padding(10, 5, 3, 0);
                lb2.AutoSize = true;

                Label lb3 = new Label();
                lb3.ForeColor = Color.Black;
                lb3.Margin = new Padding(10, 5, 3, 0);
                lb3.AutoSize = true;

                Label lb4 = new Label();
                lb4.ForeColor = Color.Black;
                lb4.Margin = new Padding(10, 5, 3, 10);
                lb4.AutoSize = true;

                lb2.Text = "Order No : " + dt1.Rows[i]["MainID"].ToString();
                lb3.Text = "Order Time : " + dt1.Rows[i]["aTime"].ToString();
                lb4.Text = "Order Type : " + dt1.Rows[i]["OrderType"].ToString();
                p2.Controls.Add(lb2);
                p2.Controls.Add(lb3);
                p2.Controls.Add(lb4);

                p1.Controls.Add(p2);

                // add products
                int mid = Convert.ToInt32(dt1.Rows[i]["MainID"].ToString());
                string query2 = @"SELECT * FROM tblMain m 
            INNER JOIN tblDetails d ON m.MainID = d.MainID 
            INNER JOIN products p ON p.pID = d.proID 
            WHERE m.MainID = @mid";

                SqlCommand cmd2 = new SqlCommand(query2, MainClass.con);
                cmd2.Parameters.AddWithValue("@mid", mid);
                DataTable dt2 = new DataTable();
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                da2.Fill(dt2);
                p1.AutoScroll = false;
                p1.SuspendLayout();
                p1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
                p1.AutoSize = true;
                p1.WrapContents = true;
                for (int j = 0; j < dt2.Rows.Count; j++)
                {
                    Label lb5 = new Label();
                    lb5.ForeColor = Color.Black;
                    lb5.Margin = new Padding(10, 5, 3, 0);
                    lb5.AutoSize = true;

                    int no = j + 1;
                    lb5.Text = " " + no + " " + dt2.Rows[j]["pName"].ToString() + " " + dt2.Rows[j]["qty"].ToString();
                    p1.Controls.Add(lb5);
                }
                p1.ResumeLayout(true);




                //add button to change the order status
                Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                b.AutoRoundedCorners = true;
                b.Size = new System.Drawing.Size(100, 35);
                b.FillColor = Color.FromArgb(37,56,60);
                b.Margin = new Padding(30, 5, 3, 10);
                b.Text = "Complete";
                b.Tag = dt1.Rows[i]["MainID"].ToString(); //store the id

                b.Click += new EventHandler(b_click);
                p1.Controls.Add(b);



                flowLayoutPanel1.Controls.Add(p1);
            }

        }

        private void b_click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32((sender as Guna.UI2.WinForms.Guna2Button).Tag.ToString());
            guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
            guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
            if (guna2MessageDialog1.Show("Are you sure you want to complete the order?") == DialogResult.Yes)
            {
                string query = @"Update tblMain set status= 'Complete' where MainID = @ID";
                using (SqlConnection con = new SqlConnection(MainClass.con_string))
                {
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);
                        con.Open();
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            System.Windows.Forms.MessageBox.Show("Order Completed Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }

                // Find the parent FlowLayoutPanel and remove it from the container
                var button = (Guna.UI2.WinForms.Guna2Button)sender;
                var flowLayoutPanel = (FlowLayoutPanel)button.Parent;
                flowLayoutPanel1.Controls.Remove(flowLayoutPanel);
            }
        }


    }
}
