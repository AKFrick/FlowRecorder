using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlowRecorder.MVVM.Model;

namespace FlowRecorder.MVVM.ViewModel
{
    public class FlowmeterViewModel : INotifyPropertyChanged
    {
        public FlowmeterViewModel(Flowmeter flowmeter)
        {

            Description = flowmeter.Description;

            flowmeterModel = flowmeter;

            flowmeter.InstantValueUpdated += (value) => { InstantValue = String.Format("{0:0.00}", value); };

            DestroyFlowmeter = new RelayCommand(obj => DestroyFlowmeterClicked?.Invoke(this));
        }

        public Flowmeter flowmeterModel { get; private set; }

        public Action<FlowmeterViewModel> DestroyFlowmeterClicked;
        public string Description { get; set; }                

        public double AccumulatedValue { get; set; } = 0.0;
        public string InstantValue { get { return instantValue; } set { instantValue = value; OnPropertyChanged(nameof(InstantValue)); } }
        string instantValue;

        public RelayCommand DestroyFlowmeter { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}