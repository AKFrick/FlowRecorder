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
        public byte DeviceAddress { get; set; } = 1;
        public void Start()
        {
            reader = new(Description, Ip, Port, DeviceAddress, dataToRead);
            reader.Connected += Connected;
            reader.Disconnected += Disconnected;
            reader.DataRead += getValues;

            reader.StartReading();
        }
        public void Stop()
        {
            reader.StopReading();
        }

        [field: NonSerialized]
        public event Action Connected;
        [field: NonSerialized]
        public event Action Disconnected;
        [field: NonSerialized]
        protected DataToRead dataToRead;

        protected virtual void getValues(ushort[] data) { }

        [field: NonSerialized] 
        protected DataReader reader;
    }
}
