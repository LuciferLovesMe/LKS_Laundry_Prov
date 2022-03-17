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
    public partial class Package : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataReader reader;
        int id, cond, idP;

        public Package()
        {
            InitializeComponent();
            loadservice();
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
            textBox4.Enabled = false;
            numericUpDown1.Enabled = false;
            numericUpDown3.Enabled = false;
            btn_add.Enabled = false;
            btn_remove.Enabled = false;
            comboBox1.Enabled = false;
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
            numericUpDown1.Enabled = true;
        }

        void loadgrid()
        {
            command = new SqlCommand("select * from package where name_package like '%' +@params+ '%'", connection);
            command.Parameters.AddWithValue("@params", textBox1.Text);
            dataGridView1.DataSource = Command.getdata(command);
            dataGridView1.Columns[0].Visible = false;

            dataGridView1.Columns[1].HeaderText = "Package's Name";
            dataGridView1.Columns[2].HeaderText = "Price";
            dataGridView1.Columns[3].HeaderText = "Description";
            dataGridView1.Columns[4].HeaderText = "Duration";

            dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        }

        void loadgrid2()
        {
            if(id != 0)
            {
                command = new SqlCommand("select Detail_Package.id_Detail_package, service.*, Detail_Package.total_unit_detail_package from detail_package join service on detail_package.id_Service = service.id_service join package on detail_package.id_package = package.id_package where detail_package.id_package = " + id, connection);
                dataGridView2.DataSource = Command.getdata(command);
                dataGridView2.Columns[0].Visible = false;
                dataGridView2.Columns[1].Visible = false;
                dataGridView2.Columns[2].Visible = false;
                dataGridView2.Columns[3].Visible = false;

                dataGridView2.Columns[4].HeaderText = "Service's Name";
                dataGridView2.Columns[5].HeaderText = "Price";
                dataGridView2.Columns[6].HeaderText = "Estimation Durations";
                dataGridView2.Columns[7].HeaderText = "Total Unit";
            }
        }

        void loadservice()
        {
            command = new SqlCommand("select * from service", connection);
            comboBox1.DataSource = Command.getdata(command);
            comboBox1.DisplayMember = "name_Service";
            comboBox1.ValueMember = "id_service";
        }

        void clear()
        {
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            numericUpDown1.Value = 0;
            numericUpDown3.Value = 0;
            dataGridView2.DataSource = null;
            dataGridView2.Rows.Clear();
        }

        bool valIns()
        {
            if(textBox2.TextLength < 1 || textBox3.TextLength < 1 || numericUpDown1.Value < 1 || textBox4.TextLength < 1)
            {
                MessageBox.Show("All fields must be filled!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        bool valDetail()
        {
            if(comboBox1.Text.Length < 1 || numericUpDown3.Value < 1)
            {
                MessageBox.Show("Detail package fields must be filled!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
            id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[0].Value);
            numericUpDown1.Value = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[4].Value);
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
            textBox4.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
            loadgrid2();
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == 8);
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
                comboBox1.Enabled = true;
                numericUpDown3.Enabled = true;
                btn_add.Enabled = true;
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
                    command = new SqlCommand("delete from detail_package where id_package = " + id, connection);
                    try
                    {
                        connection.Open();
                        Command.exec(command);

                        command = new SqlCommand("delete from package where id_package = " + id, connection);
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
            if (cond == 1 && valIns())
            {
                command = new SqlCommand("insert into package values(@name, @price, @desc, @duration)", connection);
                command.Parameters.AddWithValue("@name", textBox2.Text);
                command.Parameters.AddWithValue("@price", Convert.ToInt32(textBox3.Text));
                command.Parameters.AddWithValue("@desc", textBox4.Text);
                command.Parameters.AddWithValue("@duration", numericUpDown1.Value);

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
            else if (cond == 2 && valIns())
            {
                command = new SqlCommand("update package set name_package = @name, price_package = @price, description_package =  @desc, duration_package =  @duration where id_package = " + id, connection);
                command.Parameters.AddWithValue("@name", textBox2.Text);
                command.Parameters.AddWithValue("@price", Convert.ToInt32(textBox3.Text));
                command.Parameters.AddWithValue("@desc", textBox4.Text);
                command.Parameters.AddWithValue("@duration", numericUpDown1.Value);

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
            else if (cond == 3 && valIns() && valDetail())
            {
                command = new SqlCommand("insert into detail_package values(" + comboBox1.SelectedValue + ", " + id + ", " + numericUpDown3.Value + ")", connection);
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

        private void btn_add_Click(object sender, EventArgs e)
        {
            if (valDetail())
            {
                command = new SqlCommand("insert into detail_package values(" + comboBox1.SelectedValue + ", " + id + ", " + numericUpDown3.Value + ")", connection);
                try
                {
                    connection.Open();
                    Command.exec(command);
                    MessageBox.Show("Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    loadgrid2();
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

        private void btn_remove_Click(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentRow.Selected)
            {
                DialogResult result = MessageBox.Show("Are you sure to delete?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if(result == DialogResult.Yes)
                {
                    command = new SqlCommand("delete from detail_package where id_Detail_package = "+idP, connection);
                    try
                    {
                        connection.Open();
                        Command.exec(command);
                        MessageBox.Show("Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        loadgrid2();
                        btn_remove.Enabled = false;
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
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            loadgrid();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView2.CurrentRow.Selected = true;
            btn_remove.Enabled = true;
            idP = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells[0].Value);

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
