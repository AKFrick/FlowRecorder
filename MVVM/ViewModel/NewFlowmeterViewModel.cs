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
        public Flowmeter NewFlowmeter { get; set; }
        public event Action<Flowmeter> FlowmeterCreated;
        public NewFlowmeterViewModel()
        {
            NewFlowmeter = new Flowmeter();
            Create = new RelayCommand(obj =>
            {
                FlowmeterCreated?.Invoke(NewFlowmeter);

            });
        }
        public NewFlowmeterViewModel(Flowmeter flowmeter)
        {
            NewFlowmeter = flowmeter;
            Create = new RelayCommand(obj =>
            {
                FlowmeterCreated?.Invoke(NewFlowmeter);

            });

        }
        public RelayCommand Create { get; set; }
    }
}
