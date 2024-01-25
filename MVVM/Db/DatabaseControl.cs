using FlowRecorder.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace FlowRecorder.MVVM.Db
{
    static public class DatabaseControl
    {      
        static public void SaveFlowData(DataForHLS data)
        {
            using (AppDbContext db = new())
            {
                db.DataForHLS.Add(data);                
                db.SaveChanges();

                OutputLog.That($"Записано значение: {data.ToLog()}");                
            }
        }

        static public void SaveMeterNodes(ObservableCollection<Cabinet> cabinets)
        {
            OutputLog.That("Сохранение новой конфигурации...");
            using (AppDbContext db = new())
            {
                db.MeterNode.RemoveRange(db.MeterNode);

                foreach (Cabinet cabinet in cabinets)
                {
                    foreach(Flowmeter flowmeter in cabinet.Flowmeters)
                    {
                        db.MeterNode.Add(new MeterNode()
                        {
                            NodeName = flowmeter.Description,
                            CabinetName = flowmeter.CabinetName,

                            FlowMeterAddrIP = flowmeter.Ip,
                            DensityMeterAddrIP = flowmeter.Densitymeter.Ip,
                            FlowMeterAddrModbus = flowmeter.DeviceAddress,
                            FlowUpdateTimeInterval = flowmeter.UpdateInterval,
                            DensityUpdateTimeInterval = flowmeter.UpdateInterval,
                            TimeIntervalRecording = flowmeter.TimeIntervalRecording,
                            FlowDeltaRecording = flowmeter.FlowDeltaRecording,
                            NodeCode = flowmeter.NodeCode,
                        }); 
                    }                    
                }

                db.SaveChanges();
            }
            OutputLog.That("Конфигурация сохранена!");
        }
    }
}
