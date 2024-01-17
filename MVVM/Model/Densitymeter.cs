using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRecorder.MVVM.Model
{
    public class Densitymeter : Meter
    {
        public double Temperature { get; set; }
        public double Density { get; set; }
    }
}
