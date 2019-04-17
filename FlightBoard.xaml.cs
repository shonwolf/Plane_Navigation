using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using FlightSimulator.Model;
using FlightSimulator.ViewModels;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;

namespace FlightSimulator.Views
{
    /// <summary>
    /// Interaction logic for MazeBoard.xaml
    /// </summary>
    public partial class FlightBoard : UserControl
    {
        // the members
        ObservableDataSource<Point> planeLocations = null;
        private readonly FlightBoardViewModel viewModel;

        /// <summary>
        /// this is the constructor of this class.
        /// </summary>
        public FlightBoard()
        {
            InitializeComponent();
            planeLocations = new ObservableDataSource<Point>();
            plotter.AddLineGraph(planeLocations, Colors.Blue, 2, "Route");
            viewModel = new FlightBoardViewModel(planeLocations, plotter);
            DataContext = viewModel;
        }
    }
}

