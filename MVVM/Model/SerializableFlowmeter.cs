using System;
using System.Collections.Generic;
using System.Text;

namespace FlowRecorder.MVVM.Model
{
    [Serializable]
    public class SerializableFlowmeter
    {
        public SerializableFlowmeter(Flowmeter flowmeter)
        {
            Description = flowmeter.Description;
            Ip = flowmeter.Ip;
            DeviceAddress = flowmeter.DeviceAddress;
        }
        public string Description { get; set; }

        public string Ip { get; set; } = "192.168.0.1";

        public int DeviceAddress { get; set; } = 1;
    }
}
