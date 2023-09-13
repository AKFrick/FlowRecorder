using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using FlowRecorder.MVVM.Db;


namespace FlowRecorder.MVVM.Model
{
    public class Flowmeter
    {
        public Flowmeter()
        {

        }
        
        public Action<Flowmeter> DestroyFlowmeterClicked;
        public string Description { get; set; }        

        public string Ip { get; set; } = "192.168.10.254";
        public int Port { get; set; } = 4001;
        public void Start()
        {
            reader = new(Description);
            reader.ValueRead += (value) => { InstantValue = value; };
            reader.Connected += Connected;
            reader.Disconnected += Disconnected;

            reader.StartReading();
        }
        public void Stop()
        {
            reader.StopReading();  
        }
        double prevSavedValue;
        double diffToSave = 1;
        
        void checkValueToSave(double newFlowValue)
        {
            if ( Math.Abs(newFlowValue - prevSavedValue) > diffToSave)
            {
                prevSavedValue = newFlowValue;
                saveToDb(newFlowValue);
            }
        }
        void saveToDb(double flow)
        {
            using (AppDbContext db = new())
            {
                db.DataForHLS.Add(new DataForHLS()
                {
                    DTRecording = DateTime.Now,
                    vFlow = flow,
                    NodeCode = Convert.ToInt16(Description)
                });

                db.SaveChanges();
                OutputLog.That($"{Description} Записано новое значение {flow}");
            }
        }
        public int DeviceAddress { get; set; } = 1;           
        public double AccumulatedValue { get; set; } = 0.0;
        public double InstantValue 
        { 
            get { return instantValue; } 
            set 
            { 
                checkValueToSave(value);
                instantValue = value; InstantValueUpdated(instantValue); 
            } 
        }
        double instantValue;
        public event Action<double> InstantValueUpdated;
        public event Action Connected;
        public event Action Disconnected;

        DataReader reader;
    }
}