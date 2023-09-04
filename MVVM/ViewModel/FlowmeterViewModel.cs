using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRecorder.MVVM.ViewModel
{
    public class FlowmeterViewModel
    {
        public FlowmeterViewModel()
        {
            DestroyFlowmeter = new RelayCommand(obj => DestroyFlowmeterClicked?.Invoke(this));
        }

        public Action<FlowmeterViewModel> DestroyFlowmeterClicked;
        public string Description { get; set; }                

        public double AccumulatedValue { get; set; } = 0.0;
        public double InstantValue { get; set; } = 0.0;

        public RelayCommand DestroyFlowmeter { get; set; }
    }
}