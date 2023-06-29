using System;
using System.Collections.Generic;
using System.Text;

namespace FlowRecorder.MVVM.Model
{
    public class Flowmeter
    {
        public Flowmeter()
        {
        }
        public string Description { get; set; }
        public double AccumulatedValue { get; set; }
        public double InstantValue { get; set; }

    }
}