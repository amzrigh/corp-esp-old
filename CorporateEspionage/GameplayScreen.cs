/********************************************************
 * GameplayScreen Abstract Class                        *
 * Mark McCarthy                                        *
 *                                                      *
 * Modification to the GameScreen parent class for the  *
 * set of primary gameplay screens:                     *
 * - MainScreen                                         *
 * - OperationsScreen                                   *
 * - RnDScreen                                          *
 * - SecurityScreen                                     *
 * - MarketingScreen                                    *
 * NOT USED FOR SUBMENUS, or for FinanceScreen (TBI)    *
 ********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace CorporateEspionage
{
    class GameplayScreen : GameScreen
    {
        Texture2D paneImage;
        Texture2D borderImage;
        Texture2D progressBar;
        MenuComponent menuComponent;
        SpriteFont pageText;

        CompanySegment companySegment;        //mechanical component; passed to class to display and manipulate

        public GameplayScreen(Game game, SpriteBatch spriteBatch, Texture2D paneImage, Texture2D borderImage, SpriteFont pageText,
            SpriteFont menuText, string[] menuSelection, Vector2 menuPos)
            : base(game, spriteBatch)
        {
            this.paneImage = paneImage;
            this.borderImage = borderImage;
            this.pageText = pageText;
            menuComponent = new MenuComponent(game, spriteBatch, menuText, menuSelection, menuPos); //try (648,389) for most menus
        }

        public GameplayScreen(Game game, SpriteBatch spriteBatch, Texture2D paneImage, Texture2D borderImage, Texture2D progressBar, SpriteFont pageText,
            SpriteFont menuText, string[] menuSelection, Vector2 menuPos)
            : this(game, spriteBatch, paneImage, borderImage, pageText, menuText, menuSelection, menuPos)
        {
            this.progressBar = progressBar;
        }



        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
                        
            /*if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                ConfirmExitGameDialog() call*/
        }
    }
}
