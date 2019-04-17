using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using FlightSimulator.ViewModels;
using FlightgearSimulator;

namespace FlightSimulator.Model
{
    interface IJoystickModel : INotifyPropertyChanged
    {
        double Throttle { get; set; }
        double Aileron { get; set; }
        double Elevator { get; set; }
        double Rudder { get; set; }
        // connect to the simulator
        void connect(string ip, int port);
        // disconnect from the simulator
        void disconnect();
        void moveThrottle(double throttleVal);
        void moveAileron(double aileronVal);
        void moveElevator(double elevatorVal);
        void moveRudder(double rudderVal);
        // exectute the commands from the auto pilot in thread
        void executeCommandsInThread(string textCommandsStr, JoystickViewModel joystickViewModel);
        void setTelnetClient(ITelnetClient telnetClient);
    }
}
