using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FlightSimulator.Model;
using FlightgearSimulator.Commands;

namespace FlightSimulator.ViewModels
{
    class JoystickViewModel : INotifyPropertyChanged
    {
        // the members
        IJoystickModel joystickModel;
        private double throttle;
        private double aileron;
        private double elevator;
        private double rudder;
        private string textCommands;
        private ICommand executeCommands;
        private volatile bool notSentYet;
        private ICommand clearText;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// this is the constructor of this class.
        /// </summary>
        /// <param name="joystickModel"></param>
        public JoystickViewModel(IJoystickModel joystickModel)
        {
            this.joystickModel = joystickModel;
            notSentYet = false;
            joystickModel.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e) {
                                                       NotifyPropertyChanged(e.PropertyName); };
        }

        public double Throttle
        {
            get { return throttle; }
            set
            {
                throttle = value;
                joystickModel.moveThrottle(throttle);
            }
        }

        public double Aileron
        {
            get { return aileron; }
            set
            {
                aileron = value;
                joystickModel.moveAileron(aileron);
                NotifyPropertyChanged("Aileron");
            }
        }

        public double Elevator
        {
            get { return elevator; }
            set
            {
                elevator = value;
                joystickModel.moveElevator(elevator);
                NotifyPropertyChanged("Elevator");
            }
        }

        public double Rudder
        {
            get { return rudder; }
            set
            {
                rudder = value;
                joystickModel.moveRudder(rudder);
            }
        }

        public string TextCommands
        {
            get { return textCommands; }
            set
            {
                textCommands = value;NotifyPropertyChanged("TextCommands");
            }
        }

        /// <summary>
        /// this function execute the commands in the textBox.
        /// </summary>
        public ICommand ExecuteCommands
        {
            get
            {
                return executeCommands ?? (executeCommands = new ButtonClickCommand(() => OnClick()));
            }
        }

        /// <summary>
        /// this function execute what needed in case of pressing the ok button.
        /// </summary>
        private void OnClick()
        {
            joystickModel.executeCommandsInThread(textCommands, this);
        }

        public bool NotSentYet
        {
            get { return notSentYet; }
            set
            {
                notSentYet = value;
                NotifyPropertyChanged("NotSentYet");
            }
        }

        public ICommand ClearText
        {
            get
            {
                return clearText ?? (clearText = new ButtonClickCommand(() => OnClear()));
            }
        }

        /// <summary>
        /// this function just clear the TextBox from the commands wrritten on him.
        /// </summary>
        private void OnClear()
        {
            TextCommands = "";
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
