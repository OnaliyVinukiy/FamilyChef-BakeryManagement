using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RM
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        
       public void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void btnLogin_Click(object sender, EventArgs e)
        {
            if(MainClass.IsValidUser (txtUser.Text , txtPass.Text)==false)
            {
                guna2MessageDialog1.Show("Invalid Username or Password");
                return;

            }
            else
            {
                this.Hide();
                frmMain frm = new frmMain();
                frm.Show();
            }
        }

      

        

        public void btnLogin_Click_1(object sender, EventArgs e)
        {

            if (MainClass.IsValidUser(txtUser.Text, txtPass.Text) == false)
            {
                guna2MessageDialog1.Show("Invalid Username or Password");
                return;

            }
            else
            {
                this.Hide();
                frmMain frm = new frmMain();
                frm.Show();
            }

        }

        public void btnExit_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
