using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FlowRecorder.MVVM.ViewModel;

namespace FlowRecorder.MVVM.View
{
    /// <summary>
    /// Interaction logic for NewFlowmeterWindow.xaml
    /// </summary>
    public partial class NewMeterWindow : Window
    {
        public NewMeterWindow(NewMeterViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
            viewModel.MeterCreated += (meter) => Close();
        }
    }
}
