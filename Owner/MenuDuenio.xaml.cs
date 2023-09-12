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

namespace ProyectoDiseño.Owner
{
    /// <summary>
    /// Lógica de interacción para MenuDuenio.xaml
    /// </summary>
    public partial class MenuDuenio : Window
    {
        public MenuDuenio()
        {
            InitializeComponent();
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            Hide();
            GestionProductos gestionProductos = new GestionProductos();
            gestionProductos.Show();
        }

        private void RadioButton_Checked_2(object sender, RoutedEventArgs e)
        {
            Hide();
            EditProductWindow editProduct = new EditProductWindow();
            editProduct.Show();
        }

        private void RadioButton_Checked_3(object sender, RoutedEventArgs e)
        {
            Hide();
            MenuDuenio menuDuenio = new MenuDuenio();
            menuDuenio.Show();
        }
    }
}
