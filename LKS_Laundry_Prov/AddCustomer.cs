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
    public partial class AddCustomer : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataReader reader;

        public AddCustomer()
        {
            InitializeComponent();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if(Se.s == 1)
            {
                this.Hide();
                ServiceTransaction service = new ServiceTransaction();
                service.Show();
            }
            else if (Se.s == 2)
            {
                this.Hide();
                PackageTransaction service = new PackageTransaction();
                service.Show();
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || char.IsSymbol(e.KeyChar) || e.KeyChar == 8);
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || char.IsSymbol(e.KeyChar) || char.IsWhiteSpace(e.KeyChar) || e.KeyChar == 8);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength > 0 || textBox2.TextLength > 0 || textBox3.TextLength > 0)
            {
                command = new SqlCommand("select * from customer where phone_number_customer = @params", connection);
                command.Parameters.AddWithValue("@params", textBox1.Text);
                connection.Open();
                reader = command.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    connection.Close();
                    MessageBox.Show("Phone number was already in use", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    connection.Close();
                    command = new SqlCommand("insert into customer values(@phone, @name, @address)", connection);
                    command.Parameters.AddWithValue("@phone", textBox3.Text);
                    command.Parameters.AddWithValue("@name", textBox2.Text);
                    command.Parameters.AddWithValue("@address", textBox1.Text);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                        MessageBox.Show("Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        if (Se.s == 1)
                        {
                            this.Hide();
                            ServiceTransaction service = new ServiceTransaction();
                            service.Show();
                        }
                        else if (Se.s == 2)
                        {
                            this.Hide();
                            PackageTransaction service = new PackageTransaction();
                            service.Show();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(""+ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
    }
}
