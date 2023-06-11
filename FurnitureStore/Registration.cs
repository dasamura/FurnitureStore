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
    public partial class Registration : Form
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void Registration_Load(object sender, EventArgs e)
        {

        }

        private void Authorization_Click(object sender, EventArgs e)
        {
            this.Hide();
            Authorization authorization = new Authorization();
            authorization.Show();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void RegistButton_Click(object sender, EventArgs e)
        {
            if (userNameField.Text == "")
            {
                MessageBox.Show("Введіть ім'я"); 
                return;
            }
            if (userLastNameField.Text == "")
            {
                MessageBox.Show("Введіть прізвище");
                return;
            }
            if (userAdressField.Text == "")
            {
                MessageBox.Show("Введіть адресу");
                return;
            }
            if (userNumberField.Text == "")
            {
                MessageBox.Show("Введіть номер телефону");
                return;
            }
            if (userEmailField.Text == "")
            {
                MessageBox.Show("Введіть пошту");
                return;
            }

            if (checkUser())
                return;

            DB db = new DB();
            MySqlCommand command = new MySqlCommand("insert into `customers`(`customer_first_name`,`customer_last_name`,`customer_adress`,`customer_phone_number`,`customer_email`) values (@name,@last_name,@adress,@number,@email)", db.getConnection());
            command.Parameters.Add("@name", MySqlDbType.VarChar).Value = userNameField.Text;
            command.Parameters.Add("@last_name", MySqlDbType.VarChar).Value = userLastNameField.Text;
            command.Parameters.Add("@adress", MySqlDbType.VarChar).Value = userAdressField.Text;
            command.Parameters.Add("@number", MySqlDbType.VarChar).Value = userNumberField.Text;
            command.Parameters.Add("@email", MySqlDbType.VarChar).Value = userEmailField.Text;


            db.OpenConnection();

            if (command.ExecuteNonQuery() == 1)
                MessageBox.Show("Акаут створений!");
            else
                MessageBox.Show("Акаунт не створений!");

            db.CloseConnection();

        }

        public Boolean checkUser()
        {

            DB db = new DB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("select * from `customers` where `customer_email` = @uE", db.getConnection());
            command.Parameters.Add("@uE", MySqlDbType.VarChar).Value = userEmailField.Text;


            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Клієт с такою поштою вже є,введіть іншу");
                return true;
            }

            else
            {
                return false;
            }
                

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void Authorization_MouseEnter(object sender, EventArgs e)
        {
            Authorization.ForeColor = Color.Gray;
        }

        private void Authorization_MouseLeave(object sender, EventArgs e)
        {
            Authorization.ForeColor = Color.Black;
        }

        private void button1_MouseEnter(object sender, EventArgs e)
        {
            button1.ForeColor = Color.Red;
        }

        private void button1_DragLeave(object sender, EventArgs e)
        {

        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
            button1.ForeColor = Color.Black;
        }

        private void button1_Click(object sender, EventArgs e)
        {
           Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            button2.ForeColor = Color.Blue;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
            button2.ForeColor = Color.Black;
        }
    }
}
