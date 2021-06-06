/********************************************************
 * Research Screen Class                                *
 * by Mark McCarthy                                     *
 *                                                      *
 * Displays status of player Research segment           *
 * Connects to controls for managing Research segment   *
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
    class ResearchScreen : ActiveMenuScreen
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

        public ResearchScreen(Player player, Vector2 menuPos, Vector2 titlePos)
            : base("Research & Development", player, menuPos, titlePos)
        {
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

            #region Sprites Setup
            spriteFrameRookie = new Sprite(player.RNDSegment.TextureRookie, new Vector2(240, 245),
                new Point(16, 24), new Point(0, 0), new Point(2, 4), new Vector2(0, -1), 2f);
            spriteFrameVeteran = new Sprite(player.RNDSegment.TextureVeteran, new Vector2(212, 157),
                new Point(16, 24), new Point(0, 0), new Point(2, 4), new Vector2(0, 1), 2f);
            spriteFrameExpert = new Sprite(player.RNDSegment.TextureExpert, new Vector2(107, 240),
                new Point(16, 24), new Point(0, 0), new Point(2, 4), new Vector2(-1, 0), 2f);

            Vector2 pos = sideAnchor;
            pos.Y += 63;
            spriteSideRookie = new Sprite(player.RNDSegment.TextureRookie, pos, new Point(16, 24),
                new Point(0, 0), new Point(2, 4), new Vector2(0, 1), 2f);
            pos.Y += 96;
            spriteSideVeteran = new Sprite(player.RNDSegment.TextureVeteran, pos, new Point(16, 24),
                new Point(0, 0), new Point(2, 4), new Vector2(0, 1), 2f);
            pos.Y += 96;
            spriteSideExpert = new Sprite(player.RNDSegment.TextureExpert, pos, new Point(16, 24),
                new Point(0, 0), new Point(2, 4), new Vector2(0, 1), 2f);

            pos = sideAnchor;
            pos.Y += 15;
            pos.X += 115;
            spriteBasic = new Sprite(player.RNDSegment.TextureBasic, pos, new Point(32, 48),
                new Point(0, 0), new Point(1, 1), new Vector2(0, 1), 2f);
            pos.Y += 96;
            spriteIntermediate = new Sprite(player.RNDSegment.TextureIntermediate, pos, new Point(32, 48),
                new Point(0, 0), new Point(2, 1), new Vector2(0, 1), 2f);
            pos.Y += 96;
            spriteAdvanced = new Sprite(player.RNDSegment.TextureAdvanced, pos, new Point(32, 48),
                new Point(0, 0), new Point(2, 1), new Vector2(0, 1), 2f);
            #endregion
        }
        #region Menu Entries
        void AddNewProjectMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new RNDAddProjectScreen(player, menuPos, titlePos), e.PlayerIndex);
        }

        void ManageEmployeesMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new RNDHRScreen(player, menuPos, titlePos), e.PlayerIndex);
        }

        void ManageEquipmentMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new RNDEquipmentScreen(player, menuPos, titlePos), e.PlayerIndex);
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            Game1.trackCue.Stop(Microsoft.Xna.Framework.Audio.AudioStopOptions.AsAuthored);
            Game1.trackCue = Game1.soundBank.GetCue("lobby");
            Game1.trackCue.Play();
            base.OnCancel(playerIndex);
        }
        #endregion

        public override void LoadContent()
        {
            base.LoadContent();
            paneImage = ScreenManager.Game.Content.Load<Texture2D>("images/research_splash");

            Game1.trackCue = Game1.soundBank.GetCue("research");
            Game1.trackCue.Play();
        }

        #region Update, Draw and Related Methods
        public override void Update(GameTime gameTime, bool focus, bool covered)
        {
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

            spriteBatch.Draw(player.RNDSegment.FrameImage, splashFrame, Color.White);
            spriteBatch.Draw(paneImage, splashImage, Color.White);
        }

        //method described in ActiveMenuScreen.cs
        public override void DrawProjects(SpriteBatch spriteBatch)
        {
            Vector2 pos = projectAnchor;
            const int spacing = 120;

            player.RNDSegment.ActiveProject.Display(pos, player.RNDSegment.FrameImage, player.RNDSegment.ProgressBar,
                blank, tinyFont, spriteBatch);
            if (player.RNDSegment.ProjectQueue.Count > 0)
            {
                List<Project> qList = new List<Project>();
                foreach (Project p in player.RNDSegment.ProjectQueue)
                {
                    qList.Add(p);
                }
                for (int i = 0; i < 3 && i < qList.Count; i++)
                {
                    pos.X += spacing;
                    qList[i].Display(pos, player.RNDSegment.FrameImage, player.RNDSegment.ProgressBar, blank, tinyFont, spriteBatch);
                }
            }
        }

        //draws employee and equipment counts in sidebar
        public void DrawAssets(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Vector2 pos = sideAnchor;
            CompanySegment seg = player.RNDSegment;

            spriteBatch.DrawString(screenFont, "Assets:", pos, Color.White);

            if (seg.Rookies > 0)                        //draws each asset if exists
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
