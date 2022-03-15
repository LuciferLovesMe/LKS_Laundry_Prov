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
    public partial class MasterEmployee : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataReader reader;
        int id, cond;

        public MasterEmployee()
        {
            InitializeComponent();
            dis();
            loadgrid();
            loadjob();

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
            textBox5.Enabled = false;
            textBox6.Enabled = false;
            textBox7.Enabled = false;
            textBox8.Enabled = false;
            comboBox1.Enabled = false;
            checkBox1.Enabled = false;
            dateTimePicker1.Enabled = false;
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
            textBox5.Enabled = true;
            textBox6.Enabled = true;
            textBox7.Enabled = true;
            textBox8.Enabled = true;
            comboBox1.Enabled = true;
            checkBox1.Enabled = true;
            dateTimePicker1.Enabled = true;
        }

        void loadgrid()
        {
            command = new SqlCommand("select employee.*, job.name_job from employee join job on job.id_job = employee.id_job where name_employee like '%' + @params + '%'", connection);
            command.Parameters.AddWithValue("@params", textBox1.Text);
            dataGridView1.DataSource = Command.getdata(command);
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].Visible = false;

            dataGridView1.Columns[3].HeaderText = "Employee's Name";
            dataGridView1.Columns[4].HeaderText = "Email";
            dataGridView1.Columns[5].HeaderText = "Address";
            dataGridView1.Columns[6].HeaderText = "Phone Number";
            dataGridView1.Columns[7].HeaderText = "Date of Birth";
            dataGridView1.Columns[8].HeaderText = "Salary";
            dataGridView1.Columns[9].HeaderText = "Job";

            dataGridView1.Columns[8].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        void loadjob()
        {
            command = new SqlCommand("select * from job", connection);
            comboBox1.DisplayMember = "name_job";
            comboBox1.ValueMember = "id_job";
            comboBox1.DataSource = Command.getdata(command);
        }

        void clear()
        {
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            dateTimePicker1.Value = DateTime.Today;
        }

        bool val()
        {
            if(textBox2.TextLength < 1 || textBox3.TextLength < 1 || textBox4.TextLength < 1 || textBox5.TextLength < 1 || textBox6.TextLength < 1 || textBox7.TextLength < 1 || textBox8.TextLength < 1 || dateTimePicker1.Value == null || comboBox1.Text.Length < 1)
            {
                MessageBox.Show("All fields must be filled!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if(dateTimePicker1.Value > DateTime.Now)
            {
                MessageBox.Show("Insert a valid date!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if(textBox6.Text != textBox5.Text)
            {
                MessageBox.Show("Wrong Confirmation Password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            command = new SqlCommand("select * from employee where email_employee = @e", connection);
            command.Parameters.AddWithValue("@e", textBox3.Text);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                connection.Close();
                MessageBox.Show("Email was already in use!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            connection.Close();
            return true;
        }

        bool valUp()
        {
            if (textBox2.TextLength < 1 || textBox3.TextLength < 1 || textBox4.TextLength < 1 || textBox7.TextLength < 1 || textBox8.TextLength < 1 || dateTimePicker1.Value == null || comboBox1.Text.Length < 1)
            {
                MessageBox.Show("All fields must be filled!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (dateTimePicker1.Value > DateTime.Now)
            {
                MessageBox.Show("Insert a valid date!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            command = new SqlCommand("select * from employee where email_employee = @e", connection);
            command.Parameters.AddWithValue("@e", textBox3.Text);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows && reader.GetInt32(1) != id)
            {
                connection.Close();
                MessageBox.Show("Email was already in use!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                textBox5.Enabled = false;
                textBox6.Enabled = false;
            }
            else
                MessageBox.Show("Select an item", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Selected)
            {
                DialogResult result = MessageBox.Show("Are you sure to delete " + dataGridView1.SelectedRows[0].Cells[3].Value.ToString() + " ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            loadgrid();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if(cond == 1 && val())
            {
                command = new SqlCommand("insert into employee values(" + comboBox1.SelectedValue + ", @pass, @name, @email, @address, @phone, @date, " + Convert.ToInt32(textBox7.Text) + ")", connection);
                command.Parameters.AddWithValue("@pass", textBox5.Text);
                command.Parameters.AddWithValue("@name", textBox2.Text);
                command.Parameters.AddWithValue("@email", textBox3.Text);
                command.Parameters.AddWithValue("@address", textBox8.Text);
                command.Parameters.AddWithValue("@phone", textBox4.Text);
                command.Parameters.AddWithValue("@date", dateTimePicker1.Value.Date);

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
            else if (cond == 2&& valUp())
            {
                command = new SqlCommand("update employee set id_job = " + comboBox1.SelectedValue + ", name_employee = @name, email_Employee = @email, address_employee = @address, date_of_birth_employee = @date, salary_employee = " + Convert.ToInt32(textBox7.Text) + " where id = " + id, connection);
                command.Parameters.AddWithValue("@name", textBox2.Text);
                command.Parameters.AddWithValue("@email", textBox3.Text);
                command.Parameters.AddWithValue("@address", textBox8.Text);
                command.Parameters.AddWithValue("@phone", textBox4.Text);
                command.Parameters.AddWithValue("@date", dateTimePicker1.Value.Date);

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

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            dis();
            clear();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                textBox5.PasswordChar = '\0';
            else
                textBox5.PasswordChar = '*';
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[1].Value);
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            textBox4.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
            textBox7.Text = dataGridView1.SelectedRows[0].Cells[8].Value.ToString();
            textBox8.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
            comboBox1.Text = dataGridView1.SelectedRows[0].Cells[9].Value.ToString();
            comboBox1.SelectedValue = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            dateTimePicker1.Value = Convert.ToDateTime(dataGridView1.SelectedRows[0].Cells[7].Value);
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
