using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FlightgearSimulator;
using FlightgearSimulator.Commands;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.SqlServer.Server;

namespace FlightSimulator
{
    class FlightBoardViewModel : INotifyPropertyChanged
    {
        // the members
        private FlightBoardModel model;
        // connect command
        private ICommand connectCommand;
        private ICommand settingsCommand;
        private ICommand disconnectCommand;
        private string ip;
        private int clientPort;
        private int serverPort;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// this is the constructor of this class.
        /// </summary>
        /// <param name="planeLocations"></param>
        /// <param name="plotter"></param>
        public FlightBoardViewModel(ObservableDataSource<Point> planeLocations, ChartPlotter plotter)
        {
            model = new FlightBoardModel(planeLocations, plotter);
        }

        public ICommand ConnectCommand
        {
            get
            {
                return connectCommand ?? (connectCommand = new ButtonClickCommand(() => ConnectPressed()));
            }
        }

        public ICommand SettingsCommand
        {
            get
            {
                return settingsCommand ?? (settingsCommand = new ButtonClickCommand(() => SettingsPressed()));
            }
        }

        public ICommand DisconnectCommand
        {
            get
            {
                return disconnectCommand ?? (disconnectCommand = new ButtonClickCommand(() => disconnectSockets()));
            }
        }

        public string Ip
        {
            get { return ip; }
            set
            {
                ip = value;
                NotifyPropertyChanged("Ip");
            }
        }

        public int ClientPort
        {
            get { return clientPort; }
            set
            {
                clientPort = value;
                NotifyPropertyChanged("ClientPort");
            }
        }

        public int ServerPort
        {
            get { return serverPort; }
            set
            {
                serverPort = value;
                NotifyPropertyChanged("ServerPort");
            }
        }

        /// <summary>
        /// this function disconnect the socket.
        /// </summary>
        private void disconnectSockets()
        {
            Connector connector = Connector.getInstance();
            ITelnetClient tc = connector.getClient();
            TelnetServer ts = connector.getServer();
            if (tc != null)
            {
                tc.disconnect();
            }
            if (ts != null)
            {
                ts.closeServer();
            }
        }

        /// <summary>
        /// this function showes the window of the settings.
        /// </summary>
        private void SettingsPressed()
        {
            Window window = new SettingsWindow();

            // Show window modelessly, NOTE: Returns without waiting for window to close
            window.Show();
        }

        /// <summary>
        /// this function start the thread of the connection.
        /// </summary>
        private void ConnectPressed()
        {
            model.executeConnectCommand();
        }

        /// <summary>
        /// this function notifies when property changes.
        /// </summary>
        /// <param name="propertyName"></param>
        protected void OnPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// this function notifies that something has change.
        /// </summary>
        /// <param name="propName"></param>
        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
