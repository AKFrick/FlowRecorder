using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRecorder.MVVM.Db
{
    public class DataForHLS
    {
        [Key]
        public int Id { get; set; }
        public int NodeCode { get; set; }
        public DateTime DTRecording { get; set; }
        public double vFlow { get; set; }
        public double vDensity { get; set; }

    }

    public static class DataForHLSExtension
    {
        public static string ToLog(this DataForHLS data)
        {
            return data.vFlow.ToString();
        }
    }

}
