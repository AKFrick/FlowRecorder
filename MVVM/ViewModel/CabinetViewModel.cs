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
            {
                var newFl = new FlowmeterViewModel(flowmeter);
                newFl.ChangeFlowmeterClicked += OpenChangeFlowmeterWindow;
                Flowmeters.Add(newFl);

            }    

            Description = cabinet.Description;

            cabinetModel = cabinet;

            ((INotifyCollectionChanged)cabinet.Flowmeters).CollectionChanged += (s, a) =>
            {
                if (a.NewItems?.Count >= 1)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        foreach (Flowmeter item in a.NewItems)
                        {
                            var newFl = new FlowmeterViewModel(item);
                            newFl.ChangeFlowmeterClicked += OpenChangeFlowmeterWindow;
                            Flowmeters.Add(newFl);

                        }
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
       
        //СОЗДАНИЕ НОВОГО РАСХОДОМЕРА
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

        //ИЗМЕНЕНИЕ РАСХОДОМЕРА   
        void OpenChangeFlowmeterWindow(Flowmeter flowmeter)
        {
            OutputLog.That("Subscr");
            NewFlowmeterViewModel model = new NewFlowmeterViewModel(flowmeter);
            //model.FlowmeterCreated += AddNewFlowmeter;

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
