using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowRecorder.MVVM.Model;
using System.Windows.Media;

namespace FlowRecorder.MVVM.ViewModel
{
    public class FlowmeterViewModel : INotifyPropertyChanged
    {
        public FlowmeterViewModel(Flowmeter model)
        {

            Description = model.Description;

            flowmeterModel = model;

            flowmeterModel.InstantValueUpdated += (value) => { InstantValue = String.Format("{0:0.00}", value); };   
            flowmeterModel.Connected += () => ChangeColorToConnected();
            flowmeterModel.Disconnected += () => ChangeColorToDisconnected();

            ChangeFlowmeter = new RelayCommand(obj =>
            {
                ChangeFlowmeterClicked?.Invoke(flowmeterModel);
                OutputLog.That("Click");
            }
            
            );
        }

        public Flowmeter flowmeterModel { get; private set; }

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

        public Action<Flowmeter> ChangeFlowmeterClicked;
        public string Description { get; set; }                

        public double AccumulatedValue { get; set; } = 0.0;
        public string InstantValue { get { return instantValue; } set { instantValue = value; OnPropertyChanged(nameof(InstantValue)); } }
        string instantValue;

        public string Ip { get { return flowmeterModel.Ip; } }
        public int Port { get { return flowmeterModel.Port; } }

        public RelayCommand ChangeFlowmeter { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}