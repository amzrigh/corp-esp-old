/********************************************************
 * Production Screen Class                              *
 * by Mark McCarthy                                     *
 *                                                      *
 * Displays status of player Production segment         *
 * Connects to controls for managing Production segment *
 * based on MSDN Screen Manager suite                   *
 ********************************************************/

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
    class ProductionScreen : ActiveMenuScreen
    {
        #region Sprites
        Sprite spriteFrameRookie;           //employee sprites
        Sprite spriteFrameVeteran;          //frame 16x24px; display size 32x48px
        Sprite spriteFrameExpert;           //sheet 2 x 4 frames
        Sprite spriteSideRookie;
        Sprite spriteSideVeteran;
        Sprite spriteSideExpert;

        Sprite spriteBasic;             //equipment sprites
        Sprite spriteIntermediate;      //frame 32x48px; display size 64x96px
        Sprite spriteAdvanced;          //sheet 1x1 or 2x1 frames
        #endregion

        Texture2D paneImage;            //splash image

        Cue track;                      //facilitates multipart bgm
        
        public ProductionScreen(Player player, Vector2 menuPos, Vector2 titlePos)
            : base("Production", player, menuPos, titlePos)
        {
            #region Menu Setup
            MenuEntry addNewProjectMenuEntry = new MenuEntry("New Project");
            MenuEntry manageEmployeesMenuEntry = new MenuEntry("Manage Employees");
            MenuEntry manageEquipmentMenuEntry = new MenuEntry("Manage Equipment");
            MenuEntry backMenuEntry = new MenuEntry("Back");

            addNewProjectMenuEntry.Selected += AddNewProjectMenuEntrySelected;
            manageEmployeesMenuEntry.Selected += ManageEmployeesMenuEntrySelected;
            manageEquipmentMenuEntry.Selected += ManageEquipmentMenuEntrySelected;
            backMenuEntry.Selected += OnCancel;

            MenuEntries.Add(addNewProjectMenuEntry);
            MenuEntries.Add(manageEmployeesMenuEntry);
            MenuEntries.Add(manageEquipmentMenuEntry);
            MenuEntries.Add(backMenuEntry);
            #endregion
            #region Sprites Setup
            spriteFrameRookie = new Sprite(player.ProductionSegment.TextureRookie, new Vector2(144, 250),
                new Point(16, 24), new Point(0, 0), new Point(2, 4), new Vector2(0, -1), 2f);
            spriteFrameVeteran = new Sprite(player.ProductionSegment.TextureVeteran, new Vector2(250, 210),
                new Point(16, 24), new Point(0, 0), new Point(2, 4), new Vector2(0, 1), 2f);
            spriteFrameExpert = new Sprite(player.ProductionSegment.TextureExpert, new Vector2(375, 271),
                new Point(16, 24), new Point(0, 0), new Point(2, 4), new Vector2(-1, 0), 2f);

            Vector2 pos = sideAnchor;
            pos.Y += 63;
            spriteSideRookie = new Sprite(player.ProductionSegment.TextureRookie, pos, new Point(16, 24),
                new Point(0, 0), new Point(2, 4), new Vector2(0, 1), 2f);
            pos.Y += 96;
            spriteSideVeteran = new Sprite(player.ProductionSegment.TextureVeteran, pos, new Point(16, 24),
                new Point(0, 0), new Point(2, 4), new Vector2(0, 1), 2f);
            pos.Y += 96;
            spriteSideExpert = new Sprite(player.ProductionSegment.TextureExpert, pos, new Point(16, 24),
                new Point(0, 0), new Point(2, 4), new Vector2(0, 1), 2f);

            pos = sideAnchor;
            pos.Y += 15;
            pos.X += 115;
            spriteBasic = new Sprite(player.ProductionSegment.TextureBasic, pos, new Point(32, 48),
                new Point(0, 0), new Point(1, 1), new Vector2(0, 1), 2f);
            pos.Y += 96;
            spriteIntermediate = new Sprite(player.ProductionSegment.TextureIntermediate, pos, new Point(32, 48),
                new Point(0, 0), new Point(1, 1), new Vector2(0, 1), 2f);
            pos.Y += 96;
            spriteAdvanced = new Sprite(player.ProductionSegment.TextureAdvanced, pos, new Point(32, 48),
                new Point(0, 0), new Point(1, 1), new Vector2(0, 1), 2f);
            #endregion
        }
        #region Menu Entries
        void AddNewProjectMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new ProductionAddProjectScreen(player, menuPos, titlePos), e.PlayerIndex);
        }

        void ManageEmployeesMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new ProductionHRScreen(player, menuPos, titlePos), e.PlayerIndex);
        }

        void ManageEquipmentMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new ProductionEquipmentScreen(player, menuPos, titlePos), e.PlayerIndex);
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            Game1.trackCue.Stop(AudioStopOptions.AsAuthored);

            Game1.trackCue = Game1.soundBank.GetCue("lobby");
            Game1.trackCue.Play();
            base.OnCancel(playerIndex);
        }
        #endregion

        public override void LoadContent()
        {
            base.LoadContent();
            paneImage = ScreenManager.Game.Content.Load<Texture2D>("images/production_splash");

            Game1.trackCue = Game1.soundBank.GetCue("production");
            track = Game1.soundBank.GetCue("production");
            Game1.trackCue.Play();
        }

        #region Update, Draw and related functions
        public override void Update(GameTime gameTime, bool focus, bool covered)
        {
            if (!(Game1.trackCue.IsPlaying || this.IsExiting))  //detects when to switch multipart bgm
            {
                track.Play();
                Game1.trackCue = track;
            }

            base.Update(gameTime, focus, covered);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            spriteBatch.Begin();
            DrawPane(spriteBatch);
            DrawProjects(spriteBatch);
            DrawAssets(gameTime, spriteBatch);
            spriteBatch.End();
        }

        //method described in ActiveMenuScreen.cs
        public override void DrawPane(SpriteBatch spriteBatch)
        {
            Rectangle splashImage = new Rectangle((int)frameAnchor.X + 10, (int)frameAnchor.Y + 10, 470, 300);
            Rectangle splashFrame = new Rectangle((int)frameAnchor.X, (int)frameAnchor.Y, splashImage.Width + 20, splashImage.Height + 20);

            spriteBatch.Draw(player.ProductionSegment.FrameImage, splashFrame, Color.White);
            spriteBatch.Draw(paneImage, splashImage, Color.White);
        }

        //method described in ActiveMenuScreen.cs
        public override void DrawProjects(SpriteBatch spriteBatch)
        {
            Vector2 pos = projectAnchor;
            const int spacing = 120;

            player.ProductionSegment.ActiveProject.Display(pos, player.ProductionSegment.FrameImage, player.ProductionSegment.ProgressBar,
                blank, tinyFont, spriteBatch);                      //display active project;
            if (player.ProductionSegment.ProjectQueue.Count > 0)    //if there's anything in the queue...
            {
                List<Project> qList = new List<Project>();
                foreach (Project p in player.ProductionSegment.ProjectQueue)
                {
                    qList.Add(p);
                }
                for (int i = 0; i < 3 && i < qList.Count; i++)      //display them, too
                {
                    pos.X += spacing;
                    qList[i].Display(pos, player.ProductionSegment.FrameImage, player.ProductionSegment.ProgressBar, blank, tinyFont, spriteBatch);
                }
            }
        }

        //draws employee and equipment counts in sidebar
        public void DrawAssets(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 pos = sideAnchor;
            CompanySegment seg = player.ProductionSegment;

            spriteBatch.DrawString(screenFont, "Assets:", pos, Color.White);
            if (seg.Rookies > 0)                            //draws each asset if player has any
            {
                spriteFrameRookie.Update(gameTime, ScreenManager.Game.Window.ClientBounds);
                spriteFrameRookie.Draw(gameTime, spriteBatch);
                spriteSideRookie.Update(gameTime, ScreenManager.Game.Window.ClientBounds);
                spriteSideRookie.Draw(gameTime, spriteBatch);
                pos = spriteSideRookie.Position;
                pos.X += 50;
                pos.Y += 16;
                spriteBatch.DrawString(screenFont, "x " + seg.Rookies, pos, Color.White);
            }
            if (seg.Veterans > 0)
            {
                spriteFrameVeteran.Update(gameTime, ScreenManager.Game.Window.ClientBounds);
                spriteFrameVeteran.Draw(gameTime, spriteBatch);
                spriteSideVeteran.Update(gameTime, ScreenManager.Game.Window.ClientBounds);
                spriteSideVeteran.Draw(gameTime, spriteBatch);
                pos = spriteSideVeteran.Position;
                pos.X += 50;
                pos.Y += 16;
                spriteBatch.DrawString(screenFont, "x " + seg.Veterans, pos, Color.White);
            }
            if (seg.Experts > 0)
            {
                spriteFrameExpert.Update(gameTime, ScreenManager.Game.Window.ClientBounds);
                spriteFrameExpert.Draw(gameTime, spriteBatch);
                spriteSideExpert.Update(gameTime, ScreenManager.Game.Window.ClientBounds);
                spriteSideExpert.Draw(gameTime, spriteBatch);
                pos = spriteSideExpert.Position;
                pos.X += 50;
                pos.Y += 16;
                spriteBatch.DrawString(screenFont, "x " + seg.Experts, pos, Color.White);
            }
            if (seg.Tools > 0)
            {
                spriteBasic.Update(gameTime, ScreenManager.Game.Window.ClientBounds);
                spriteBasic.Draw(gameTime, spriteBatch);
                pos = spriteBasic.Position;
                pos.X += 70;
                pos.Y += 64;
                spriteBatch.DrawString(screenFont, "x " + seg.Tools, pos, Color.White);
            }
            if (seg.Machinery > 0)
            {
                spriteIntermediate.Update(gameTime, ScreenManager.Game.Window.ClientBounds);
                spriteIntermediate.Draw(gameTime, spriteBatch);
                pos = spriteIntermediate.Position;
                pos.X += 70;
                pos.Y += 64;
                spriteBatch.DrawString(screenFont, "x " + seg.Machinery, pos, Color.White);
            }
            if (seg.HeavyMachinery > 0)
            {
                spriteAdvanced.Update(gameTime, ScreenManager.Game.Window.ClientBounds);
                spriteAdvanced.Draw(gameTime, spriteBatch);
                pos = spriteAdvanced.Position;
                pos.X += 70;
                pos.Y += 64;
                spriteBatch.DrawString(screenFont, "x " + seg.HeavyMachinery, pos, Color.White);
            }
        }
        #endregion
    }
}
