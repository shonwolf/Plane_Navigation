using FlightgearSimulator.Commands;
using FlightgearSimulator.Properties;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace FlightSimulator
{
    class SettingsViewModel : INotifyPropertyChanged
    {
        // the members
        SettingsModel model;
        // current ip
        string ipCurrent;
        // currect port of server
        string portServerCurrent;
        // currect port of client
        string portClientCurrent;
        // current window
        Window curWindow;
        private ICommand oKSettingsButton;
        private ICommand closeSettingsButton;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// this is the constructor of this class.
        /// </summary>
        /// <param name="window"></param>
        public SettingsViewModel(Window window)
        {
            model = new SettingsModel();
            curWindow = window;
        }

        public string IPText
        {
            set
            {
                ipCurrent = value;
                model.IPText = value;
            }
        }

        public string PortClient
        {
            set
            {
                portClientCurrent = value;
                model.PortClient = value;
            }
        }

        public string PortServer
        {
            set
            {
                portServerCurrent = value;
                model.PortServer = value;
            }
        }

        /// <summary>
        /// this fuction notifies when property changes.
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
        /// this function is activated when the ok button in the settings is pressend.
        /// </summary>
        public ICommand OKSettingsButton
        {
            get
            {
                return oKSettingsButton ?? (oKSettingsButton = new ButtonClickCommand(() => OKButtonPressed()));
            }
        }

        /// <summary>
        /// this function initialize the data when the ok button is pressend.
        /// </summary>
        private void OKButtonPressed()
        {
            // save deta and close the window
            Settings.Default["IP"] = ipCurrent;
            Settings.Default["PortServer"] = portServerCurrent;
            Settings.Default["PortClient"] = portClientCurrent;
            curWindow.Close();
        }

        /// <summary>
        /// this function is activated when the ok button in the settings is pressend - close the settings window.
        /// </summary>
        public ICommand CloseSettingsButton
        {
            get
            {
                return oKSettingsButton ?? (closeSettingsButton = new ButtonClickCommand(() => CloseWindow()));
            }
        }

        /// <summary>
        /// this function close the settings window.
        /// </summary>
        private void CloseWindow()
        {
            // close the window without savind of data
            curWindow.Close();
        }
    }
}
