using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ProyectoDiseño.Owner
{
    public partial class ChatDueno : Window
    {
        public ChatDueno()
        {
            InitializeComponent();

            AgregarElementosComboBox();
            btn_accept.IsEnabled = false;
            btn_Cancel.IsEnabled = false;
            

            txt_chat_owner.KeyDown += txt_chat_owner_KeyDown; 
        }
        private Dictionary<string, (string Producto, int CantidadFaltante, decimal PrecioUnitario)> stockFaltante2;
        Dictionary<string, (string, int, decimal)> stockFaltante = new Dictionary<string, (string, int, decimal)>();
        private string ObtenerCategoriaProveedor(string proveedor)
        {
            switch (proveedor)
            {
                case "Meat supplier":
                    return "carnes";
                case "Beans supplier":
                    return "granos";
                case "Vegetable supplier":
                    return "vegetales";
                default:
                    return null; 
            }
        }


        private void AgregarElementosComboBox()
        {
          
            comboBox.Items.Add("Meat supplier");
            comboBox.Items.Add("Beans supplier");
            comboBox.Items.Add("Vegetable supplier");
        }

        private void txt_chat_owner_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnMandar_Click(sender, e); 
                e.Handled = true; 
            }
        }


        private Dictionary<string, (string Producto, int CantidadFaltante, decimal PrecioUnitario)> MostrarStockFaltante(string proveedor, string categoria)
        {
            //string connectionString = "Server=DESKTOP-NR8ON3L;Database=GroceryStore;Integrated Security=true";
            //string connectionString = "Server=DESKTOP-NR8ON3L;Database=GroceryStore;Integrated Security=true";
            //string connectionString = "Server=DESKTOP-J135DBV;Database=GroceryStore;Integrated Security=true";
            string connectionString = "Server=LAPTOP-INLUF1R9;Database=GroceryStore;Integrated Security=true";

            Dictionary<string, (string, int, decimal)> stockFaltante = new Dictionary<string, (string, int, decimal)>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT A.id, A.nombre, A.cantidad AS CantidadAlmacen, P.cantidad AS CantidadProductos, P.precio " +
                               "FROM AlmacenProduct A " +
                               "INNER JOIN productos P ON A.nombre = P.nombre " +
                               "WHERE P.categoria = @Categoria";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Categoria", categoria);
                    SqlDataReader reader = command.ExecuteReader();

                    int counter = 1; 
                    while (reader.Read())
                    {
                        string id = reader["id"].ToString();
                        string producto = reader["nombre"].ToString();
                        int cantidadAlmacen = Convert.ToInt32(reader["CantidadAlmacen"]);
                        int cantidadProductos = Convert.ToInt32(reader["CantidadProductos"]);
                        decimal precioUnitario = Convert.ToDecimal(reader["precio"]);
                        int cantidadFaltante = cantidadAlmacen - cantidadProductos;

                        string uniqueKey = $"{proveedor}-{id}-{counter}";
                        stockFaltante.Add(uniqueKey, (producto, cantidadFaltante, precioUnitario));

                        counter++; 
                    }
                }
            }

            return stockFaltante;
        }



        private decimal costoTotalEstimado = 0;

        private void btnMandar_Click(object sender, RoutedEventArgs e)
        {
            string _provedor = comboBox.SelectedItem as string;
            string _texto_Client = txt_chat_owner.Text.Trim();

            if (!string.IsNullOrEmpty(_provedor) && !string.IsNullOrWhiteSpace(_texto_Client))
            {
                // Concatenar el nuevo mensaje con el contenido anterior
                _textBox.Text += $"Owner : {_texto_Client}\n";

                switch (_texto_Client)
                {
                    case "Hola":
                        _textBox.Text += $"{_provedor}: ¡Hola! ¿En qué te puedo ayudar?\n";
                        break;
                    case "Me gustaria restablecer el stock":
                        _textBox.Text += $"{_provedor}: Claro! Te haré la cotización....\n";

                        // Obtener la categoría correspondiente al proveedor seleccionado
                        string categoriaProveedor = ObtenerCategoriaProveedor(_provedor);

                        if (!string.IsNullOrEmpty(categoriaProveedor))
                        {
                            // Obtener la cantidad faltante de productos para el proveedor y categoría especificados
                            Dictionary<string, (string Producto, int CantidadFaltante, decimal PrecioUnitario)> stockFaltante = MostrarStockFaltante(_provedor, categoriaProveedor);

                            // Mostrar la cantidad faltante de productos en el mensaje
                            _textBox.Text += $"{_provedor}: Te hace falta lo siguiente:\n";

                            costoTotalEstimado = 0; // Reiniciar el costo total estimado

                            foreach (var producto in stockFaltante)
                            {
                                int cantidadFaltante = producto.Value.CantidadFaltante;
                                decimal precioUnitario = producto.Value.PrecioUnitario;

                                _textBox.Text += $"{producto.Value.Producto}: {cantidadFaltante} unidades\n";

                                // Calcular y acumular el costo total estimado
                                decimal costoProducto = cantidadFaltante * precioUnitario;
                                costoTotalEstimado += costoProducto;
                            }

                            _textBox.Text += $"{_provedor}: El costo total estimado es de: ${costoTotalEstimado}\n";
                            btn_accept.IsEnabled = true;
                            btn_Cancel.IsEnabled = true;
                        }
                        else
                        {
                            _textBox.Text += $"{_provedor}: Proveedor no reconocido.\n";
                        }

                        break;
                    default:
                        _textBox.Text += $"{_provedor}: No tengo una respuesta para eso.\n";
                        break;
                }

                txt_chat_owner.Clear(); // Limpiar el cuadro de texto después de enviar el mensaje
            }
            else
            {
                MessageBox.Show("Selecciona un proveedor y escribe un mensaje.");
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

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btn_accept_Click(object sender, RoutedEventArgs e)
        {
            if (costoTotalEstimado > 0)
            {
                string connectionString = "Server=LAPTOP-INLUF1R9;Database=GroceryStore;Integrated Security=true";

                //string connectionString = "Server=DESKTOP-NR8ON3L;Database=GroceryStore;Integrated Security=true";
                //string connectionString = "Server=DESKTOP-J135DBV;Database=GroceryStore;Integrated Security=true"; // Reemplaza con tu cadena de conexión

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    stockFaltante2 = stockFaltante; // Asigna el valor de stockFaltante a stockFaltante2

                    foreach (var producto in stockFaltante2)
                    { string productoNombre = producto.Value.Producto;
                        int cantidadFaltante = producto.Value.CantidadFaltante;

                        // Actualizar stock en la base de datos
                        string updateQuery = "UPDATE productos SET cantidad = cantidad + @CantidadFaltante WHERE nombre = @ProductoNombre";

                        using (SqlCommand command = new SqlCommand(updateQuery, connection))
                        {
                            command.Parameters.AddWithValue("@CantidadFaltante", cantidadFaltante);
                            command.Parameters.AddWithValue("@ProductoNombre", productoNombre);
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                // Actualización exitosa, puedes mostrar un mensaje o realizar acciones adicionales si es necesario
                                MessageBox.Show($"El stock de {productoNombre} ha sido actualizado.");
                            }
                            else
                            {
                                // Error al actualizar, puedes mostrar un mensaje o realizar acciones adicionales si es necesario
                                MessageBox.Show($"Error al actualizar el stock de {productoNombre}.");
                            }
                        }
                    }

                    // Actualizar el saldo en la tabla Admin
                    decimal nuevoSaldo = ObtenerSaldoActual(connection) - costoTotalEstimado; // Asumiendo que hay un método ObtenerSaldoActual() que obtiene el saldo actual

                    string updateSaldoQuery = "UPDATE Admin SET DineroInicial = @NuevoSaldo";
                    using (SqlCommand command = new SqlCommand(updateSaldoQuery, connection))
                    {
                        command.Parameters.AddWithValue("@NuevoSaldo", nuevoSaldo);
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Actualización exitosa del saldo, puedes mostrar un mensaje o realizar acciones adicionales si es necesario
                            MessageBox.Show("El saldo ha sido actualizado.");
                        }
                        else
                        {
                            // Error al actualizar el saldo, puedes mostrar un mensaje o realizar acciones adicionales si es necesario
                            MessageBox.Show("Error al actualizar el saldo.");
                        }
                    }

                    MessageBox.Show("Gracias por comprar con nosotros, vuelve pronto! ");
                    MessageBox.Show("El stock ha sido restablecido y el saldo actualizado.");
                }
            }
            else
            {
                MessageBox.Show("No hay costo estimado para restablecer el stock.");
            }
        }

        private decimal ObtenerSaldoActual(SqlConnection connection)
        {
            string query = "SELECT DineroInicial FROM Admin";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                decimal saldo = (decimal)command.ExecuteScalar();
                return saldo;
            }
        }




        private void btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            string _provedor = comboBox.SelectedItem as string;
            _textBox.Text += $"{_provedor}: Perfecto! Cuando ocupes algo avisame\n";
        }

        private void txt_chat_owner_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void bnt_Back_Click(object sender, RoutedEventArgs e)
        {
            Hide();

            MenuDueno menuDueno = new MenuDueno();
            menuDueno.Show();
        }
    }
}
