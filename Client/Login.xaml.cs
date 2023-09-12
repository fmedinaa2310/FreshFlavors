using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace ProyectoDiseño
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }
        private void btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool VerifyCredentials(string userId, string password)
        {
           //string connectionString = "Server=DESKTOP-NR8ON3L;Database=GroceryStore;Integrated Security=true";
            //string connectionString = "Server=DESKTOP-J135DBV;Database=GroceryStore;Integrated Security=true";
            string connectionString = "Server=LAPTOP-INLUF1R9;Database=GroceryStore;Integrated Security=true";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM Clients WHERE ID = @ID AND Contrasena = @Contrasena";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", userId);
                    command.Parameters.AddWithValue("@Contrasena", password);

                    int count = (int)command.ExecuteScalar();

                    return count > 0;
                }
            }
        }


        private void btnLoginUser_Click(object sender, RoutedEventArgs e)
        {
            string user = txt_Identification.Text;
            string password = txt_Password.Password;

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Por favor complete los campos vacíos.");
                return;
            }

            try
            {
                if (VerifyCredentials(user, password))
                {
                    MessageBox.Show("Acceso concedido, disfruta.");
                    Hide();
                    MenuUsuario menuUsuario = new MenuUsuario();
                    menuUsuario.LoadUserData(user);
                    menuUsuario.Show();
                }
                else
                {
                    MessageBox.Show("Acceso denegado");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }


        private void bnt_Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void bnt_Back_Click(object sender, RoutedEventArgs e)
        {
            Hide();

            RegistroUsuario registroUsuario = new RegistroUsuario();
            registroUsuario.Show();
        }
    }
}
