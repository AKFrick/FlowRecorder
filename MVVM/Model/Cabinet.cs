using FlowRecorder.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using FlowRecorder.MVVM.View;

namespace FlowRecorder.MVVM.Model
{
    public class Cabinet
    {
        public Cabinet()
        {

            //AddFlowmeterCommand = new RelayCommand(obj => AddFlowmeterClicked?.Invoke(this));      
            DestroyCabinet = new RelayCommand(obj => DestroyCabinetClicked?.Invoke(this));

            Flowmeters = new ObservableCollection<Flowmeter>();

            AddFlowmeterCommand = new RelayCommand(obj => OpenNewFlowmeterWindow());

        }
        public void AddNewFlowmeter(Flowmeter flowmeter)
        {
            flowmeter.DestroyFlowmeterClicked += DestroyFlowmeter;
            Flowmeters.Add(flowmeter);
        }

        public Action<Cabinet> AddFlowmeterClicked;
        public Action<Cabinet> DestroyCabinetClicked;
        public string Description { get; set; }
        public ObservableCollection<Flowmeter> Flowmeters { get; set; }

        public ObservableCollection<Flowmeter> Densitymeters { get; set; }

        public RelayCommand AddFlowmeterCommand { get; set; }
        void OpenNewFlowmeterWindow()
        {
            NewFlowmeterViewModel model = new NewFlowmeterViewModel();
            model.FlowmeterCreated += AddNewFlowmeter;

            NewFlowmeterWindow newFlowmeter = new NewFlowmeterWindow(model);
            newFlowmeter.ShowDialog();
        }
        void DestroyFlowmeter(Flowmeter flowmeter)
        {
            Flowmeters.Remove(flowmeter);
        }
        public RelayCommand DestroyCabinet { get; set; }
    }
}