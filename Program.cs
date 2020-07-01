using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using BasicRender;

namespace SwitchboardClient {
    class Program {

        public static String Prefix = "DISCONNECTED";
        public static Switchboard.SwitchboardClient MainClient;

        static void Main(string[] args) {

            Console.Title = "Switchboard Console: Disconnected";

            Console.Clear();

            SwitchboardLogo Logo = new SwitchboardLogo();

            Logo.draw(2,1);
            Render.Sprite("Switchboard Console [Version 1.0]",Console.BackgroundColor,ConsoleColor.White,3 + Logo.GetWidth(),2);
            Render.Sprite("(C)2020 Chopo, No Rights Reserved",Console.BackgroundColor,ConsoleColor.White,3 + Logo.GetWidth(),3);

            Render.SetPos(0,2 + Logo.GetHeight());

            Render.Echo("Welcome to the Switchboard Console! Type CONNECT [IP]:[PORT] to connect to a server! \n\n");

            while(true) {

                String Prompt = PromptInput();
                String[] PromptSplit = Prompt.Split(' ');


                switch(PromptSplit[0].ToUpper()) {
                    case "CONNECT":
                        if(MainClient != null) { Render.Echo("There's already an ongoing connection! Close this one to open another one."); }
                        else if(PromptSplit.Length!=2) { Render.Echo("Impropper connection request. Try something like 127.0.0.1:909"); } 
                        else {
                            String[] IPPortSplit = PromptSplit[1].Split(':');
                            MainClient = new Switchboard.SwitchboardClient(IPPortSplit[0],int.Parse(IPPortSplit[1]));
                            if(MainClient.Connect()) { Prefix = IPPortSplit[0];   Console.Title = "Switchboard Console: " + IPPortSplit[0];} else { MainClient = null; }
                        }
                        break;
                    case "CLOSE":
                        if(MainClient == null) { Render.Echo("No connection to close"); } 
                        else { MainClient.Close(); Prefix = "DISCONNECTED"; Console.Title = "Switchboard Console: Disconnected"; MainClient = null; }
                        break;
                    default:
                        if(MainClient == null || !MainClient.Connected) { Render.Echo("Client is not connected! Connect using CONNECT [IP]:[PORT]"); } 
                        else { Render.Echo(MainClient.SendReceive(Prompt)); } 
                        break;
                }

            }


        }

        public static string PromptInput() {
            Render.Echo("\n\n"+Prefix+"> ");
            return Console.ReadLine();

        }




    }
}
