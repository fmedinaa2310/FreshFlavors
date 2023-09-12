using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ProyectoDiseño.Client
{
    public class CarritoViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Producto> _productosEnCarrito;

        public ObservableCollection<Producto> ProductosEnCarrito
        {
            get { return _productosEnCarrito; }
            set
            {
                _productosEnCarrito = value;
                OnPropertyChanged();
            }
        }

        public CarritoViewModel()
        {
            ProductosEnCarrito = new ObservableCollection<Producto>();
        }

        public void AgregarAlCarrito(Producto producto)
        {
            if (producto.CantidadDeseada <= 0)
            {
                MessageBox.Show("La cantidad debe ser mayor que cero.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ProductosEnCarrito.Add(producto);

            MessageBox.Show($"El producto '{producto.Nombre}' se ha agregado al carrito.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
        }



        public void IncrementProductQuantity(Producto producto)
        {
            // Buscar el producto en el carrito
            var carritoItem = ProductosEnCarrito.FirstOrDefault(item => item.Id == producto.Id);

            if (carritoItem != null)
            {
                // Incrementar la cantidad
                carritoItem.CantidadDeseada++;
                OnPropertyChanged(nameof(ProductosEnCarrito));
            }
        }
        public void DecrementProductQuantity(Producto producto)
        {
            // Buscar el producto en el carrito
            var carritoItem = ProductosEnCarrito.FirstOrDefault(item => item.Id == producto.Id);

            if (carritoItem != null)
            {
                // Decrementar la cantidad si es mayor que 0
                if (carritoItem.CantidadDeseada > 0)
                {
                    carritoItem.CantidadDeseada--;
                    OnPropertyChanged(nameof(ProductosEnCarrito));
                }
                else
                {
                    MessageBox.Show("La cantidad no puede ser menor que cero.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
