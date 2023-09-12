using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PropertyChanged;

namespace ProyectoDiseño.Client
{
    [AddINotifyPropertyChangedInterface]
    public class Producto : INotifyPropertyChanged
    {
        private int _id;
        private string _nombre;
        private string _categoria;
        private decimal _precio;
        private int _cantidad;
        private int _cantidadDeseada; // Nueva propiedad

        public int Id
        {
            get { return _id; }
            set { _id = value; OnPropertyChanged(); }
        }

        public string Nombre
        {
            get { return _nombre; }
            set { _nombre = value; OnPropertyChanged(); }
        }

        public string Categoria
        {
            get { return _categoria; }
            set { _categoria = value; OnPropertyChanged(); }
        }

        public decimal Precio
        {
            get { return _precio; }
            set { _precio = value; OnPropertyChanged(); }
        }

        public int Cantidad
        {
            get { return _cantidad; }
            set { _cantidad = value; OnPropertyChanged(); }
        }

        public int CantidadDeseada
        {
            get { return _cantidadDeseada; }
            set { _cantidadDeseada = value; OnPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}