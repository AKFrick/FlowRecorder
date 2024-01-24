using FlowRecorder.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FlowRecorder
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            if(e.Args.Length > 0)
            {
                if(e.Args[0] == "programode")
                {
                    MainWindowViewModel.ProgrammableMode = true;
                    CabinetViewModel.ProgrammableMode = true;
                }
            }
            base.OnStartup(e);
        }
    }
}
