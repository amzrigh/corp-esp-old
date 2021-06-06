/****************************************************************************************
 * Player main menu/gameplay screen class                                               *
 * Mark McCarthy                                                                        *
 *                                                                                      *
 * First layer of player interface, displaying current active projects (?)              *
 * and selections to examine all gameplay fields (i.e., switch screens).                *
 * based on MSDN Screen Management suite                                                *
 ****************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CorporateEspionage
{
    class MainScreen : ActiveMenuScreen
    {
        CPUOpponent opponent;
        
        Texture2D border;
        Texture2D paneImage;            //natural 235 x 150, scale to double

        Sprite receptionist;

        //game duration management
        //game > turn > day > tick
        //turns per game and time per tick to be determined during game setup
        //1 turn = 90 days
        //1 day = 8 ticks
        double daysToTurn;              //time til end of turn
        int turnsToEnd;                 //number of turns remaining
        int gameSpeed;                  //duration of ticks
        int nextTick;                   //progress through tick

        Boolean gameOver = false;               //is the game over

        PlayerIndex playerIndex;        //needed to call EndOfTurnScreen

        public MainScreen(Player player, CPUOpponent opponent, Vector2 menuPos, Vector2 titlePos, PlayerIndex playerIndex, int gameSpeed, int turnsToEnd)
            : base("Main Menu", player, menuPos, titlePos)
        {
            this.opponent = opponent;
            this.playerIndex = playerIndex;

            #region Menu Entries
            MenuEntry productionMenuEntry = new MenuEntry("Production");
            MenuEntry rndMenuEntry = new MenuEntry("R & D");

            productionMenuEntry.Selected += ProductionMenuEntrySelected;
            rndMenuEntry.Selected += RNDMenuEntrySelected;

            MenuEntries.Add(productionMenuEntry);
            MenuEntries.Add(rndMenuEntry);
            #endregion

            daysToTurn = 90;
            this.turnsToEnd = turnsToEnd;
            this.gameSpeed = gameSpeed;
            nextTick = 0;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            border = ScreenManager.Game.Content.Load<Texture2D>("images/main_border");
            paneImage = ScreenManager.Game.Content.Load<Texture2D>("images/main_splash");

            receptionist = new Sprite(ScreenManager.Game.Content.Load<Texture2D>("images/receptionist"), new Vector2(198,176), new Point(16, 24),
                new Point(0, 0), new Point(2, 4), new Vector2(0, 1), 2f);

            player.Load();
            opponent.Load();

            Game1.trackCue = Game1.soundBank.GetCue("lobby");
            Game1.trackCue.Play();
        }
        #region Menu Entries
        void ProductionMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            Game1.trackCue.Stop(Microsoft.Xna.Framework.Audio.AudioStopOptions.AsAuthored);
            ScreenManager.AddScreen(new ProductionScreen(player, menuPos, titlePos), e.PlayerIndex);
        }

        void RNDMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            Game1.trackCue.Stop(Microsoft.Xna.Framework.Audio.AudioStopOptions.AsAuthored);
            ScreenManager.AddScreen(new ResearchScreen(player, menuPos, titlePos), e.PlayerIndex);
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to abandon this game?";

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }

        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            Game1.trackCue.Stop(Microsoft.Xna.Framework.Audio.AudioStopOptions.AsAuthored);
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new StartScreen());
        }
        #endregion
        public override void Update(GameTime gameTime, bool focus, bool covered)
        {
            base.Update(gameTime, focus, covered);

            receptionist.Update(gameTime, ScreenManager.Game.Window.ClientBounds);

            if (!gameOver)
            {
                nextTick += gameTime.ElapsedGameTime.Milliseconds;
                if (nextTick >= gameSpeed)
                {
                    player.TickUpdate();
                    opponent.TickUpdate();
                    daysToTurn -= .125;             //progresses time 1 tick
                    nextTick -= gameSpeed;          //resets tick counter
                    if (daysToTurn == 0)
                    {
                        turnsToEnd--;
                        if (turnsToEnd == 0)
                        {
                            gameOver = true;
                        }
                        else
                        {
                            daysToTurn = 90;        //resets days in the turn
                        }
                        gameOver = EndOfTurn(gameOver);   //calculates and displays costs and profits for the turn; if gameOver, ends game
                        
                    }
                }
            }

        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            
            DrawPane(spriteBatch);
            DrawProjects(spriteBatch);
            DrawTime(spriteBatch);

            receptionist.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        //method described in ActiveMenuScreen.cs
        public override void DrawPane(SpriteBatch spriteBatch)
        {
            Rectangle splashImage = new Rectangle((int)frameAnchor.X + 10, (int)frameAnchor.Y + 10, 470, 300);
            Rectangle splashFrame = new Rectangle((int)frameAnchor.X, (int)frameAnchor.Y, splashImage.Width + 20, splashImage.Height + 20);

            spriteBatch.Draw(border, splashFrame, Color.White);
            spriteBatch.Draw(paneImage, splashImage, Color.White);
        }

        //method described in ActiveMenuScreen.cs
        public override void DrawProjects(SpriteBatch spriteBatch)
        {
            Vector2 pos = projectAnchor;
            const int spacing = 120;

            player.ProductionSegment.ActiveProject.Display(pos, player.ProductionSegment.FrameImage, player.ProductionSegment.ProgressBar,
                blank, tinyFont, spriteBatch);
            pos.X += spacing;
            player.RNDSegment.ActiveProject.Display(pos, player.RNDSegment.FrameImage, player.RNDSegment.ProgressBar,
                blank, tinyFont, spriteBatch);
        }

        //draws number of turns remaining and amount of time left in current turn
        public void DrawTime(SpriteBatch spriteBatch)
        {
            Vector2 pos = sideAnchor;

            string displayTurn = turnsToEnd + "";
            string displayDays = Math.Ceiling(daysToTurn) + "";

            Vector2 originTurn = screenFont.MeasureString(displayTurn);
            originTurn.Y = 0;

            Vector2 originDays = screenFont.MeasureString(displayDays);
            originDays.Y = 0;

            spriteBatch.DrawString(screenFont, "Quarters Remaining:", pos, Color.White);
            pos.Y += 35;
            spriteBatch.DrawString(screenFont, "Days to Quarter:", pos, Color.White);

            pos.Y = sideAnchor.Y;
            pos.X += 210;
            spriteBatch.DrawString(screenFont, displayTurn, pos, Color.CornflowerBlue, 0f, originTurn, 1f, SpriteEffects.None, 0f);
            pos.Y += 35;
            spriteBatch.DrawString(screenFont, displayDays, pos, Color.CornflowerBlue, 0f, originDays, 1f, SpriteEffects.None, 0f);
            
        }

        //performs end-of-turn calculations, calls EndOfTurn screen
        Boolean EndOfTurn(Boolean gameOver)
        {
            int playerMoney = player.Money;         //freeze current state for calculation and display
            int opponentMoney = opponent.Money;
            int playerProfit = 0;
            int opponentProfit = 0;
            int playerSpent = player.QuarterlyCosts;
            int opponentSpent = opponent.QuarterlyCosts;
            List<Product> playerProducts = new List<Product>();
            foreach (Product p in player.Products)
                playerProducts.Add(new Product(p));
            List<Product> opponentProducts = new List<Product>();
            foreach (Product p in opponent.Products)
                opponentProducts.Add(new Product(p));

            for (int i = 0; i < playerProducts.Count; i++)  //sums revenue
            {
                if (playerProducts[i].Stock + opponentProducts[i].Stock < playerProducts[i].SaturationPoint)
                {
                    playerProfit += playerProducts[i].SalePrice * playerProducts[i].Stock;
                    opponentProfit += opponentProducts[i].SalePrice * opponentProducts[i].Stock;
                }
                else
                {
                    playerProfit += playerProducts[i].SaturatedSalePrice(playerProducts[i].Stock + opponentProducts[i].Stock) * playerProducts[i].Stock;
                    opponentProfit += opponentProducts[i].SaturatedSalePrice(playerProducts[i].Stock + opponentProducts[i].Stock) * opponentProducts[i].Stock;
                }
            }

            player.Money += playerProfit - playerSpent;         //applies changes
            opponent.Money += opponentProfit - opponentSpent;

            if (!gameOver)                                      //if the game isn't over anyway...
            {
                if (player.Money <= 0 || opponent.Money <= 0)   //check to see if a player lost
                    gameOver = true;
            }

            ScreenManager.AddScreen(new EndOfTurnScreen(playerMoney, opponentMoney, playerProfit, opponentProfit, playerSpent, opponentSpent,
                playerProducts, opponentProducts, gameOver), playerIndex);

            player.NewQuarter();        //reset stocks (sales)
            opponent.NewQuarter();

            return gameOver;            //returns gameOver so Update method will know whether to keep processing game mechanics
        }
    }
}
