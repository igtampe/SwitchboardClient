using System;
using System.IO;

namespace BasicRender {
    /// <summary>Holds a Drawable BasicRenderGraphic</summary>
    public abstract class BasicRenderGraphic {
        protected String[] Contents;
        protected String Name;

        public abstract void draw(int LeftPos,int TopPos);
        public string getName() { return Name; }

        /// <summary>Gets the width of the graphic</summary>
        /// <returns>Length of the first line</returns>
        public int GetWidth() {
            if(Contents==null) { return 0; }
            if(Contents[0]==null) { return 0; }
            return Contents[0].Length;
        }

        /// <summary>Gets the height of the graphic</summary>
        /// <returns>The number of lines of the graphic</returns>
        public int GetHeight() {
            if(Contents==null) { return 0; }
            return Contents.Length;
        }
    }

    /// <summary>Holds a BasicGraphic</summary>
    public abstract class BasicGraphic:BasicRenderGraphic {
        public override void draw(int LeftPos,int TopPos) {
            foreach(String Line in Contents) {
                Render.SetPos(LeftPos,TopPos++);
                Render.Draw(Line);
            }
        }
    }

    /// <summary>Holds a HiColorGraphic</summary>
    public abstract class HiColorGraphic:BasicRenderGraphic {
        public override void draw(int LeftPos,int TopPos) {
            foreach(String Line in Contents) {
                Render.SetPos(LeftPos,TopPos++);
                Render.HiColorDraw(Line);
            }
        }
    }

    /// <summary>Holds a BasicGraphic from a file</summary>
    public class BasicGraphicFromFile:BasicGraphic {

        /// <summary>Generates a BasicGraphic item from a file</summary>
        public BasicGraphicFromFile(String Filename) {

            if(!File.Exists(Filename)) { throw new FileNotFoundException(); }
            Name=Filename;
            Contents=File.ReadAllLines(Filename);

        }

    }

    /// <summary>Holds a HiColorGraphic from a file</summary>
    public class HiColorGraphicFromFile:HiColorGraphic {

        /// <summary>Generates a HiColorGraphic item from a file</summary>
        public HiColorGraphicFromFile(String Filename) {

            if(!File.Exists(Filename)) { throw new FileNotFoundException(); }
            Name=Filename;
            Contents=File.ReadAllLines(Filename);
        }
    }

    /// <summary>Switchboard Logo</summary>
    public class SwitchboardLogo:BasicGraphic {

        public SwitchboardLogo() {

            String[] Cloud = {
                "88888888888",
                "88F88788F88",
                "88888888888",
                "88788F88788",
                "88888888888"
            };

            Contents=Cloud;
            Name="SwitchboardLogo Graphic";
        }


    }

}
