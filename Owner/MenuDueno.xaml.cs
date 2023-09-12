using System;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProyectoDiseño.Owner
{
    public partial class MenuDueno : Window
    {
        private string userId;

        public MenuDueno()
        {
            InitializeComponent();
            txt_Identification.KeyDown += txt_Identification_KeyDown;
        }

        private void txt_Identification_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string enteredId = txt_Identification.Text.Trim();

                if (!string.IsNullOrWhiteSpace(enteredId))
                {
                    userId = enteredId;
                    LoadUserData(userId);
                }
                else
                {
                    MessageBox.Show("ID inválido, verifique su ID.");
                }
            }
        }


        

        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            ChatDueno chatDueno = new ChatDueno();
            chatDueno.Show();
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        public void LoadUserData(string userName)
        {
            //string connectionString = "Server=DESKTOP-J135DBV;Database=GroceryStore;Integrated Security=true";
            // string connectionString = "Server=DESKTOP-NR8ON3L;Database=GroceryStore;Integrated Security=true";
            string connectionString = "Server=LAPTOP-INLUF1R9;Database=GroceryStore;Integrated Security=true";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Nombre, PrimerApellido, SegundoApellido, DineroInicial FROM Admin WHERE Username = @Username";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", userName);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        showntxtName.Text = reader["Nombre"].ToString();
                        showntxtSurname.Text = reader["PrimerApellido"].ToString();
                        showntxtSecondSurname.Text = reader["SegundoApellido"].ToString();
                        shownInitialMoney.Text = reader["DineroInicial"].ToString();
                        txt_Identification.IsEnabled = false;
                    }
                }
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

        private void bnt_Back_Click(object sender, RoutedEventArgs e)
        {
            LandingPage landingPage = new LandingPage();
            landingPage.Show();
            Close();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            SaldoDueno saldoDueno = new SaldoDueno();
            saldoDueno.Show();
            Close();
        }

        private void RadioButton_Checked_5(object sender, RoutedEventArgs e)
        {
            ChatDueno chat = new ChatDueno();
            chat.Show();
            Close();
        }

        private void btnEditMyInfo(object sender, RoutedEventArgs e)
        {
            // Agregar lógica para editar la información del dueño
        }

        private void RadioButton_Checked_3(object sender, RoutedEventArgs e)
        {
            SaldoDueno sal = new SaldoDueno();
            sal.Show();
            Close();
        }

        private void showntxtName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void showntxtName_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
    }
}
