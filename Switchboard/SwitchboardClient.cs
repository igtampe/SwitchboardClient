using BasicRender;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Switchboard {
    public class SwitchboardClient {

        //In the event of using this outside the console, probably remove the Basicrender calls.

        private TcpClient Client;
        private NetworkStream River;

        public bool Connected => Client.Connected;
        public bool Available => River.DataAvailable;

        private static int ConnectionStatus=0;

        private readonly String IP;
        private readonly int Port;

        /// <summary>Generates a Switchboard Client, but Does not start it</summary>
        public SwitchboardClient(String IP, int Port) {
            Render.Echo("Attempting to connect to " + IP + ":" + Port + " ");
            this.IP = IP;
            this.Port = Port;
            Client = new TcpClient();
        }

        /// <summary>Initiate the connection</summary>
        /// <returns>True if it managed to connect, false otherwise</returns>
        public Boolean Connect() {
            Client.ConnectAsync(IP,Port);

            //15 second time out
            for(int i = 0; i < 30; i++) {
                ConnectAnim();
                if(Client.Connected) { break; }
                Thread.Sleep(500);
            }

            if(!Client.Connected) {
                Render.Sprite("\nERROR: Could not connect to server. Maybe it's busy?",ConsoleColor.Black,ConsoleColor.Red);
                return false ;
            }

            Render.Sprite("\nConnected to the server!\n\n",ConsoleColor.Black,ConsoleColor.Green);
            River = Client.GetStream();

            return true;
        }

        public void Close() {
            if(!Connected) { return; } //Make sure attempting to close an already closed connection doesn't cause an exception. That's kinda bobo.
            //Send CLOSE to the server, closing that side.
            Send("CLOSE");

            //I mean that should close the TCPClient and the stream, no?
            River.Close();
            Client.Close();

        }

        /// <summary>Send data to the switchboard server, and retrieve a response</summary>
        public String SendReceive(String Data) {
            Send(Data);
            return Receive();
        }

        /// <summary>Only send data, do not receive. Maybe could be useful in the future.</summary>
        public void Send(String data) {
            if(!Connected) { throw new InvalidOperationException("This client is not connected right now!"); }
            Byte[] Bytes = Encoding.ASCII.GetBytes(data);
            River.Write(Bytes,0,Bytes.Length);
        }

        /// <summary>Only receive data</summary>
        public String Receive() {
            if(!Connected) { throw new InvalidOperationException("This client is not connected right now!"); }
            while(!Available) {
                Thread.Sleep(500);
                //on a GUI application, here would be a good place to check if your user needs to  cancel the operation.
            }

            List<Byte> Bytes = new List<Byte>();
            while(Available) { Bytes.Add((byte)(River.ReadByte())); }
            return Encoding.ASCII.GetString(Bytes.ToArray());


        }

        /// <summary>Spinner animation</summary>
        public static void ConnectAnim() {
            Render.SetPos(Console.CursorLeft-1,Console.CursorTop);
            String Output = "";
            switch(ConnectionStatus) {
                case 0:
                    Output = "/";
                    break;
                case 1:
                    Output = "-";
                    break;
                case 2:
                    Output = "\\";
                    break;
                case 3:
                    Output = "|";
                    ConnectionStatus = -1; //negative one so it is incremented back to 0
                    break;
                default:
                    break;
            }

            ConnectionStatus++;
            Render.Echo(Output);
        }

    }
}
