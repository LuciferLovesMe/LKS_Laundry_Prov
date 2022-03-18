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
    public partial class PackageTransaction : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataReader reader;
        int idCust, idPack, idTrans;
        public PackageTransaction()
        {
            InitializeComponent();
            loadPackage();

            lbltime.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy");
            lblname.Text = Model.name;
            lblid.Text = "ID Transaction : " + getId().ToString();
        }

        void loadPackage()
        {
            command = new SqlCommand("select * from package", connection);
            comboBox1.ValueMember = "id_package";
            comboBox1.DisplayMember = "name_package";
            comboBox1.DataSource = Command.getdata(command);
        }

        int getId()
        {
            command = new SqlCommand("select id_header_Transaction from header_transaction order by id_header_transaction desc", connection);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            int i = reader.GetInt32(0) + 1;
            connection.Close();
            return i;
        }

        int getPrice()
        {
            command = new SqlCommand("select price_package from package where id_package = " + comboBox1.SelectedValue, connection);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            int p = reader.GetInt32(0);
            connection.Close();
            return p;
        }

        int getEst()
        {
            command = new SqlCommand("select duration_package from package where id_package = " + comboBox1.SelectedValue, connection);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            int p = reader.GetInt32(0);
            connection.Close();
            return p;
        }

        bool val()
        {
            if (comboBox1.Text.Length < 1 || numericUpDown1.Value < 1)
            {
                MessageBox.Show("Please select a service and insert the unit value!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if (idCust == 0)
            {
                MessageBox.Show("Please select a customer!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        int getTotal()
        {
            int t = 0;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                t += Convert.ToInt32(dataGridView1.Rows[i].Cells[5].Value);
            }

            return t;
        }

        int getHours()
        {
            int t = 0;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                t += Convert.ToInt32(dataGridView1.Rows[i].Cells[4].Value);
            }

            return t;
        }

        void clear()
        {
            idCust = 0;
            dataGridView1.Rows.Clear();
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            numericUpDown1.Value = 0;

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

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            command = new SqlCommand("select top(1) * from customer where phone_number_customer like '%' +@params+ '%' order by id_customer desc", connection);
            command.Parameters.AddWithValue("@params", textBox2.Text);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            if (reader.HasRows)
            {
                idCust = reader.GetInt32(0);
                textBox3.Text = reader.GetString(1);
                textBox4.Text = reader.GetString(3);
                connection.Close();
            }
            else
            {
                idCust = 0;
                textBox3.Text = "";
                textBox4.Text = "";
                connection.Close();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            AddCustomer add = new AddCustomer();
            add.Show();
            Se.s = 2;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (val())
            {
                int row = dataGridView1.Rows.Add();
                dataGridView1.Rows[row].Cells[0].Value = comboBox1.SelectedValue;
                dataGridView1.Rows[row].Cells[1].Value = comboBox1.Text;
                dataGridView1.Rows[row].Cells[2].Value = getPrice();
                dataGridView1.Rows[row].Cells[3].Value = numericUpDown1.Value;
                dataGridView1.Rows[row].Cells[4].Value = getEst();
                dataGridView1.Rows[row].Cells[5].Value = numericUpDown1.Value * getPrice();

                lbltotal.Text = getTotal().ToString();
                lblest.Text = getHours().ToString();
            }
        }

        private void btn_remove_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Selected)
            {
                dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);

                lbltotal.Text = getTotal().ToString();
                lblest.Text = getHours().ToString();
            }
            else
                MessageBox.Show("Please select an item", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Selected = true;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 0 && idCust > 0)
            {
                command = new SqlCommand("insert into header_transaction values(" + Model.id + ", " + idCust + ", getdate(), null)", connection);
                try
                {
                    connection.Open();
                    Command.exec(command);
                    connection.Close();

                    command = new SqlCommand("select top(1) id_header_Transaction from header_transaction order by id_header_Transaction desc", connection);
                    connection.Open();
                    reader = command.ExecuteReader();
                    reader.Read();
                    idTrans = reader.GetInt32(0);
                    connection.Close();

                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        command = new SqlCommand("insert into detail_transaction values(null, " + idTrans + ", " + dataGridView1.Rows[i].Cells[0].Value + ", " + dataGridView1.Rows[i].Cells[5].Value + ", " + dataGridView1.Rows[i].Cells[3].Value + ", null)", connection);
                        try
                        {
                            connection.Open();
                            Command.exec(command);
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

                    MessageBox.Show("Successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear();
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
