using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Extended;
using GeonBit.UI;
using GeonBit.UI.Entities;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework;

namespace NNGame.Classes.Gui
{
    public class GameMenu
    {
        private readonly Main main;

        public Panel panel1;

        public GameMenu()
        {            
            panel1 = new Panel(size: new Vector2(230, 75), skin: PanelSkin.Fancy, anchor: Anchor.TopLeft);

            UserInterface.Active.AddEntity(panel1);           

            panel1.AddChild(new Paragraph("x:0\ny:0\nTile:", Anchor.CenterLeft, offset: new Vector2(0, 0)), false);

            UserInterface.Active.ShowCursor = false;
        }
    }
}
