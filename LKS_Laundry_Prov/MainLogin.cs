using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LKS_Laundry_Prov
{
    public partial class MainLogin : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataReader reader;

        public MainLogin()
        {
            InitializeComponent();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                textBox2.PasswordChar = '\0';
            else
                textBox2.PasswordChar = '*';
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength < 1 || textBox2.TextLength < 1)
                MessageBox.Show("All fields must be filled!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                command = new SqlCommand("select * from employee where email_employee = @user and password_employee = @pass", connection);
                command.Parameters.AddWithValue("@user", textBox1.Text);
                command.Parameters.AddWithValue("@pass", textBox2.Text);
                connection.Open();
                reader = command.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    Model.name = reader.GetString(3);
                    Model.id = reader.GetInt32(1);
                    connection.Close();
                    MainForm main = new MainForm();
                    this.Hide();
                    main.Show();
                }
                else
                {
                    MessageBox.Show("Can't find user!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    connection.Close();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
