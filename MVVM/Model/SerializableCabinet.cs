using FlowRecorder.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FlowRecorder.MVVM.Model
{
    [Serializable]
    public class SerializableCabinet
    {
        public SerializableCabinet(Cabinet cab)
        {
            Description = cab.Description;
            Flowmeters = new List<SerializableFlowmeter>();

            cab.Flowmeters.CollectionChanged += (s, a) =>
            {
                if (a.NewItems?.Count >= 1)
                {
                    foreach (Flowmeter item in a.NewItems)
                        Flowmeters.Add(new SerializableFlowmeter(item));
                }
                if (a.OldItems?.Count >= 1)
                {
                    foreach (Flowmeter item in a.OldItems)
                        Flowmeters.RemoveAll(obj => obj.Description == item.Description);
                }
            };
        }

        public string Description { get; set; }

        public List<SerializableFlowmeter> Flowmeters { get; set; }

        public List<SerializableFlowmeter> Densitymeters { get; set; }
    }
}
 