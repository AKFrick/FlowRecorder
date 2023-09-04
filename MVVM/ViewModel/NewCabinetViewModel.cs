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
    public class NewCabinetViewModel
    {
        public CabinetViewModel NewCabinet { get; set; }
        public event Action<CabinetViewModel> CabinetCreated;

        public NewCabinetViewModel()
        {
            NewCabinet = new CabinetViewModel();
            Create = new RelayCommand(obj =>
            {
                CabinetCreated?.Invoke(NewCabinet);
            });
        }
        public RelayCommand Create { get; set; }
    }
}
