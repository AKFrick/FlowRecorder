using FlowRecorder.MVVM.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRecorder.MVVM.ViewModel
{
    public class CabinetViewModel
    {
        public CabinetViewModel()
        {
            Flowmeters = new ObservableCollection<FlowmeterViewModel>();

            DestroyCabinet = new RelayCommand(obj => DestroyCabinetClicked?.Invoke(this));

            AddFlowmeterCommand = new RelayCommand(obj => OpenNewFlowmeterWindow());

        }
        public void AddNewFlowmeter(FlowmeterViewModel flowmeter)
        {
            flowmeter.DestroyFlowmeterClicked += DestroyFlowmeter;
            Flowmeters.Add(flowmeter);
        }

        public Action<CabinetViewModel> AddFlowmeterClicked;
        public string Description { get; set; }
        public virtual ObservableCollection<FlowmeterViewModel> Flowmeters { get; set; }

        public ObservableCollection<FlowmeterViewModel> Densitymeters { get; set; }

        public RelayCommand AddFlowmeterCommand { get; set; }
        void OpenNewFlowmeterWindow()
        {
            NewFlowmeterViewModel model = new NewFlowmeterViewModel();
            model.FlowmeterCreated += AddNewFlowmeter;

            NewFlowmeterWindow newFlowmeter = new NewFlowmeterWindow(model);
            newFlowmeter.ShowDialog();
        }
        void DestroyFlowmeter(FlowmeterViewModel flowmeter)
        {
            Flowmeters.Remove(flowmeter);
        }
        public Action<CabinetViewModel> DestroyCabinetClicked;
        public RelayCommand DestroyCabinet { get; set; }

    }
}
