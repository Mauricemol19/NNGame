using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;

namespace NNGame.Classes.Gui
{
    /// <summary>
    /// ingame GUI Menu
    /// </summary>
    public class GameMenu
    {
        private readonly Main main;

        public Panel panel1;

        public GameMenu()
        {            
            panel1 = new Panel(size: new Vector2(500, 80), skin: PanelSkin.Fancy, anchor: Anchor.TopLeft);

            UserInterface.Active.AddEntity(panel1);           

            panel1.AddChild(new Paragraph("x:0\ny:0\nTile:", Anchor.CenterLeft, offset: new Vector2(0, 0)), false);

            UserInterface.Active.ShowCursor = false;
        }
    }
}
