using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRecorder.MVVM.Db
{
    public class MeterNode
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string NodeName { get; set; }
        [Required]
        public int NodeCode { get; set; }
        public string FlowMeterAddrIP { get; set; }
        public string DensityMeterAddrIP { get; set; }
        public int FlowMeterAddrModbus { get; set; }
        public int FlowUpdateTimeInterval { get; set; }
        public int DensityUpdateTimeInterval { get; set; }
        public int TimeIntervalRecording { get; set; }
        public double FlowDeltaRecording { get; set; }        
        public string CabinetName { get; set; }
    }
}
