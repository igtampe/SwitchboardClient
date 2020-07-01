using System;
using BasicRender;

namespace SwitchboardClient {
    class Program {

        /// <summary>Prefix for the "Command Prompt"</summary>
        public static String Prefix = "DISCONNECTED";

        /// <summary>The SwitchboardClient object</summary>
        public static Switchboard.SwitchboardClient MainClient;

        static void Main(string[] args) {

            //Set title and clear the screan
            Console.Title = "Switchboard Console: Disconnected";
            Console.Clear();

            //Draw the header
            SwitchboardLogo Logo = new SwitchboardLogo();
            Logo.draw(2,1);
            Render.Sprite("Switchboard Console [Version 1.0]",Console.BackgroundColor,ConsoleColor.White,3 + Logo.GetWidth(),2);
            Render.Sprite("(C)2020 Chopo, No Rights Reserved",Console.BackgroundColor,ConsoleColor.White,3 + Logo.GetWidth(),3);

            //Set the position, and draw a neat little message
            Render.SetPos(0,2 + Logo.GetHeight());
            Render.Echo("Welcome to the Switchboard Console! Type CONNECT [IP]:[PORT] to connect to a server! \n\n");

            //The Main Loop
            while(true) {

                //Get input
                String Prompt = PromptInput();
                String[] PromptSplit = Prompt.Split(' ');

                //Try to locally parse the message
                switch(PromptSplit[0].ToUpper()) {
                    case "CONNECT":
                        if(MainClient != null) { Render.Echo("There's already an ongoing connection! Close this one to open another one."); } //Make sure we're not already connected
                        else if(PromptSplit.Length!=2) { Render.Echo("Impropper connection request. Try something like 127.0.0.1:909"); } //Make sure the connection prompt is the right length
                        else {
                            String[] IPPortSplit = PromptSplit[1].Split(':'); //Split the IP and port
                            if(IPPortSplit.Length != 2) { Render.Echo("IP/Port combination not valid"); break; } //Make sure the IP/Port combination is the right length.
                            
                            MainClient = new Switchboard.SwitchboardClient(IPPortSplit[0],int.Parse(IPPortSplit[1])); //Create client
                            if(MainClient.Connect()) { Prefix = IPPortSplit[0];   Console.Title = "Switchboard Console: " + IPPortSplit[0];}  //Initialize it, and if we manage to connect, setup the prefix and title.
                            else { MainClient = null; } //If not reset mainclient to null.
                        }
                        break;
                    case "CLOSE":
                        if(MainClient == null) { Render.Echo("No connection to close"); }  //Make sure we cannot close if there is no connection.
                        else { MainClient.Close(); Prefix = "DISCONNECTED"; Console.Title = "Switchboard Console: Disconnected"; MainClient = null; } //Close the connection.
                        break;
                    case "READ":
                        if(MainClient == null) { Render.Echo("No connection to read from!"); break; } 
                        if(!MainClient.Available) {
                            Render.Echo("No data is available! Wait for data? ");
                            if(!YesNo()) { break; } //Display a warning if there is no data to read, and if the user wants to read the data.
                        }
                        try { Render.Echo(MainClient.Receive()); } 
                        catch(Exception) { Render.Sprite("There was an error sending/receiving this command. Perhaps the server was disconnected?",ConsoleColor.Black,ConsoleColor.Red); }
                         //Read the data and display it.
                        break;
                    default:

                        //Try to send the commend to the server.
                        if(MainClient == null || !MainClient.Connected) { Render.Echo("Client is not connected! Connect using CONNECT [IP]:[PORT]"); }  //warn the user if there's no connection.
                        else {
                            try {Render.Echo(MainClient.SendReceive(Prompt));} 
                            catch(Exception) {Render.Sprite("There was an error sending/receiving this command. Perhaps the server was disconnected?",ConsoleColor.Black,ConsoleColor.Red);}
                            
                        
                        }
                        
                        break;
                }

            }


        }

        /// <summary>Prompts the user for input</summary>
        public static string PromptInput() {
            Render.Echo("\n\n"+Prefix+"> ");
            return Console.ReadLine();
        }

        /// <summary>Prompts the user with Y/N.</summary>
        /// <returns>True if the user hits Y, false otherwise.</returns>
        public static bool YesNo() {
            Render.Echo("(Y/N) ");
            if(Console.ReadKey().Key==ConsoleKey.Y) { return true; } else { return false; }
        }

    }
}
