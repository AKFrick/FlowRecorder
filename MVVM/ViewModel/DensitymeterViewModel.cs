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

            //densitymeterModel.InstantValueUpdated += (value) => { InstantValue = String.Format("{0:0.00}", value); };
            //densitymeterModel.Connected += () => ChangeColorToConnected();
            //densitymeterModel.Disconnected += () => ChangeColorToDisconnected();

            ChangeFlowmeter = new RelayCommand(obj =>
            {
                ChangeMeterClicked?.Invoke(densitymeterModel);
                OutputLog.That("Click");
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

        public Action<Meter> ChangeMeterClicked;
        public string Description { get; set; }

        public double AccumulatedValue { get; set; } = 0.0;
        public string InstantValue { get { return instantValue; } set { instantValue = value; OnPropertyChanged(nameof(InstantValue)); } }
        string instantValue;

        public string Ip { get { return densitymeterModel.Ip; } }
        public int Port { get { return densitymeterModel.Port; } }

        public RelayCommand ChangeFlowmeter { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
