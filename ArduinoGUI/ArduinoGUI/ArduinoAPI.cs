using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;

namespace ArduinoGUI
{
    public class ArduinoAPI
    {
        SerialPort serialPort;
        int numPort;

        public ArduinoAPI()
        {
            serialPort = new SerialPort();
        }

        ~ArduinoAPI()
        {
            closePort();
        }

        public void setPort(int numPort)
        {
            closePort();
            this.numPort = numPort;
            serialPort.PortName = "COM" + numPort;
            serialPort.BaudRate = 9600;
        }

        public bool openPort()
        {
            closePort();
            try
            {
                serialPort.Open();
            }
            catch(System.IO.IOException e)
            {
                //En caso el puerto no exista
                Console.WriteLine(e.Message);
            }

            return serialPort.IsOpen;
        }

        public void closePort()
        {
            if (serialPort.IsOpen)
                serialPort.Close();
        }

        public bool isOpen()
        {
            return serialPort.IsOpen;
        }

        public void write(string text)
        {
            if(serialPort.IsOpen)
                serialPort.Write(text);
        }

        public void writeLine(string text)
        {
            if (serialPort.IsOpen)
                serialPort.WriteLine(text);
        }

        public int bytesToRead()
        {
            return serialPort.BytesToRead;
        }

        public int readByte()
        {
            return serialPort.ReadByte();
        }

        public int readChar()
        {
            return serialPort.ReadChar();
        }

        public string readLine()
        {
            return serialPort.ReadLine();
        }

    }
}
