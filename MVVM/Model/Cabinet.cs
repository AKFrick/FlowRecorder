using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using FlowRecorder.MVVM.View;

namespace FlowRecorder.MVVM.Model
{
    [Serializable]
    public class Cabinet
    {
        public Cabinet()
        {
            Flowmeters = new();
            Densitymeters = new();

            //DestroyCabinet = new RelayCommand(obj => DestroyCabinetClicked?.Invoke(this));

            //AddFlowmeterCommand = new RelayCommand(obj => OpenNewFlowmeterWindow());

        }
        public void AddNewFlowmeter(Flowmeter flowmeter)
        {
            Flowmeters.Add(flowmeter);
        }
        public void AddNewDensityMeter(Densitymeter densitymeter)
        {
            Densitymeters.Add(densitymeter);

            foreach (var meter in Flowmeters)
                meter.Densitymeter = densitymeter;
        }

        public void Start()
        {
            foreach (Flowmeter flowmeter in Flowmeters)
                flowmeter.Start();
        }
        public void Stop()
        {
            foreach(Flowmeter flowmeter in Flowmeters)
                flowmeter.Stop();
        }

        public string Description { get; set; }        
        public ObservableCollection<Flowmeter> Flowmeters { get; set; }
        public ObservableCollection<Densitymeter> Densitymeters { get; set; }

    }
}