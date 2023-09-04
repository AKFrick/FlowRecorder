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
            Flowmeters = new ObservableCollection<Flowmeter>();

            //DestroyCabinet = new RelayCommand(obj => DestroyCabinetClicked?.Invoke(this));

            //AddFlowmeterCommand = new RelayCommand(obj => OpenNewFlowmeterWindow());

        }
        public void AddNewFlowmeter(Flowmeter flowmeter)
        {
            flowmeter.DestroyFlowmeterClicked += DestroyFlowmeter;
            Flowmeters.Add(flowmeter);
        }

        public Action<Cabinet> AddFlowmeterClicked;        
        public string Description { get; set; }
        public virtual ObservableCollection<Flowmeter> Flowmeters { get; set; }

        public ObservableCollection<Flowmeter> Densitymeters { get; set; }
        
        void DestroyFlowmeter(Flowmeter flowmeter)
        {
            Flowmeters.Remove(flowmeter);
        }
        public Action<Cabinet> DestroyCabinetClicked;        
    }
}