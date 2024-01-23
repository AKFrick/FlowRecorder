using FlowRecorder.MVVM.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace FlowRecorder.MVVM.ViewModel
{
    public class DensitymeterViewModel : INotifyPropertyChanged
    {
        public DensitymeterViewModel(Densitymeter model)
        {

            Description = model.Description;

            densitymeterModel = model;

            densitymeterModel.TemperatureUpdated += (value) => { Temperature = String.Format("{0:0.00}", value); };
            densitymeterModel.Connected += () => ChangeColorToConnected();
            densitymeterModel.Disconnected += () => ChangeColorToDisconnected();

            Edit = new RelayCommand(obj =>
            {
                EditClicked?.Invoke(densitymeterModel);
            }

            );
        }
        public Densitymeter densitymeterModel { get; private set; }

        public Brush StatusColor { get { return statusColor; } set { statusColor = value; OnPropertyChanged(nameof(StatusColor)); } }
        Brush statusColor;
        void ChangeColorToConnected()
        {
            StatusColor = Brushes.Green;
        }
        void ChangeColorToDisconnected()
        {
            StatusColor = Brushes.Red;
        }

        public Action<Meter> EditClicked;
        public string Description { get; set; }

        public string Temperature { get { return temperature; } set { temperature = value; OnPropertyChanged(nameof(Temperature)); } }
        string temperature;

        public string Ip { get { return densitymeterModel.Ip; } }
        public int Port { get { return densitymeterModel.Port; } }

        public RelayCommand Edit  { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
