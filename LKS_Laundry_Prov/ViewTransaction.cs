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
    public partial class ViewTransaction : Form
    {
        SqlConnection connection = new SqlConnection(Utils.conn);
        SqlCommand command;
        SqlDataReader reader;
        int idTrans, idDetail;

        public ViewTransaction()
        {
            InitializeComponent();
            loadgrid();
            lbltime.Text = DateTime.Now.ToString("dddd, dd MMMM yyyy");
            lblname.Text = Model.name;
        }

        void loadgrid()
        {
            command = new SqlCommand("select header_Transaction.*, employee.name_employee, customer.name_customer from header_Transaction join customer on customer.id_Customer = header_Transaction.id_customer join employee on header_transaction.id_employee = employee.id order by id_header_transaction desc", connection);
            dataGridView1.DataSource = Command.getdata(command);
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].Visible = false;

            dataGridView1.Columns[3].DefaultCellStyle.Format = "dddd, dd-MM-yyyy HH:mm:ss";
            dataGridView1.Columns[4].DefaultCellStyle.Format = "dddd, dd-MM-yyyy HH:mm:ss";

            dataGridView1.Columns[3].HeaderText = "Transaction Date";
            dataGridView1.Columns[4].HeaderText = "Complete Date";
            dataGridView1.Columns[5].HeaderText = "Employee's Name";
            dataGridView1.Columns[6].HeaderText = "Customer's Name";
        }

        void loadgrid2()
        {
            command = new SqlCommand("select detail_transaction.*, package.name_package, service.name_Service from detail_transaction join package on package.id_package = detail_transaction.id_package join service on service.id_Service = detail_transaction.id_Service where id_header_transaction = " + idTrans, connection);
            dataGridView2.DataSource = Command.getdata(command);
            dataGridView2.Columns[0].Visible = false;
            dataGridView2.Columns[1].Visible = false;
            dataGridView2.Columns[2].Visible = false;
            dataGridView2.Columns[3].Visible = false;

            dataGridView2.Columns[4].DefaultCellStyle.Format = "dddd, dd-MM-yyyy HH:mm:ss";

            dataGridView2.Columns[4].HeaderText = "Total Price";
            dataGridView2.Columns[5].HeaderText = "Total Units";
            dataGridView2.Columns[6].HeaderText = "Complete Date";
            dataGridView2.Columns[7].HeaderText = "Package Name";
            dataGridView2.Columns[8].HeaderText = "Service Name";
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
            idTrans = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells[2].Value);

            loadgrid2();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView2.CurrentRow.Selected = true;
            idDetail = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells[2].Value);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentRow.Selected)
            {
                command = new SqlCommand("update detail_transaction set complete_datetime_detail_Transaction = getdate() where id_detail_transaction = " + idDetail, connection);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

                command = new SqlCommand("select * from detail_transaction where id_header_Transaction = " + idTrans + " and complete_datetime_Detail_transaction is not null", connection);
                reader = command.ExecuteReader();
                reader.Read();
                if (!reader.HasRows)
                {
                    connection.Close();
                    command = new SqlCommand("update header_Transaction set complete_date_time_header_transaction = getdate() where id_header_transaction = " + idTrans, connection);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                connection.Close();

                MessageBox.Show("Successfully Completed!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                loadgrid();
                dataGridView2.DataSource = null;
                dataGridView2.Rows.Clear();
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
