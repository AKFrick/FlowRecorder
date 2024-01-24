using System;
using NModbus.Utility;

namespace FlowRecorder.MVVM.Model
{
    [Serializable]
    public class Densitymeter : Meter
    {
        public Densitymeter(Meter meter)
        {
            this.Description = meter.Description;
            this.Ip = meter.Ip;
            this.Port = meter.Port;
            this.DeviceAddress = meter.DeviceAddress;

            dataToRead = new()
            {
                StartAddress = 0,
                NumOfPoint = 10,
                FunctionCode = DataToRead.FunctionCodes.ReadInput,
            };
        }

        protected override void getValues(ushort[] data)
        {
            Temperature = ModbusUtility.GetSingle(data[3], data[2]); 
            Density = ModbusUtility.GetSingle(data[1], data[0]);
        }

        [field: NonSerialized]
        public event Action<double> TemperatureUpdated;
        public double Temperature
        {
            get { return temperature; }
            set
            {
                temperature = value; TemperatureUpdated(temperature);
            }
        }
        [field: NonSerialized]
        double temperature;

        [field: NonSerialized]
        public event Action<double> DensityUpdated;
        public double Density
        {
            get { return density; }
            set
            {
                density = value; DensityUpdated(density);
            }
        }
        [field: NonSerialized]
        double density;
    }
}
