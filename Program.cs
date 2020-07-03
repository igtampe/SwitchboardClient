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
            DrawHeader();


            //The Main Loop
            while(true) {

                //Get input
                String Prompt = PromptInput();
                String[] PromptSplit = Prompt.Split(' ');

                //Try to locally parse the message
                switch(PromptSplit[0].ToUpper()) {
                    case "CONNECT":
                        //attempt to connect to a Server
                        if(MainClient != null) { Render.Echo("There's already an ongoing connection! Close this one to open another one."); break; } //Make sure we're not already connected
                        if(PromptSplit.Length == 2) {
                            String[] IPPortSplit = PromptSplit[1].Split(':'); //Split the IP and port
                            String IP = IPPortSplit[0];
                            String Port;
                            if(IPPortSplit.Length == 1) { Port = "909"; } else { Port = IPPortSplit[1]; }

                            MainClient = new Switchboard.SwitchboardClient(IP,int.Parse(Port)); //Create client
                            if(MainClient.Connect()) { UpdatePrefix(IP); }  //Initialize it, and if we manage to connect, setup the prefix and title.
                            else { MainClient = null; } //If not reset mainclient to null.

                        } else {Render.Echo("Impropper connection request. Try something like 127.0.0.1:909");}
                        break;
                    case "CLOSE":
                        //Close the connection.
                        if(MainClient == null) { Render.Echo("No connection to close"); }  //Make sure we cannot close if there is no connection.
                        else { MainClient.Close(); Prefix = "DISCONNECTED"; Console.Title = "Switchboard Console: Disconnected"; MainClient = null; } //Close the connection.
                        break;
                    case "READ":
                        //Attempt to read any data that may be there to read.
                        if(MainClient == null) { Render.Echo("No connection to read from!"); break; }
                        if(!MainClient.Available) {
                            Render.Echo("No data is available! Wait for data? ");
                            if(!YesNo()) { break; } //Display a warning if there is no data to read, and if the user wants to read the data.
                        }
                        try { Render.Echo(MainClient.Receive()); } catch(Exception) { Render.Sprite("There was an error sending/receiving this command. Perhaps the server was disconnected?",ConsoleColor.Black,ConsoleColor.Red); }
                        //Read the data and display it.
                        break;
                    case "CLS":
                        //Clear the screen.
                        DrawHeader();
                        break;
                    case "LOGIN":
                        if(PromptSplit.Length != 3) { Render.Echo("Invalid Login Credentials"); break; }
                        //now let's log in.
                        switch(MainClient.Login(PromptSplit[1],PromptSplit[2])) {
                            case Switchboard.SwitchboardClient.LoginResult.ALREADY:
                                Render.Echo("Already logged in");
                                break;
                            case Switchboard.SwitchboardClient.LoginResult.INVALID:
                                Render.Echo("Invalid Login Credentials");
                                break;
                            case Switchboard.SwitchboardClient.LoginResult.OTHERLOCALE:
                                Render.Echo("Already logged in on another connection");
                                break;
                            case Switchboard.SwitchboardClient.LoginResult.SUCCESS:
                                Render.Echo("Successfully logged in as " + PromptSplit[1]);
                                UpdatePrefix(PromptSplit[1] + "@" + MainClient.GetIP());
                                break;
                            default:
                                break;
                        }
                        break;
                    case "LOGOUT":
                        //Try to log out.
                        if(MainClient.Logout()) {
                            Render.Echo("Logged out successfully!");
                            UpdatePrefix(MainClient.GetIP());
                        } else { Render.Echo("Unable to log out. You're already logged out!"); }

                        break;
                    default:
                        //Try to send the commend to the server.
                        if(MainClient == null || !MainClient.Connected) { Render.Echo("Client is not connected! Connect using CONNECT [IP]:[PORT]"); }  //warn the user if there's no connection.
                        else {
                            //The console doesn't need to check if the client is busy since there are no other threads, but if you're doing this with a background worker and are trying to send
                            //anything maybe check that.
                            try {Render.Echo(MainClient.SendReceive(Prompt));} 
                            catch(Exception) {Render.Sprite("There was an error sending/receiving this command. Perhaps the server was disconnected?",ConsoleColor.Black,ConsoleColor.Red);}
                        }
                        break;
                }

            }


        }

        /// <summary>Updates the connection prefix.</summary>
        public static void UpdatePrefix(String NewPrefix) {
            Prefix = NewPrefix;
            Console.Title = "Switchboard Console: " + Prefix;
        }

        /// <summary>Clears the screen and draws the header</summary>
        public static void DrawHeader() {
            Console.Clear();

            //Draw the header
            SwitchboardLogo Logo = new SwitchboardLogo();
            Logo.draw(2,1);
            Render.Sprite("Switchboard Console [Version 1.0]",Console.BackgroundColor,ConsoleColor.White,3 + Logo.GetWidth(),2);
            Render.Sprite("(C)2020 Chopo, No Rights Reserved",Console.BackgroundColor,ConsoleColor.White,3 + Logo.GetWidth(),3);

            //Set the position, and draw a neat little message
            Render.SetPos(0,2 + Logo.GetHeight());
            Render.Echo("Welcome to the Switchboard Console! Type CONNECT [IP]:[PORT] to connect to a server! \n\n");
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
