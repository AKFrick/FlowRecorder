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

            //using (StreamReader streamReader = new StreamReader("user.dat"))
            //{
            //    BinaryFormatter binaryFormatter = new BinaryFormatter();
            //    object obj;
            //    try
            //    {
            //        obj = binaryFormatter.Deserialize(streamReader.BaseStream);
            //        var cabs = (List<SerializableCabinet>)obj;
            //        foreach (var bincabinet in cabs)
            //        {
            //            Cabinet cab = new Cabinet()
            //            {
            //                Description = bincabinet.Description
            //            };
            //            addCabinet(cab);
            //            OutputLog.That($"Расходомеров: {bincabinet.Flowmeters.Count}");

            //            editCabinet = cab;
            //            foreach (var binflowmeter in bincabinet.Flowmeters)
            //            {
            //                OutputLog.That("Расходомер!!");
            //                Flowmeter flow = new Flowmeter()
            //                {
            //                    Description = binflowmeter.Description
            //                };
            //                addFlowmeter(flow);

            //            }
            //        }
            //    }
            //    catch (SerializationException ex)
            //    {
            //        OutputLog.That(ex.ToString());
            //    }
            //}





            #region Command            
            Save = new RelayCommand(obj => saveClick());
            AddCabinet = new RelayCommand(obj => addCabinetClick());


            #endregion
        }
        public ObservableCollection<LogItem> OutputItems { get; set; }
        public ObservableCollection<Cabinet> Cabinets { get; set; } = new ObservableCollection<Cabinet>();
        private List<SerializableCabinet> serializedCabinets { get; set; } = new List<SerializableCabinet>();

        public RelayCommand Save { get; set; }

        public RelayCommand AddCabinet { get; set; }
        void saveClick()
        {
            BinaryFormatter binFormat = new BinaryFormatter();
            // Сохранить объект в локальном файле.
            using (Stream fStream = new FileStream("user.dat",
               FileMode.Create, FileAccess.Write, FileShare.None))
            {                
                binFormat.Serialize(fStream, serializedCabinets);
            }

            using (AppDbContext  db = new AppDbContext())
            {
                MeterNode node = new MeterNode()
                {
                    NodeName = "Новое тестовое имя",
                    NodeCode = 1,
                    FlowMeterAddrIP = "192.168.10.1",
                    DensityMeterAddrIP = "192.168.10.2",
                    FlowMeterAddrModbus = 1,
                    FlowUpdateTimeInterval = 5000,
                    DensityUpdateTimeInterval = 6000,
                    TimeIntervalRecording = 12000,
                    FlowDeltaRecording = 500                    
                };

                db.MeterNode.Add(node);
                db.SaveChanges();
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
            cabinet.DestroyCabinetClicked += DestroyCabinetClick;
            Cabinets.Add(cabinet);
            OutputLog.That($"Добавлен новый ящик: {cabinet.Description}");
        }
        Cabinet editCabinet;                
        private void DestroyCabinetClick(Cabinet cabinet)
        {
            Cabinets.Remove(cabinet);
        }     
    }
}

