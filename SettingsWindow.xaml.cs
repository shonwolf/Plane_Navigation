using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FlightSimulator
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        // the members
        private readonly SettingsViewModel viewModel;

        /// <summary>
        /// this is the constructor of this class.
        /// </summary>
        public SettingsWindow()
        {
            InitializeComponent();
            viewModel = new SettingsViewModel(this);
            DataContext = viewModel;
        }
    }
}
