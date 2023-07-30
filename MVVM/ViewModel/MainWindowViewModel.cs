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
using FlowRecorder.MVVM.View;

namespace FlowRecorder.MVVM.ViewModel
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            #region LOG
            OutputItems = new ObservableCollection<LogItem>(OutputLog.GetInstance().LogItems);
            ((INotifyCollectionChanged)OutputLog.GetInstance().LogItems).CollectionChanged += (s, a) =>
            {
                if (a.NewItems?.Count >= 1)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        foreach (LogItem item in a.NewItems)
                            OutputItems.Add(item);
                    }));
                if (a.OldItems?.Count >= 1)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        foreach (LogItem item in a.OldItems)
                            OutputItems.Remove(item);
                    }));
            };
            #endregion

            #region Command            
            AddCabinet = new RelayCommand(obj => addCabinetClick());           
          
            #endregion
        }

        public ObservableCollection<LogItem> OutputItems { get; set; }
        public ObservableCollection<Cabinet> Cabinets { get; set; } = new ObservableCollection<Cabinet>();

        public RelayCommand AddCabinet { get; set; }

        void addCabinet(Cabinet cabinet)
        {
            cabinet.Flowmeters = new ObservableCollection<Flowmeter>();
            cabinet.AddFlowmeterClicked += addFlowmeterClick;
            Cabinets.Add(cabinet);
            OutputLog.That($"Добавлен новый ящик: {cabinet.Description}");
        }

        void addCabinetClick()
        {
            NewCabinetViewModel viewModel = new NewCabinetViewModel();
            viewModel.CabinetCreated += addCabinet;
            NewCabinetWindow cabinetWindow = new NewCabinetWindow(viewModel);
            cabinetWindow.ShowDialog();
            
        }


        Cabinet editCabinet;
        void addFlowmeter(Flowmeter flowmeter)
        {
            editCabinet.Flowmeters.Add(flowmeter);
            OutputLog.That($"Добавлен новый расходомер: {flowmeter.Description}");
        }

        private void addFlowmeterClick(Cabinet cabinet)
        {
            editCabinet = cabinet;
            NewFlowmeterViewModel model = new NewFlowmeterViewModel();
            model.FlowmeterCreated += addFlowmeter;

            NewFlowmeterWindow newFlowmeter = new NewFlowmeterWindow(model);
            newFlowmeter.ShowDialog();            
        }
    }
}

