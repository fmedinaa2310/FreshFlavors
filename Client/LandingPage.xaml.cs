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
    /// Lógica de interacción para LandingPage.xaml
    /// </summary>
    public partial class LandingPage : Window
    {
        public LandingPage()
        {
            InitializeComponent();
        }
        private void bnt_Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btn_Exit_Click(object sender, RoutedEventArgs e)
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

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
           
            RegistroUsuario registroUsuario = new RegistroUsuario();
            registroUsuario.Show();
            this.Close();
        }

        private void Duenio_Checked(object sender, RoutedEventArgs e)
        {
            Hide();
            LoginDuenio loginDuenio = new LoginDuenio();
            loginDuenio.Show();
            
        }

        private void Supplier_Checked(object sender, RoutedEventArgs e)
        {            
            
        }
    }
}
