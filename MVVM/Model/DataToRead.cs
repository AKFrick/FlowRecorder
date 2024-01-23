using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FlowRecorder.MVVM.Model
{
    public class DataToRead
    {
        public ushort StartAddress { get; set; } = 0;
        public ushort NumOfPoint { get; set; } = 1;

        public FunctionCodes FunctionCode { get; set; } = FunctionCodes.ReadInput;

        public enum FunctionCodes
        {
            ReadHolding,
            ReadInput
        }



    }
}
