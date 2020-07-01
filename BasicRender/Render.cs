using System;
using System.Threading;

namespace BasicRender{
    /// <summary></summary>
    public class Render {

        //===============================================================
        //                  BASIC RENDER C#, VERSION 1.0
        //===============================================================
        //(C)2020 Igtampe No rights Reserved.
        //
        //This class is designed to help and quickly render items
        //onscreen in a commandline display. Simply set a few parameters
        //and you should be good to go!

        //It also provides some basic utilities to make coding on C#
        //just a tiny bit more like coding on batch.

        //**************************************************************
        //WINDOWS AND DIALOGBOX UTILITIES HAVE BEEN MOVED TO RESPECTIVE 
        //OBJECTS/CLASSES
        //**************************************************************

        //Configuration of BasicRender:

        public const ConsoleColor WindowBG = ConsoleColor.Black;
        public const ConsoleColor WindowFG = ConsoleColor.White;

        /// <summary>Renders a sprite at the current cursor position</summary>
        /// <param name="sprite"></param>
        /// <param name="BG"></param>
        /// <param name="FG"></param>
        public static void Sprite(string sprite, ConsoleColor BG, ConsoleColor FG) {Sprite(sprite, BG, FG, -1, -1);}

        /// <summary>renders a sprite at the specified cursor position</summary>
        /// <param name="sprite"></param>
        /// <param name="BG"></param>
        /// <param name="FG"></param>
        /// <param name="LeftPos"></param>
        /// <param name="TopPos"></param>
        public static void Sprite(string sprite, ConsoleColor BG, ConsoleColor FG, int LeftPos, int TopPos) {

            if (LeftPos != -1 && TopPos != -1) { SetPos(LeftPos, TopPos); }

            ConsoleColor OldBG = Console.BackgroundColor;
            ConsoleColor OldFG = Console.ForegroundColor;

            Color(BG, FG);
            Console.Write(sprite);
            Color(OldBG, OldFG);

        }

        /// <summary>Draws a block of a certain color at the current cursor position</summary>
        /// <param name="Color"></param>
        public static void Block(ConsoleColor Color) { Block(Color, -1, -1); }

        /// <summary>Draws a block of a certain color at the specified position</summary>
        /// <param name="Color"></param>
        public static void Block(ConsoleColor Color, int LeftPos, int TopPos) { Sprite(" ", Color, Color, LeftPos, TopPos); }

        /// <summary>Draws a box of the specified color of the specified height and width at the specified position</summary>
        /// <param name="Color"></param>
        /// <param name="Length"></param>
        /// <param name="Height"></param>
        /// <param name="LeftPos"></param>
        /// <param name="TopPos"></param>
        public static void Box(ConsoleColor Color, int Length, int Height, int LeftPos, int TopPos) { for (int i = 0; i < Height; i++) { Row(Color, Length, LeftPos, TopPos + i); } }

        /// <summary>Draws a row of blocks of the specified color of specified length at the current cursor position</summary>
        /// <param name="RowColor"></param>
        /// <param name="Length"></param>
        public static void Row(ConsoleColor RowColor, int Length) { Row(RowColor, Length, -1, -1); }

        /// <summary>Draws a row of blocks of the specified color of specified length at the specified position</summary>
        /// <param name="RowColor"></param>
        /// <param name="Length"></param>
        /// <param name="LeftPos"></param>
        /// <param name="TopPos"></param>
        public static void Row(ConsoleColor RowColor, int Length, int LeftPos, int TopPos){

            if (LeftPos != -1 && TopPos != -1) { SetPos(LeftPos, TopPos); }

            ConsoleColor OldBG = Console.BackgroundColor;
            ConsoleColor OldFG = Console.ForegroundColor;

            Color(RowColor,RowColor);

            for (int i = 0; i < Length; i++){Console.Write(" ");}

            Color(OldBG, OldFG);
        }

        /// <summary>Clears the specified line using the configured WindowClearColor</summary>
        /// <param name="TopPos"></param>
        public static void ClearLine(int TopPos) { Row(Console.BackgroundColor, Console.WindowWidth - 1, 0, TopPos); }

        /// <summary>Draws text centered on the screen at the current row, and with curent colors</summary>
        /// <param name="Text"></param>
        public static void CenterText(string Text) { CenterText(Text, Console.CursorTop, Console.BackgroundColor, Console.ForegroundColor); }

        /// <summary>Draws text centered on the screen at the specified row, with the current colors</summary>
        /// <param name="Text"></param>
        /// <param name="TopPos"></param>
        public static void CenterText(string Text, int TopPos) { CenterText(Text, TopPos, Console.BackgroundColor, Console.ForegroundColor); }

        /// <summary>Draws text centered on the screen at the specified row, with the specified colors</summary>
        /// <param name="Text"></param>
        /// <param name="TopPos"></param>
        /// <param name="BG"></param>
        /// <param name="FG"></param>
        public static void CenterText(string Text, int TopPos, ConsoleColor BG, ConsoleColor FG) {

            //Find the position of this text where its centered. -1 so that it preffers left center rather than right center.
            int leftpos = (Console.WindowWidth - Text.Length-1) / 2;

            ConsoleColor OldBG = Console.BackgroundColor;
            ConsoleColor OldFG = Console.ForegroundColor;

            Color(BG, FG);

            SetPos(leftpos, TopPos);
            Console.Write(Text);

            Color(OldBG, OldFG);

        }

