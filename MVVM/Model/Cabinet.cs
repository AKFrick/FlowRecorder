using System;
using System.Collections.Generic;
using System.Text;

namespace FlowRecorder.MVVM.Model
{
    public class Cabinet
    {
        public Cabinet()
        {
        }
        public string Description { get; set; }

        public List<Flowmeter> Flowmeters { get; set; }


    }
}
