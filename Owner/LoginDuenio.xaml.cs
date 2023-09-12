using ProyectoDiseño.Owner;
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

namespace ProyectoDiseño
{
    /// <summary>
    /// Lógica de interacción para LoginView.xaml
    /// </summary>
    public partial class LoginDuenio : Window
    {
        public LoginDuenio()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void bnt_Minimize_Click(object sender, RoutedEventArgs e)
        {

            WindowState = WindowState.Minimized;
        }

        private void bnt_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private bool VerifyCredentials(string userId, string password)
        {
            // string connectionString = "Server=DESKTOP-NR8ON3L;Database=GroseryStore;Integrated Security=true";
            // string connectionString = "Server=DESKTOP-J135DBV;Database=GroceryStore;Integrated Security=true";
            string connectionString = "Server=LAPTOP-INLUF1R9;Database=GroceryStore;Integrated Security=true";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM Admin WHERE Username = @Username AND Password = @Password";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", userId);
                    command.Parameters.AddWithValue("@Password", password);

                    int count = (int)command.ExecuteScalar();

                    return count > 0;
                }
            }
        }


        private void bnt_Enter_Click(object sender, RoutedEventArgs e)
        {
            string user = txt_User.Text;
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
                    MenuDueno menuUsuario = new MenuDueno();
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

        private void bnt_Back_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            LandingPage landingPage = new LandingPage();
            landingPage.Show();
        }
    }
}
