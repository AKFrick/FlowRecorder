using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            Temperature = data[0];
        }

        [field: NonSerialized]
        public event Action<double> TemperatureUpdated;
        public double Temperature
        {
            get { return instantValue; }
            set
            {
                temperature = value; TemperatureUpdated(temperature);
            }
        }
        [field: NonSerialized]
        double temperature;
        [field: NonSerialized]
        double instantValue;
        [field: NonSerialized]
        public double Density { get; set; }
    }
}