        /// <summary>Halt execution of the program for the specified number of miliseconds</summary>
        /// <param name="Time"></param>
        public static void Sleep(int Time) { Thread.Sleep(Time); }

        /// <summary>Halt execution of the program until the user presses a key to continue</summary>
        public static void Pause() { Console.ReadKey(true); }

        /// <summary>Draws a ColorString comprised of ColorChars.<br><</br>
        /// The colorstring '0123456789ABCDEF' will render a rainbow.
        /// </summary>
        /// <param name="ColorString"></param>
        public static void Draw(string ColorString) {foreach (char ColorChar in ColorString){Block(ColorCharToConsoleColor(ColorChar));} }

        /// <summary>Takes a ColorCharacter and turns it into a consolecolor. The dictionary is as follows
        /// <br></br>
        ///0 = Black       |8 = Gray<br></br>
        ///1 = Blue        |9 = Light Blue<br></br>
        ///2 = Green       |A = Light Green<br></br>
        ///3 = Aqua        |B = Light Aqua<br></br>
        ///4 = Red         |C = Light Red<br></br>
        ///5 = Purple      |D = Light Purple<br></br>
        ///6 = Yellow      |E = Light Yellow<br></br>
        ///7 = White       |F = Bright White<br></br>
        /// <br></br>
        /// </summary>
        /// <param name="ColorChar"></param>
        /// <returns>The corresponding consolecolor</returns>
        private static ConsoleColor ColorCharToConsoleColor(char ColorChar) {

            switch (ColorChar)
            {
                case '0':
                    return ConsoleColor.Black;
                case '1':
                    return ConsoleColor.DarkBlue;
                case '2':
                    return ConsoleColor.DarkGreen;
                case '3':
                    return ConsoleColor.DarkCyan;
                case '4':
                    return ConsoleColor.DarkRed;
                case '5':
                    return ConsoleColor.DarkMagenta;
                case '6':
                    return ConsoleColor.DarkYellow;
                case '7':
                    return ConsoleColor.Gray;
                case '8':
                    return ConsoleColor.DarkGray;
                case '9':
                    return ConsoleColor.Blue;
                case 'A':
                    return ConsoleColor.Green;
                case 'B':
                    return ConsoleColor.Cyan;
                case 'C':
                    return ConsoleColor.Red;
                case 'D':
                    return ConsoleColor.Magenta;
                case 'E':
                    return ConsoleColor.Yellow;
                case 'F':
                    return ConsoleColor.White;
                default:
                    return Console.BackgroundColor;
            }

        }

    /// <summary>
    /// Draws a HiColorString, an example of which is '0F0-0F1-0F2', where the first character is ColorChar 1, second character is ColorChar 2, and the third character determines the gradient between the two colors
    /// </summary>
    /// <param name="HiColorString"></param>
        public static void HiColorDraw(string HiColorString) {

            //An example would be 0F1-0F2-1F3
            String[] HiColorRow = HiColorString.Split('-');

            foreach (String HiColorBlock in HiColorRow) {
                if (String.IsNullOrWhiteSpace(HiColorBlock)||HiColorBlock.Length<3) { Block(Console.BackgroundColor); }
                else{
                    ConsoleColor BG = ColorCharToConsoleColor(HiColorBlock[0]);
                    ConsoleColor FG = ColorCharToConsoleColor(HiColorBlock[1]);

                    switch (HiColorBlock[2])
                    {
                        case '0':
                            Sprite("░", BG, FG);
                            break;
                        case '1':
                            Sprite("▒", BG, FG);
                            break;
                        case '2':
                            Sprite("▓", BG, FG);
                            break;
                        default:
                            Block(Console.BackgroundColor);
                            break;
                    }
                }

            }

        }

        /// <summary>displays text at the currentline, without a linebreak. <br></br>WILL LINEBREAK IF ONLY '.' IS SPECIFIED</summary>
        /// <param name="text"></param>
        public static void Echo(String text) { Echo(text, false); }

        /// <summary>displays text at the currentline, possibly with a linebreak if specified. <br></br>WILL LINEBREAK IF ONLY '.' IS SPECIFIED</summary>
        /// <param name="text"></param>
        /// <param name="Linebreak"></param>
        public static void Echo(String text, Boolean Linebreak) {
            if (text == ".") { Echo("", true); return; }
            if (Linebreak) { Console.WriteLine(text); } else { Console.Write(text); }
        }

        /// <summary>Sets the cursor position to the specified one</summary>
        /// <param name="LeftPos"></param>
        /// <param name="TopPos"></param>
        public static void SetPos(int LeftPos, int TopPos) { Console.SetCursorPosition(LeftPos, TopPos); }

        /// <summary>Sets the foreground color of the console</summary>
        /// <param name="FG"></param>
        public static void Color(ConsoleColor FG) { Console.ForegroundColor = FG; }

        /// <summary>sets the background color of the console</summary>
        /// <param name="BG"></param>
        /// <param name="FG"></param>
        public static void Color(ConsoleColor BG, ConsoleColor FG) { Console.BackgroundColor = BG; Console.ForegroundColor = FG; }

    }
}
