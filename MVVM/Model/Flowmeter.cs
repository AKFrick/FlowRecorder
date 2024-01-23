using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using FlowRecorder.MVVM.Db;


namespace FlowRecorder.MVVM.Model
{
    [Serializable]
    public class Flowmeter : Meter
    {
        public Flowmeter(Meter meter)
        {
            this.Description = meter.Description;
            this.Ip = meter.Ip;
            this.Port = meter.Port;
            this.DeviceAddress = meter.DeviceAddress;

            dataToRead = new()
            {
                StartAddress = 0,
                NumOfPoint = 33,
                FunctionCode = DataToRead.FunctionCodes.ReadInput,
            };
        }   

        double prevSavedValue = 0;
        double diffToSave = 1;

        protected override void getValues(ushort[] data)
        {
            InstantValue = data[26];
        }
        void checkValueToSave(double newFlowValue)
        {
            if ( Math.Abs(newFlowValue - prevSavedValue) > diffToSave)
            {
                prevSavedValue = newFlowValue;
                //saveToDb(newFlowValue);
            }
        }
        void saveToDb(double flow)
        {
            DatabaseControl.SaveFlowData(new DataForHLS()
            {
                vFlow = flow,
                vDensity = Densitymeter.Density,
            });
        }        
        public double AccumulatedValue { get; set; } = 0.0;
        public double InstantValue 
        { 
            get { return instantValue; } 
            set 
            { 
                checkValueToSave(value);
                instantValue = value; InstantValueUpdated(instantValue); 
            } 
        }
        [field: NonSerialized]
        double instantValue;
        [field:NonSerialized]
        public event Action<double> InstantValueUpdated;
        [field: NonSerialized]
        public Densitymeter Densitymeter { get; set; }
    }
}