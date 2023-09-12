using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for GestionProductos.xaml
    /// </summary>
    public partial class GestionProductos : Window
    {
        private ObservableCollection<Producto> productos;
        private int codigoAutoGenerado;

        public GestionProductos()
        {
            InitializeComponent();
            productos = new ObservableCollection<Producto>();
            lvProductos.ItemsSource = productos;
            codigoAutoGenerado = 0;
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            string nombre = txtNombre.Text;
            string descripcion = txtDescripcion.Text;
            string categoria = cmbCategoria.Text;

            //Logica para generar el codigo auto
            string codigo = codigoAutoGenerado.ToString("000000");

            Producto nuevoProducto = new Producto(nombre, descripcion, categoria, codigo);
            productos.Add(nuevoProducto);

            codigoAutoGenerado++;
            LimpiarCampos();
        }

        private void lvProductos_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (lvProductos.SelectedIndex != -1)
            {
                if (lvProductos.SelectedIndex != -1)
                {
                    Producto productoSeleccionado = lvProductos.SelectedItem as Producto;

                    if (productoSeleccionado != null)
                    {
                        MessageBoxResult result = MessageBox.Show("¿Qué acción deseas realizar?",
                                                                  "Editar o Eliminar",
                                                                  MessageBoxButton.YesNoCancel);

                        if (result == MessageBoxResult.Yes) // Editar
                        {

                            EditProductWindow editWindow = new EditProductWindow(productoSeleccionado);
                            editWindow.ShowDialog();

                            lvProductos.SelectedIndex = -1;
                        }
                        else if (result == MessageBoxResult.No)
                        {
                            // Confirmar la eliminación del producto
                            MessageBoxResult confirmDelete = MessageBox.Show($"¿Estás seguro de eliminar el producto '{productoSeleccionado.nombreProducto}'?",
                                                                             "Confirmar Eliminación",
                                                                             MessageBoxButton.YesNo);

                            if (confirmDelete == MessageBoxResult.Yes)
                            {
                                productos.Remove(productoSeleccionado);

                                lvProductos.SelectedIndex = -1;
                            }
                        }
                    }
                }
            }
        }

        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            cmbCategoria.SelectedIndex = -1;
        }
    }

    public class Producto
    {
        public string nombreProducto { get; set; }
        public double precioProducto { get; set; }
        public int idProducto { get; set; }
        public string descripcionProducto { get; set; }
        public string categoriaProducto { get; set; }

        public Producto(string nombre, string descripcion, string categoria, string codigo)
        {
            nombreProducto = nombre;
            descripcionProducto = descripcion;
            categoriaProducto = categoria;
            idProducto = int.Parse(codigo);
        }
    }
}
