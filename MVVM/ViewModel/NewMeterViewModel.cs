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
    public class NewMeterViewModel
    {
        public Meter NewMeter { get; set; }

        public event Action<Meter> MeterCreated;

        public NewMeterViewModel(Meter meter)
        {
            NewMeter = meter;
            Create = new RelayCommand(obj =>
            {
                MeterCreated?.Invoke(NewMeter);

            });

        }
        public RelayCommand Create { get; set; }
    }
}
