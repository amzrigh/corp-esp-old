/**************************************
 * End of Turn Screen Class           *
 * by Mark McCarthy                   *
 *                                    *
 * Displays final results of game.    *
 * based on MSDN Screen Manager suite *
 **************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace CorporateEspionage
{
    class GameOverScreen : MenuScreen
    {
        int player;
        int opponent;

        SpriteFont largeFont;
        SpriteFont smallFont;

        Vector2 center;         //anchor for text display

        Cue track;              //facilitates multipart song

        public GameOverScreen(int player, int opponent)
            : base("")
        {
            this.player = player;
            this.opponent = opponent;

            MenuEntry endMenuEntry = new MenuEntry(string.Empty);

            endMenuEntry.Selected += OnCancel;

            MenuEntries.Add(endMenuEntry);
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            Game1.trackCue.Stop(AudioStopOptions.AsAuthored);
            LoadingScreen.Load(ScreenManager, true, playerIndex, new StartScreen());
        }

        public override void LoadContent()
        {
            smallFont = ScreenManager.Game.Content.Load<SpriteFont>("fonts/Ketchup Spaghetti");
            string font;
            string song;
            if (player > opponent)
            {
                font = "fonts/Aerovias Brasil NF";
                song = "win";
            }
            else
            {
                font = "fonts/A.D. MONO";
                song = "gameover";
            }
            largeFont = ScreenManager.Game.Content.Load<SpriteFont>(font);

            center = new Vector2(ScreenManager.Game.Window.ClientBounds.Width / 2, ScreenManager.Game.Window.ClientBounds.Height / 2);



            Game1.trackCue = Game1.soundBank.GetCue(song);
            track = Game1.soundBank.GetCue(song);
            Game1.trackCue.Play();

            base.LoadContent();
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            if (!(Game1.trackCue.IsPlaying || this.IsExiting))  //detects when to switch multipart bgm
            {
                track.Play();
                Game1.trackCue = track;
            }

            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Vector2 pos = center;
            pos.Y -= 150;
            string str1;
            string str2;
            string score1;
            string score2;
            Color color;

            if (player > opponent)              //sets up results in proper order
            {
                str1 = "YOU";
                str2 = "WIN";
                color = Color.Gold;
                score1 = "Your Assets: $" + player;
                score2 = "Opponent's Assets: $" + opponent;
            }
            else
            {
                str1 = "CPU";
                str2 = "WINS";
                color = Color.Gray;
                score1 = "Opponent's Assets: $" + opponent;
                score2 = "Your Assets: $" + player;
            }

            spriteBatch.Begin();

            //draws results
            Vector2 origin = largeFont.MeasureString(str1) / 2;
            spriteBatch.DrawString(largeFont, str1, pos, color, 0f, origin, 1f, SpriteEffects.None, 0f);
            
            pos.Y += 80;
            origin = largeFont.MeasureString(str2) / 2;
            spriteBatch.DrawString(largeFont, str2, pos, color, 0f, origin, 1f, SpriteEffects.None, 0f);

            pos = center;
            pos.Y += 100;

            origin = smallFont.MeasureString(score1) / 2;
            spriteBatch.DrawString(smallFont, score1, pos, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);

            pos.Y += 20;
            origin = smallFont.MeasureString(score2) / 2;
            spriteBatch.DrawString(smallFont, score2, pos, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);

            spriteBatch.End();
        }
    }
}
