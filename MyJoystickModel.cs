using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightgearSimulator;
using System.ComponentModel;
using System.Threading;
using FlightSimulator.ViewModels;

namespace FlightSimulator.Model
{
    class MyJoystickModel : IJoystickModel
    {
        // the members
        ITelnetClient telnetClient;
        private double throttle;
        private double aileron;
        private double elevator;
        private double rudder;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// this is the constructor of this class.
        /// </summary>
        /// <param name="telnetClient"></param>
        public MyJoystickModel(ITelnetClient telnetClient)
        {
            this.telnetClient = telnetClient;
        }

        public double Throttle
        {
            get { return throttle; }
            set
            {
                throttle = value;
                NotifyPropertyChanged("Throttle");
            }
        }

        public double Aileron
        {
            get { return aileron; }
            set
            {
                aileron = value;
                NotifyPropertyChanged("Aileron");
            }
        }

        public double Elevator
        {
            get { return elevator; }
            set
            {
                elevator = value;
                NotifyPropertyChanged("Elevator");
            }
        }

        public double Rudder
        {
            get { return rudder; }
            set
            {
                rudder = value;
                NotifyPropertyChanged("Rudder");
            }
        }

        /// <summary>
        /// this function connect to the simulator in by the ip and port that she gets.
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void connect(string ip, int port)
        {
            telnetClient.connect(ip, port);
        }

        /// <summary>
        /// this function disconnect from the simulator.
        /// </summary>
        public void disconnect()
        {
            telnetClient.disconnect();
        }

        /// <summary>
        /// this function set the telnet client.
        /// </summary>
        /// <param name="telnetClient"></param>
        public void setTelnetClient(ITelnetClient telnetClient)
        {
            this.telnetClient = telnetClient;
        }

        /// <summary>
        /// this function notify that something has change.
        /// </summary>
        /// <param name="propName"></param>        public void NotifyPropertyChanged(string propName)
        {
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }        /// <summary>
        /// this function send order to the simulator to move the throttle.
        /// </summary>
        /// <param name="throttleVal"></param>        public void moveThrottle(double throttleVal)
        {
            this.telnetClient.write("set controls/engines/current-engine/throttle " + Convert.ToString(throttleVal));
        }
        /// <summary>
        /// this function send order to the simulator to move the aileron.
        /// </summary>
        /// <param name="aileroVal"></param>
        public void moveAileron(double aileronVal)
        {
            this.telnetClient.write("set controls/flight/aileron " + Convert.ToString(aileronVal));
        }

        /// <summary>
        /// this function send order to the simulator to move the elevator.
        /// </summary>
        /// <param name="elevatorVal"></param>
        public void moveElevator(double elevatorVal)
        {
            this.telnetClient.write("set controls/flight/elevator " + Convert.ToString(elevatorVal));
        }

        /// <summary>
        /// this function send order to the simulator to move the rudder.
        /// </summary>
        /// <param name="rudderVal"></param>
        public void moveRudder(double rudderVal)
        {
            this.telnetClient.write("set controls/flight/rudder " + Convert.ToString(rudderVal));
        }

        /// <summary>
        /// this function send the commands to the simulator in other thread.
        /// </summary>
        /// <param name="textCommandsStr"></param>
        public void executeCommandsInThread(string textCommandsStr, JoystickViewModel joystickViewModel)
        {
            // set the background of the TextBox to be PaleVioletRed
            joystickViewModel.NotSentYet = true;
            Thread thread = new Thread(new ThreadStart(() =>
            {
                // get the commands in array
                string[] commands = textCommandsStr.Split('\n');
                int index = 0;
                int len = commands.Length;
                foreach (var command in commands)
                {
                    this.telnetClient.write(command);
                    // if this is not the last command to be send to the simulator
                    if (index < len - 1)
                    {
                        // wait 2 seconds before sending the next command to the simulator
                        Thread.Sleep(2000);
                    }
                    index++;
                }
                // set the background of the TextBox back to white
                joystickViewModel.NotSentYet = false;
            }));
            thread.Start();
        }
    }
}