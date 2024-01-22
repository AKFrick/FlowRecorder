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
                newFl.ChangeFlowmeterClicked += OpenChangeMeterWindow;
                Flowmeters.Add(newFl);

            }    

            Description = cabinet.Description;

            cabinetModel = cabinet;

            //Подписка на расходомеры для отображения
            ((INotifyCollectionChanged)cabinet.Flowmeters).CollectionChanged += (s, a) =>
            {
                if (a.NewItems?.Count >= 1)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        foreach (Flowmeter item in a.NewItems)
                        {
                            var newFl = new FlowmeterViewModel(item);
                            newFl.ChangeFlowmeterClicked += OpenChangeMeterWindow;
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
            //Подписка на плотномеры для отображения
            ((INotifyCollectionChanged)cabinet.Densitymeters).CollectionChanged += (s, a) =>
            {
                if (a.NewItems?.Count >= 1)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        foreach (Densitymeter item in a.NewItems)
                        {
                            var newFl = new DensitymeterViewModel(item);
                            newFl.ChangeMeterClicked += OpenChangeMeterWindow;
                            Densitymeters.Add(newFl);

                        }
                    }));
                if (a.OldItems?.Count >= 1)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        //foreach (LogItem item in a.OldItems)
                        //    OutputItems.Remove(item);
                    }));
            };
           

            AddFlowmeterCommand = new RelayCommand(obj => OpenNewFlowmeterWindow());
            AddDensitymeterCommand = new RelayCommand(obj => OpenNewDensitymeterWindow());
        }
        public Cabinet cabinetModel { get; private set; }
        
        public string Description { get; set; }
        public virtual ObservableCollection<FlowmeterViewModel> Flowmeters { get; set; }
        public virtual ObservableCollection<DensitymeterViewModel> Densitymeters { get; set; }

        //СОЗДАНИЕ НОВОГО РАСХОДОМЕРА
        public RelayCommand AddFlowmeterCommand { get; set; }
        public RelayCommand AddDensitymeterCommand { get; set; }
        void OpenNewFlowmeterWindow()
        {
            NewMeterViewModel model = new NewMeterViewModel(new Meter());
            model.MeterCreated += AddNewFlowmeter;

            NewMeterWindow newFlowmeter = new NewMeterWindow(model);
            newFlowmeter.ShowDialog();
        }
        void OpenNewDensitymeterWindow()
        {
            NewMeterViewModel model = new NewMeterViewModel(new Meter());
            model.MeterCreated += AddNewDensitymeter;

            NewMeterWindow newFlowmeter = new NewMeterWindow(model);
            newFlowmeter.ShowDialog();
        }
        public void AddNewFlowmeter(Meter meter)
        {
            //flowmeter.DestroyFlowmeterClicked += DestroyFlowmeter;
            //Flowmeters.Add(flowmeter);

            cabinetModel.AddNewFlowmeter(new Flowmeter(meter));
        }
        public void AddNewDensitymeter(Meter meter)
        {
            cabinetModel.AddNewDensityMeter(new Densitymeter(meter));
        }

        //ИЗМЕНЕНИЕ РАСХОДОМЕРА   
        void OpenChangeMeterWindow(Meter meter)
        {
            OutputLog.That("Subscr");
            NewMeterViewModel model = new NewMeterViewModel(meter);
            //model.FlowmeterCreated += AddNewFlowmeter;

            NewMeterWindow newFlowmeter = new NewMeterWindow(model);
            newFlowmeter.ShowDialog();
        }




    }
}
