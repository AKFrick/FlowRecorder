using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FlowRecorder.MVVM.Model
{
    public class Cabinet
    {
        public Cabinet()
        {
        }
        public string Description { get; set; }

        public ObservableCollection<Flowmeter> Flowmeters { get; set; }

        public ObservableCollection<Flowmeter> Densitymeters { get; set; }


    }
}
