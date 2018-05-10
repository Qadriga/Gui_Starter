using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO.Ports;
using System.Threading;

namespace GUI_Starter.classes
{
    public class HotPlugEventArgs : EventArgs
    {
        public String Message{ get; set; }

    }
    public sealed class Com_Ports 
    {
        private static Com_Ports instance = null;
        private static readonly object threadLock = new object();
        private SerialPort connection = null;
        public string portname { get; set; } = null;
        private Byte[] LastreadData { get; set; }

        private Timer HotPlugTimer = null;
        private int FoundPorts = 0;

        public event EventHandler DataReady;
        public event EventHandler HotPlugEvent;
        
        public string[] Port_names { get{
            return SerialPort.GetPortNames();
            } } // read only array of all port names
        Com_Ports()
        {
            
            Console.WriteLine("Found some Ports:");
            foreach(string port in this.Port_names)
            {
                Console.WriteLine(port);
            }
            this.portname = this.Port_names[0];
            AutoResetEvent ev = new AutoResetEvent(false);
            this.HotPlugTimer = new Timer(this.HotPlugCheck,ev,0,5000);
        }

        private void HotPlugCheck(Object sender)
        {
            Console.WriteLine("Check for new Ports");
            String[] cp = SerialPort.GetPortNames();
            if(cp.Length > this.FoundPorts)
            {
                HotPlugEventArgs ev = new HotPlugEventArgs();
                ev.Message = "Port Added";

                HotPlugEvent?.Invoke(this, ev);
            }
            else if (cp.Length < this.FoundPorts)
            {
                HotPlugEventArgs ev = new HotPlugEventArgs();
                ev.Message = "Port Removed";
                HotPlugEvent?.Invoke(this, ev);
            }
            this.FoundPorts = cp.Length;
        }
        
        public bool openConnection(String portname)
        {
            return _openConnection(portname);
        }
        private bool _openConnection(String portname)
        {
            SerialPort tmp = null;
            
            if(Array.IndexOf(this.Port_names,portname) < 0 ){
                return false;
            }
            else
            {
                this.portname = portname;
            }
            try
            {
                tmp = new SerialPort(this.portname);
            }
            catch (System.IO.IOException)
            {
                Console.WriteLine("Serial Port could not opend");
                return false;
            }
            if (connection != null)
            {
                connection.Close();
            }
            connection = tmp;
            this.connection.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(_serial_PortDataReveived);
            return true;
        }

        public bool sendString(String Data)
        {
            if (this.portname == null && this.Port_names.Length <= 0)
            {
                return false;
            }            
            if(connection == null)
            {
                openConnection(this.portname);
            }
            if (!connection.IsOpen)
            {
                try
                {
                    connection.Open();
                }                
                catch (Exception e)
                {
                    Console.WriteLine(e.GetType().ToString() + " " + e.Message);
                    return false;
                }

            }
            byte[] sendData = Encoding.GetEncoding("UTF-8").GetBytes(Data.ToCharArray());
            List<byte> tmp = sendData.ToList();
            tmp.Add((byte) 13);// CR
            tmp.Add((byte) 10);// LF
            sendData = tmp.ToArray();
            try
            {
                connection.Write(sendData, 0, sendData.Length);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.GetType().ToString() + " " + e.Message);
                return false;
            }

            return true;
        }
        public static Com_Ports Instance
        {
            get
            {
                lock (threadLock)
                {
                    if ( instance == null)
                    {
                        instance = new Com_Ports();                        
                    }
                    return instance;
                }
            }
        }

        void _serial_PortDataReveived(Object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort _Dataport = (SerialPort)sender;
            Byte[] ReadData = new Byte[_Dataport.BytesToRead];
            for(int i = 0; i < _Dataport.BytesToRead; i++)
            {
                ReadData[i] = (byte) _Dataport.ReadByte();
            }
            this.LastreadData = ReadData;
            DataReady?.Invoke(this, e);
        }
    }
}
