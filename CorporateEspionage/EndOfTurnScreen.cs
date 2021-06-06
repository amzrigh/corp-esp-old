/********************************************************
 * End of Turn Screen Class                             *
 * by Mark McCarthy                                     *
 *                                                      *
 * Displays reports for each company consisting of      *
 * figures for per-item sales, total revenue and        *
 * total expenditure                                    *
 * based on MSDN Screen Manager suite                   *
 ********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CorporateEspionage
{
    class EndOfTurnScreen : MenuScreen
    {
        Boolean gameOver;       //determines whether continuing back to game or loading Game Over screen
        
        int playerMoney;        //values displayed
        int opponentMoney;

        int playerProfit;
        int opponentProfit;

        int playerSpent;
        int opponentSpent;

        List<Product> playerProducts;
        List<Product> opponentProducts;

        Vector2 anchor = new Vector2(10,10);
        SpriteFont font;
        SpriteFont smallFont;

        int PlayerFinal
        {
            get { return playerMoney + playerProfit - playerSpent; }
        }

        int OpponentFinal
        {
            get { return opponentMoney + opponentProfit - opponentSpent; }
        }
        public EndOfTurnScreen(int playerMoney, int opponentMoney, int playerProfit, int opponentProfit, int playerSpent, int opponentSpent,
            List<Product> playerProducts, List<Product> opponentProducts, Boolean gameOver)
            : base("Report")
        {
            this.playerMoney = playerMoney;
            this.opponentMoney = opponentMoney;
            this.playerProfit = playerProfit;
            this.opponentProfit = opponentProfit;
            this.playerSpent = playerSpent;
            this.opponentSpent = opponentSpent;
            this.playerProducts = playerProducts;
            this.opponentProducts = opponentProducts;

            this.gameOver = gameOver;

            MenuEntry continueMenuEntry = new MenuEntry(string.Empty);

            continueMenuEntry.Selected += OnCancel;

            MenuEntries.Add(continueMenuEntry);
        }

        public override void  LoadContent()
        {
 	        base.LoadContent();
            font = ScreenManager.Game.Content.Load<SpriteFont>("fonts/ketchup spaghetti");
            smallFont = ScreenManager.Game.Content.Load<SpriteFont>("fonts/lilliput steps");
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            if (!gameOver)                          //if the last turn has ended or a player has lost, go to GameOverScreen
                base.OnCancel(playerIndex);
            else
            {
                Game1.trackCue.Stop(Microsoft.Xna.Framework.Audio.AudioStopOptions.AsAuthored);
                LoadingScreen.Load(ScreenManager, true, playerIndex, new GameOverScreen(PlayerFinal, OpponentFinal));
            }
        }

        public override void Update(GameTime gameTime, bool focus, bool covered)
        {
            base.Update(gameTime, focus, covered);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Vector2 pos = anchor;

            spriteBatch.Begin();

            DrawReport("Your Company", pos, playerMoney, playerProfit, playerSpent, playerProducts, spriteBatch);
            pos.X += ScreenManager.Game.Window.ClientBounds.Width / 2;
            DrawReport("Opponent's Company", pos, opponentMoney, opponentProfit, opponentSpent, opponentProducts, spriteBatch);

            spriteBatch.End();
        }

        //draws one player's report from data passed in
        void DrawReport(string name, Vector2 pos, int money, int profit, int spent, List<Product> itemized, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, name, pos, Color.White);

            pos.Y += 30;
            if (itemized[0].IsResearched)                       //if product not researched, product panel not drawn
                DrawItem(itemized[0], pos, playerProducts[0].Stock + opponentProducts[0].Stock, spriteBatch);
            pos.X += 200;
            if (itemized[1].IsResearched)
                DrawItem(itemized[1], pos, playerProducts[1].Stock + opponentProducts[1].Stock, spriteBatch);
            pos.X -= 200;
            pos.Y += 120;
            if (itemized[2].IsResearched)
                DrawItem(itemized[2], pos, playerProducts[2].Stock + opponentProducts[2].Stock, spriteBatch);
            pos.X += 200;
            if (itemized[3].IsResearched)
                DrawItem(itemized[3], pos, playerProducts[3].Stock + opponentProducts[3].Stock, spriteBatch);
            pos.X -= 200;
            pos.Y += 120;
            if (itemized[4].IsResearched)
                DrawItem(itemized[4], pos, playerProducts[4].Stock + opponentProducts[4].Stock, spriteBatch);
            pos.X += 200;
            if (itemized[5].IsResearched)
                DrawItem(itemized[5], pos, playerProducts[5].Stock + opponentProducts[5].Stock, spriteBatch);

            pos.X -= 200;
            pos.Y += 120;
            spriteBatch.DrawString(font, "Starting Assets:\nRevenue:\nQuarterly costs:\n-------------------------------------------\nNew total:", pos, Color.White);
            
            pos.X += 380;       //drawing values, right-aligned
            string str = "$" + money;
            Vector2 origin = font.MeasureString(str);
            origin.Y = 0;
            spriteBatch.DrawString(font, str, pos, Color.CornflowerBlue, 0f, origin, 1f, SpriteEffects.None, 0f);
            
            pos.Y += 22;
            str = "$" + profit;
            origin = font.MeasureString(str);
            origin.Y = 0;
            spriteBatch.DrawString(font, str, pos, Color.White, 0f, origin, 1f, SpriteEffects.None, 0f);

            pos.Y += 22;
            str = "$(" + spent + ")";
            origin = font.MeasureString(str);
            origin.Y = 0;
            spriteBatch.DrawString(font, str, pos, Color.Red, 0f, origin, 1f, SpriteEffects.None, 0f);

            pos.Y += 44;
            str = "$" + (money + profit - spent);
            origin = font.MeasureString(str);
            origin.Y = 0;
            spriteBatch.DrawString(font, str, pos, Color.CornflowerBlue, 0f, origin, 1f, SpriteEffects.None, 0f);
        }

        //draws product panel at given position, consisting of icon, number sold and unit sale price
        void DrawItem(Product product, Vector2 pos, int total, SpriteBatch spriteBatch)
        {
            Rectangle itemPane = new Rectangle((int)pos.X, (int)pos.Y, 90, 90);
            Rectangle itemIcon = new Rectangle(itemPane.X + ((itemPane.Width - product.ItemIcon.Width) / 2),
                    itemPane.Y + ((itemPane.Height - product.ItemIcon.Height) / 2),
                    product.ItemIcon.Width, product.ItemIcon.Height);
            if (itemIcon.Width > itemPane.Width)
            {
                itemIcon.Width = itemPane.Width;
                itemIcon.X = itemPane.X;
            }
            if (itemIcon.Height > itemPane.Height)
            {
                itemIcon.Height = itemPane.Height;
                itemIcon.Y = itemPane.Y;
            }

            Vector2 textPos = pos;
            textPos.X += 92;
            textPos.Y += 15;
            spriteBatch.Draw(product.ItemIcon, itemIcon, Color.White);
            string str = product.Name + "\nSold: " + product.Stock + "\nAt:\n";
            if (total > product.SaturationPoint)
                str += "$" + product.SaturatedSalePrice(total);
            else
                str += "$" + product.SalePrice;
            str += " ea.";
            spriteBatch.DrawString(smallFont, str, textPos, Color.White);
        }
    }
}
