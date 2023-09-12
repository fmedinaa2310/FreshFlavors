using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ProyectoDiseño.Client; // Asegúrate de tener el namespace correcto para tus clases de entidad
using ProyectoDiseño.Properties;

namespace ProyectoDiseño.Client
{
    public partial class Stock : Window
    {
        private List<Producto> productos;
        private List<Producto> productosEnCarrito = new List<Producto>(); // Lista para almacenar productos en el carrito
        private CarritoViewModel _carritoViewModel;
        private string userId;
        
        private StringBuilder factura;
        public string FacturaCompleta { get; private set; }

        public Stock()
        {

            InitializeComponent();
            ComboBox.Items.Add("granos");
            ComboBox.Items.Add("carnes");
            ComboBox.Items.Add("vegetales");
            Validacion();

            txt_Identification.KeyDown += txt_Identification_KeyDown; // Asignar el evento addID_KeyDown al TextBox
           
            _carritoViewModel = new CarritoViewModel();
            DataContext = _carritoViewModel;
            ComboBox.SelectionChanged += ComboBox_SelectionChanged;
            factura = new StringBuilder();
   
        }

        private void txt_Identification_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Validacion();
                string enteredId = txt_Identification.Text;

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

        private bool VerificarIdCliente(string userId)
        {
           // string connectionString = "Server=DESKTOP-NR8ON3L;Database=GroceryStore;Integrated Security=true";
            //string connectionString = "Server=DESKTOP-J135DBV;Database=GroceryStore;Integrated Security=true";
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


        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboBox.SelectedItem != null)
            {
                string proveedorSeleccionado = ComboBox.SelectedItem.ToString();
                CargarProductos(proveedorSeleccionado);
            }
        }

