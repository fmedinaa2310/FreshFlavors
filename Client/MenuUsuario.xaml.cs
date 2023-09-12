using ProyectoDiseño.Client;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ProyectoDiseño
{
    /// <summary>
    /// Lógica de interacción para MenuUsuario.xaml
    /// </summary>
    public partial class MenuUsuario : Window
    {
        public MenuUsuario()
        {
            InitializeComponent();
       //     LoadUserData(username);
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Close_Checked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        

        private void btnEditMyInfo(object sender, RoutedEventArgs e)
        {
            EditarInfo editarInfo = new EditarInfo();
            RegistroUsuario registroUsuario = new RegistroUsuario();
            editarInfo.EdittxtName.Text = showntxtName.Text;
            editarInfo.EdittxtFirstLastName.Text = showntxtSurname.Text;
            editarInfo.EdittxtSecondSurname.Text = showntxtSecondSurname.Text;
            editarInfo.Edittxt_Password.Password = shownPassword.Text;
            editarInfo.EdittxtId.Text = shownIdentification.Text;

            Hide();
            editarInfo.Show();
        }


        public void LoadUserData(string userName)

        {
            //string connectionString = "Server=DESKTOP-NR8ON3L;Database=GroceryStore;Integrated Security=true";
            //string connectionString = "Server=DESKTOP-J135DBV;Database=GroceryStore;Integrated Security=true";
            // string connectionString = "Server=DESKTOP-J135DBV;Database=GroceryStore;Integrated Security=true";
             string connectionString = "Server=LAPTOP-INLUF1R9;Database=GroceryStore;Integrated Security=true";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Nombre, PrimerApellido, SegundoApellido, Contrasena, DineroInicial FROM Clients WHERE ID = @ID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", userName);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        showntxtName.Text = reader["Nombre"].ToString();
                        showntxtSurname.Text = reader["PrimerApellido"].ToString();
                        showntxtSecondSurname.Text = reader["SegundoApellido"].ToString();
                        shownPassword.Text = reader["Contrasena"].ToString();
                        shownIdentification.Text = userName;

                        // Mostrar el dinero inicial en un control de tu interfaz, por ejemplo:
                        shownInitialMoney.Text = reader["DineroInicial"].ToString();
                    }
                }
            }
        }

        private void bnt_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void bnt_Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void bnt_Back_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            LandingPage landingPage = new LandingPage();
            landingPage.Show();
        }

        private void showntxtName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
           
        }

        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            Hide();
            Stock stock = new Stock();
            stock.Show();
        }

        private void RadioButton_Checked_3(object sender, RoutedEventArgs e)
        {
            Hide();
            Saldo saldo = new Saldo();
            saldo.Show();
        }
    }
}
