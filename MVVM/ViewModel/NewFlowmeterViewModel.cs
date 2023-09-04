using System;
using System.Collections.Generic;
using System.Text;
using FlowRecorder.MVVM.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Media;
using System.Configuration;
using System.Windows;
using System.Windows.Input;

namespace FlowRecorder.MVVM.ViewModel
{
    public class NewFlowmeterViewModel
    {
        public FlowmeterViewModel NewFlowmeter { get; set; }
        public event Action<FlowmeterViewModel> FlowmeterCreated;
        public NewFlowmeterViewModel()
        {
            NewFlowmeter = new FlowmeterViewModel();
            Create = new RelayCommand(obj =>
            {
                FlowmeterCreated?.Invoke(NewFlowmeter);

            });
        }
        public RelayCommand Create { get; set; }
    }
}