        public void LoadMoney(string userName)
        {
          //  string connectionString = "Server=DESKTOP-NR8ON3L;Database=GroceryStore;Integrated Security=true";
            // string connectionString = "Server=DESKTOP-J135DBV;Database=GroceryStore;Integrated Security=true";
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
                        txt_shownMoney.Text = reader["DineroInicial"].ToString();
                        txt_Identification.IsEnabled = false;
                    }
                }
            }
        }


        private void CargarProductos(string proveedor)
        {
           // string connectionString = "Server=DESKTOP-NR8ON3L;Database=GroseryStore;Integrated Security=true";
            //string connectionString = "Server=LAPTOP-52S2J9MV\\MSSQLSERVER01;Database=GroceryStore;Integrated Security = true";
            // string connectionString = "Server=DESKTOP-J135DBV;Database=GroceryStore;Integrated Security=true";
           string connectionString = "Server=LAPTOP-INLUF1R9;Database=GroceryStore;Integrated Security=true";

            productos = new List<Producto>(); // Inicialización de la lista productos

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT id, nombre, categoria, precio, cantidad FROM productos WHERE categoria = @Proveedor";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Proveedor", proveedor);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int id = Convert.ToInt32(reader["id"]);
                            string nombre = reader["nombre"].ToString();
                            string categoria = reader["categoria"].ToString();
                            decimal precio = Convert.ToDecimal(reader["precio"]);
                            int cantidad = Convert.ToInt32(reader["cantidad"]);

                            productos.Add(new Producto
                            {
                                Id = id,
                                Nombre = nombre,
                                Categoria = categoria,
                                Precio = precio,
                                Cantidad = cantidad
                            });
                        }
                    }
                }
            }

            listViewProductos.ItemsSource = productos;
        }


        private void DecrementButton_Click(object sender, RoutedEventArgs e)
        {
            if (listViewProductos.SelectedItem is Producto selectedProduct)
            {
                _carritoViewModel.DecrementProductQuantity(selectedProduct);
            }
        }

        private void IncrementButton_Click(object sender, RoutedEventArgs e)
        {
            if (listViewProductos.SelectedItem is Producto selectedProduct)
            {
                _carritoViewModel.IncrementProductQuantity(selectedProduct);
            }
        }


        private void RefreshListView()
        {
            listViewProductos.ItemsSource = null;
            listViewProductos.ItemsSource = productos;
        }

        private void bnt_Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void bnt_Back_Click(object sender, RoutedEventArgs e)
        {
            MenuUsuario men = new MenuUsuario();
            Hide();
            men.LoadUserData(userId);
            men.Show();

        }

        private void bnt_Exit_Click(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void listViewProductos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void Validacion()
        {
            if (string.IsNullOrWhiteSpace(txt_Identification.Text))
            {
                addToCart.IsEnabled = false;
            }
            else
            {
                addToCart.IsEnabled = true;
            }
        }

        private void addToCart_Click(object sender, RoutedEventArgs e)
        {
            if (listViewProductos.SelectedItem is Producto selectedProduct)
            {
                int cantidadDeseada = selectedProduct.CantidadDeseada;
                int cantidadEnStock = ObtenerCantidadEnStock(selectedProduct.Id);

                if (cantidadDeseada > 0 && cantidadDeseada <= cantidadEnStock)
                {
                    decimal costoTotal = selectedProduct.Precio * cantidadDeseada;
                    decimal saldoActual = CargarSaldoActual();

                    if (costoTotal <= saldoActual)
                    {
                        _carritoViewModel.AgregarAlCarrito(selectedProduct);
                        ActualizarStockEnBaseDeDatos(selectedProduct.Id, cantidadEnStock - cantidadDeseada);
                        RestarSaldo(costoTotal);
                        MessageBox.Show($"Agregado al carrito: {selectedProduct.Nombre} x {cantidadDeseada}");


                        AgregarProductoAlCarrito(selectedProduct);
                        string facturaCompleta = FacturaCompleta;
                        MessageBox.Show("Contenido de la factura:\n" + facturaCompleta);

                        // Actualizar el saldo mostrado en la interfaz
                        txt_shownMoney.Text = $"{saldoActual - costoTotal}";
                    }
                    else
                    {
                        MessageBox.Show("Saldo insuficiente para realizar la compra.", "Saldo Insuficiente", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else if (cantidadDeseada <= 0)
                {
                    MessageBox.Show("La cantidad deseada debe ser mayor que 0.");
                }
                else if (cantidadDeseada > cantidadEnStock)
                {
                    MessageBox.Show("La cantidad deseada excede el stock disponible.");
                }
            }
        }



        private decimal CargarSaldoActual()
        {
            decimal saldoActual = 0;
           // string connectionString = "Server=DESKTOP-NR8ON3L;Database=GroceryStore;Integrated Security=true";
            //string connectionString = "Server=DESKTOP-J135DBV;Database=GroceryStore;Integrated Security=true";
             string connectionString = "Server=LAPTOP-INLUF1R9;Database=GroceryStore;Integrated Security=true";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT DineroInicial FROM Clients WHERE ID = @ID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", userId);
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        saldoActual = Convert.ToDecimal(result);
                    }
                }
            }

            return saldoActual;
        }

        private void RestarSaldo(decimal costoTotal)
        {
          //  string connectionString = "Server=DESKTOP-NR8ON3L;Database=GroceryStore;Integrated Security=true";
            //string connectionString = "Server=DESKTOP-J135DBV;Database=GroceryStore;Integrated Security=true";
            string connectionString = "Server=LAPTOP-INLUF1R9;Database=GroceryStore;Integrated Security=true";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Clients SET DineroInicial = DineroInicial - @CostoTotal WHERE ID = @ID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CostoTotal", costoTotal);
                    command.Parameters.AddWithValue("@ID", userId);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Saldo actualizado correctamente en la base de datos.");
                    }
                    else
                    {
                        MessageBox.Show("Error al actualizar el saldo en la base de datos.");
                    }
                }
            }
        }


            private int ObtenerCantidadEnStock(int productoId)
        {
            int cantidadEnStock = 0;

          //  string connectionString = "Server=DESKTOP-NR8ON3L;Database=GroceryStore;Integrated Security=true";
            //string connectionString = "Server=DESKTOP-J135DBV;Database=GroceryStore;Integrated Security=true"; // Reemplaza con tu cadena de conexión
            //  string connectionString = "Server=DESKTOP-J135DBV;Database=GroceryStore;Integrated Security=true"; // Reemplaza con tu cadena de conexión
              string connectionString = "Server=LAPTOP-INLUF1R9;Database=GroceryStore;Integrated Security=true";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT cantidad FROM productos WHERE id = @ProductoId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductoId", productoId);
                    object result = command.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                    {
                        cantidadEnStock = Convert.ToInt32(result);
                    }
                }
            }

            return cantidadEnStock;
        }


        private void ActualizarStockEnBaseDeDatos(int productoId, int nuevoStock)
        {
            // string connectionString = "Server=DESKTOP-J135DBV;Database=GroceryStore;Integrated Security=true";
            //string connectionString = "Server=DESKTOP-J135DBV;Database=GroceryStore;Integrated Security=true";
            //  string connectionString = "Server=DESKTOP-NR8ON3L;Database=GroceryStore;Integrated Security=true";
            string connectionString = "Server=LAPTOP-INLUF1R9;Database=GroceryStore;Integrated Security=true";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE productos SET cantidad = @NuevoStock WHERE id = @ProductoId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NuevoStock", nuevoStock);
                    command.Parameters.AddWithValue("@ProductoId", productoId);

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Stock actualizado correctamente en la base de datos.");
                    }
                    else
                    {
                        MessageBox.Show("Error al actualizar el stock en la base de datos.");
                    }
                }
            }
        }

        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            if (listViewProductos.SelectedItem is Producto selectedProduct)
            {
                int cantidadDeseada = selectedProduct.CantidadDeseada;
                int cantidadEnStock = ObtenerCantidadEnStock(selectedProduct.Id);

                if (cantidadDeseada > 0 && cantidadDeseada <= cantidadEnStock)
                {
                    decimal costoTotal = selectedProduct.Precio * cantidadDeseada;
                    
                        MessageBox.Show($"Tiene en el carrito:  {cantidadDeseada} {selectedProduct.Nombre} ");


                     //   AgregarProductoAlCarrito(selectedProduct);
                        string facturaCompleta = FacturaCompleta;
                        MessageBox.Show("Contenido del carrito:\n" + facturaCompleta);
                }
            }
        }
        public void AgregarProductoAlCarrito(Producto producto)
        {
            factura.AppendLine($"Producto: {producto.Nombre}, Precio: {producto.Precio}, Cantidad: {producto.CantidadDeseada}");
            FacturaCompleta = factura.ToString(); // Actualiza la propiedad con el valor de la factura
        }

        public string ObtenerFacturaCompleta()
        {
            return factura.ToString();
        }
        private void addID_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void showntxtMoney_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        //A APARTIR DE AQUI TODO ESTO ES FACTURACION

        private bool VerifyCredentials(string userId)
        {
           // string connectionString = "Server=DESKTOP-NR8ON3L;Database=GroceryStore;Integrated Security=true";
            // string connectionString = "Server=DESKTOP-NR8ON3L;Database=GroseryStore;Integrated Security=true";
            //string connectionString = "Server=DESKTOP-J135DBV;Database=GroceryStore;Integrated Security=true";
            string connectionString = "Server=LAPTOP-INLUF1R9;Database=GroceryStore;Integrated Security=true";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT COUNT(*) FROM Clients WHERE ID = @ID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", userId);

                    int count = (int)command.ExecuteScalar();

                    return count > 0;
                }
            }
        }
        public int GenerarNumFactura()
        {
            Random random = new Random();
            return random.Next(1000, 100000); // Genera un número aleatorio entre 1000 y 9999
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            string carpetaDescargas = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + "\\Downloads";
            string rutaArchivo = Path.Combine(carpetaDescargas, "factura.txt");
            string user = txt_Identification.Text;
            Producto producto = new Producto();


            Factura factura = new Factura
            {
                numeroFactura = GenerarNumFactura(),
                Fecha = DateTime.Now
                // Inicializa otras propiedades de la factura
            };

            if (string.IsNullOrEmpty(user))
            {
                MessageBox.Show("Por favor complete los campos vacíos.");
                return;
            }
            try
            {
                if (VerifyCredentials(user))
                {
                    if (listViewProductos.SelectedItem is Producto selectedProduct)
                    {

                        int cantidadDeseada = selectedProduct.CantidadDeseada;
                        decimal costoTotal = selectedProduct.Precio * cantidadDeseada;
                        using (StreamWriter writer = new StreamWriter(rutaArchivo))
                        {
                           // string connectionString = "Server=DESKTOP-NR8ON3L;Database=GroceryStore;Integrated Security=true";
                            string connectionString = "Server=LAPTOP-INLUF1R9;Database=GroceryStore;Integrated Security=true";
                            // string connectionString = "Server=DESKTOP-J135DBV;Database=GroceryStore;Integrated Security=true";
                            using (SqlConnection connection = new SqlConnection(connectionString))
                            {
                                connection.Open();
                                string query = "SELECT Nombre, PrimerApellido, SegundoApellido FROM Clients WHERE ID = @ID";

                                using (SqlCommand command = new SqlCommand(query, connection))
                                {
                                    command.Parameters.AddWithValue("@ID", user);
                                    SqlDataReader reader = command.ExecuteReader();
                                    if (reader.Read())
                                    {
                                        writer.WriteLine($"Número de Factura: {factura.numeroFactura}");
                                        writer.WriteLine($"Fecha: {factura.Fecha}");
                                        writer.WriteLine($"Cliente: {reader["Nombre"].ToString()} {reader["PrimerApellido"].ToString()} {reader["SegundoApellido"].ToString()}");

                                        foreach (Producto selectedProducto in listViewProductos.SelectedItems)
                                        {
                                            writer.WriteLine($"Producto: {FacturaCompleta}");
                                            writer.WriteLine($"");
                                        }
                                    }
                                }
                            }

                        }
                        MessageBox.Show("Compra autorizada, gracias por utilizar nuestra app.");
                        MessageBox.Show($"Archivo {rutaArchivo} generado en la carpeta de Descargas.");

                    }
                }
                else
                {
                    MessageBox.Show("Compra denegada.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void txt_Identification_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
    
}