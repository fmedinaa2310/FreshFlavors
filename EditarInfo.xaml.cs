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
    /// Lógica de interacción para EditarInfoNuevo.xaml
    /// </summary>
    public partial class EditarInfo : Window
    {
        public EditarInfo()
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

        private void btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void bnt_Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnRegistrar_Click(object sender, RoutedEventArgs e)
        {
            string Pssw1 = Edittxt_Password.Password;
            string Pssw2 = EdittxtConfimationPassword.Password;

            if (Pssw1 != Pssw2)
            {
                MessageBox.Show("Las contraseña no coincide" +
                               "\n verifique las contraseñas.");
            }
            else
            {
                string id = EdittxtId.Text;
                string connectionString = "Server=DESKTOP-J135DBV;Database=GroceryStore;Integrated Security=true";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string updateQuery = "UPDATE Clients SET Nombre = @Nombre, PrimerApellido = @PrimerApellido, SegundoApellido = @SegundoApellido, Contrasena = @Contrasena WHERE ID = @ID";

                    using (SqlCommand command = new SqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Nombre", EdittxtName.Text);
                        command.Parameters.AddWithValue("@PrimerApellido", EdittxtFirstLastName.Text);
                        command.Parameters.AddWithValue("@SegundoApellido", EdittxtSecondSurname.Text);
                        command.Parameters.AddWithValue("@Contrasena", Edittxt_Password.Password);
                        command.Parameters.AddWithValue("@ID", EdittxtId.Text);

                        try
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Datos actualizados correctamente.");
                                Hide();
                                MenuUsuario menuUsuario = new MenuUsuario();
                                menuUsuario.Show();
                                menuUsuario.LoadUserData(id);

                            }
                            else
                            {
                                MessageBox.Show("No se pudieron actualizar los datos.");
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

    }
}

