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
            InstantValue = ModbusUtility.GetSingle(data[12], data[11]);
            AccumulatedValue = ModbusUtility.GetUInt32(data[24], data[23]);
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
        [field:NonSerialized]
        public event Action<double> InstantValueUpdated;
        [field: NonSerialized]
        public event Action<uint> AccumulatedValueUpdated;
        [field: NonSerialized]
        public Densitymeter Densitymeter { get; set; }
    }
}