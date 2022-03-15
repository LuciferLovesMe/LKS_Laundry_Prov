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
    public partial class MasterCustomer : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataReader reader;
        int cond, id;

        public MasterCustomer()
        {
            InitializeComponent();
            loadgrid();
            dis();

            lbltime.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy");
            lblname.Text = Model.name;
        }

        void dis()
        {
            btn_insert.Enabled = true;
            btn_update.Enabled = true;
            btn_delete.Enabled = true;
            btn_save.Enabled = false;
            btn_cancel.Enabled = false;
            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
        }

        void enable()
        {
            btn_insert.Enabled = false;
            btn_update.Enabled = false;
            btn_delete.Enabled = false;
            btn_save.Enabled = true;
            btn_cancel.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox4.Enabled = true;
        }

        void loadgrid()
        {
            command = new SqlCommand("select * from customer where name_customer like '%' + @params + '%' or phone_number_customer like '%' + @params + '%' or address_customer like '%' + @params + '%'", connection);
            command.Parameters.AddWithValue("@params", textBox1.Text);
            dataGridView1.DataSource = Command.getdata(command);
            dataGridView1.Columns[0].Visible = false;

            dataGridView1.Columns[1].HeaderText = "Customer's Name";
            dataGridView1.Columns[2].HeaderText = "Phone Number";
            dataGridView1.Columns[3].HeaderText = "Address";
        }

        void clear()
        {
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
        }

        bool val()
        {
            if(textBox2.TextLength < 1 || textBox3.TextLength <1 || textBox4.TextLength < 1)
            {
                MessageBox.Show("All fields must be filled!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            command = new SqlCommand("select * from customer where phone_number_customer = @p", connection);
            command.Parameters.AddWithValue("@p", textBox3.Text);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                connection.Close();
                MessageBox.Show("Phone number was already in used!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            connection.Close();
            return true;
        }

        bool valUp()
        {
            if (textBox2.TextLength < 1 || textBox3.TextLength < 1 || textBox4.TextLength < 1)
            {
                MessageBox.Show("All fields must be filled!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            command = new SqlCommand("select * from customer where phone_number_customer = @p", connection);
            command.Parameters.AddWithValue("@p", textBox3.Text);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows && reader.GetInt32(0) != id)
            {
                connection.Close();
                MessageBox.Show("Phone number was already in used!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            connection.Close();
            return true;
        }

        private void panel_employee_Click(object sender, EventArgs e)
        {
            MasterEmployee master = new MasterEmployee();
            this.Hide();
            master.Show();
        }

        private void panel_service_Click(object sender, EventArgs e)
        {
            MasterService service = new MasterService();
            this.Hide();
            service.Show();
        }

        private void panel_package_Click(object sender, EventArgs e)
        {
            Package package = new Package();
            this.Hide();
            package.Show();
        }

        private void panel_service_transaction_Click(object sender, EventArgs e)
        {
            ServiceTransaction service = new ServiceTransaction();
            this.Hide();
            service.Show();
        }

        private void panel_package_transaction_Click(object sender, EventArgs e)
        {
            PackageTransaction package = new PackageTransaction();
            this.Hide();
            package.Show();
        }

        private void panel_report_Click(object sender, EventArgs e)
        {
            ViewTransaction viewTransaction = new ViewTransaction();
            this.Hide();
            viewTransaction.Show();
        }

        private void panel_customer_Click(object sender, EventArgs e)
        {
            MasterCustomer master = new MasterCustomer();
            this.Hide();
            master.Show();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btn_insert_Click(object sender, EventArgs e)
        {
            cond = 1;
            enable();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Selected)
            {
                cond = 2;
                enable();
            }
            else
                MessageBox.Show("Select an item", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Selected)
            {
                DialogResult result = MessageBox.Show("Are you sure to delete " + dataGridView1.SelectedRows[0].Cells[1].Value.ToString() + " ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    command = new SqlCommand("delete from customer where id_customer = " + id, connection);
                    try
                    {
                        connection.Open();
                        Command.exec(command);
                        MessageBox.Show("Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear();
                        loadgrid();
                        dis();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            else
                MessageBox.Show("Select an item", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if (cond == 1 && val())
            {
                command = new SqlCommand("insert into customer values(@name, @phone, @address)", connection);
                command.Parameters.AddWithValue("@name", textBox2.Text);
                command.Parameters.AddWithValue("@phone", textBox3.Text);
                command.Parameters.AddWithValue("@address", textBox4.Text);

                try
                {
                    connection.Open();
                    Command.exec(command);
                    MessageBox.Show("Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                    loadgrid();
                    dis();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
            else if (cond == 2 && valUp())
            {
                command = new SqlCommand("update customer set name_customer = @name, phone_number_customer = @phone, address_customer = @address where id_customer = " + id, connection);
                command.Parameters.AddWithValue("@name", textBox2.Text);
                command.Parameters.AddWithValue("@phone", textBox3.Text);
                command.Parameters.AddWithValue("@address", textBox4.Text);

                try
                {
                    connection.Open();
                    Command.exec(command);
                    MessageBox.Show("Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
                    loadgrid();
                    dis();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("" + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            loadgrid();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            dis();
            clear();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            textBox4.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure to logout?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                MainLogin main = new MainLogin();
                this.Hide();
                main.Show();
            }
        }
    }
}
