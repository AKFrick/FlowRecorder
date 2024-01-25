using System;
using FlowRecorder.MVVM.Db;
using NModbus.Utility;


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
            this.FlowDeltaRecording = meter.FlowDeltaRecording;
            this.TimeIntervalRecording = meter.TimeIntervalRecording;
            this.UpdateInterval = meter.UpdateInterval;

            dataToRead = new()
            {
                StartAddress = 0,
                NumOfPoint = 33,
                FunctionCode = DataToRead.FunctionCodes.ReadInput,
            };
        }
        public string CabinetName { get; set; }
        [field:NonSerialized]
        public int NodeCode { get; set; } = 0;

        public void AddDensityMeter(Densitymeter densitymeter)
        {
            densitymeter.TemperatureUpdated += (value) => { temp = value; };
            densitymeter.DensityUpdated += (value) => { dens = value; };
            Densitymeter = densitymeter;
        }

        double prevSavedValue = 0;
        DateTime prevRecord;

        protected override void getValues(ushort[] data)
        {
            InstantValue = ModbusUtility.GetSingle(data[12], data[11]);
            AccumulatedValue = ModbusUtility.GetUInt32(data[24], data[23]);
        }
        void checkValueToSave(double newFlowValue)
        {
            TimeSpan ts = DateTime.Now - prevRecord;

            if ((Math.Abs(newFlowValue - prevSavedValue) > FlowDeltaRecording)
                || (ts.TotalMilliseconds > TimeIntervalRecording))
            {
                prevSavedValue = newFlowValue;
                prevRecord = DateTime.Now;  
                saveToDb(newFlowValue);
            }
        }
        void saveToDb(double flow)
        {
            DatabaseControl.SaveFlowData(new DataForHLS()
            {
                NodeCode = NodeCode,
                DTRecording = DateTime.Now,
                vFlow = flow,
                vDensity = dens,
            });
        }
        public uint AccumulatedValue
        {
            get { return accumulatedValue; }
            set
            {
                checkValueToSave(value);
                accumulatedValue = value; AccumulatedValueUpdated(accumulatedValue);
            }
        }
        [field: NonSerialized]
        uint accumulatedValue;
        public double InstantValue 
        { 
            get { return instantValue; } 
            set 
            {                 
               instantValue = value; InstantValueUpdated(instantValue); 
            } 
        }
        [field: NonSerialized]
        double instantValue;

        double temp;
        double dens;

        [field:NonSerialized]
        public event Action<double> InstantValueUpdated;
        [field: NonSerialized]
        public event Action<uint> AccumulatedValueUpdated;
        [field: NonSerialized]
        public Densitymeter Densitymeter;
    }
}