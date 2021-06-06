/********************************************
 * Start Screen Class                       *
 * Mark McCarthy                            *
 *                                          *
 * Initial screen displays title            *
 * Allows player to begin new game or quit  *
 * Based on MSDN Screen Manager suite       *
 ********************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CorporateEspionage
{
    class StartScreen : MenuScreen
    {
        SpriteFont titleFont;
        SpriteFont footFont;

        public StartScreen()
            : base("")
        {
            MenuEntry newGameMenuEntry = new MenuEntry("New Game");
            MenuEntry howToMenuEntry = new MenuEntry("How to Play");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");

            newGameMenuEntry.Selected += NewGameMenuEntrySelected;
            howToMenuEntry.Selected += HowToMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            MenuEntries.Add(newGameMenuEntry);
            MenuEntries.Add(howToMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            titleFont = ScreenManager.Game.Content.Load<SpriteFont>("fonts/americana dreams");
            footFont = ScreenManager.Game.Content.Load<SpriteFont>("fonts/lilliput steps");

            Game1.trackCue = Game1.soundBank.GetCue("start");
            Game1.trackCue.Play();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime,otherScreenHasFocus,coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            //spriteBatch.Draw(image,imageRectangle,Color.White);
            base.Draw(gameTime, new Vector2((ScreenManager.Game.Window.ClientBounds.Width - ScreenManager.Font.MeasureString(MenuEntries[0].Text).X) / 2, ScreenManager.Game.Window.ClientBounds.Height / 2),
                                new Vector2(ScreenManager.Game.Window.ClientBounds.Width / 2, 25));

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Vector2 titlePos = new Vector2(ScreenManager.Game.Window.ClientBounds.Width / 2, 100);

            string str = "Corporate";
            Vector2 origin = titleFont.MeasureString(str) / 2;

            spriteBatch.Begin();

            spriteBatch.DrawString(titleFont, str, titlePos, Color.Gold, 0f, origin, 1f, SpriteEffects.None, 0f);

            titlePos.Y += 90;
            str = "Espionage";
            origin = titleFont.MeasureString(str) / 2;

            spriteBatch.DrawString(titleFont, str, titlePos, Color.Gold, 0f, origin, 1f, SpriteEffects.None, 0f);

            titlePos.Y = ScreenManager.Game.Window.ClientBounds.Height - 24;
            str = "Corporate Espionage © 2011 Mark McCarthy, KHPSoftWorks";
            origin = footFont.MeasureString(str) / 2;

            spriteBatch.DrawString(footFont, str, titlePos, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);

            titlePos.Y += 8;
            str = "Graphics from EarthBound © 1994, 1995, 2003 Nintendo, HAL Labs, Ape Inc.";
            origin = footFont.MeasureString(str) / 2;

            spriteBatch.DrawString(footFont, str, titlePos, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);

            titlePos.Y += 8;
            str = "Music from various games © Keiichi Suzuki, Nobuo Uematsu, Yoko Shimomura";
            origin = footFont.MeasureString(str) / 2;

            spriteBatch.DrawString(footFont, str, titlePos, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);

            spriteBatch.End();
        }
        #region Menu Entries
        void NewGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new SetupScreen(), null);
        }

        void HowToMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new HowToPlayScreen(), null);
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to quit?";

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }

        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }
        #endregion
    }
}
