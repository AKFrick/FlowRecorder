using System;
using System.Collections.Generic;
using System.Text;
using FlowRecorder.MVVM.Model;
using FlowRecorder.MVVM.Db;
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


            cabinetsModel = new();

            #region ящики
            ((INotifyCollectionChanged)cabinetsModel).CollectionChanged += (s, a) =>
            {
                if (a.NewItems?.Count >= 1)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        foreach (Cabinet item in a.NewItems)
                            Cabinets.Add(new CabinetViewModel(item));
                    }));
                if (a.OldItems?.Count >= 1)
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        //foreach (Cabinet item in a.OldItems)
                        //    Cabinets.Remove();
                    }));
            };
            #endregion

            #region сериализация
            if (File.Exists("user.dat"))
            {
                using (StreamReader streamReader = new StreamReader("user.dat"))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    object obj;
                    try
                    {
                        obj = binaryFormatter.Deserialize(streamReader.BaseStream);
                        var cabs = (ObservableCollection<Cabinet>)obj;
                        foreach (var bincabinet in cabs)
                        {
                            Cabinet cab = new Cabinet()
                            {
                                Description = bincabinet.Description
                            };
                            foreach (var binflowmeter in bincabinet.Flowmeters)
                            {
                                Flowmeter flow = new Flowmeter()
                                {
                                    Description = binflowmeter.Description
                                };
                                cab.AddNewFlowmeter(flow);
                            }
                            addCabinet(cab);

                            OutputLog.That($"Расходомеров: {bincabinet.Flowmeters.Count}");

                        }
                    }
                    catch (SerializationException ex)
                    {
                        OutputLog.That(ex.ToString());
                    }
                }
            }            
            #endregion

            #region Command            
            BtnSave = new(obj => btnSaveClick());
            BtnStart = new(obj => btnStartClick());
            BtnStop = new(obj => btnStopClick());
            AddCabinet = new(obj => addCabinetClick());
            #endregion            
        }
        public ObservableCollection<LogItem> OutputItems { get; set; }
        public ObservableCollection<CabinetViewModel> Cabinets { get; set; } = new ObservableCollection<CabinetViewModel>();
        ObservableCollection<Cabinet> cabinetsModel;
        //private List<SerializableCabinet> serializedCabinets { get; set; } = new List<SerializableCabinet>();
        public RelayCommand BtnSave { get; set; }
        void btnSaveClick()
        {
            BinaryFormatter binFormat = new BinaryFormatter();
            // Сохранить объект в локальном файле.
            using (Stream fStream = new FileStream("user.dat",
               FileMode.Create, FileAccess.Write, FileShare.None))
            {
                binFormat.Serialize(fStream, cabinetsModel);
            }
            //DatabaseControl.SaveMeterNodes(cabinetsModel);
        }
        public RelayCommand BtnStart { get; set; }

        void btnStartClick() 
        {
            cabinetsModel[0].Start();

            //foreach (var cab in cabinetsModel)
            //     cab.Start();
        }
        public RelayCommand BtnStop { get; set; }
        void btnStopClick()         
        {
            foreach (var cabinet in cabinetsModel)
               cabinet.Stop();
        }
        public RelayCommand AddCabinet { get; set; }
        void addCabinetClick()
        {
            NewCabinetViewModel viewModel = new NewCabinetViewModel();
            viewModel.CabinetCreated += addCabinet;
            NewCabinetWindow cabinetWindow = new NewCabinetWindow(viewModel);
            cabinetWindow.ShowDialog();
        }        
        void addCabinet(Cabinet cabinet)
        {            
            //cabinet.DestroyCabinetClicked += DestroyCabinetClick;
            cabinetsModel.Add(cabinet);
            
            OutputLog.That($"Добавлен новый ящик: {cabinet.Description}");
        }                        
    }
}

