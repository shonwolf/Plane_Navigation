using FlightgearSimulator;
using FlightgearSimulator.Properties;
using FlightSimulator.Model;
using Microsoft.Research.DynamicDataDisplay;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FlightSimulator
{
    class FlightBoardModel : INotifyPropertyChanged
    {
        // the members
        /* locations of plane */
        ObservableDataSource<Point> planeLocations;
        /* plotter */
        ChartPlotter plotter;
        /* if connection has been performed - 1 */
        private int connectPerformed = 0;
        /* Server */
        TelnetServer ts;
        /* Client */
        ITelnetClient mtc;
        /* ip of connection */
        private string ip;
        /* port of client */
        private int clientPort;
        /* port of server */
        private int serverPort;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// this is the constructor of this class.
        /// </summary>
        /// <param name="_planeLocations"></param>
        /// <param name="_plotter"></param>
        public FlightBoardModel(ObservableDataSource<Point> _planeLocations, ChartPlotter _plotter)
        {
            planeLocations = _planeLocations;
            plotter = _plotter;
            ts = new TelnetServer(planeLocations, plotter);
            mtc = new MyTelnetClient();
            ip = (string)Settings.Default["IP"];
            clientPort = Int32.Parse((string)Settings.Default["PortClient"]);
            serverPort = Int32.Parse((string)Settings.Default["PortServer"]);
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

        public string Ip
        {
            get { return ip; }
            set
            {
                ip = value;
                NotifyPropertyChanged("Ip");
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
        /// this function notifies that something has change.
        /// </summary>
        /// <param name="propName"></param>
        public void NotifyPropertyChanged(string propName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        /// <summary>
        /// this function execute the connectCommand.
        /// </summary>
        public void executeConnectCommand()
        {
            Connector connector = Connector.getInstance();

            /* if connection established at first */
            if (connectPerformed == 0)
            {
                connectPerformed = 1;
                // update the ip
                Ip = (string)Settings.Default["IP"];
                ServerPort = Int32.Parse((string)Settings.Default["PortServer"]);
                int check = ts.startServer(ip, serverPort);
                if (check == 0)
                {
                    // update the port
                    ClientPort = Int32.Parse((string)Settings.Default["PortClient"]);
                    mtc.connect(ip, clientPort);
                    connector.setServer(ts);
                    connector.setClient(mtc);
                    IJoystickModel model = connector.getJoystickModel();
                    model.setTelnetClient(mtc);
                    ts.readData();
                }
            }
            /* if connection has been established once */
            else
            {
                ts.closeServer();
                Ip = (string)Settings.Default["IP"];
                ServerPort = Int32.Parse((string)Settings.Default["PortServer"]);
                int check = ts.startServer(ip, serverPort);
                if (check == 0)
                {
                    mtc.disconnect();
                    // update the port
                    ClientPort = Int32.Parse((string)Settings.Default["PortClient"]);
                    mtc.connect(ip, clientPort);
                    connector.setServer(ts);
                    connector.setClient(mtc);
                    IJoystickModel model = connector.getJoystickModel();
                    model.setTelnetClient(mtc);
                    ts.readData();
                }
            }
        }
    }
}