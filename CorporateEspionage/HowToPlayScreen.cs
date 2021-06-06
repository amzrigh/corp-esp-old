using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace CorporateEspionage
{
    class HowToPlayScreen : MenuScreen
    {
        SpriteFont font;
        string text;

        public HowToPlayScreen()
            : base("How to Play")
        {
            MenuEntry backMenuEntry = new MenuEntry(string.Empty);

            backMenuEntry.Selected += OnCancel;

            MenuEntries.Add(backMenuEntry);

            text = "\tCorporate Espionage is a business simulation/strategy game. The object of the game is to make more\nmoney than your opponent through ";
            text += "selling more products, making better products, and wasting less money. The game\nends after the final quarter has elapsed, ";
            text += "or when a company ends a quarter in the red.\n";
            text += "\tYour company has two segments: Production and R&D. In Production, as the name suggests, you manage\nthe production of goods to sell. ";
            text += "In Research, you manage the discovery of new goods to produce, as well as improve\nexisting production methods. Each segment is controlled ";
            text += "in a similar fashion, as described below. In each segment\nscreen, the current active project and first three enqueued projects are ";
            text += "displayed across the bottom of the screen.\nOn the main screen, only the active projects for each segment are displayed.\n\n";
            text += "Add Project: Select and enqueue an new project for the segment. Production can only produce, but Research can\n\tmake new products, or improve ";
            text += "the quality (sale price), efficiency (cost to produce) or speed (time to\n\tproduce) of existing products. Production projects cost money ";
            text += "for materials, while research projects\n\tcost nothing more than the man-hours that go into them.\n";
            text += "Manage Employees: Hire and fire employees of three different skill levels -- Rookie, Veteran and Expert. Their salaries\n\tare listed below ";
            text += "their sprites. They receive their salary once upon hiring, and again at the end of each\n\tquarter.\n";
            text += "Manage Equipment: Buy and sell tools your employees can use to perform their jobs better. Equipment does nothing\n\twithout an employee to ";
            text += "operate it, so take care when purchasing. Prices are listed below their sprites.\n\tEquipment maintenance costs one third its purchase price ";
            text += "per quarter. Equipment can be sold back for\n\thalf its purchase price.\n\n";
            text += "\tRemember: the more your produce, the more money you make, but be careful not to flood the market!\nPrices will drop if too much of a single ";
            text += "product is produced.";
        }

        public override void LoadContent()
        {
            font = ScreenManager.Game.Content.Load<SpriteFont>("fonts/HowToFont");
            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime, new Vector2(0, 0), new Vector2(ScreenManager.Game.Window.ClientBounds.Width / 2, 25));
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            spriteBatch.DrawString(font, text, new Vector2(5, 40), Color.White);
            spriteBatch.End();
        }


    }
}
