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
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;

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

            using (StreamReader streamReader = new StreamReader("user.dat"))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                object obj;
                try
                {
                    obj = binaryFormatter.Deserialize(streamReader.BaseStream);
                    var cabs  = (List<SerializableCabinet>)obj;
                    foreach (var cabinet in cabs)
                    {
                        Cabinet cab = new Cabinet()
                        {
                            Description = cabinet.Description
                        };
                        addCabinet(cab);
                    }
                }
                catch (SerializationException ex)
                {
                    OutputLog.That(ex.ToString());
                }
            }

            Cabinets.CollectionChanged += (s, a) =>
            {
                if (a.NewItems?.Count >= 1)
                {
                    foreach (Cabinet cabinet in a.NewItems)
                        serializedCabinets.Add(new SerializableCabinet(cabinet));
                }
                if (a.OldItems?.Count >= 1)
                {
                    foreach (Cabinet cabinet in a.OldItems)
                        serializedCabinets.RemoveAll(obj => obj.Description == cabinet.Description);
                }
            };



            #region Command            
            Save = new RelayCommand(obj => SaveClick());
            AddCabinet = new RelayCommand(obj => addCabinetClick());


            #endregion
        }

        public ObservableCollection<LogItem> OutputItems { get; set; }
        public ObservableCollection<Cabinet> Cabinets { get; set; } = new ObservableCollection<Cabinet>();
        private List<SerializableCabinet> serializedCabinets { get; set; } = new List<SerializableCabinet>();



        public RelayCommand Save { get; set; }

        public RelayCommand AddCabinet { get; set; }
        void SaveClick()
        {
            BinaryFormatter binFormat = new BinaryFormatter();
            // Сохранить объект в локальном файле.
            using (Stream fStream = new FileStream("user.dat",
               FileMode.Create, FileAccess.Write, FileShare.None))
            {
                binFormat.Serialize(fStream, serializedCabinets);
            }
        }

        //Добавить ящик
        void addCabinetClick()
        {
            NewCabinetViewModel viewModel = new NewCabinetViewModel();
            viewModel.CabinetCreated += addCabinet;
            NewCabinetWindow cabinetWindow = new NewCabinetWindow(viewModel);
            cabinetWindow.ShowDialog();
            
        }
        void addCabinet(Cabinet cabinet)
        {
            cabinet.Flowmeters = new ObservableCollection<Flowmeter>();
            cabinet.AddFlowmeterClicked += addFlowmeterClick;
            cabinet.DestroyCabinetClicked += DestroyCabinetClick;
            Cabinets.Add(cabinet);
            OutputLog.That($"Добавлен новый ящик: {cabinet.Description}");
        }

        Cabinet editCabinet;
        void addFlowmeter(Flowmeter flowmeter)
        {
            flowmeter.cabinet = editCabinet;
            flowmeter.DestroyFlowmeterClicked += DestroyFlowmeterClick;
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
        private void DestroyCabinetClick(Cabinet cabinet)
        {
            Cabinets.Remove(cabinet);
        }
        private void DestroyFlowmeterClick(Cabinet cabinet, Flowmeter flowmeter)
        {
            cabinet.Flowmeters.Remove(flowmeter);
        }

        
    }
}

