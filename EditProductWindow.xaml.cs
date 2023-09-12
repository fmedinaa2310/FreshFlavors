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

namespace ProyectoDiseño
{
    /// <summary>
    /// Interaction logic for EditProductWindow.xaml
    /// </summary>
    public partial class EditProductWindow : Window
    {
        private Producto producto;

        public EditProductWindow(Producto producto)
        {
            InitializeComponent();
            this.producto = producto;
            CargarDatosProducto();
        }

        private void CargarDatosProducto()
        {
            txtNombre.Text = producto.nombreProducto;
            txtDescripcion.Text = producto.descripcionProducto;
            cmbCategoria.Text = producto.categoriaProducto;
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            producto.nombreProducto = txtNombre.Text;
            producto.descripcionProducto = txtDescripcion.Text;
            producto.categoriaProducto = cmbCategoria.Text;

            this.Close();
        }
    }
}
