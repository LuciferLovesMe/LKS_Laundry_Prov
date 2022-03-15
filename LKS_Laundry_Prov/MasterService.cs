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
    public partial class MasterService : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataReader reader;
        int id, cond;

        public MasterService()
        {
            InitializeComponent();
            loadunit();
            loadcate();
            dis();
            loadgrid();
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
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            numericUpDown1.Enabled = false;
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
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            numericUpDown1.Enabled = true;
        }

        void loadunit()
        {
            command = new SqlCommand("select * from category", connection);
            comboBox1.DataSource = Command.getdata(command);
            comboBox1.ValueMember = "id_category";
            comboBox1.DisplayMember = "name_category";
        }

        void loadcate()
        {
            command = new SqlCommand("select * from category", connection);
            comboBox2.DataSource = Command.getdata(command);
            comboBox2.ValueMember = "id_category";
            comboBox2.DisplayMember = "name_category";
        }

        void loadgrid()
        {
            command = new SqlCommand("select service.*, unit.name_unit, category.name_Category from service join category on service.id_category = category.id_category join unit on service.id_unit = unit.id_unit where name_service like '%' + @name + '%'", connection);
            command.Parameters.AddWithValue("@name", textBox1.Text);
            dataGridView1.DataSource = Command.getdata(command);
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].Visible = false;

            dataGridView1.Columns[3].HeaderText = "Service's Name";
            dataGridView1.Columns[4].HeaderText = "Price per Unit";
            dataGridView1.Columns[5].HeaderText = "Estimation Duration";
            dataGridView1.Columns[6].HeaderText = "Unit Name";
            dataGridView1.Columns[7].HeaderText = "Category";

            dataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        void clear()
        {
            textBox2.Text = "";
            textBox3.Text = "";
            numericUpDown1.Value = 0;

        }

        bool val()
        {
            if(textBox2.TextLength < 1 || textBox3.TextLength < 1 || numericUpDown1.Value < 1 || comboBox1.Text.Length < 1 || comboBox2.Text.Length < 1)
            {
                MessageBox.Show("All fields must be filled", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            loadgrid();
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
                DialogResult result = MessageBox.Show("Are you sure to delete " + dataGridView1.SelectedRows[0].Cells[3].Value.ToString() + " ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(result == DialogResult.Yes)
                {
                    command = new SqlCommand("delete from service where id_service = " + id, connection);
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
            if(cond == 1 && val())
            {
                command = new SqlCommand("insert into service values(" + comboBox1.SelectedValue + ", " + comboBox2.SelectedValue + ", @name, " + Convert.ToInt32(textBox3.Text) + ", " + numericUpDown1.Value + ")", connection);
                command.Parameters.AddWithValue("@name", textBox2.Text);
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
            else if(cond == 2 && val())
            {
                command = new SqlCommand("update service set id_category = " + comboBox1.SelectedValue + ", id_unit = " + comboBox2.SelectedValue + ", name_service = @name, price_unit_Service = " + Convert.ToInt32(textBox3.Text) + ", estimation_duration_Service = " + numericUpDown1.Value + " where id_Service = " + id, connection);
                command.Parameters.AddWithValue("@name", textBox2.Text);
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

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == 8);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[2].Value);
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            comboBox1.Text = dataGridView1.SelectedRows[0].Cells[7].Value.ToString();
            comboBox1.SelectedValue = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            comboBox2.SelectedValue = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[1].Value);
            comboBox2.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
            numericUpDown1.Value = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[5].Value);
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
