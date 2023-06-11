using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FurnitureStore
{
    public partial class FurnitureStore : Form
    {
        public FurnitureStore()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {
          
        }
        private static string userName;
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                label2.Text = userName; 
            }
        }

        private static string userEmail;

        public string UserEmail
        {
            get { return userEmail; }
            set { userEmail = value; }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            DB db = new DB();
            DataTable table = new DataTable();

            string query = "SELECT * FROM design_projects";
            MySqlCommand command = new MySqlCommand(query, db.getConnection());
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);

            adapter.Fill(table);

            dataGridView1.DataSource = table;

        }

        private void label4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Authorization authorization = new Authorization();
            authorization.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DB db = new DB();
            DataTable table = new DataTable();

            string query = "SELECT * FROM Products";
            MySqlCommand command = new MySqlCommand(query, db.getConnection());
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);

            adapter.Fill(table);

            dataGridView2.DataSource = table;
        }

        private void FurnitureStore_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Orders order = new Orders();
            order.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(UserEmail))
            {
                MessageBox.Show("Такої пошти немає в базі."); 
                return;
            }

            DB db = new DB();
            DataTable table = new DataTable();

            string query = "SELECT customer_id FROM Customers WHERE customer_email = @uE";
            MySqlCommand command = new MySqlCommand(query, db.getConnection());
            command.Parameters.Add("@uE", MySqlDbType.VarChar).Value = UserEmail; 
            MySqlDataAdapter adapter = new MySqlDataAdapter(command);

            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                string customerId = table.Rows[0]["customer_id"].ToString();
                MessageBox.Show("ID користувача: " + customerId);
            }
            else
            {
                MessageBox.Show("Користувач не знайдений.");
            }

        }

        private void label4_MouseEnter(object sender, EventArgs e)
        {
            label4.ForeColor = Color.Gray;
        }

        private void label4_MouseLeave(object sender, EventArgs e)
        {
            label4.ForeColor = Color.Black;
        }

        private void button6_MouseEnter(object sender, EventArgs e)
        {
            button6.ForeColor = Color.Red;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button6_MouseLeave(object sender, EventArgs e)
        {
            button6.ForeColor = Color.Black;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button5_MouseEnter(object sender, EventArgs e)
        {
            button5.ForeColor = Color.Blue;
        }

        private void button5_MouseLeave(object sender, EventArgs e)
        {
            button5.ForeColor = Color.Black;
        }
    }
}
