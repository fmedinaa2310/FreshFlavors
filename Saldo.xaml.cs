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

namespace ProyectoDiseño
{
    /// <summary>
    /// Lógica de interacción para Saldo.xaml
    /// </summary>
    public partial class Saldo : Window
    {
        public Saldo()
        {
            InitializeComponent();
            doughnut();
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
                    Title="Meat",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(8)},
                    DataLabels=true,
                },
                 new PieSeries
                {
                    Title="Bread",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(5)},
                    DataLabels=true,
                },
                  new PieSeries
                {
                    Title="Sauces",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(3)},
                    DataLabels=true,
                },
                   new PieSeries
                {
                    Title="Chicken",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(4)},
                    DataLabels=true,
                },
            };
            DataContext = this;

            seriesCollectionDos = new SeriesCollection
            {
                new PieSeries
                {
                    Title="June",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(8)},
                    DataLabels=true,
                },
                 new PieSeries
                {
                    Title="July",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(5)},
                    DataLabels=true,
                },
                  new PieSeries
                {
                    Title="May",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(3)},
                    DataLabels=true,
                },
                   new PieSeries
                {
                    Title="August",
                    Values=new ChartValues<ObservableValue> {new ObservableValue(4)},
                    DataLabels=true,
                },
            };
            DataContext = this; 
        }
    }
}
