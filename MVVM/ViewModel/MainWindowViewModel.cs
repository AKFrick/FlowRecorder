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
            MyCommand = new RelayCommand(obj =>

            {
                OutputLog.That("Worked!");
                Cabinets[0].Flowmeters.Add(new Flowmeter() { Description = "Путь 11" });
            })
            {

            };
            AddCabinet = new RelayCommand(obj =>
            {
                OutputLog.That("Worked!");
                var cab = new Cabinet()
                {
                    Description = "AC1H1",
                    Flowmeters = new ObservableCollection<Flowmeter>()
                };
                cab.AddFlowmeterClicked += addFlowmeter;
                Cabinets.Add(cab);
            });
          
            #endregion
        }

        public ObservableCollection<LogItem> OutputItems { get; set; }
        public ObservableCollection<Cabinet> Cabinets { get; set; } = new ObservableCollection<Cabinet>()
        {
            new Cabinet()
            {
                Description = "Путь",
                Flowmeters = new ObservableCollection<Flowmeter>()
                {
                    new Flowmeter() { Description = "Тест"},
                    new Flowmeter() { Description = "Тест"},
                    new Flowmeter() { Description = "Тест"},
                    new Flowmeter() { Description = "Тест"},
                    new Flowmeter() { Description = "Тест"},
                    new Flowmeter() { Description = "Тест"},
                    new Flowmeter() { Description = "Тест"}
                },

                Densitymeters = new ObservableCollection<Flowmeter>()
                {
                    new Flowmeter() { Description = "Test"}
                }
                
            }
        };
        public string MyDescript { get; set; } = "RABOTAET";

        public RelayCommand MyCommand { get; set; }
        public RelayCommand AddCabinet { get; set; }

        private void addFlowmeter(Cabinet cabinet)
        {
            cabinet.Flowmeters.Add(new Flowmeter() { Description = "Worked!" });
        }
    }
}

