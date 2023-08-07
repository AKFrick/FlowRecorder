using FlowRecorder.MVVM.ViewModel;
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
            DestroyFlowmeter = new RelayCommand(obj => DestroyFlowmeterClicked?.Invoke(cabinet, this));
        }

        public Cabinet cabinet;
        public Action<Cabinet, Flowmeter> DestroyFlowmeterClicked;
        public string Description { get; set; }        

        public string Ip { get; set; } = "192.168.0.1";

        public int DeviceAddress { get; set; } = 1;


        public double AccumulatedValue { get; set; } = 0.0;
        public double InstantValue { get; set; } = 0.0;

        public RelayCommand DestroyFlowmeter { get; set; }
    }
}