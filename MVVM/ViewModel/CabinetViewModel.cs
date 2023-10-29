using FlowRecorder.MVVM.View;
using FlowRecorder.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Windows;


namespace FlowRecorder.MVVM.ViewModel
{
    public class CabinetViewModel 
    {
        public CabinetViewModel(Cabinet cabinet)
        {     
            Flowmeters = new ObservableCollection<FlowmeterViewModel>();
            foreach (var flowmeter in cabinet.Flowmeters)
                Flowmeters.Add(new FlowmeterViewModel(flowmeter));

            Description = cabinet.Description;

            cabinetModel = cabinet;

            ((INotifyCollectionChanged)cabinet.Flowmeters).CollectionChanged += (s, a) =>
            {
                if (a.NewItems?.Count >= 1)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        foreach (Flowmeter item in a.NewItems)
                            Flowmeters.Add(new FlowmeterViewModel(item));
                    }));
                if (a.OldItems?.Count >= 1)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        //foreach (LogItem item in a.OldItems)
                        //    OutputItems.Remove(item);
                    }));
            };


            DestroyCabinet = new RelayCommand(obj => DestroyCabinetClicked?.Invoke(this));

            AddFlowmeterCommand = new RelayCommand(obj => OpenNewFlowmeterWindow());

        }
        public Cabinet cabinetModel { get; private set; }
        
        public string Description { get; set; }
        public virtual ObservableCollection<FlowmeterViewModel> Flowmeters { get; set; }        

        public RelayCommand AddFlowmeterCommand { get; set; }
        void OpenNewFlowmeterWindow()
        {
            NewFlowmeterViewModel model = new NewFlowmeterViewModel();
            model.FlowmeterCreated += AddNewFlowmeter;

            NewFlowmeterWindow newFlowmeter = new NewFlowmeterWindow(model);
            newFlowmeter.ShowDialog();
        }
        public void AddNewFlowmeter(Flowmeter flowmeter)
        {
            //flowmeter.DestroyFlowmeterClicked += DestroyFlowmeter;
            //Flowmeters.Add(flowmeter);

            cabinetModel.AddNewFlowmeter(flowmeter);
        }
        void DestroyFlowmeter(FlowmeterViewModel flowmeter)
        {
            Flowmeters.Remove(flowmeter);
        }
        public Action<CabinetViewModel> DestroyCabinetClicked;
        public RelayCommand DestroyCabinet { get; set; }


    }
}
