/********************************************************
 * ActiveMenuScreen Abstract Class                      *
 * Mark McCarthy                                        *
 *                                                      *
 * Modification to the MenuScreen parent class for the  *
 * set of primary gameplay screens:                     *
 * - MainScreen                                         *
 * - ProductionScreen                                   *
 * - ResearchScreen                                     *
 * - SecurityScreen (TBI)                               *
 * - MarketingScreen (TBI)                              *
 *                                                      *
 * based on MSDN Screen Manager suite                   *
 ********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace CorporateEspionage
{
    abstract class ActiveMenuScreen : MenuScreen
    {

        protected Player player;        //mechanical component; passed to class to display and manipulate

        protected Vector2 menuPos;
        protected Vector2 titlePos;

        
        protected Texture2D blank;                //blank texture for various display purposes

        protected Vector2 frameAnchor = new Vector2(25, 40);        //anchor position for frame
        protected Vector2 projectAnchor = new Vector2(25, 380);     //anchor position for project frames
        protected Vector2 sideAnchor = new Vector2(530, 60);       //anchor position for sidebar
        
        protected SpriteFont screenFont;
        protected SpriteFont tinyFont;

        #region Accessors
        public Vector2 MenuPos
        {
            get { return menuPos; }
        }

        public Vector2 TitlePos
        {
            get { return titlePos; }
        }
        #endregion

        public ActiveMenuScreen(string title, Player player, Vector2 menuPos, Vector2 titlePos)
            : base(title)
        {
            this.player = player;
            this.menuPos = menuPos;
            this.titlePos = titlePos;
        }

        public override void LoadContent()
        {
            screenFont = ScreenManager.Game.Content.Load<SpriteFont>("fonts/Ketchup Spaghetti");
            blank = ScreenManager.Game.Content.Load<Texture2D>("images/blank");
            tinyFont = ScreenManager.Game.Content.Load<SpriteFont>("fonts/Lilliput Steps");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime,bool focus, bool covered)
        {
            base.Update(gameTime, focus, covered);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime, menuPos, titlePos);
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            DrawMoney(spriteBatch);
            spriteBatch.End();
        }

        public virtual void DrawPane(SpriteBatch spriteBatch)
        {
            //draws splash pane and border
        }

        public virtual void DrawProjects(SpriteBatch spriteBatch)
        {
            //draws project panes
            //for main screen, draws active project from each segment
            //for segment screens, draws active project and first three queued projects
        }

        //draws player money value at top of screen
        public void DrawMoney(SpriteBatch spriteBatch)
        {
            string money = "$" + player.Money;
            Vector2 origin = screenFont.MeasureString(money);
            origin.Y = 0;

            spriteBatch.DrawString(screenFont, "Current Funds:", new Vector2(frameAnchor.X, 5), Color.White);
            spriteBatch.DrawString(screenFont, money, new Vector2(frameAnchor.X + 490, 5), Color.CornflowerBlue, 0f, origin, 1f, SpriteEffects.None, 0f);
        }

        //plays buzzer when player tries to perform a disallowed action
        protected void PlayFailSound()
        {
            Cue sound = Game1.soundBank.GetCue("error");
            sound.Play();
        }
    }
}
