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

            MyCommand = new RelayCommand( obj => OutputLog.That("Worked!"));

            OutputLog.That("Первое сообщение");
        }

        public ObservableCollection<LogItem> OutputItems { get; set; }
        public List<Cabinet> Cabinets { get; set; } = new List<Cabinet>()
        {
            new Cabinet()
            {
                Description = "AC1H1",
                Flowmeters = new List<Flowmeter>
                {
                    new Flowmeter() { Description = "Путь 101"},
                    new Flowmeter() { Description = "Путь 102"}
                }
            },
            new Cabinet()
            {
                Description = "AC1H2",
                Flowmeters = new List<Flowmeter>
                {
                    new Flowmeter() { Description = "Путь 201"},
                    new Flowmeter() { Description = "Путь 202"}
                }
            }
        };
        public string MyDescript { get; set; } = "RABOTAET";

        public RelayCommand MyCommand { get; set; }
    }
}
