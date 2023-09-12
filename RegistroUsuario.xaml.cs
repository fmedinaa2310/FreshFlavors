using LiveCharts.Wpf;
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
    /// Lógica de interacción para Registro.xaml
    /// </summary>
    public partial class RegistroUsuario : Window
    {
        private string connectionString = "Server=DESKTOP-J135DBV;Database=GroceryStore;Integrated Security=true";

        public RegistroUsuario()
        {
            InitializeComponent();
        }

        private void bnt_Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btn_Exit_Registro_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btnRegistrarUsuario(object sender, RoutedEventArgs e)
        {
            string password1 = txt_Password.Password;
            string password2 = txtConfimationPassword.Password;
            string name = txtName.Text;
            string surname = txtFirstLastName.Text;
            string secondSurname = txtSecondSurname.Text;
            string id = txtIdentification.Text;

            if (string.IsNullOrEmpty(password1) || string.IsNullOrEmpty(password2) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(secondSurname) || string.IsNullOrEmpty(id))
            {
                MessageBox.Show("Por favor complete los campos vacíos.");
                return;
            }

            if (password1 != password2)
            {
                MessageBox.Show("Contraseñas diferentes");
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO Clients (ID, Nombre, PrimerApellido, SegundoApellido, Contrasena) VALUES (@ID, @Nombre, @PrimerApellido, @SegundoApellido, @Contrasena)";

                    using (SqlCommand command = new SqlCommand(insertQuery, connection))
                    {
                        command.Parameters.Add("@ID", System.Data.SqlDbType.NVarChar).Value = id;
                        command.Parameters.Add("@Nombre", System.Data.SqlDbType.NVarChar).Value = name;
                        command.Parameters.Add("@PrimerApellido", System.Data.SqlDbType.NVarChar).Value = surname;
                        command.Parameters.Add("@SegundoApellido", System.Data.SqlDbType.NVarChar).Value = secondSurname;
                        command.Parameters.Add("@Contrasena", System.Data.SqlDbType.NVarChar).Value = password1;

                        try
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Usuario registrado correctamente.");
                                Hide();
                                Login login = new Login();
                                login.Show();
                                
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message);
                        }
                    }
                }
            }
        }

            private void btnaInicioSesion_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            Login login = new Login();
            login.Show();
        }

        private void bnt_Back_Click(object sender, RoutedEventArgs e)
        {
            Hide();

            LandingPage landingPage = new LandingPage();
            landingPage.Show();
        }
    }
}
