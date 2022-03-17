using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LKS_Laundry_Prov
{
    class Utils
    {
        public static string conn = @"Data Source=desktop-00eposj;Initial Catalog=LKS_Laundry_Prov;Integrated Security=True";
    }

    class Command
    {
        public static DataTable getdata(SqlCommand command)
        {
            SqlConnection connection = new SqlConnection(Utils.conn);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable data = new DataTable();
            adapter.Fill(data);
            return data;
        }

        public static void exec(SqlCommand command)
        {
            SqlConnection connection = new SqlConnection(Utils.conn);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
        }
    }

    class Model
    {
        public static string name { set; get; }
        public static int id { set; get; }
    }

    class Encrypt
    {
        public static string enc(string data)
        {
            using(SHA256Managed managed = new SHA256Managed())
            {
                return Convert.ToBase64String(managed.ComputeHash(Encoding.UTF8.GetBytes(data)));
            }
        }
    }

    class Se
    {
        public static int s { set; get; }
    }
}
