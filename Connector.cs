using FlightSimulator;
using FlightSimulator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightgearSimulator
{
    class Connector
    {
        // the members
        // instance of connector
        private static Connector instance = null;
        // model of joystick
        private IJoystickModel joystickModel;
        // client
        private ITelnetClient client;
        // server
        private TelnetServer server;

        /// <summary>
        /// this function set the joystickModel.
        /// </summary>
        /// <param name="_joystickModel"></param>
        public void setJoystickModel(IJoystickModel _joystickModel)
        {
            this.joystickModel = _joystickModel;
        }

        /// <summary>
        /// this function return the joystickModel.
        /// </summary>
        /// <returns>joystickModel</returns>
        public IJoystickModel getJoystickModel()
        {
            return this.joystickModel;
        }

        /// <summary>
        /// this function set the telnetClient.
        /// </summary>
        public void setClient(ITelnetClient _client)
        {
            client = _client;
        }

        /// <summary>
        /// this function return the telnetClient
        /// </summary>
        /// <returns>telnetClient</returns>
        public ITelnetClient getClient()
        {
            return this.client;
        }

        /// <summary>
        /// this fucntion set the telnetServer.
        /// </summary>
        /// <param name="_server"></param>
        public void setServer(TelnetServer _server)
        {
            server = _server;
        }

        /// <summary>
        /// this function return the telnetServer.
        /// </summary>
        /// <returns>telnetServer</returns>
        public TelnetServer getServer()
        {
            return this.server;
        }

        /// <summary>
        /// this function return the instance of this class, if there is already one then return it, othrerwise create
        /// one and return it.
        /// </summary>
        /// <returns></returns>
        public static Connector getInstance()
        {
            if (instance == null)
            {
                instance = new Connector();
            }
            return instance;
        }
    }
}
