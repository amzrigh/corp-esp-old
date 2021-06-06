/****************************************
 * Setup Screen Class                   *
 * by Mark McCarthy                     *
 *                                      *
 * Allows player to select some game    *
 * parameters before beginning          *
 * based on MSDN Screen Manager suite   *
 ****************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;


namespace CorporateEspionage
{
    class SetupScreen : MenuScreen
    {
        string[] gameSpeed = { "Very Slow", "Slow", "Normal", "Fast", "Very Fast" };        //Very Slow: 1 day = 5 sec;     Very Fast: 1 day = 1 sec;
        int speedIndex = 2;

        string[] gameLength = { "Very Short", "Short", "Normal", "Long", "Very Long" };     //Very Short: 4 turns;          Very Long: 12 turns;
        int lengthIndex = 2;

        Vector2 pos;

        MenuEntry lengthMenuEntry;
        MenuEntry speedMenuEntry;
        MenuEntry startMenuEntry;
        MenuEntry backMenuEntry;

        public SetupScreen()
            : base("Game Setup")
        {
            lengthMenuEntry = new MenuEntry(string.Empty);
            speedMenuEntry = new MenuEntry(string.Empty);
            startMenuEntry = new MenuEntry("Start Game");
            backMenuEntry = new MenuEntry("Back");

            SetMenuEntryText();

            lengthMenuEntry.Selected += LengthMenuEntrySelected;
            speedMenuEntry.Selected += SpeedMenuEntrySelected;
            startMenuEntry.Selected += StartMenuEntrySelected;
            backMenuEntry.Selected += OnCancel;

            MenuEntries.Add(lengthMenuEntry);
            MenuEntries.Add(speedMenuEntry);
            MenuEntries.Add(startMenuEntry);
            MenuEntries.Add(backMenuEntry);
        }
        #region Menu Entries
        void SetMenuEntryText()
        {
            lengthMenuEntry.Text = "Game Length: " + gameLength[lengthIndex];
            speedMenuEntry.Text = "Speed: " + gameSpeed[speedIndex];
        }

        void LengthMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            lengthIndex++;
            if (lengthIndex >= gameLength.Length)
                lengthIndex = 0;

            SetMenuEntryText();
        }

        void SpeedMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            speedIndex++;
            if (speedIndex >= gameSpeed.Length)
                speedIndex = 0;

            SetMenuEntryText();
        }

        void StartMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            int length = (lengthIndex + 2) * 2;
            int speed = (5 - speedIndex) * 125;
            Game1.trackCue.Stop(Microsoft.Xna.Framework.Audio.AudioStopOptions.AsAuthored);
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new MainScreen(new Player(ScreenManager.Game), new CPUOpponent(ScreenManager.Game),
                new Vector2(ScreenManager.Game.Window.ClientBounds.Width - 275, (ScreenManager.Game.Window.ClientBounds.Height / 2) + 100),
                new Vector2(ScreenManager.Game.Window.ClientBounds.Width - 145, 25), e.PlayerIndex, speed, length));
        }

        #endregion
        public override void LoadContent()
        {
            base.LoadContent();

            pos = new Vector2((ScreenManager.Game.Window.ClientBounds.Width - ScreenManager.Font.MeasureString("Game Length: Very Slow").X) / 2, 100);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime, pos, new Vector2(ScreenManager.Game.Window.ClientBounds.Width /2, 25));
        }
    }
}
