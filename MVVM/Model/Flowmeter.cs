using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;


namespace FlowRecorder.MVVM.Model
{
    public class Flowmeter
    {
        public Flowmeter()
        {
            reader = new();
            reader.ValueRead += (value) => { InstantValue = value; };
        }
        
        public Action<Flowmeter> DestroyFlowmeterClicked;
        public string Description { get; set; }        

        public string Ip { get; set; } = "192.168.10.254";
        public int Port { get; set; } = 4001;
        public void Start()
        {
            reader.StartReading();
        }
        public void Stop()
        {
            reader.StopReading();  
        }
        public int DeviceAddress { get; set; } = 1;       
        public double AccumulatedValue { get; set; } = 0.0;
        public double InstantValue { get { return instantValue; } set { instantValue = value; InstantValueUpdated(instantValue); } }
        double instantValue;
        public event Action<double> InstantValueUpdated;

        DataReader reader;
    }
}