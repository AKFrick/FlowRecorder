using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRecorder.MVVM.Model
{
    [Serializable]
    public class Meter
    {
        public string Description { get; set; }
        public string Ip { get; set; } = "192.168.10.254";
        public int Port { get; set; } = 4001;
        public int DeviceAddress { get; set; } = 1;
    }
}
