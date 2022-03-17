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
    public partial class ServiceTransaction : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataReader reader;
        int idCust, idServ, idTrans;

        public ServiceTransaction()
        {
            InitializeComponent();
            loadservice();

            lbltime.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy");
            lblname.Text = Model.name;
        }

        void loadservice()
        {
            command = new SqlCommand("Select * from service ", connection);
            comboBox1.DisplayMember = "name_service";
            comboBox1.ValueMember = "id_service";
            comboBox1.DataSource = Command.getdata(command);
        }

        int getPrice()
        {
            command = new SqlCommand("select price_unit_Service from service where id_service = " + comboBox1.SelectedValue, connection);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            int p = reader.GetInt32(0);
            connection.Close();
            return p;
        }

        int getEst()
        {
            command = new SqlCommand("select estimation_duration_Service from service where id_service = " + comboBox1.SelectedValue, connection);
            connection.Open();
            reader = command.ExecuteReader();
            reader.Read();
            int p = reader.GetInt32(0);
            connection.Close();
            return p;
        }

        bool val()
        {
            if(comboBox1.Text.Length < 1 || numericUpDown1.Value < 1)
            {
                MessageBox.Show("Please select a service and insert the unit value!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if(idCust == 0)
            {
                MessageBox.Show("Please select a customer!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            AddCustomer customer = new AddCustomer();
            customer.Show();
            Se.s = 1;
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
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            Application.Exit();
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
