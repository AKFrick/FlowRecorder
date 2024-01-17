using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using FlowRecorder.MVVM.Db;


namespace FlowRecorder.MVVM.Model
{
    [Serializable]
    public class Flowmeter : Meter
    {
        public Flowmeter()
        {

        }
        
        public Action<Flowmeter> DestroyFlowmeterClicked;

        public void Start()
        {
            reader = new(Description, Ip, Port);
            reader.ValueRead += (value) => { InstantValue = value; };
            reader.Connected += Connected;
            reader.Disconnected += Disconnected;

            reader.StartReading();
        }
        public void Stop()
        {
            reader.StopReading();  
        }

        double prevSavedValue = 0;
        double diffToSave = 1;
        
        void checkValueToSave(double newFlowValue)
        {
            if ( Math.Abs(newFlowValue - prevSavedValue) > diffToSave)
            {
                prevSavedValue = newFlowValue;
                //saveToDb(newFlowValue);
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
        [field: NonSerialized]
        double instantValue;
        [field:NonSerialized]
        public event Action<double> InstantValueUpdated;
        [field: NonSerialized]
        public event Action Connected;
        [field: NonSerialized]
        public event Action Disconnected;
        [field: NonSerialized]
        DataReader reader;

        public Densitymeter Densitymeter { get; set; }
    }
}