using LiveCharts.Defaults;
using LiveCharts.Wpf;
using LiveCharts;
using System;
using System.Collections.Generic;
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
using System.Data.SqlClient;

namespace ProyectoDiseño
{
    /// <summary>
    /// Lógica de interacción para Saldo.xaml
    /// </summary>
    public partial class Saldo : Window
    {
        private string userId;

        public Saldo()
        {
            InitializeComponent();
            doughnut();
            addID.KeyDown += addID_KeyDown;

        }
        private void addID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                string enteredId = addID.Text;

                if (!string.IsNullOrWhiteSpace(enteredId) && VerificarIdCliente(enteredId))
                {
                    userId = enteredId;
                    LoadMoney(userId); // Llama al método para cargar el dinero inicial
                }
                else
                {
                    MessageBox.Show("ID inválido,verifique su ID");

                }
            }
        }
        public void LoadMoney(string userName)
        {
           // string connectionString = "Server=DESKTOP-NR8ON3L;Database=GroceryStore;Integrated Security=true";
            //string connectionString = "Server=DESKTOP-J135DBV;Database=GroceryStore;Integrated Security=true";
            string connectionString = "Server=LAPTOP-INLUF1R9;Database=GroceryStore;Integrated Security=true";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT DineroInicial FROM Clients WHERE ID = @ID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", userName);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {

                        // Mostrar el dinero inicial en un control de tu interfaz, por ejemplo:
                        shownMoney.Text = reader["DineroInicial"].ToString();
                        addID.IsEnabled = false;
                    }
                }
            }
        }

        private bool VerificarIdCliente(string userId)
        {
          //  string connectionString = "Server=DESKTOP-NR8ON3L;Database=GroceryStore;Integrated Security=true";
            // string connectionString = "Server=DESKTOP-J135DBV;Database=GroceryStore;Integrated Security=true";
            string connectionString = "Server=LAPTOP-INLUF1R9;Database=GroceryStore;Integrated Security=true";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM Clients WHERE ID = @ID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", userId);
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    // Si el COUNT es mayor que cero, el ID es válido y existe en la base de datos
                    return count > 0;
                }
            }
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

        public SeriesCollection seriesCollection { get; set; }
        public SeriesCollection seriesCollectionDos { get; set; }

        public void doughnut()
        {
            seriesCollection = new SeriesCollection
            {
                new PieSeries
                {
                    Title="Granos",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(30)},
                    DataLabels=true,
                },
                 new PieSeries
                {
                    Title="Carne",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(30)},
                    DataLabels=true,
                },
                  new PieSeries
                {
                    Title="Vegetales",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(30)},
                    DataLabels=true,
                },
            };
            DataContext = this;
        }

        private void bnt_Back_Click(object sender, RoutedEventArgs e)
        {
            string id = addID.Text;
            Hide();
            MenuUsuario menuUsuario = new MenuUsuario();
            menuUsuario.LoadUserData(id);
            menuUsuario.Show();
        }

    private void addID_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
