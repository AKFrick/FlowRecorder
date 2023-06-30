using System;
using System.Collections.Generic;
using System.Text;
using FlowRecorder.MVVM.Model;

namespace FlowRecorder.MVVM.ViewModel
{
    public class MainWindowViewModel
    {
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
    }
}
