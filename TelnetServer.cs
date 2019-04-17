using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay;
using System.Windows;
using System.Windows.Threading;
using FlightgearSimulator.Properties;
using System.Threading;

namespace FlightSimulator
{
    class TelnetServer
    {
        // the members
        private Socket s;
        ObservableDataSource<Point> planeLocations;
        ChartPlotter plotter;
        private volatile bool isConnected;
        private Thread thread;

        /// <summary>
        /// this is the constructor of this class.
        /// </summary>
        /// <param name="_planeLocations"></param>
        /// <param name="_plotter"></param>
        public TelnetServer(ObservableDataSource<Point> _planeLocations, ChartPlotter _plotter)
        {
            this.s = null;
            planeLocations = _planeLocations;
            plotter = _plotter;
            isConnected = false;
        }

        /// <summary>
        /// this function starts the server and return 0 if secceded, 1 otherwise.
        /// </summary>
        /// <returns>0 if secceded, 1 otherwise</returns>
        public int startServer(string ip, int port)
        {
            try
            {
                IPAddress ipAd = IPAddress.Parse(ip);
                /* Initializes the Listener */
                TcpListener myList = new TcpListener(ipAd, port);
                /* Start Listeneting at the specified port */
                myList.Start();
                this.s = myList.AcceptSocket();
                isConnected = true;
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error In Connecting...." + e.StackTrace);
                return 1;
            }
        }

        /// <summary>
        /// this function reads data.
        /// </summary>
        public void readData()
        {
            thread = new Thread(new ThreadStart(() =>
            {
                /* perform reading of data until socket shall be closed */
                while (isConnected)
                {
                    // try
                    //{
                    bool part1 = s.Poll(1000, SelectMode.SelectRead);
                    bool part2 = (s.Available == 0);
                    if (s == null || part1 && part2)
                    {
                        break;
                    }
                    byte[] b = new byte[100000];
                    int k = s.Receive(b);
                    string lon = " ";
                    string lat = " ";

                    /* get bytes representation to string representation */
                    string str = Encoding.UTF8.GetString(b, 0, k);
                    var builder = new StringBuilder();
                    int timesCommaEncountered = 0;

                    /* pass each char that has been sended from client */
                    for (int i = 0; i < k; i++)
                    {
                        if (str[i] == ',')
                        {
                            if (timesCommaEncountered == 1)
                            {
                                lon = builder.ToString();
                            }
                            else
                            {
                                lat = builder.ToString();
                            }
                            builder.Clear();
                            timesCommaEncountered++;
                            if (timesCommaEncountered >= 2)
                            {
                                break;
                            }
                            continue;
                        }
                        builder.Append(str[i]);
                    }

                    /* create point to be added to plot */
                    Point p1 = new Point(double.Parse(lon, System.Globalization.CultureInfo.InvariantCulture),
                                         double.Parse(lat, System.Globalization.CultureInfo.InvariantCulture));
                    planeLocations.AppendAsync(plotter.Dispatcher, p1);

                    /* Notify client that his data has been accepted */
                    ASCIIEncoding asen = new ASCIIEncoding();
                    s.Send(asen.GetBytes("The string was recieved by the server."));
                }
            }));
            thread.Start();
        }

        /// <summary>
        /// this function closes the server.
        /// </summary>
        public void closeServer()
        {
            isConnected = false;
            // stop recive data from the simulator - stop the thread that recive data
            thread.Abort();
            if (s != null)
            {
                s.Close();
            }
        }
    }
}