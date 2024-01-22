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
        }



        [field:NonSerialized]
        public double Temperature { get; set; }
        [field: NonSerialized]
        public double Density { get; set; }
    }
}
