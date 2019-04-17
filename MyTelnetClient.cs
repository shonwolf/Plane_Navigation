using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;

namespace FlightgearSimulator
{
    class MyTelnetClient : ITelnetClient
    {
        // the members
        TcpClient tcpClient;
        Stream stream;

        /// <summary>
        /// this function connect to the simulator as client.
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void connect(string ip, int port)
        {
            try
            {
                this.tcpClient = new TcpClient();
                tcpClient.Connect(ip, port);
                this.stream = this.tcpClient.GetStream();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error In Connecting...." + e.StackTrace);
            }
        }

        /// <summary>
        /// this function wirte commands to the simulator.
        /// </summary>
        /// <param name="command"></param>
        public void write(string command)
        {
            try
            {
                // adding those to chars becouse that the format the simulator need to understand the message
                command = command + "\r\n";
                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(command);
                // wirte message to simulator
                this.stream.Write(ba, 0, ba.Length);
                byte[] bb = new byte[1024];
                // just read the message the simulator send back
                int k = this.stream.Read(bb, 0, 1024);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error In Sending Message...." + e.StackTrace);
            }
        }

        /// <summary>
        /// this function disconnect from the simulator.
        /// </summary>
        public void disconnect()
        {
            try
            {
                this.tcpClient.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error In Disonnecting.... " + e.StackTrace);
            }
        }
    }
}
