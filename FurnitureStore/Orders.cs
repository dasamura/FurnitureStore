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
    public partial class Orders : Form
    {
        private int lastOrderId = 0;
        private int lastAssemblyId = 0;

        public Orders()
        {
            InitializeComponent();
        }

        private void Orders_Load(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {
            this.Hide();
            FurnitureStore furnitureStore = new FurnitureStore();
            furnitureStore.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (userIdField.Text == "")
            {
                MessageBox.Show("Введіть ваш ID");
                return;
            }
            if (adressField.Text == "")
            {
                MessageBox.Show("Введіть адресу");
                return;
            }

            DateTime orderDate = DateTime.Now;
            string orderStatus = "Замовлення прийнято";
            DB db = new DB();

            MySqlCommand command;

            if (string.IsNullOrEmpty(productIdField.Text) && string.IsNullOrEmpty(projectIdField.Text))
            {
                MessageBox.Show("Введіть ID продукту чи ID проєкту");
                return;
            }
            else if (string.IsNullOrEmpty(productIdField.Text))
            {
                if (string.IsNullOrEmpty(projectIdField.Text))
                {
                    MessageBox.Show("Введіть ID проєкту");
                    return;
                }
                // productIdField.Text порожнє
                command = new MySqlCommand("INSERT INTO `orders` (`customer_id`, `project_id`, `order_date`, `order_status`, `order_delivery_address`) VALUES (@cus_id, @project_id, @date, @status, @address)", db.getConnection());
                command.Parameters.Add("@cus_id", MySqlDbType.Int32).Value = userIdField.Text;
                command.Parameters.Add("@project_id", MySqlDbType.VarChar).Value = projectIdField.Text;
            }
            else if (string.IsNullOrEmpty(projectIdField.Text))
            {
                if (string.IsNullOrEmpty(productIdField.Text))
                {
                    MessageBox.Show("Введіть ID продукту");
                    return;
                }
                // projectIdField.Text порожнє
                command = new MySqlCommand("INSERT INTO `orders` (`customer_id`, `product_id`, `order_date`, `order_status`, `order_delivery_address`) VALUES (@cus_id, @product_id, @date, @status, @address)", db.getConnection());
                command.Parameters.Add("@cus_id", MySqlDbType.Int32).Value = userIdField.Text;
                command.Parameters.Add("@product_id", MySqlDbType.VarChar).Value = productIdField.Text;
            }
            else
            {
                // projectIdField.Text і productIdField.Text не порожні
                command = new MySqlCommand("INSERT INTO `orders` (`customer_id`, `product_id`, `project_id`, `order_date`, `order_status`, `order_delivery_address`) VALUES (@cus_id, @product_id, @project_id, @date, @status, @address)", db.getConnection());
                command.Parameters.Add("@cus_id", MySqlDbType.Int32).Value = userIdField.Text;
                command.Parameters.Add("@product_id", MySqlDbType.VarChar).Value = productIdField.Text;
                command.Parameters.Add("@project_id", MySqlDbType.VarChar).Value = projectIdField.Text;
            }

            command.Parameters.Add("@date", MySqlDbType.Date).Value = orderDate;
            command.Parameters.Add("@status", MySqlDbType.VarChar).Value = orderStatus;
            command.Parameters.Add("@address", MySqlDbType.VarChar).Value = adressField.Text;

            db.OpenConnection();

            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Замовлення прийнято!");
                lastOrderId = (int)command.LastInsertedId; // Зберігаємо останнє ID замовлення
            }
            else
            {
                MessageBox.Show("Замовлення не прийнято!");
            }

            db.CloseConnection();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (lastOrderId == 0)
            {
                MessageBox.Show("Зробіть спочатку замовлення");
                return;
            }

            int amountOfProduct;
            if (!int.TryParse(amountOfProductField.Text, out amountOfProduct))
            {
                amountOfProduct = 0; // Якщо користувач не замовив продукт, встановлюємо значення 0
            }

            int amountOfProject;
            if (!int.TryParse(amountOfProjectField.Text, out amountOfProject))
            {
                amountOfProject = 0; // Якщо користувач не замовив проєкт, встановлюємо значення 0
            }

            // Перевіряємо, які товари замовлено і встановлюємо значення для відповідних стовпців
            int assemblyAmountProduct = 0;
            int assemblyAmountProject = 0;
            if (amountOfProduct > 0 && amountOfProject > 0)
            {
                assemblyAmountProduct = amountOfProduct;
                assemblyAmountProject = amountOfProject;
            }
            else if (amountOfProduct > 0)
            {
                assemblyAmountProduct = amountOfProduct;
            }
            else if (amountOfProject > 0)
            {
                assemblyAmountProject = amountOfProject;
            }
            else
            {
                MessageBox.Show("Введіть кількість продукту або проєктів");
                return;
            }

            // Отримуємо з'єднання з базою даних
            DB db = new DB();
            MySqlConnection connection = db.getConnection();
            connection.Open();

            try
            {
                // Виконуємо INSERT-запит для вставки даних у таблицю `assembly`
                MySqlCommand command = new MySqlCommand("INSERT INTO `assembly` (`order_id`, `assembly_amount_product`, `assembly_amount_project`) VALUES (@order_id, @amount_product, @amount_project)", connection);
                command.Parameters.AddWithValue("@order_id", lastOrderId);
                command.Parameters.AddWithValue("@amount_product", assemblyAmountProduct);
                command.Parameters.AddWithValue("@amount_project", assemblyAmountProject);

                if (command.ExecuteNonQuery() == 1)
                {
                    // Отримуємо id останнього вставленого запису
                    lastAssemblyId = (int)command.LastInsertedId;

                    MessageBox.Show("Дані успішно відправлені до бази даних!");
                }
                else
                {
                    MessageBox.Show("Помилка при відправленні даних до бази даних!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (lastOrderId == 0)
            {
                MessageBox.Show("Ще не було здійснено жодного замовлення");
            }
            else
            {
                MessageBox.Show("Останнє ID замовлення: " + lastOrderId.ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (lastAssemblyId == 0)
            {
                MessageBox.Show("Зробіть спочатку комплектування");
                return;
            }

            string collectionType = comboBox1.SelectedItem.ToString();

            // Отримуємо з'єднання з базою даних
            DB db = new DB();
            MySqlConnection connection = db.getConnection();
            connection.Open();

            try
            {
                // Виконуємо INSERT-запит для вставки даних у таблицю `collection`
                MySqlCommand command = new MySqlCommand("INSERT INTO `collection` (`assembly_id`, `collection_assembly_on_site`) VALUES (@assembly_id, @collection_type)", connection);
                command.Parameters.AddWithValue("@assembly_id", lastAssemblyId);
                command.Parameters.AddWithValue("@collection_type", collectionType);

                if (command.ExecuteNonQuery() == 1)
                {
                    MessageBox.Show("Дані успішно відправлені до бази даних!");
                }
                else
                {
                    MessageBox.Show("Помилка при відправленні даних до бази даних!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (lastAssemblyId == 0)
            {
                MessageBox.Show("Ще не було зроблено комплектування");
            }
            else
            {
                MessageBox.Show("Останнє id комплектування: " + lastAssemblyId);
            }
        }


        private int getLastCollectionId()
        {
            int lastCollectionId = 0;

            // Отримуємо з'єднання з базою даних
            DB db = new DB();
            MySqlConnection connection = db.getConnection();
            connection.Open();

            try
            {
                // Виконуємо SELECT-запит для отримання останнього `collection_id`
                MySqlCommand command = new MySqlCommand("SELECT MAX(`collection_id`) FROM `collection`", connection);

                object result = command.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    lastCollectionId = Convert.ToInt32(result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return lastCollectionId;
        }
        private void button7_Click(object sender, EventArgs e)
        {
            int lastCollectionId = getLastCollectionId();

            if (lastCollectionId == 0)
            {
                MessageBox.Show("Ще не було зроблено збірки");
            }
            else
            {
                MessageBox.Show("Останнє id збірки: " + lastCollectionId);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            int lastCollectionId = getLastCollectionId();

            if (lastCollectionId == 0)
            {
                MessageBox.Show("Ще не було зроблено збірки");
                return;
            }

            DateTime deliveryDate = DateTime.Now.AddDays(new Random().Next(3, 5));
            string deliveryStatus = "Готується до доставки";

            // Отримуємо з'єднання з базою даних
            DB db = new DB();
            MySqlConnection connection = db.getConnection();
            connection.Open();

            try
            {
                // Виконуємо INSERT-запит для додавання запису в таблицю `delivery`
                MySqlCommand command = new MySqlCommand("INSERT INTO `delivery` (`collection_id`, `delivery_date`, `delivery_status`) VALUES (@collection_id, @delivery_date, @delivery_status)", connection);
                command.Parameters.AddWithValue("@collection_id", lastCollectionId);
                command.Parameters.AddWithValue("@delivery_date", deliveryDate);
                command.Parameters.AddWithValue("@delivery_status", deliveryStatus);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // Отримуємо дані про продукт або проєкт

                    string message = "Запис про доставку успішно додано\n\n";
                    message += "Статус доставки: " + deliveryStatus + "\n";
                    message += "Приблизна дата доставки: " + deliveryDate.ToString("dd/MM/yyyy");

                    MessageBox.Show(message);
                }
                else
                {
                    MessageBox.Show("Помилка при додаванні запису про доставку");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DB db = new DB();
            MySqlCommand command = new MySqlCommand(@"SELECT
                                                c.customer_id, c.customer_first_name, c.customer_last_name, c.customer_adress, c.customer_phone_number, c.customer_email,
                                                p.product_id, p.product_name, p.product_description, p.product_price, p.product_availability,
                                                pj.project_id, pj.project_name, pj.project_description, pj.project_price,
                                                o.order_id, o.order_date, o.order_status, o.order_delivery_address,
                                                a.assembly_amount_product, a.assembly_amount_project,
                                                co.collection_assembly_on_site,
                                                d.delivery_id, d.delivery_date, d.delivery_status
                                            FROM
                                                customers c
                                            INNER JOIN orders o ON c.customer_id = o.customer_id
                                            LEFT JOIN products p ON o.product_id = p.product_id
                                            LEFT JOIN design_projects pj ON o.project_id = pj.project_id
                                            LEFT JOIN assembly a ON o.order_id = a.order_id
                                            LEFT JOIN collection co ON a.assembly_id = co.assembly_id
                                            LEFT JOIN delivery d ON co.collection_id = d.collection_id
                                            WHERE
                                                o.order_id = @order_id", db.getConnection());
            command.Parameters.Add("@order_id", MySqlDbType.Int32).Value = lastOrderId; // Останній order_id

            try
            {
                db.OpenConnection();
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // Отримання даних з результуючого набору
                    int customerId = reader.GetInt32("customer_id");
                    string customerFirstName = reader.GetString("customer_first_name");
                    string customerLastName = reader.GetString("customer_last_name");
                    string customerAddress = reader.GetString("customer_adress");
                    string customerPhoneNumber = reader.GetString("customer_phone_number");
                    string customerEmail = reader.GetString("customer_email");

                    int productId = reader.GetInt32("product_id");
                    string productName = reader.GetString("product_name");
                    string productDescription = reader.GetString("product_description");
                    decimal productPrice = reader.GetDecimal("product_price");
                    string productAvailability = reader.GetString("product_availability");

                    int projectId = reader.GetInt32("project_id");
                    string projectName = reader.GetString("project_name");
                    string projectDescription = reader.GetString("project_description");
                    decimal projectPrice = reader.GetDecimal("project_price");

                    int orderId = reader.GetInt32("order_id");
                    DateTime orderDate = reader.GetDateTime("order_date");
                    string orderStatus = reader.GetString("order_status");
                    string orderDeliveryAddress = reader.GetString("order_delivery_address");

                    int assemblyAmountProduct = reader.GetInt32("assembly_amount_product");
                    int assemblyAmountProject = reader.GetInt32("assembly_amount_project");

                    string collectionAssemblyOnSite = reader.GetString("collection_assembly_on_site");

                    int deliveryId = reader.GetInt32("delivery_id");
                    DateTime deliveryDate = reader.GetDateTime("delivery_date");
                    string deliveryStatus = reader.GetString("delivery_status");

                    // Формування повідомлення зі зчитаними даними
                    string message = "Інформація про замовлення:\n\n";
                    message += $"Ідентифікатор клієнта: {customerId}\n";
                    message += $"Ім'я клієнта: {customerFirstName} {customerLastName}\n";
                    message += $"Адреса клієнта: {customerAddress}\n";
                    message += $"Номер телефону клієнта: {customerPhoneNumber}\n";
                    message += $"Email клієнта: {customerEmail}\n\n";

                    if (productId != 0)
                    {
                        message += "Інформація про продукт:\n";
                        message += $"Ідентифікатор продукту: {productId}\n";
                        message += $"Назва продукту: {productName}\n";
                        message += $"Опис продукту: {productDescription}\n";
                        message += $"Ціна продукту: {productPrice}\n";
                        message += $"Наявність продукту: {productAvailability}\n\n";
                    }

                    if (projectId != 0)
                    {
                        message += "Інформація про проєкт:\n";
                        message += $"Ідентифікатор проєкту: {projectId}\n";
                        message += $"Назва проєкту: {projectName}\n";
                        message += $"Опис проєкту: {projectDescription}\n";
                        message += $"Ціна проєкту: {projectPrice}\n\n";
                    }

                    message += "Інформація про замовлення:\n";
                    message += $"Ідентифікатор замовлення: {orderId}\n";
                    message += $"Дата замовлення: {orderDate}\n";
                    message += $"Статус замовлення: {orderStatus}\n";
                    message += $"Адреса доставки: {orderDeliveryAddress}\n\n";

                    message += "Інформація про комплектування:\n";
                    message += $"Кількість продуктів: {assemblyAmountProduct}\n";
                    message += $"Кількість проєктів: {assemblyAmountProject}\n\n";

                    message += "Інформація про збірку:\n";
                    message += $"Місце збірки : {collectionAssemblyOnSite}\n\n";

                    message += "Інформація про доставку:\n";
                    message += $"Ідентифікатор доставки: {deliveryId}\n";
                    message += $"Дата доставки: {deliveryDate}\n";
                    message += $"Статус доставки: {deliveryStatus}\n";

                    MessageBox.Show(message, "Інформація про замовлення");
                }
                else
                {
                    MessageBox.Show("Запис не знайдено");
                }

                reader.Close();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Помилка: " + ex.Message);
            }
            finally
            {
                db.CloseConnection();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button10_MouseEnter(object sender, EventArgs e)
        {
            button10.ForeColor = Color.Red;
        }

        private void button10_MouseLeave(object sender, EventArgs e)
        {
            button10.ForeColor = Color.Black;
        }

        private void button9_MouseEnter(object sender, EventArgs e)
        {
            button2.ForeColor = Color.Blue;
        }

        private void button9_MouseLeave(object sender, EventArgs e)
        {
            button2.ForeColor = Color.Black;
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            this.Hide();
            FurnitureStore furnitureStore = new FurnitureStore();
            furnitureStore.Show();
        }
    }
}
