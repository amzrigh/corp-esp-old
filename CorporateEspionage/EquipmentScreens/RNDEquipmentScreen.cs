/********************************************
 * Research Equipment Screen Class          *
 * by Mark McCarthy                         *
 *                                          *
 * Provides player controls for managing    *
 * equipment; displays equipment available  *
 * based on MSDN Screen Manager suite       *
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
    class RNDEquipmentScreen : ActiveMenuScreen
    {
        Vector2 anchor = new Vector2(100, 200);

        Sprite basic;           //equipment sprites
        Sprite intermediate;
        Sprite advanced;

        enum Action { Buy, Sell }
        static string[] equipment = { "Computer", "Robot Assistant", "Anti-Grav Simulation Unit" };
        static int equipmentIndex = 0;

        Action action = Action.Buy;
        
        MenuEntry actionMenuEntry;
        MenuEntry equipmentMenuEntry;
        MenuEntry confirmMenuEntry;
        MenuEntry backMenuEntry;

        public RNDEquipmentScreen(Player player, Vector2 menuPos, Vector2 titlePos)
            : base("Human Resources", player, menuPos, titlePos)
        {
            actionMenuEntry = new MenuEntry(string.Empty);
            equipmentMenuEntry = new MenuEntry(string.Empty);
            confirmMenuEntry = new MenuEntry(string.Empty);
            backMenuEntry = new MenuEntry("Back");

            SetMenuEntryText();

            actionMenuEntry.Selected += ActionMenuEntrySelected;
            equipmentMenuEntry.Selected += EquipmentMenuEntrySelected;
            confirmMenuEntry.Selected += ConfirmMenuEntrySelected;
            backMenuEntry.Selected += OnCancel;

            MenuEntries.Add(actionMenuEntry);
            MenuEntries.Add(equipmentMenuEntry);
            MenuEntries.Add(confirmMenuEntry);
            MenuEntries.Add(backMenuEntry);

            Vector2 pos = anchor;
            basic = new Sprite(player.RNDSegment.TextureBasic, pos, new Point(32, 48), new Point(0, 0), new Point(1, 1), new Vector2(0, 1), 2f);
            pos.X += 150;
            intermediate = new Sprite(player.RNDSegment.TextureIntermediate, pos, new Point(32, 48), new Point(0, 0), new Point(2, 1), new Vector2(0, 1), 2f);
            pos.X += 150;
            advanced = new Sprite(player.RNDSegment.TextureAdvanced, pos, new Point(32, 48), new Point(0, 0), new Point(2, 1), new Vector2(0, 1), 2f);
        }

        #region Menu Elements
        void SetMenuEntryText()
        {
            actionMenuEntry.Text = "Action: " + action;
            equipmentMenuEntry.Text = "Gear: " + equipment[equipmentIndex];
            string str;
            if (action == Action.Buy)
                str = "Buy";
            else
                str = "Sell";
            confirmMenuEntry.Text = str;
        }

        void ActionMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            action++;
            if (action > Action.Sell)
                action = Action.Buy;

            SetMenuEntryText();
        }

        void EquipmentMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            equipmentIndex++;
            if (equipmentIndex >= equipment.Length)
                equipmentIndex = 0;

            SetMenuEntryText();
        }

        void ConfirmMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            CompanySegment seg = player.RNDSegment;
            Boolean fail = false;

            if (action == Action.Sell)
            {
                if (equipmentIndex == 0)
                {
                    if (seg.Tools < 1)
                        fail = true;
                    else
                    {
                        seg.Tools--;
                        player.Money += seg.CostTools / 2;
                        Game1.soundBank.PlayCue("sell_equip");
                    }
                }
                else if (equipmentIndex == 1)
                {
                    if (seg.Machinery < 1)
                        fail = true;
                    else
                    {
                        seg.Machinery--;
                        player.Money += seg.CostMach / 2;
                        Game1.soundBank.PlayCue("sell_equip");
                    }
                }
                else
                {
                    if (seg.HeavyMachinery < 1)
                        fail = true;
                    else
                    {
                        seg.HeavyMachinery--;
                        player.Money += seg.CostHeavyMach / 2;
                        Game1.soundBank.PlayCue("sell_equip");
                    }
                }
            }
            else
            {
                if (equipmentIndex == 0)
                {
                    if (seg.CostTools > player.Money)
                        fail = true;
                    else
                    {
                        player.Money -= seg.CostTools;
                        seg.Tools++;
                        Game1.soundBank.PlayCue("buy_asset");
                    }
                }
                else if (equipmentIndex == 1)
                {
                    if (seg.CostMach > player.Money)
                        fail = true;
                    else
                    {
                        player.Money -= seg.CostMach;
                        seg.Machinery++;
                        Game1.soundBank.PlayCue("buy_asset");
                    }
                }
                else
                {
                    if (seg.CostHeavyMach > player.Money)
                        fail = true;
                    else
                    {
                        player.Money -= seg.CostHeavyMach;
                        seg.HeavyMachinery++;
                        Game1.soundBank.PlayCue("buy_asset");
                    }
                }
            }

            if (fail)
                PlayFailSound();
        }
        #endregion

        public override void Update(GameTime gameTime, bool focus, bool covered)
        {
            base.Update(gameTime, focus, covered);
            Rectangle window = ScreenManager.Game.Window.ClientBounds;
            basic.Update(gameTime, window);
            intermediate.Update(gameTime, window);
            advanced.Update(gameTime, window);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            CompanySegment seg = player.RNDSegment;
            base.Draw(gameTime);
            spriteBatch.Begin();


            //display current counts
            Vector2 pos = anchor;
            pos.Y += 64;
            pos.X += 70;
            spriteBatch.DrawString(screenFont, "x " + seg.Tools, pos, Color.White);
            pos.X += 150;
            spriteBatch.DrawString(screenFont, "x " + seg.Machinery, pos, Color.White);
            pos.X += 150;
            spriteBatch.DrawString(screenFont, "x " + seg.HeavyMachinery, pos, Color.White);

            //display costs
            pos = anchor;
            pos.Y += 100;
            pos.X += 5;
            spriteBatch.DrawString(screenFont, "$" + seg.CostTools, pos, Color.White);
            pos.X += 150;
            spriteBatch.DrawString(screenFont, "$" + seg.CostMach, pos, Color.White);
            pos.X += 150;
            spriteBatch.DrawString(screenFont, "$" + seg.CostHeavyMach, pos, Color.White);

            //display sprites
            basic.Draw(gameTime, spriteBatch);
            intermediate.Draw(gameTime, spriteBatch);
            advanced.Draw(gameTime, spriteBatch);


            spriteBatch.End();
        }
    }
}
