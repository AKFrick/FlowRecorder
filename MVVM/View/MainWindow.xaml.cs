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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FlowRecorder.MVVM.Model;

namespace FlowRecorder
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }
        public List<Cabinet> Cabinets { get; set; } = new List<Cabinet>() 
        { 
            new Cabinet() 
            {
                Description = "AC1H1",
                Flowmeters = new List<Flowmeter>
                {
                    new Flowmeter() { Description = "Путь 101"},
                    new Flowmeter() { Description = "Путь 102"}
                }
            }, 
            new Cabinet() 
            {
                Description = "AC1H2",
                Flowmeters = new List<Flowmeter>
                {
                    new Flowmeter() { Description = "Путь 201"},
                    new Flowmeter() { Description = "Путь 202"}
                }
            }
        };
        public string MyDescript { get; set; } = "RABOTAET";
    }
}
