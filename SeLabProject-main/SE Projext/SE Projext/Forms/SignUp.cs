using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CRUD_Operations;

namespace SE_Projext.Forms
{
    public partial class SignUp : Form
    {
        public SignUp()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            SigninSgnup Signin = new SigninSgnup();
            Signin.ShowDialog();
        }

        private void signin_Click(object sender, EventArgs e)
        {
            try
            {


                if (textpassword.Text == "" || textusername.Text == "" || txtEmail.Text=="")
                {
                    MessageBox.Show("Fields can not be Emty !");
                }
                else
                {
                    var con = Configuration.getInstance().getConnection();
                    SqlCommand cmd = new SqlCommand("INSERT INTO Users values( (Select MAX(UserID) From Users)+1, @Username, @Password,  @RegistrationDate,@Email )", con);
                    cmd.Parameters.AddWithValue("@Username", textusername.Text);
                    cmd.Parameters.AddWithValue("@Password", textpassword.Text);
                    cmd.Parameters.AddWithValue("@RegistrationDate",DateTime.Now);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Successfully saved");

                   

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception Message: " + ex.Message);
                // If Track and source needed
                //+ "\n\nException Source: " +ex.Source + "\n\nException Stack Trace:"+ ex.StackTrace
            }
        }
    }
}
