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
            Flowmeters = new();

            //DestroyCabinet = new RelayCommand(obj => DestroyCabinetClicked?.Invoke(this));

            //AddFlowmeterCommand = new RelayCommand(obj => OpenNewFlowmeterWindow());

        }
        public void AddNewFlowmeter(Flowmeter flowmeter)
        {
            Flowmeters.Add(flowmeter);
        }
        public void Start()
        {
            foreach (Flowmeter flowmeter in Flowmeters)
                flowmeter.Start();
        }
        public string Description { get; set; }        
        public ObservableCollection<Flowmeter> Flowmeters { get; set; }        
       
    }
}