using FlowRecorder.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;


namespace FlowRecorder.MVVM.Model
{
    public class Cabinet
    {
        public Cabinet()
        {
            AddFlowmeter = new RelayCommand(obj => AddFlowmeterClicked(this));                                    
        }

        public Action<Cabinet> AddFlowmeterClicked;
        public string Description { get; set; }

        public ObservableCollection<Flowmeter> Flowmeters { get; set; }

        public ObservableCollection<Flowmeter> Densitymeters { get; set; }

        public RelayCommand AddFlowmeter { get; set; }

    }
}
